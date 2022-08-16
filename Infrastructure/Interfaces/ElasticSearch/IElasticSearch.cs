using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.ElasticSearch {
    public interface IElasticSearch {
        bool bulkInsert<T>(List<T> documents) where T : class;
        Task<bool> delete<T>(object id) where T : class;
        Task<bool> insert<T>(List<T> document) where T : class;
        Task<bool> insert<T>(T document) where T : class;
        Task<bool> update<T>(object id, dynamic updateDoc) where T : class;
    }
}