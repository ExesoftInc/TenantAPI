// ----------------------------------------------------------------------------------
// <copyright company="Exesoft Inc.">
//	This code was generated by Instant Web API code automation software (https://www.InstantWebAPI.com)
//	Copyright Exesoft Inc. © 2019.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TenantAPI.Models;

namespace TenantAPI.Services {
    
    
    public interface IUserRoleBuilder {
        
        Task<IQueryable<UserRoleModel>> GetUserRoles();
        
        IList<ExpandoObject> GetDisplayModels(List<string> propNames);
        
        Task<BuilderResponse> GetUserRole_ByRoleIdUserId(int roleId, string userId);
        
        Task<IQueryable<UserRoleModel>> GetUserRole_ByRoleId(int roleId);
        
        Task<IQueryable<UserRoleModel>> GetUserRole_ByUserId(string userId);
        
        Task<BuilderResponse> AddUserRole(UserRoleModel model);
        
        Task<BuilderResponse> UpdateUserRole(UserRoleModel model);
        
        Task<BuilderResponse> DeleteUserRole(int roleId, string userId);
    }
}
