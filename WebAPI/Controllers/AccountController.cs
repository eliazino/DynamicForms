using Core.Application.DTOs.Configurations;
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
    public class AccountController : ControllerBase {
        private readonly SystemVariables _config;
        private readonly ExceptionMessage _errHandler;
        private readonly IUserUseCase _useCase;
        private readonly IIdentityMngr _identity;
        public AccountController(IUserUseCase usecase, IOptionsMonitor<SystemVariables> config, IIdentityMngr identity) {
            this._useCase = usecase;
            this._config = config.CurrentValue;
            this._identity = identity;
            _errHandler = new ExceptionMessage(_config, _identity.endPointAddress);
        }

        [HttpPost, Route("v1/login")]
        [Consumes("application/json")]
        public async Task<IActionResult> setUp([FromBody] RequestBody<AccountRequest> Request) {
            try {
                return new ObjectResult(await _useCase.login(Request.data.email, Request.data.code));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpPost, Route("v1/invite")]
        [Consumes("application/json")]
        public async Task<IActionResult> invite([FromBody] RequestBody<AccountRequest> Request) {
            try {
                return new ObjectResult(await _useCase.inviteToProject(Request.data.email, Request.data.projectID, Request.data.isOwner));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        [HttpPost, Route("v1/remove")]
        [Consumes("application/json")]
        public async Task<IActionResult> remove([FromBody] RequestBody<AccountRequest> Request) {
            try {
                return new ObjectResult(await _useCase.removeFromProject(Request.data.email, Request.data.projectID));
            } catch (Exception err) {
                return new ObjectResult(_errHandler.getMessage(err)) { StatusCode = _errHandler.statusCode };
            }
        }

        public class AccountRequest {
            public string email { get; set; }
            public string code { get; set; }
            public string projectID { get; set; }
            public bool isOwner { get; set; } = false;
        }

    }
}
