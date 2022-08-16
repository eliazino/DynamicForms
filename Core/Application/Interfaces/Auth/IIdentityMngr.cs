using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Interfaces.Auth {
    public interface IIdentityMngr {
        string message { get; }
        public bool APIonlyValid { get; }
        string basicUsername { get; }
        string basicPassword { get; }
        bool valid { get; }
        bool sessionValid();
        string endPointAddress { get; }
        string getJWTIdentity(Dictionary<string, string> identity, int expiry = 0);
        T getProfile<T>();
        string getHeaderValue(string key);
        IDictionary<string, object> getAllHeader();
        void loadCustomHeaders(IDictionary<string, object> header);
        string IPAddress { get; }
    }
}
