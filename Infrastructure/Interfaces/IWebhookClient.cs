using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces {
    public interface IWebhookClient {
        Task<bool> callUrl(JObject payload, string url = null);
    }
}
