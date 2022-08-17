using Core.Application.DTOs.Request;
using Core.Exceptions;
using Core.Models.Enums;
using Core.Models.ValueObjects;
using Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Entities {
    public class FormData : BaseEntity {
        public string id { get; protected set; }
        public string schemaID { get; protected set; }
        public string projectID { get; protected set; }
        public Dictionary<string, dynamic> data { get; protected set; }
        public FormData(FormDataDTO dataOb, Schema schema) {
            this.id = Cryptography.CharGenerator.genID(8, Cryptography.CharGenerator.characterSet.HEX_STRING);
            schemaID = schema.id;
            projectID = schema.projectID;
            this.data = new Dictionary<string, dynamic>();
            foreach (SchemaField field in schema.SchemaField) {
                if ((bool)field.behavior?.Contains(FieldBehaviour.AUTO_GENERATED_DATE)) {
                    data.Add(field.fieldID, Utilities.getTodayDate().modernDate);
                    continue;
                }
                if ((bool)field.behavior?.Contains(FieldBehaviour.AUTO_GENERATED_ID)) {
                    data.Add(field.fieldID, Cryptography.CharGenerator.genID(8, Cryptography.CharGenerator.characterSet.HEX_STRING));
                    continue;
                }
                Type t = typeof(double);
                dynamic fdata;
                if (field.fieldType.Equals(FieldType.NUMBER)) {
                    fdata = Utilities.findNumber(dataOb.dataField, field.fieldID);                    
                }else if (field.fieldType.Equals(FieldType.LONG_NUMBER)) {
                    fdata = Utilities.findLongNumber(dataOb.dataField, field.fieldID);
                    t = typeof(long);
                }else {
                    fdata = Utilities.findString(dataOb.dataField, field.fieldID);
                    t = typeof(string);
                }
                if (field.behavior.Contains(FieldBehaviour.REQUIRED))
                    if (fdata == getDefaultValue(t))
                        throw new InputError("Field "+field.fieldName+" is a required field of type "+field.fieldType.ToString());
                data.Add(field.fieldID, fdata);
            }
        }
        public FormData() {

        }
        object getDefaultValue(Type t) {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}
