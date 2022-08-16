using Core.Application.Interfaces.Repository.MongoDB;
using Core.Models.Entities;
using Infrastructure.Persistence;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class FormDataRepository : Repository<FormData>, IFormDataRepository, Interfaces.MongoDB.IRepository<FormData> {
        public FormDataRepository(IMongoDb db) : base(db) { }
        public Task<bool> createFormData(FormData data) {
            throw new NotImplementedException();
        }

        public Task<bool> deleteFormData(FormData data) {
            throw new NotImplementedException();
        }

        public Task<List<FormData>> getFormData(string schemaID) {
            throw new NotImplementedException();
        }

        public Task<List<FormData>> getFormData(string schemaID, List<KeyValuePair<string, dynamic>> field) {
            throw new NotImplementedException();
        }

        public Task<bool> updateFormData(FormData data) {
            throw new NotImplementedException();
        }
    }
}
