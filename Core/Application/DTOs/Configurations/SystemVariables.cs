using Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Configurations {
    public class SystemVariables {
        public string jwtsecret { get; set; }
        public string dayCodeDictionary { get; set; }
        public string version { get; set; }
        public string appName { get; set; }
        public string environmentName { get; set; }
        public string siteRoot { get; set; }
        public bool debug { get; set; }
        public bool identityExpires { get; set; } = true;
        public int identityExpiryMins { get; set; } = 1440; //1 Day
        public DBConfig MySQL { get; set; }
        public DBConfig SQLite { get; set; }
        public DBConfig MongoDB { get; set; }
        public ElasticSearch ElasticSearch { get; set; }
        public Redis Redis { get; set; }
    }
    public class DBConfig {
        public string server { get; set; }
        public string port { get; set; }
        public string database { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
    
    public class ElasticSearch {
        public BasicAuthentication BasicAuthentication { get; set; }
        public string[] nodes { get; set; }
        public ApiKeyAuthentication ApiKeyAuthentication { get; set; }
        public string indexPrefix { get; set; }
    }
    public class BasicAuthentication {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class ApiKeyAuthentication {
        public string id { get; set; }
        public string apiKey { get; set; }
    }

    public class Redis {
        public string password { get; set; }
        public bool allowAdmin { get; set; }
        public bool ssl { get; set; }
        public int connectTimeout { get; set; }
        public int connectRetry { get; set; }
        public int database { get; set; }
        public List<RedisHost> Hosts { get; set; }
    }
    public class RedisHost {
        public string host { get; set; }
        public int port { get; set; }
    }
}