using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyNoSqlServer.DataReader;
using Service.FrontendKeyValue.Domain.Models.NoSql;
using Service.FrontendKeyValue.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.FrontendKeyValue.Client
{
    public static class AutofacHelper
    {
        public static void RegisterFrontendKeyValueClient(this ContainerBuilder builder, MyNoSqlTcpClient client, string grpcServiceUrl)
        {
            var factory = new FrontendKeyValueClientFactory(grpcServiceUrl);

            builder.RegisterMyNoSqlReader<FrontKeyValueNoSql>(client, FrontKeyValueNoSql.TableName);

            builder
                .RegisterType<FrontKeyValueCachedService>()
                .WithParameter("service", factory.GetFrontKeyValueService())
                .As<IFrontKeyValueService>()
                .SingleInstance();
        }

        public static void RegisterFrontendKeyValueClientNoCache(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new FrontendKeyValueClientFactory(grpcServiceUrl);
            
            builder.RegisterInstance(factory.GetFrontKeyValueService()).As<IFrontKeyValueService>().SingleInstance();
        }
    }
}
