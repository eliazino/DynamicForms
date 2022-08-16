using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Application.Interfaces.Auth;
using Core.Application.DTOs.Configurations;
using NetCore.AutoRegisterDi;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Services {
    [RegisterAsScoped]
    public class IdentityMngr : IIdentityMngr {
        private readonly SystemVariables _config;
        private readonly IHttpContextAccessor _contextAccessor;
        private protected IHeaderDictionary headers;
        private protected JObject profile;
        public bool valid { get; private set; }
        public bool APIonlyValid { get; private set; } = false;
        public string message { get; private set; }
        public string basicUsername { get; private set; }
        public string basicPassword { get; private set; }
        private readonly JWTIdentity tokenMngr;
        public string IPAddress { get; private set; }
        public string endPointAddress { get; private set; }
        public IdentityMngr(IOptionsMonitor<SystemVariables> config, IHttpContextAccessor contextAccessor) {
            this._config = config.CurrentValue;
            this._contextAccessor = contextAccessor;
            this.tokenMngr = new JWTIdentity(_config.jwtsecret);
            loadProfile();
            getIPAddress();
            getRoute();
        }

        private void getRoute() {
            try {
                this.endPointAddress = _contextAccessor.HttpContext.Request.Path;
            } catch { }
        }
        public bool sessionValid() {
            loadProfile(true);
            return valid;
        }
        public string getHeaderValue(string key) {
            StringValues p;
            this.headers.TryGetValue(key, out p);
            return p;
        }
        private void getIPAddress() {
            string[] ipAddresses = new string[] { null };
            string altIPAddress = null;
            try {
                try {
                    ipAddresses = this.getHeaderValue("X-Forwarded-For").ToString().Split(':');
                } catch { }
                altIPAddress = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            } catch { }
            var ipAddress = string.IsNullOrEmpty(ipAddresses[0]) ? altIPAddress : ipAddresses[0];
            this.IPAddress = ipAddress;
        }
        private void loadProfile(bool enforceExpiryCheck = false) {
            try {
                this.headers = _contextAccessor.HttpContext.Request.Headers;
                string h = headers["Authorization"];
                if (!string.IsNullOrEmpty(h)) {
                    h = h.Trim();
                    string[] auths = h.Split(" ");
                    if (h.StartsWith("Bearer")) {
                        if (auths.Length >= 2) {
                            h = auths[1];
                            profile = tokenMngr.verifyToken(h, enforceExpiryCheck ? enforceExpiryCheck : _config.identityExpires);
                            valid = profile != null;
                            bool APIOnly = false;
                            if (valid && profile.ContainsKey("APIOnly")) {
                                APIOnly = profile["APIOnly"].ToString() == "1";
                            }
                            valid = valid && !APIOnly;
                            APIonlyValid = APIOnly;
                            if (!valid)
                                message = "The authentication is invalid or expired";
                        } else {
                            valid = false;
                            message = "The authentication Structure could not be undersood";
                        }
                    } else if (h.StartsWith("Basic")) {
                        try {
                            var credentialBytes = Convert.FromBase64String(auths[1]);
                            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                            this.basicUsername = credentials[0];
                            this.basicPassword = credentials[1];
                            valid = false;
                        } catch {
                            valid = false; this.message = "Authorization Header is not valid. Login again or contact admin";
                        }
                    } else {
                        valid = false; this.message = "Authorization Header is not valid. Login again or contact admin";
                    }                    
                } else { valid = false; this.message = "Authorization Header is missing in request"; }
            } catch {
                valid = false;
                this.message = "Authorization Header is missing in request";
            }
        }

        public string getJWTIdentity(Dictionary<string, string> identity, int expiry = 0) {
            return tokenMngr.getToken(identity, expiry < 1? _config.identityExpiryMins : expiry);
        }

        public T getProfile<T>() {
            return profile.ToObject<T>();
        }
        public IDictionary<string, object> getAllHeader() {
            var comparer = StringComparer.OrdinalIgnoreCase;
            IDictionary<string, object> snew = new Dictionary<string, object>(comparer);
            foreach (KeyValuePair<string, StringValues> val in this.headers) {
                snew.Add(val.Key, val.Value.ToString());
            }
            return snew;
        }
        public void loadCustomHeaders(IDictionary<string, object> header) {
            var headersUsable = new Dictionary<String, StringValues>();
            foreach (var kvp in header) {
                headersUsable.Add(kvp.Key, (StringValues)System.Text.Encoding.Default.GetString((byte[])kvp.Value));
            }
            var headers = new HeaderDictionary(headersUsable) as IHeaderDictionary;
            this.headers = headers;
            loadProfile();
        }

    }
}
