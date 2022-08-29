using Core.Application.DTOs.Configurations;
using Core.Application.DTOs.Request;
using Core.Application.Interfaces.Auth;
using Core.Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Model;

namespace WebAPI.Controllers {    
    public class FormsController : ControllerBase {
        private readonly SystemVariables _config;
        private readonly ExceptionMessage _errHandler;
        private readonly IFormCreatorUseCase _useCase;
        private readonly IIdentityMngr _identity;
        public FormsController(IFormCreatorUseCase usecase, IOptionsMonitor<SystemVariables> config, IIdentityMngr identity) {
            this._useCase = usecase;
            this._config = config.CurrentValue;
            this._identity = identity;
            _errHandler = new ExceptionMessage(_config, _identity.endPointAddress);
        }

        [HttpPost, Route("v1/create/project")]
        [Consumes("application/json")]
        public async Task<IActionResult> setUp([FromBody] RequestBody<ProjectDTO> Request) {
            try {
                return new ObjectResult(await _useCase.createProject(Request.data));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpPost, Route("v1/add/data")]
        [Consumes("application/json")]
        public async Task<IActionResult> uploadData([FromBody] RequestBody<FormDataDTO> Request) {
            try {
                return new ObjectResult(await _useCase.addData(Request.data));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpPost, Route("v1/create/schema")]
        [Consumes("application/json")]
        public async Task<IActionResult> createSchema([FromBody] RequestBody<SchemaDTO> Request) {
            try {
                return new ObjectResult(await _useCase.createSchema(Request.data));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpPost, Route("v1/fetch/data")]
        [Consumes("application/json")]
        public async Task<IActionResult> fetchData([FromBody] RequestBody<DataFilterDTO> Request) {
            try {
                return new ObjectResult(await _useCase.getData(Request.data));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpGet, Route("v1/fetch/projects")]
        [Consumes("application/json")]
        public async Task<IActionResult> fetchProject() {
            try {
                return new ObjectResult(await _useCase.getProject());
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpGet, Route("v1/fetch/schemaByID/{schemaID}")]
        [Consumes("application/json")]
        public async Task<IActionResult> fetchSchemaByID(string schemaID) {
            try {
                return new ObjectResult(await _useCase.getSchemaByID(schemaID));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpGet, Route("v1/fetch/schemaByProject/{projectID}")]
        [Consumes("application/json")]
        public async Task<IActionResult> fetchSchemaByProject(string projectID) {
            try {
                return new ObjectResult(await _useCase.getSchemaByProject(projectID));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpGet, Route("v1/fetch/filterSchemaByID/{schemaID}")]
        [Consumes("application/json")]
        public async Task<IActionResult> fetchFilterSchemaByProject(string schemaID) {
            try {
                return new ObjectResult(await _useCase.getFilterSchemaByID(schemaID));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpGet, Route("v1/delete/data/{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> deleteData(string id) {
            try {
                return new ObjectResult(await _useCase.deleteData(id));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }
    }
}
