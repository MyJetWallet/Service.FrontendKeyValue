using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.FrontendKeyValue.Domain.Models;

namespace Service.FrontendKeyValue.Grpc.Models
{
    [DataContract]
    public class SetFrontKeysRequest
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }

        [DataMember(Order = 2)]
        public List<FrontKeyValue> KeyValues { get; set; }
    }
}