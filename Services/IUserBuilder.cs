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
    
    
    public interface IUserBuilder {
        
        Task<IQueryable<UserModel>> GetUsers();
        
        IList<ExpandoObject> GetDisplayModels(List<string> propNames);
        
        Task<BuilderResponse> GetUser_ById(string id);
        
        Task<IQueryable<UserModel>> GetUser_ByTenantId(System.Guid tenantId);
        
        Task<IQueryable<UserModel>> GetUser_ByAuthProvider(string authProvider);
        
        Task<IQueryable<UserModel>> GetUser_ByTitleId(int titleId);
        
        Task<BuilderResponse> AddUser(UserModel model);
        
        Task<BuilderResponse> UpdateUser(UserModel model);
        
        Task<BuilderResponse> DeleteUser(string id);
    }
}

