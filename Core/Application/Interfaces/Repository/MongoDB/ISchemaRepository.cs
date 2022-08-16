using Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repository.MongoDB {
    public interface ISchemaRepository {
        Task<bool> createSchema(Schema schema);
        Task<List<Schema>> getSchema();
        Task<List<Schema>> getSchemaById(string schemaID);
        Task<List<Schema>> getSchemaByProject(string projectID);
        Task<bool> updateSchema(Schema schema);
        Task<bool> lockSchema(string id);
    }
}
