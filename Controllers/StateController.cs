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
    [Route("State")]
    public class StateController : ControllerBase {
        
        private IStateBuilder _builder;
        
        public StateController(IStateBuilder builder) {
            _builder = builder;
        }
        
        [HttpGet("")]
        public async Task<ActionResult> GetStates() {

            return Ok(await _builder.GetStates());
        }
        
        [HttpGet("Display")]
        public async Task<ActionResult> GetDisplayModels() {
            //List all model properties that should be displayed
            //Here only a couple have been added as an example
            var propNames = new List<string>();
            propNames.Add(nameof(StateModel.Id));
            propNames.Add(nameof(StateModel.Abbreviation));

            return Ok(await Task.FromResult(_builder.GetDisplayModels(propNames)));
        }
        
        [HttpGet("Paged")]
        public async Task<ActionResult> Paged(int pageIndex, int pageSize) {

            var models = await _builder.GetStates();

            return Ok(models.ToPagedList(pageIndex, pageSize, 0, models.Count()));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult> GetState_ById(int id) {

             var response = await _builder.GetState_ById(id);
            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return Ok(response.Model);
        }
        
        [HttpPost("")]
        [ModelStateValidation()]
        public async Task<ActionResult> AddState([FromBody]StateModel model) {

            var response = await _builder.AddState(model);

            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return CreatedAtAction("GetState_ById", new {id = ((StateModel)response.Model).Id}, response.Model);
        }
        
        [HttpPut("")]
        [ModelStateValidation()]
        public async Task<ActionResult> UpdateState([FromBody]StateModel model) {
            var response = await _builder.UpdateState(model);

            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return AcceptedAtAction("GetState_ById", new {id = model.Id}, model);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteState(int id) {

            var response = await _builder.DeleteState(id);

            if (response.ValidationMessage != null) {
                return BadRequest(response.ValidationMessage);
            }

            return StatusCode(response.StatusCode);
        }
    }
}

