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
    public class FilterEntitiesRepository : Repository<FilterEntities>, IFilterEntitiesRepository, Interfaces.MongoDB.IRepository<FilterEntities> {
        public FilterEntitiesRepository(IMongoDb db) : base(db) { }
        public async Task<bool> createOnce(FilterEntities entity) {
            if (await isExists(entity))
                return true;
            return await create(entity);
        }
        private async Task<bool> isExists(FilterEntities entities) {
            Expression<Func<FilterEntities, bool>> condition = F => F.fieldID == entities.fieldID && F.fieldValue == entities.fieldValue && F.schemaID == entities.schemaID;
            var t = (List<FilterEntities>)await getByCondition(condition);
            return t.Count > 0;
        }
        public async Task<List<FilterEntities>> get(string schemaID) {
            Expression<Func<FilterEntities, bool>> condition = F => F.schemaID == schemaID;
            var t = (List<FilterEntities>)await getByCondition(condition);
            return t;
        }
    }
}
