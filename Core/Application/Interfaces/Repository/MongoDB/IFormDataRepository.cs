using Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repository.MongoDB {
    public interface IFormDataRepository {
        Task<List<FormData>> getFormData(string schemaID);
        Task<List<FormData>> getFormData(string schemaID, List<KeyValuePair<string, dynamic>> field);
        Task<bool> updateFormData(FormData data);
        Task<bool> createFormData(FormData data);
        Task<bool> deleteFormData(FormData data);
    }
}
