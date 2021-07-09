using Service.FrontendKeyValue.Domain.Models;

namespace Service.FrontendKeyValue.Postgres
{
    public class FrontKeyValueDbEntity: FrontKeyValue
    {
        public string ClientId { get; set; }

        public static FrontKeyValueDbEntity Create(string clientId, FrontKeyValue value)
        {
            return new FrontKeyValueDbEntity()
            {
                ClientId = clientId,
                Key = value.Key,
                Value = value.Value
            };
        }

        public FrontKeyValue ToValue()
        {
            return new FrontKeyValue()
            {
                Key = Key,
                Value = Value
            };
        }
    }
}