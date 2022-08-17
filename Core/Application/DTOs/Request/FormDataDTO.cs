using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Request {
    public class FormDataDTO {
        public string schemaID { get; set; }
        public string projectID { get; set; }
        public JObject dataField { get; set; }
    }
}
