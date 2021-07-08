using MyNoSqlServer.Abstractions;

namespace Service.FrontendKeyValue.Domain.Models.NoSql
{
    public class FrontKeyValueNoSql: MyNoSqlDbEntity
    {
        public const string TableName = "jetwallet-frontend-key-value";

        public static string GeneratePartitionKey(string clientId) => clientId;
        public static string GenerateRowKey(string key) => key;

        public FrontKeyValue KeyValue { get; set; }

        public static FrontKeyValueNoSql Create(string clientId, FrontKeyValue keyValue)
        {
            return new FrontKeyValueNoSql()
            {
                PartitionKey = GeneratePartitionKey(clientId),
                RowKey = GenerateRowKey(keyValue.Key),
                KeyValue = keyValue
            };
        }
    }
}