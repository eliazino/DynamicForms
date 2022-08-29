using Core.Application.DTOs.Request;
using Core.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Core.Application.DTOs.Response.ResponseFormat;

namespace Core.Application.Interfaces.UseCases {
    public interface IFormCreatorUseCase {
        Task<RawResponse> createProject(ProjectDTO dto);
        Task<RawResponse> closeProject(ProjectDTO dto);
        Task<RawResponse> getProject();
        Task<RawResponse> createSchema(SchemaDTO dto);        
        Task<RawResponse> lockSchema(SchemaDTO dto);
        Task<RawResponse> getSchemaByID(string schemaID);
        Task<RawResponse> getFilterSchemaByID(string schemaID);
        Task<RawResponse> getSchemaByProject(string projectID);
        Task<RawResponse> addData(FormDataDTO dto);
        Task<RawResponse> getData(DataFilterDTO filter);
        Task<RawResponse> deleteData(string id);
    }
}
