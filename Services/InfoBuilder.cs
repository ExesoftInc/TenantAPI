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
    
    
    public class InfoBuilder : IInfoBuilder {
        
        private IDbEntities _entities;
        
        private ILoggerManager _logger;
        
        public InfoBuilder(EntitiesContext context, ILoggerManager logger) {
            _entities = context;
            _logger = logger;
        }
        
        private Expression<Func<Info, InfoModel>>  ProjectToModel {
            get {
                return entity => new InfoModel(entity);
            }
        }
        
        public async Task<IQueryable<InfoModel>> GetInfoes() {
            return await Task.FromResult(_entities.Infoes.Select(ProjectToModel));
        }
        
        public IList<ExpandoObject> GetDisplayModels(List<string> propNames) {
            var models = _entities.Infoes.Select(ProjectToModel);

            var displayModels = new List<ExpandoObject>();
            foreach (var model in models)
            {
                dynamic displayModel = DynamicHelper.ConvertToExpando(model, propNames);
                displayModels.Add(displayModel);
            }

            return displayModels;
        }
        
        public async Task<BuilderResponse> GetInfo_ById(int id) {
            var query = Search(_entities.Infoes, x => x.Id == id).Select(ProjectToModel);
            if (query.Any()) {
                return await Task.FromResult(new BuilderResponse{ Model = query.Single() }); 
            }
            else {
                return await Task.FromResult(new BuilderResponse { ValidationMessage = $"Record Not Found; Info with id = '{id}' doesn't exist." }); 
            }
        }
        
        public async Task<BuilderResponse> AddInfo(InfoModel model) {


            var entity = ModelExtender.ToEntity(model);
            _entities.Infoes.Add(entity);
            await _entities.SaveChangesAsync();
            _logger.LogInfo(string.Format("Info added with values: '{0}'", JsonConvert.SerializeObject(model)));

            return new BuilderResponse{ Model = new InfoModel(entity) }; 
        }
        
        public async Task<BuilderResponse> UpdateInfo(InfoModel model) {

            var query = Search(_entities.Infoes, x =>  x.Id == model.Id);
            if (!query.Any()) {
                return new BuilderResponse { ValidationMessage = "Record Not Found; " + string.Format("Info with _id = '{0}' doesn't exist.",model.Id)}; 
            }

            Info entity = query.SingleOrDefault();
            entity = model.ToEntity(entity);
            await _entities.SaveChangesAsync();
            _logger.LogInfo(string.Format("Info update with values: '{0}'", JsonConvert.SerializeObject(model)));

            return new BuilderResponse{ StatusCode = (int)HttpStatusCode.Accepted }; 
        }
        
        public async Task<BuilderResponse> DeleteInfo(int id) {

            var query = Search(_entities.Infoes, x => x.Id == id);
            if (!query.Any()) {
                return new BuilderResponse { ValidationMessage = "Record Not Found; " + string.Format("Info with _id = '{0}' doesn't exist.",id)}; 
            }

            var entity = query.SingleOrDefault();

            _entities.Infoes.Remove(entity);
            await _entities.SaveChangesAsync();
            _logger.LogInfo(string.Format("Info deleted with values: '{0}'", JsonConvert.SerializeObject(new InfoModel(entity))));

            return new BuilderResponse{ StatusCode = (int)HttpStatusCode.NoContent }; 
        }
        
        private IQueryable<Info> Search(IQueryable<Info> query, Expression<Func<Info, bool>> filter) {
            return query.Where(filter);
        }
    }
}
