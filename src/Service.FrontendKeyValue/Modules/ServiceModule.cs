﻿using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.FrontendKeyValue.Domain.Models.NoSql;

namespace Service.FrontendKeyValue.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl));

            builder.RegisterMyNoSqlWriter<FrontKeyValueNoSql>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), FrontKeyValueNoSql.TableName, true);

            builder.RegisterMyNoSqlReader<FrontKeyValueNoSql>(noSqlClient, FrontKeyValueNoSql.TableName);
        }
    }
}