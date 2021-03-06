using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyNoSqlServer.Abstractions;
using Service.FrontendKeyValue.Domain.Models;
using Service.FrontendKeyValue.Domain.Models.NoSql;
using Service.FrontendKeyValue.Grpc;
using Service.FrontendKeyValue.Grpc.Models;
using Service.FrontendKeyValue.Postgres;
using Service.FrontendKeyValue.Settings;

namespace Service.FrontendKeyValue.Services
{
    public class FrontKeyValueService : IFrontKeyValueService
    {
        private readonly TimeSpan _cacheLifeTime = TimeSpan.FromDays(1);

        private readonly ILogger<FrontKeyValueService> _logger;
        private readonly IMyNoSqlServerDataWriter<FrontKeyValueNoSql> _writer;
        private readonly IMyContextFactory _contextFactory;
        private readonly INoSqlCleanupJob _cleanupJob;


        public FrontKeyValueService(ILogger<FrontKeyValueService> logger, 
            IMyNoSqlServerDataWriter<FrontKeyValueNoSql> writer,
            IMyContextFactory contextFactory,
            INoSqlCleanupJob cleanupJob)
        {
            _logger = logger;
            _writer = writer;
            _contextFactory = contextFactory;
            _cleanupJob = cleanupJob;
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

            await using var ctx = _contextFactory.Create();
            var databaseList = list.Select(e => FrontKeyValueDbEntity.Create(request.ClientId, e)).ToList();
            await ctx.Upsert(databaseList);

            await UploadClientToCache(ctx, request.ClientId);

            _logger.LogDebug("Key values is updated. ClientId: {clientId}; Count: {count}", request.ClientId, list.Count);
        }

        private async Task<List<FrontKeyValue>> UploadClientToCache(MyContext ctx, string clientId)
        {
            using var activity = MyTelemetry.StartActivity("Upload client to cache");

            clientId.AddToActivityAsTag("clientId");

            var data = await ctx.FrontKeyValue.Where(e => e.ClientId == clientId).AsNoTracking().ToListAsync();

            data.Count.AddToActivityAsTag("count");

            try
            {
                var list = data.Select(e => FrontKeyValueNoSql.Create(clientId, e, _cacheLifeTime)).ToList();

                await _writer.CleanAndBulkInsertAsync(FrontKeyValueNoSql.GeneratePartitionKey(clientId), list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot update nosql cache for client {clientId}", clientId);
                _cleanupJob.ForceCleanup();
            }

            return data.Select(e => e.ToValue()).ToList();
        }

        public async Task DeleteKeysAsync(DeleteFrontKeysRequest request)
        {
            if (request.Keys?.Any() != true)
            {
                return;
            }
            await using var ctx = _contextFactory.Create();

            await DeleteFromDatabase(ctx, request);

            await UploadClientToCache(ctx, request.ClientId);
            
            _logger.LogDebug("Key values is deleted. ClientId: {clientId}; Count: {count}", request.ClientId, request.Keys.Count);
        }

        private async Task DeleteFromDatabase(MyContext ctx, DeleteFrontKeysRequest request)
        {
            using var activity = MyTelemetry.StartActivity("Delete from database");

            await ctx.DeleteKeys(request.ClientId, request.Keys);
        }

        public async Task<GetKeysResponse> GetKeysAsync(GetFrontKeysRequest request)
        {
            await using var ctx = _contextFactory.Create();

            var list = await UploadClientToCache(ctx, request.ClientId);

            if (!list.Any())
            {
                await SetKeysAsync(new SetFrontKeysRequest()
                {
                    ClientId = request.ClientId,
                    KeyValues = new List<FrontKeyValue>()
                    {
                        new FrontKeyValue("default", "")
                    }
                });

                list.Add(new FrontKeyValue("default", ""));
            }

            return new GetKeysResponse()
            {
                KeyValues = list
            };
        }
    }
}
