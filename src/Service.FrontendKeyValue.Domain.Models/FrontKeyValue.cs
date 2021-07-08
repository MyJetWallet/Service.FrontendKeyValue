using System.Runtime.Serialization;

namespace Service.FrontendKeyValue.Domain.Models
{
    [DataContract]
    public class FrontKeyValue
    {
        public FrontKeyValue()
        {
        }

        public FrontKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [DataMember(Order = 1)]
        public string Key { get; set; }

        [DataMember(Order = 2)]
        public string Value { get; set; }
    }
}