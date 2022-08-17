using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repository.Cache {
    public interface ICacheService {
        Task<T> getWithKey<T>(string key);
        Task<bool> addWithKey(string key, string value, int expiry = 600);
        Task<bool> deleteWithKey(string key);
        Task<bool> addWithKey<T>(string key, T value, int expiry = 600);
        Task<string> getWithKey(string key);
        string error { get; }
    }
}
