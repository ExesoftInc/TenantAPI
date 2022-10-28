// ----------------------------------------------------------------------------------
// <copyright company="Exesoft Inc.">
//	This code was generated by Instant Web API code automation software (https://www.InstantWebAPI.com)
//	Copyright Exesoft Inc. © 2019.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------

using InstantHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TenantAPI.Models;
using TenantAPI.Services;

namespace TenantAPI.Controllers {
    
    
    // TODO: Uncomment the following line to use an API Key; change the value of the key in appSetting (X-API-Key)
    // [ApiKey()]
    [Route("UserRole")]
    public class UserRoleController : ControllerBase {
        
        private IUserRoleBuilder _builder;
        
        public UserRoleController(IUserRoleBuilder builder) {
            _builder = builder;
        }
        
        [HttpGet("")]
        public async Task<ActionResult> GetUserRoles() {

            return Ok(await _builder.GetUserRoles());
        }
        
        [HttpGet("Display")]
        public async Task<ActionResult> GetDisplayModels() {
            //List all model properties that should be displayed
            //Here only a couple have been added as an example
            var propNames = new List<string>();
            propNames.Add(nameof(UserRoleModel.RoleId));
            propNames.Add(nameof(UserRoleModel.UserId));

            return Ok(await Task.FromResult(_builder.GetDisplayModels(propNames)));
        }
        
        [HttpGet("Paged")]
        public async Task<ActionResult> Paged(int pageIndex, int pageSize) {

            var models = await _builder.GetUserRoles();

            return Ok(models.ToPagedList(pageIndex, pageSize, 0, models.Count()));
        }
        
        [HttpGet("{roleId}/{userId}")]
        public async Task<ActionResult> GetUserRole_ByRoleIdUserId(int roleId, string userId) {

             var response = await _builder.GetUserRole_ByRoleIdUserId(roleId, userId);
            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return Ok(response.Model);
        }
        
        [HttpGet("GetUserRole_ByRoleId/{roleId}")]
        public async Task<IQueryable<UserRoleModel>> GetUserRole_ByRoleId(int roleId) {

            return await _builder.GetUserRole_ByRoleId(roleId);
        }
        
        [HttpGet("GetUserRole_ByUserId/{userId}")]
        public async Task<IQueryable<UserRoleModel>> GetUserRole_ByUserId(string userId) {

            return await _builder.GetUserRole_ByUserId(userId);
        }
        
        [HttpPost("")]
        [ModelStateValidation()]
        public async Task<ActionResult> AddUserRole([FromBody]UserRoleModel model) {

            var response = await _builder.AddUserRole(model);

            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return CreatedAtAction("GetUserRole_ByRoleIdUserId", new {roleId = ((UserRoleModel)response.Model).RoleId, userId = ((UserRoleModel)response.Model).UserId}, response.Model);
        }
        
        [HttpPut("")]
        [ModelStateValidation()]
        public async Task<ActionResult> UpdateUserRole([FromBody]UserRoleModel model) {
            var response = await _builder.UpdateUserRole(model);

            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return AcceptedAtAction("GetUserRole_ByRoleIdUserId", new {roleId = model.RoleId, userId = model.UserId}, model);
        }
        
        [HttpDelete("{roleId}/{userId}")]
        public async Task<ActionResult> DeleteUserRole(int roleId, string userId) {

            var response = await _builder.DeleteUserRole(roleId, userId);

            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return StatusCode(response.StatusCode);
        }
    }
}
