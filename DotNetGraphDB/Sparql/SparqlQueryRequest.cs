using Newtonsoft.Json;
using Refit;

namespace DotNetGraphDB.Sparql
{
    /// <summary>
    /// Basic request object for a SPARQL query, which includes the query string
    /// </summary>
    public class SparqlQueryRequest
    {
        [AliasAs("query")]
        [JsonProperty("query")]
        public required string Query { get; set; }
    }
}
