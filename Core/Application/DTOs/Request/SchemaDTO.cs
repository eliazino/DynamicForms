using Core.Models.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Request {
    public class SchemaDTO {
        public string id { get; set; }
        public string projectID { get; set; }
        public string schemaName { get; set; }
        public string schemaID { get; set; }
        public bool locked { get; set; } = false;
        public List<SchemaField> SchemaField { get; set; }
    }
}
