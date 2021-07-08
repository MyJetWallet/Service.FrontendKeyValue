using System.Runtime.Serialization;

namespace Service.FrontendKeyValue.Grpc.Models
{
    [DataContract]
    public class GetFrontKeysRequest
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }
    }
}