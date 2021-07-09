using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Grpc.Core.Logging;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service.Tools;
using MyNoSqlServer.Abstractions;
using Service.FrontendKeyValue.Domain.Models.NoSql;

namespace Service.FrontendKeyValue.Services
{
    public class NoSqlCleanupJob: IStartable, IDisposable
    {
        private readonly ILogger<NoSqlCleanupJob> _logger;
        private readonly IMyNoSqlServerDataWriter<FrontKeyValueNoSql> _writer;
        private readonly MyTaskTimer _timer;

        public NoSqlCleanupJob(ILogger<NoSqlCleanupJob> logger, IMyNoSqlServerDataWriter<FrontKeyValueNoSql> writer)
        {
            _logger = logger;
            _writer = writer;
            _timer = new MyTaskTimer(nameof(NoSqlCleanupJob), TimeSpan.FromMinutes(5), _logger, DoTime);
        }

        private async Task DoTime()
        {
            await _writer.CleanAndKeepMaxPartitions(Program.Settings.CountClientInCache);
            _logger.LogInformation("Cleanup NoSql table is success, keep {count} clients", Program.Settings.CountClientInCache);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
