using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Service.FrontendKeyValue.Domain.Models.NoSql;
using Service.FrontendKeyValue.Grpc;
using Service.FrontendKeyValue.Grpc.Models;

namespace Service.FrontendKeyValue.Client
{
    public class FrontKeyValueCachedService : IFrontKeyValueService
    {
        private readonly IMyNoSqlServerDataReader<FrontKeyValueNoSql> _reader;
        private readonly IFrontKeyValueService _service;
        private readonly ILogger<FrontKeyValueCachedService> _logger;

        public FrontKeyValueCachedService(IMyNoSqlServerDataReader<FrontKeyValueNoSql> reader, IFrontKeyValueService service, ILogger<FrontKeyValueCachedService> logger)
        {
            _reader = reader;
            _service = service;
            _logger = logger;
        }

        public Task SetKeysAsync(SetFrontKeysRequest request)
        {
            return _service.SetKeysAsync(request);
        }

        public Task DeleteKeysAsync(DeleteFrontKeysRequest request)
        {
            return _service.DeleteKeysAsync(request);
        }

        public async Task<GetKeysResponse> GetKeysAsync(GetFrontKeysRequest request)
        {
            request.ClientId.AddToActivityAsTag("clientId");

            try
            {
                var data = _reader.Get(FrontKeyValueNoSql.GeneratePartitionKey(request.ClientId));
                if (data.Any())
                {
                    data.Count.AddToActivityAsTag("count");

                    return new GetKeysResponse()
                    {
                        KeyValues = data.Select(e => e.KeyValue).ToList()
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Cannot get data from reader by client {clientId}", request.ClientId);
            }

            var response = await _service.GetKeysAsync(request);

            response.KeyValues?.Count.AddToActivityAsTag("count");

            _logger.LogInformation("Load key value for client {clientId} from service", request.ClientId);
            
            return response;
        }
    }
}