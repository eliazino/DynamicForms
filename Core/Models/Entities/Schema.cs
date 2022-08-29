using Core.Application.DTOs.Request;
using Core.Exceptions;
using Core.Models.Enums;
using Core.Models.ValueObjects;
using Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Entities {
    public class Schema : BaseEntity{
        public string id { get; protected set; }
        public string projectID { get; protected set; }
        public string schemaName { get; protected set; }
        public bool locked { get; protected set; } = false;
        public List<SchemaField> SchemaField { get; protected set; }
        public Schema(SchemaDTO data) {
            if (isNullOrEmpty(data.projectID, data.schemaName))
                throw new InputError("All fields are required");            
            this.id = Cryptography.CharGenerator.genID(8, Cryptography.CharGenerator.characterSet.HEX_STRING);
            loadSchemaFields(data);
            this.projectID = data.projectID;
            this.locked = false;            
            this.schemaName = data.schemaName;
        }
        public void loadSchemaFields(SchemaDTO data) {
            List<string> fieldIDs = new List<string>();
            for (int i = 0; i < data.SchemaField.Count; i++) {
                var field = data.SchemaField[i];
                if (isNullOrEmpty(field.fieldName, field.fieldID))
                    throw new InputError("Field Name and fieldID is required for all fields @ Field Number " + i);
                if (!Utilities.isLettersAndNumber(field.fieldID))
                    throw new InputError("Field ID can only have numbers and letters @ " + field.fieldName);
                if (!Enum.IsDefined(typeof(FieldType), field.fieldType))
                    throw new InputError("Field Type must be on of documented @ " + field.fieldName);
                if (field.behavior.Contains(FieldBehaviour.AUTO_GENERATED_DATE) && field.behavior.Contains(FieldBehaviour.AUTO_GENERATED_ID))
                    throw new InputError("Only use AUTO_GENERATED_DATE or AUTO_GENERATED_ID @ " + field.fieldName);
                if (field.fieldType == FieldType.DROP_DOWN) {
                    if ((field.data == null || field.data.Length < 1) && string.IsNullOrEmpty(field.dataSource))
                        throw new InputError("Field Type drop down must have a data source @ " + field.fieldName);
                }
                if (fieldIDs.Contains(field.fieldID))
                    throw new InputError("Field ID Must be unique @ " + field.fieldName);
                fieldIDs.Add(field.fieldID);
            }
            this.SchemaField = data.SchemaField;
        }
        public Schema() { }
        public void updateSchema(SchemaDTO data) {
            if (isNullOrEmpty(data.projectID, data.schemaName, data.id))
                throw new InputError("All fields are required");
            loadSchemaFields(data);
        }
    }
}
