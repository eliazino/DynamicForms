using Core.Application.DTOs.Configurations;
using Core.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Core.Application.DTOs.Response.ResponseFormat;

namespace WebAPI.Helpers {
    public class ExceptionMessage {        
        private readonly SystemVariables _var;
        public int statusCode { get; private set; } = 200;
        ResponseFormat response = new ResponseFormat();
        private readonly string routeName;
        public ExceptionMessage(SystemVariables _var, string route = "") {
            this._var = _var;
            this.routeName = route;
        }
        public RawResponse getMessage(Exception _exc, string req = null) {
            if(_exc is Core.Exceptions.AuthenticationError) {
                this.statusCode = 401;
                return response.failed(_exc.Message);
            }
            if (_exc is Core.Exceptions.InputError) {
                return response.failed(_exc.Message);
            }
            if (_exc is Core.Exceptions.LogicError) {
                return response.failed(_exc.Message);
            }
            if (_var.debug) {
                if(_var.environmentName == "Development") {
                    return response.failed(_exc.ToString());
                } else {
                    return response.failed(_exc.Message);
                }
            }
            if(_var.environmentName == "Development") {
                return response.failed(_exc.Message);
            }            
            return response.failed("Service is unavailable. Try again after some time or contact admin");
        }
    }
}
