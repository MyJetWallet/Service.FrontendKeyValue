using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.FrontendKeyValue.Settings
{
    public class SettingsModel
    {
        [YamlProperty("FrontendKeyValue.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("FrontendKeyValue.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("FrontendKeyValue.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("FrontendKeyValue.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }

        [YamlProperty("FrontendKeyValue.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }

        [YamlProperty("FrontendKeyValue.CountClientInCache")]
        public int CountClientInCache { get; set; }
    }
}
