using Core.Application.DTOs.Configurations;
using Elasticsearch.Net;
using Infrastructure.Interfaces.ElasticSearch;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence {
    public class ElasticSearch : IElasticSearch {
        private readonly Core.Application.DTOs.Configurations.ElasticSearch _config;
        private protected ElasticClient _client;
        public ElasticSearch(IOptionsMonitor<SystemVariables> config) {
            _config = config.CurrentValue.ElasticSearch;
            _init();
        }
        public ElasticSearch(Core.Application.DTOs.Configurations.ElasticSearch config) {
            _config = config;
            _init();
        }

        private void _init() {
            var nodes = getNodes(_config.nodes);
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            settings = basicAuth(settings);
            settings = apiKey(settings);
            _client = new ElasticClient(settings);
        }

        public async Task<bool> insert<T>(T document) where T : class {
            string indexName = typeof(T).Name.ToLower();
            indexName = string.IsNullOrEmpty(_config.indexPrefix) ? indexName : string.Concat(_config.indexPrefix, "-", indexName);
            var response = await _client.IndexAsync<T>(document, idx => idx.Index(indexName));
            return response?.Shards?.Successful > 0;
        }
        public async Task<bool> insert<T>(List<T> document) where T : class {
            string indexName = typeof(T).Name.ToLower();
            indexName = string.IsNullOrEmpty(_config.indexPrefix) ? indexName : string.Concat(_config.indexPrefix, "-", indexName);
            var response = await _client.IndexManyAsync<T>(document, indexName);
            return (bool)!response?.Errors;
        }

        public async Task<bool> update<T>(object id, dynamic updateDoc) where T : class {
            string indexName = typeof(T).Name.ToLower();
            indexName = string.IsNullOrEmpty(_config.indexPrefix) ? indexName : string.Concat(_config.indexPrefix, "-", indexName);
            var result = await _client.UpdateAsync<object>(id.ToString(), u => u.Index(indexName).Doc(updateDoc));
            return result?.Shards.Successful > 0;
        }

        public async Task<bool> delete<T>(object id) where T : class {
            string indexName = typeof(T).Name.ToLower();
            indexName = string.IsNullOrEmpty(_config.indexPrefix) ? indexName : string.Concat(_config.indexPrefix, "-", indexName);
            var result = await _client.DeleteAsync<T>(id.ToString(), u => u.Index(indexName));
            return result?.Shards.Successful > 0;
        }

        public bool bulkInsert<T>(List<T> documents) where T : class {
            string indexName = typeof(T).Name.ToLower();
            indexName = string.IsNullOrEmpty(_config.indexPrefix) ? indexName : string.Concat(_config.indexPrefix, "-", indexName);
            var bulkAllObservable = _client.BulkAll<T>(documents,
                b => b.RetryDocumentPredicate((bulkResponseItem, document) => { return true; })
                .Index(indexName)
                .BackOffRetries(15)
                .BackOffTime(TimeSpan.FromSeconds(55))
                .MaxDegreeOfParallelism(4)
                .Size(1000)
                .ContinueAfterDroppedDocuments(true));
            var waitHandle = new ManualResetEvent(false);
            ExceptionDispatchInfo exceptionDispatchInfo = null;
            var observer = new BulkAllObserver(
                onNext: response => { },
                onError: exception => {
                    exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
                    waitHandle.Set();
                },
                onCompleted: () => waitHandle.Set());
            bulkAllObservable.Subscribe(observer);
            exceptionDispatchInfo?.Throw();
            waitHandle.WaitOne();
            return true;
        }

        private Uri[] getNodes(string[] uris) {
            List<Uri> uriObj = new List<Uri>();
            foreach (string uri in uris) {
                uriObj.Add(new Uri(uri));
            }
            return uriObj.ToArray();
        }

        private ConnectionSettings basicAuth(ConnectionSettings config) {
            if (!string.IsNullOrEmpty(_config?.BasicAuthentication?.username)) {
                config.BasicAuthentication(_config?.BasicAuthentication?.username, _config?.BasicAuthentication?.password);
                config.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
            }
            return config;
        }
        private ConnectionSettings apiKey(ConnectionSettings config) {
            if (!string.IsNullOrEmpty(_config?.ApiKeyAuthentication?.apiKey)) {
                config.ApiKeyAuthentication(_config?.ApiKeyAuthentication.id, _config?.ApiKeyAuthentication.apiKey);
                config.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
            }
            return config;
        }
    }
}
