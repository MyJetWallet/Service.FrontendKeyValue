using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.FrontendKeyValue.Domain.Models;

namespace Service.FrontendKeyValue.Grpc.Models
{
    [DataContract]
    public class GetKeysResponse
    {
        [DataMember(Order = 1)]
        public List<FrontKeyValue> KeyValues { get; set; }
    }
}