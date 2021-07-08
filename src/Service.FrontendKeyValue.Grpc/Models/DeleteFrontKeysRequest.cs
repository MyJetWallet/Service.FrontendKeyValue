using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.FrontendKeyValue.Grpc.Models
{
    [DataContract]
    public class DeleteFrontKeysRequest
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }

        [DataMember(Order = 2)]
        public List<string> Keys { get; set; }
    }
}