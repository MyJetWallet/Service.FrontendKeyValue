using Autofac;
using Service.FrontendKeyValue.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.FrontendKeyValue.Client
{
    public static class AutofacHelper
    {
        public static void RegisterFrontendKeyValueClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new FrontendKeyValueClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetFrontKeyValueService()).As<IFrontKeyValueService>().SingleInstance();
        }
    }
}
