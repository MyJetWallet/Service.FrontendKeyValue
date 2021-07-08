using System.ServiceModel;
using System.Threading.Tasks;
using Service.FrontendKeyValue.Domain.Models;
using Service.FrontendKeyValue.Grpc.Models;

namespace Service.FrontendKeyValue.Grpc
{
    [ServiceContract]
    public interface IFrontKeyValueService
    {
        [OperationContract]
        Task SetKeysAsync(SetFrontKeysRequest request);

        [OperationContract]
        Task DeleteKeysAsync(DeleteFrontKeysRequest request);

        [OperationContract]
        Task<GetKeysResponse> GetKeysAsync(GetFrontKeysRequest request);
    }
}