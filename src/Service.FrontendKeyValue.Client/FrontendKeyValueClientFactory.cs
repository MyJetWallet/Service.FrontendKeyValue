using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.FrontendKeyValue.Grpc;

namespace Service.FrontendKeyValue.Client
{
    [UsedImplicitly]
    public class FrontendKeyValueClientFactory: MyGrpcClientFactory
    {
        public FrontendKeyValueClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IFrontKeyValueService GetFrontKeyValueService() => CreateGrpcService<IFrontKeyValueService>();
    }
}
