using Newtonsoft.Json;
using Refit;

namespace DotNetGraphDB.Jena.Requests
{
    internal class JenaCreateDatasetRequest
    {
        [AliasAs("dbName")]
        [JsonProperty("dbName")]
        public string DbName { get; set; }
        [AliasAs("dbType")]
        [JsonProperty("dbType")]
        public string DbType { get; set; }
    }
}