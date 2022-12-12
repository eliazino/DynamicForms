using Core.Application.DTOs.Configurations;
using Core.Application.Interfaces;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Termii {
    [RegisterAsSingleton]
    public class Termii : ITermii {
        private readonly SystemVariables _sysVar;
        private HttpReqClient _httpClient;
        public Termii(IOptionsMonitor<SystemVariables> config) {
            _sysVar = config.CurrentValue;
            _httpClient = new HttpReqClient();
        }
        public async Task<bool> sendMailOTP(string otp, string email) {
            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(email))
                return false;
            JObject req = new JObject();
            req.Add("api_key", _sysVar.TermiiConfig.api_key);
            req.Add("email_address", email);
            req.Add("code", otp);
            req.Add("email_configuration_id", _sysVar.TermiiConfig.email_configuration_id);
            var t = await _httpClient.client(_sysVar.TermiiConfig.url, req.ToString(), HTTPVerb.POST, "application/json");
            return t.statusCode == 200;
        }
    }
}
