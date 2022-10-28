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
    
    
    public interface IAuthProviderBuilder {
        
        Task<IQueryable<AuthProviderModel>> GetAuthProviders();
        
        IList<ExpandoObject> GetDisplayModels(List<string> propNames);
        
        Task<BuilderResponse> GetAuthProvider_ByName(string name);
        
        Task<BuilderResponse> AddAuthProvider(AuthProviderModel model);
        
        Task<BuilderResponse> UpdateAuthProvider(AuthProviderModel model);
        
        Task<BuilderResponse> DeleteAuthProvider(string name);
    }
}
