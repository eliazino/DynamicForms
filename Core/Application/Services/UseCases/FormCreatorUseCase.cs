using Core.Application.DTOs.Request;
using Core.Application.DTOs.Response;
using Core.Application.Interfaces.Repository.MongoDB;
using Core.Application.Interfaces.UseCases;
using Core.Models.Entities;
using Core.Models.Enums;
using Core.Models.ValueObjects;
using Core.Shared;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Application.DTOs.Response.ResponseFormat;

namespace Core.Application.Services.UseCases {
    [RegisterAsScoped]
    public class FormCreatorUseCase : IFormCreatorUseCase {
        private readonly IFilterEntitiesRepository _filterEnt;
        private readonly IFormDataRepository _formdata;
        private readonly IProjectRepository _project;
        private readonly ISchemaRepository _schema;
        public FormCreatorUseCase(IFilterEntitiesRepository filterEnt, IFormDataRepository formdata, IProjectRepository project, ISchemaRepository schema) {
            this._filterEnt = filterEnt;
            this._formdata = formdata;
            this._project = project;
            this._schema = schema;
        }

        public async Task<RawResponse> addData(FormDataDTO dto) {
            ResponseFormat response = new ResponseFormat();
            if (string.IsNullOrEmpty(dto.projectID) || string.IsNullOrEmpty(dto.schemaID))
                return response.failed("Invalid Request. Schema and project is required");
            var schema = await _schema.getSchemaById(dto.schemaID);
            if (schema.Count < 1)
                return response.failed("Schema was not found");
            FormData entity = new FormData(dto, schema[0]);
            var uniques = schema[0].SchemaField.FindAll(SF => SF.behavior.Contains(FieldBehaviour.UNIQUE));
            var filtrees = schema[0].SchemaField.FindAll(SF => SF.behavior.Contains(FieldBehaviour.FILTERABLE_DROPDOWN));
            Dictionary<string, string> filterEnt = new Dictionary<string, string>();
            if(uniques?.Count > 0) {
                foreach(SchemaField field in uniques) {
                    var Filter = new List<KeyValuePair<string, dynamic>>();
                    string value = Utilities.findString(dto.dataField, field.fieldID);
                    Filter.Add(new KeyValuePair<string, dynamic>(field.fieldID, value));
                    if((await _formdata.getFormData(dto.schemaID, Filter)).Count > 0) {
                        return response.failed("Contraint violation on field "+field.fieldName+" which already has a value of '"+value+"'");
                    }                    
                }
            }
            if (filtrees?.Count > 0) {
                foreach (SchemaField field in filtrees) {
                    string value = Utilities.findString(dto.dataField, field.fieldID);
                    var g = new FilterEntities(field.fieldName, value, field.fieldID, dto.schemaID);
                    await _filterEnt.createOnce(g);
                }
            }
            if (await _formdata.createFormData(entity))
                return response.success("Successful Entry. Data saved");
            return response.failed("Data failed to save");
        }

        public Task<RawResponse> closeProject(ProjectDTO dto) {
            throw new NotImplementedException();
        }

        public async Task<RawResponse> createProject(ProjectDTO dto) {
            ResponseFormat response = new ResponseFormat();
            var entity = new Project(dto);
            if (await _project.createProject(entity))
                return response.success("Created Successfully", new { project = entity });
            return response.failed("Could not create Project");
        }

        public async Task<RawResponse> createSchema(SchemaDTO dto) {
            ResponseFormat response = new ResponseFormat();
            var entity = new Schema(dto);
            if (!string.IsNullOrEmpty(dto.id)){
                var schemaEnt = await _schema.getSchemaById(dto.schemaID);
                if (schemaEnt != null && schemaEnt.Count > 0)
                    schemaEnt[0].updateSchema(dto);
                await _schema.updateSchema(schemaEnt[0]);
                return response.success("Schema has been updated!");
            }
            if (await _schema.createSchema(entity))
                return response.success("Created Successfully", new { schema = entity });
            return response.failed("Could not create Schema");
        }

        public async Task<RawResponse> getData(DataFilterDTO filter) {
            ResponseFormat response = new ResponseFormat();
            return response.success("Grabbed", new { data = await _formdata.getFormData(filter.schemaID, filter.filterBody) });
        }

        public async Task<RawResponse> getProject() {
            ResponseFormat response = new ResponseFormat();
            return response.success("Grabbed", new { data = await _project.getProject() });
        }

        public async Task<RawResponse> getSchemaByID(string schemaID) {
            ResponseFormat response = new ResponseFormat();
            var schema = await _schema.getSchemaById(schemaID);
            if (schema.Count < 1)
                return response.failed("Schema Was not found");
            var schemaData = schema[0];
            for(int f = 0; f < schemaData.SchemaField.Count; f++) {
                if (string.IsNullOrEmpty(schemaData.SchemaField[f].dataSource))
                    continue;
                schemaData.SchemaField[f].data = await getDropDown(schemaData.SchemaField[f].dataSource);
            }            
            return response.success("Grabbed", new { schema = schemaData });
        }
        public async Task<RawResponse> getFilterSchemaByID(string schemaID) {
            ResponseFormat response = new ResponseFormat();
            var schema = await _schema.getSchemaById(schemaID);
            if (schema.Count < 1)
                return response.failed("Schema Was not found");
            var filterData = await _filterEnt.get(schemaID);
            var schemaData = schema[0];
            List<FieldBehaviour> desirableBehaviours = new List<FieldBehaviour>() { FieldBehaviour.FILTERABLE_DATE_RANGE, FieldBehaviour.FILTERABLE_DROPDOWN, FieldBehaviour.FILTERABLE_INPUT };
            SchemaDTO sdto = new SchemaDTO();
            sdto.id = schemaData.id;
            sdto.projectID = schemaData.projectID;
            sdto.schemaID = schemaData.id;
            sdto.schemaName = schemaData.schemaName;
            sdto.SchemaField = new List<SchemaField>();
            for (int f = 0; f < schemaData.SchemaField.Count; f++) {
                var behaviours = (from sfd in schemaData.SchemaField[f].behavior
                           where desirableBehaviours.Contains(sfd)
                           select sfd);
                if (behaviours != null && behaviours.Count() > 0){
                    var schemaField = schemaData.SchemaField[f];
                    var filtersField = (from ff in filterData where filterData.Any(F => F.fieldID == schemaField.fieldID) select ff.fieldValue);
                    schemaField.data = filtersField?.ToArray();
                    sdto.SchemaField.Add(schemaField);
                }                   
            }
            return response.success("Grabbed", new { schema = sdto });
        }

        private async Task<string[]> getDropDown(string dataSource) {
            if (string.IsNullOrEmpty(dataSource))
                return new string[] { };
            string[] datasourceSplitted = dataSource.Split(".");
            if (datasourceSplitted.Length < 2)
                return new string[] { };
            var data = await _formdata.getFormData(datasourceSplitted[0]);
            List<string> dropDowns = new List<string>();
            if(data.Count > 0) {
                for(int i = 0; i < data.Count; i++) {
                    try {
                        dropDowns.Add(data[i].data[datasourceSplitted[1]].ToString());
                    } catch { }
                }                
            }
            return dropDowns.ToArray();
        }
        public async Task<RawResponse> getSchemaByProject(string projectID) {
            ResponseFormat response = new ResponseFormat();
            return response.success("Grabbed", new { schema = await _schema.getSchemaByProject(projectID) });
        }

        public Task<RawResponse> lockSchema(SchemaDTO dto) {
            throw new NotImplementedException();
        }

        public async Task<RawResponse> deleteData(string id) {
            ResponseFormat response = new ResponseFormat();
            var data = await _formdata.deleteFormData(id);
            return response.success("Grabbed");
        }
    }
}
