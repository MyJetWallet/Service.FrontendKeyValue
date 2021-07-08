using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.FrontendKeyValue.Domain.Models;
using Service.FrontendKeyValue.Domain.Models.NoSql;
using Service.FrontendKeyValue.Grpc;
using Service.FrontendKeyValue.Grpc.Models;
using Service.FrontendKeyValue.Settings;

namespace Service.FrontendKeyValue.Services
{
    public class FrontKeyValueService : IFrontKeyValueService
    {
        private readonly ILogger<FrontKeyValueService> _logger;
        private readonly IMyNoSqlServerDataWriter<FrontKeyValueNoSql> _writer;
        private readonly IMyNoSqlServerDataReader<FrontKeyValueNoSql> _reader;

        public FrontKeyValueService(ILogger<FrontKeyValueService> logger, 
            IMyNoSqlServerDataWriter<FrontKeyValueNoSql> writer,
            IMyNoSqlServerDataReader<FrontKeyValueNoSql> reader)
        {
            _logger = logger;
            _writer = writer;
            _reader = reader;
        }

        public async Task SetKeysAsync(SetFrontKeysRequest request)
        {
            if (string.IsNullOrEmpty(request.ClientId))
            {
                _logger.LogWarning("Cannot set key-value without client id");
                return;
            }

            var list = request.KeyValues ?? new List<FrontKeyValue>();
            if (!list.Any())
            {
                return;
            }

            var entities = list.Select(e => FrontKeyValueNoSql.Create(request.ClientId, e)).ToList();

            await _writer.BulkInsertOrReplaceAsync(entities);

            _logger.LogDebug("Key values is updated. ClientId: {clientId}; Count: {count}", request.ClientId, list.Count);
        }

        public async Task DeleteKeysAsync(DeleteFrontKeysRequest request)
        {
            if (request.Keys?.Any() != true)
            {
                return;
            }

            var taskList = request.Keys
                .Select(e => _writer.DeleteAsync(FrontKeyValueNoSql.GeneratePartitionKey(request.ClientId), FrontKeyValueNoSql.GenerateRowKey(e)).AsTask())
                .ToList();

            await Task.WhenAll(taskList);

            _logger.LogDebug("Key values is deleted. ClientId: {clientId}; Count: {count}", request.ClientId, taskList.Count);
        }

        public Task<GetKeysResponse> GetKeysAsync(GetFrontKeysRequest request)
        {
            var values = _reader.Get(FrontKeyValueNoSql.GeneratePartitionKey(request.ClientId));

            var response = new GetKeysResponse
            {
                KeyValues = values.Select(e => e.KeyValue).ToList()
            };

            return Task.FromResult(response);
        }
    }
}
