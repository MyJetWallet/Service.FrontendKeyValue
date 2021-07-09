using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Postgres;
using MyJetWallet.Sdk.Service;

namespace Service.FrontendKeyValue.Postgres
{
    public class MyContext : DbContext
    {
        public const string Schema = "frontend_key_value";
        public const string FrontKeyValueTableName = "key_value";

        public DbSet<FrontKeyValueDbEntity> FrontKeyValue { get; set; }

        public MyContext(DbContextOptions options) : base(options)
        {
        }

        public static ILoggerFactory LoggerFactory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (LoggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(LoggerFactory).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<FrontKeyValueDbEntity>().ToTable(FrontKeyValueTableName);
            modelBuilder.Entity<FrontKeyValueDbEntity>().HasKey(e => new { e.ClientId, e.Key});
            modelBuilder.Entity<FrontKeyValueDbEntity>().Property(e => e.ClientId).HasMaxLength(128);
            modelBuilder.Entity<FrontKeyValueDbEntity>().Property(e => e.Key).HasMaxLength(1024);
            modelBuilder.Entity<FrontKeyValueDbEntity>().HasIndex(e => e.ClientId);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> Upsert(List<FrontKeyValueDbEntity> entities)
        {
            using var activity = MyTelemetry.StartActivity("Upsert FrontKeyValueDbEntity");
            var count = await FrontKeyValue.UpsertRange(entities).On(e => new {e.ClientId, e.Key}).RunAsync();
            entities.Count.AddToActivityAsTag("count-entities");
            count.AddToActivityAsTag("count-updated");
            return count;
        }
    }

    public interface IMyContextFactory
    {
        MyContext Create();
    }

    public class MyContextFactory : IMyContextFactory
    {
        private readonly DbContextOptionsBuilder<MyContext> _dbContextOptionsBuilder;

        public MyContextFactory(DbContextOptionsBuilder<MyContext> dbContextOptionsBuilder)
        {
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
        }

        public MyContext Create()
        {
            return new MyContext(_dbContextOptionsBuilder.Options);
        }
    }
}
