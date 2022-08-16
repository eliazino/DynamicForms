using Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.ValueObjects {
    public class SchemaField {
        public string fieldName { get; set; }
        public string fieldID { get; set; }
        public FieldType fieldType { get; set; }
        public List<FieldBehaviour> behavior { get; set; }
        public bool hide { get; set; }
        public string[] data { get; set; }
        public string dataSource { get; set; }
    }
}
