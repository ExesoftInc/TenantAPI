using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TenantAPI.Authentication;
using TenantAPI.Entities;
using TenantAPI.Helper;
using TenantAPI.Models;
using TenantAPI.Services;

namespace TenantAPI.Controllers
{


    public class AuthenticateController : ControllerBase
    {
        private IOptions<AppSettings> _appSettings;
        private IDbEntities _entities;
        private IAuthenticate _authenticate;
        private readonly IEmailSender _emailSender;
        private readonly string _angularRoot;
        private ILoggerManager _logger;

        public AuthenticateController(IOptions<AppSettings> appSettings, EntitiesContext context, 
            IAuthenticate authenticate, ILoggerManager logger, IEmailSender emailSender)
        {
            _appSettings = appSettings;
            _entities = context;
            _authenticate = authenticate;
            _logger = logger;
            _emailSender = emailSender;
            _angularRoot = _appSettings.Value.AngularRoot;
        }


        [HttpPost("Register")]
        [ModelStateValidation()]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(model.TenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + tenant);

            var tenantId = tenant.Id;
            if (_authenticate.UserExists(tenantId, model.Email, null))
                return BadRequest("This email already exists.");

            var user = new User
            {
                TenantId = tenantId,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                TitleId = model.TitleId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Token = Guid.NewGuid().ToString()
            };

            var createdUser = await _authenticate.Register(user, model.Password);

            var userModel = new UserModel(createdUser);

            string callbackUrl = Url.ActionLink("ConfirmEmail", "Authenticate", values: new { model.TenantName, userModel.Token, userModel.Email });
            var emailController = new EmailController(_emailSender);
            emailController.SendRegistrationEmail(userModel.Email, callbackUrl);

            return Ok(new
            {
                token = await _authenticate.GenerateToken(tenant.DatabaseName, userModel, true)
            });
        }


        [HttpGet("confirmEmail")]
        public ActionResult ConfirmEmail(string tenantName, string email, string token)
        {
            string confirmedEmail = _angularRoot + "confirmedEmail/";
            string invalidRequest = _angularRoot + "invalidEmailToken?errorMessage=";
            string errorMessage;
            if (string.IsNullOrEmpty(tenantName))
            {
                errorMessage = Globals.TENANT_NOT_FOUND + tenantName + Globals.TENANT_NOT_REGISTERED;
                return Redirect(invalidRequest + errorMessage);
            }

            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(tenantName));
            if(tenant == null)
            {
                errorMessage = Globals.TENANT_NOT_FOUND + tenantName + Globals.TENANT_NOT_FOUND;
                return Redirect(invalidRequest + errorMessage);
            }

            var userExits = _authenticate.UserExists(tenant.Id, email, null);
            if (!userExits)
            {
                errorMessage = Globals.EMAIL_NOT_FOUND + email + Globals.EMAIL_DOES_NOT_EXIST;
                return Redirect(invalidRequest + errorMessage);
            }

            var matchUser = _entities.Users.Where(x => x.Email.Equals(email) &&
                x.AuthProvider.Equals(Authenticate.LOCAL_AUTH));

            var user = matchUser.FirstOrDefault();
            if (!user.Token.Equals(token))
            {
                user.LockoutEnabled = true;
                _entities.SaveChangesAsync();
                errorMessage = "Invalid Token;" + string.Format("Your account '{0}' is locked. Please contact customer service.", user.Email);
                return Redirect(invalidRequest + errorMessage);
            }

            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            _entities.SaveChangesAsync();
            return Redirect(confirmedEmail);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(login.TenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + login.TenantName);;

            var user = _authenticate.Login(tenant.Id, login.Email, login.Password).Result;

            if (user == null) return BadRequest("Unauthorized");
            if (!user.LockoutEnabled && user.AccessFailedCount > 0) return BadRequest("Unauthorized");
            
            if (!user.EmailConfirmed)
                return BadRequest("Please confirm your email, before trying to log in.");

            if (user.LockoutEnabled)
                return BadRequest("Login lockedout:" + string.Format("Please try again after this time '{0}'.", user.LockoutEnd));

            var userModel = new UserModel(user);
            return Ok(new
            {
                token = await _authenticate.GenerateToken(tenant.DatabaseName, userModel, false)
            });
        }

        [HttpGet("NeedsProviderRegisterForm")]
        public ActionResult NeedsProviderRegisterForm(string tenantName, string email, string provider)
        {
            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(tenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + tenantName);

            var tenantId = tenant.Id;
            var result = _authenticate.NeedsProviderRegisterForm(tenantId,email, provider);

            return Ok(result);
        }

        [HttpPost("registerByProvider")]
        [ModelStateValidation()]
        public async Task<IActionResult> RegisterByProvider([FromBody] RegisterByProviderModel model)
        {
            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(model.TenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + model.TenantName);

            var tenantId = tenant.Id;
            if (_authenticate.UserExists(tenantId, model.Email, model.Provider))
                return BadRequest("This email already exists.");

            var user = new User();
            user.Id = model.Id;
            user.AuthProvider = model.Provider;
            user.TenantId = tenantId;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.TitleId = model.TitleId;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var createdUser = await _authenticate.RegisterByProvider(user);
            var userModel = new UserModel(createdUser);

            return Ok(new
            {
                token = await _authenticate.GenerateToken(tenant.DatabaseName, userModel, true)
            });
        }

        [HttpPost("LoginByProvider")]
        public async Task<IActionResult> LoginByProvider(string tenantName, string id)
        {
            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(tenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + tenantName);

            var tenantId = tenant.Id;
            var user = _authenticate.LoginByProvider(tenantId, id).Result;

            if (user == null)
                return Unauthorized();

            var userModel = new UserModel(user);

            return Ok(new
            {
                token = await _authenticate.GenerateToken(tenant.DatabaseName, userModel, false)
            });
        }

        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword(string tenantName, string email)
        {
            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(tenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + tenantName);

            var userExits = _authenticate.UserExists(tenant.Id, email, null);
            if (!userExits)
            {
                return BadRequest(Globals.EMAIL_NOT_FOUND + email + Globals.EMAIL_DOES_NOT_EXIST);
            }

            var token = SequentialGuid.NewGuid().ToString();
            var matchEmail = _entities.Users.Where(x => x.Email.Equals(email) && x.TenantId.Equals(tenant.Id)
                && x.AuthProvider.Equals(Authenticate.LOCAL_AUTH));
            var user = matchEmail.FirstOrDefault();
            user.Token = token;
            user.TokenDate = DateTime.Now;

            var callbackUrl = Url.ActionLink("ResetPasswordLink", "Authenticate", values: new { tenantName, token, email });
            var emailController = new EmailController( _emailSender);
            emailController.ResetPasswordEmail(email, callbackUrl);
            _entities.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("ResetPasswordLink")]
        public IActionResult ResetPasswordLink(string tenantName, string token, string email)
        {
            string resetPassword = _angularRoot + "changePassword?token=";
            string invalidRequest = _angularRoot + "invalidEmailToken?errorMessage=";
            string errorMessage;

            var tenant = _entities.Tenants.SingleOrDefault(x => x.DatabaseName.Equals(tenantName));
            if (tenant == null) return BadRequest(Globals.TENANT_NOT_FOUND + tenantName);

            if (string.IsNullOrEmpty(token))
            {
                errorMessage = "Token not provided; " + string.Format("Token is missing.");
                return Redirect(invalidRequest + errorMessage);
            }

            var userExits = _authenticate.UserExists(tenant.Id, email, null);
            if (!userExits)
            {
                errorMessage = Globals.EMAIL_NOT_FOUND + tenantName + Globals.EMAIL_DOES_NOT_EXIST;
                return Redirect(invalidRequest + errorMessage);
            }

            var matchEmail = _entities.Users.Where(x => x.Email.Equals(email) && x.TenantId.Equals(tenant.Id)
                && x.AuthProvider.Equals(Authenticate.LOCAL_AUTH));
            var matchToken = matchEmail.Where(x => x.Token.Equals(token));
            if (!matchToken.Any())
            {
                errorMessage = "Invalid token provided; " + string.Format("Token doesn't match.");
                return Redirect(invalidRequest + errorMessage);
            }

            var user = matchToken.FirstOrDefault();
            var now = DateTime.Now;
            if (user.TokenDate.HasValue && user.TokenDate.Value.AddDays(1) < DateTime.Now)
            {
                errorMessage = "Token expired; " + string.Format("Please try again.");
                return Redirect(invalidRequest + errorMessage);
            }
            else
            {
                return Redirect(resetPassword + WebUtility.UrlEncode(token));
            }            
        }

        [HttpPost("ChangePassword")]
        [ModelStateValidation()]
        public async Task<IActionResult> ChangePassword([FromBody] TokenModel resetModel)
        {
            var matchUser = _entities.Users.Where(x => x.Token.Equals(resetModel.Token));
            if (!matchUser.Any())
            {
                return BadRequest("Invalid information, please try again.");                
            }

            bool result = await _authenticate.ChangePassword(matchUser.FirstOrDefault(), resetModel.Password);

            return Ok(result);
        }

    }
}
