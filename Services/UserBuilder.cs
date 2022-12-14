// ----------------------------------------------------------------------------------
// <copyright company="Exesoft Inc.">
//	This code was generated by Instant Web API code automation software (https://www.InstantWebAPI.com)
//	Copyright Exesoft Inc. © 2019.  All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using TenantAPI.Entities;
using TenantAPI.Models;
using TenantAPI.Services;

namespace TenantAPI.Services {
    
    
    public class UserBuilder : IUserBuilder {
        
        private IDbEntities _entities;
        
        private ILoggerManager _logger;
        
        public UserBuilder(EntitiesContext context, ILoggerManager logger) {
            _entities = context;
            _logger = logger;
        }
        
        private Expression<Func<User, UserModel>>  ProjectToModel {
            get {
                return entity => new UserModel(entity);
            }
        }
        
        public async Task<IQueryable<UserModel>> GetUsers() {
            return await Task.FromResult(_entities.Users.Select(ProjectToModel));
        }
        
        public IList<ExpandoObject> GetDisplayModels(List<string> propNames) {
            var models = _entities.Users.Select(ProjectToModel);

            var displayModels = new List<ExpandoObject>();
            foreach (var model in models)
            {
                dynamic displayModel = DynamicHelper.ConvertToExpando(model, propNames);
                displayModels.Add(displayModel);
            }

            return displayModels;
        }
        
        public async Task<BuilderResponse> GetUser_ById(string id) {
            var query = Search(_entities.Users, x => x.Id == id).Select(ProjectToModel);
            if (query.Any()) {
                return await Task.FromResult(new BuilderResponse{ Model = query.Single() }); 
            }
            else {
                return await Task.FromResult(new BuilderResponse { ValidationMessage = $"Record Not Found; User with id = '{id}' doesn't exist." }); 
            }
        }
        
        public async Task<IQueryable<UserModel>> GetUser_ByTenantId(System.Guid tenantId) {

            var query = await Task.FromResult(Search(_entities.Users, x => x.TenantId == tenantId).Select(ProjectToModel));

            return query;
        }
        
        public async Task<IQueryable<UserModel>> GetUser_ByAuthProvider(string authProvider) {

            var query = await Task.FromResult(Search(_entities.Users, x => x.AuthProvider == authProvider).Select(ProjectToModel));

            return query;
        }
        
        public async Task<IQueryable<UserModel>> GetUser_ByTitleId(int titleId) {

            var query = await Task.FromResult(Search(_entities.Users, x => x.TitleId == titleId).Select(ProjectToModel));

            return query;
        }
        
        public async Task<BuilderResponse> AddUser(UserModel model) {

            var matchTenantId = _entities.Tenants.Where(x => x.Id.Equals(model.TenantId));
            if (!matchTenantId.Any()) {
                return new BuilderResponse { ValidationMessage = $"Foreign Key Violation; " + nameof(model.TenantId) + " '{model.TenantId}' doesn't exist in the system."}; 
            }

            var matchAuthProvider = _entities.AuthProviders.Where(x => x.Name.Equals(model.AuthProvider));
            if (!matchAuthProvider.Any()) {
                return new BuilderResponse { ValidationMessage = $"Foreign Key Violation; " + nameof(model.AuthProvider) + " '{model.AuthProvider}' doesn't exist in the system."}; 
            }

            if (model.TitleId != null) {
                var matchTitleId = _entities.UserTitles.Where(x => x.TitleId.Equals(model.TitleId));
                if (!matchTitleId.Any()) {
                return new BuilderResponse { ValidationMessage = $"Foreign Key Violation; " + nameof(model.TitleId) + " '{model.TitleId}' doesn't exist in the system."}; 
                }
            }


            var entity = ModelExtender.ToEntity(model);
            _entities.Users.Add(entity);
            await _entities.SaveChangesAsync();
            _logger.LogInfo(string.Format("User added with values: '{0}'", JsonConvert.SerializeObject(model)));

            return new BuilderResponse{ Model = new UserModel(entity) }; 
        }
        
        public async Task<BuilderResponse> UpdateUser(UserModel model) {

            var query = Search(_entities.Users, x =>  x.Id == model.Id);
            if (!query.Any()) {
            return new BuilderResponse { ValidationMessage = "Record Not Found; " + string.Format("User with _id = '{0}' doesn't exist.",model.Id)}; 
            }

            var matchTenantId = _entities.Tenants.Where(x => x.Id.Equals(model.TenantId));
            if (!matchTenantId.Any()) {
            return new BuilderResponse { ValidationMessage = "Foreign Key Violation; " + nameof(model.TenantId) + string.Format("TenantId = '{0}' doesn't exist in the system.", model.TenantId)}; 
            }

            if (!string.IsNullOrEmpty(model.AuthProvider))
            {
                var matchAuthProvider = _entities.AuthProviders.Where(x => x.Name.Equals(model.AuthProvider));
                if (!matchAuthProvider.Any())
                {
                    return new BuilderResponse { ValidationMessage = "Foreign Key Violation; " + nameof(model.AuthProvider) + string.Format("AuthProvider = '{0}' doesn't exist in the system.", model.AuthProvider) };
                }
            }


            if (model.TitleId != null) {
                var matchTitleId = _entities.UserTitles.Where(x => x.TitleId.Equals(model.TitleId));
                if (!matchTitleId.Any()) {
            return new BuilderResponse { ValidationMessage = "Foreign Key Violation; " + nameof(model.TitleId) + string.Format("TitleId = '{0}' doesn't exist in the system.", model.TitleId)}; 
                }
            }

            User entity = query.SingleOrDefault();

            entity = model.ToEntity(entity);
            await _entities.SaveChangesAsync();
            _logger.LogInfo(string.Format("User update with values: '{0}'", JsonConvert.SerializeObject(model)));

            return new BuilderResponse{ StatusCode = (int)HttpStatusCode.Accepted }; 
        }
        
        public async Task<BuilderResponse> DeleteUser(string id) {

            var query = Search(_entities.Users, x => x.Id == id);
            if (!query.Any()) {
                return new BuilderResponse { ValidationMessage = "Record Not Found; " + string.Format("User with _id = '{0}' doesn't exist.",id)}; 
            }

            var entity = query.SingleOrDefault();

            _entities.Users.Remove(entity);
            await _entities.SaveChangesAsync();
            _logger.LogInfo(string.Format("User deleted with values: '{0}'", JsonConvert.SerializeObject(new UserModel(entity))));

            return new BuilderResponse{ StatusCode = (int)HttpStatusCode.NoContent }; 
        }
        
        private IQueryable<User> Search(IQueryable<User> query, Expression<Func<User, bool>> filter) {
            return query.Where(filter);
        }
    }
}

