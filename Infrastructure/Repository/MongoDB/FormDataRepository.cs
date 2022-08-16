using Core.Application.Interfaces.Repository.MongoDB;
using Core.Models.Entities;
using Infrastructure.Persistence;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class FormDataRepository : Repository<FormData>, IFormDataRepository, Interfaces.MongoDB.IRepository<FormData> {
        public FormDataRepository(IMongoDb db) : base(db) { }
        public async Task<bool> createFormData(FormData data) {
            return await create(data);
        }

        public async Task<bool> deleteFormData(string id) {
            Expression<Func<FormData, bool>> condition = F => F.id == id;
            return await delete(condition);
        }

        public async Task<List<FormData>> getFormData(string schemaID) {
            Expression<Func<FormData, bool>> condition = F => F.schemaID == schemaID;
            return (List<FormData>)await getByCondition(condition);
        }

        public async Task<List<FormData>> getFormData(string schemaID, List<KeyValuePair<string, dynamic>> field) {
            var data = await this.getFormData(schemaID);
            foreach(KeyValuePair<string, dynamic> search in field) {
                data = data.FindAll(G => G.data[search.Key] == search.Value);
            }
            return data;
        }

        public async Task<bool> updateFormData(FormData data) {
            Expression<Func<FormData, bool>> condition = F => F.id == data.id;
            return await update(new { data = data.data }, condition);
        }
    }
}
