using Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repository.MongoDB {
    public interface IFilterEntitiesRepository {
        Task<bool> createOnce(FilterEntities entity);
        Task<List<FilterEntities>> get(string schemeID);
    }
}
