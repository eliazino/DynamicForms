using Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Entities {
    public class FilterEntities {
        public string id { get; protected set; }
        public string fieldName { get; protected set; }
        public string fieldValue { get; protected set; }
        public string schemaID { get; protected set; }
        public string fieldID { get; protected set; }
        public FilterEntities() { }
        public FilterEntities(string fieldName, string fieldValue, string fieldID, string schemaID) {
            this.id = Cryptography.CharGenerator.genID();
            this.fieldName = fieldName;
            this.fieldValue = fieldValue;
            this.schemaID = schemaID;
            this.fieldID = fieldID;
        }
    }
}
