using Core.Application.Interfaces.Repository.MongoDB;
using Core.Models.Entities;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.MongoDB {
    public class SchemaRepository : Repository<Schema>, ISchemaRepository, Interfaces.MongoDB.IRepository<Schema> {
        public SchemaRepository(IMongoDb db) : base(db) { }
        public async Task<bool> createSchema(Schema schema) {
            return await create(schema);
        }

        public async Task<List<Schema>> getSchema() {
            return (List<Schema>)await getAll();
        }

        public async Task<List<Schema>> getSchemaById(string schemaID) {
            Expression<Func<Schema, bool>> condition = F => F.id == schemaID;
            return (List<Schema>) await getByCondition(condition);
        }

        public async Task<List<Schema>> getSchemaByProject(string projectID) {
            Expression<Func<Schema, bool>> condition = F => F.projectID == projectID;
            return (List<Schema>)await getByCondition(condition);
        }

        public async Task<bool> lockSchema(string id) {
            Expression<Func<Schema, bool>> condition = F => F.id == id;
            return await update(new { locked = true }, condition);
        }

        public async Task<bool> updateSchema(Schema schema) {
            Expression<Func<Schema, bool>> condition = F => F.id == schema.id;
            return await update(new { SchemaField = schema.SchemaField }, condition);
        }
    }
}
