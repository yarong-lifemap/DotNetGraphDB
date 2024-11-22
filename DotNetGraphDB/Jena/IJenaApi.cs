using DotNetGraphDB.Sparql;
using Newtonsoft.Json;
using Refit;

namespace DotNetGraphDB.Jena
{
    /// <summary>
    /// Refit interface for the GraphDB API
    /// </summary>
    internal interface IJenaApi
    {
        [Post("/$/datasets")]
        Task<HttpResponseMessage> CreateDataset(
            [Body(BodySerializationMethod.UrlEncoded)] JenaCreateDatasetRequest createRequest
        );

        [Delete("/$/datasets/{datasetId}")]
        Task<HttpResponseMessage> DeleteRepository(string datasetId);

        [Headers(
            "Accept: application/x-sparqlstar-results+json, application/sparql-results+json;q=0.9, */*;q=0.8")]
        [Post("/{datasetId}/query")]
        Task<TypedSparqlApiResult<T>> ExecuteSparqlQuery<T>(
            string datasetId,
            [Body(BodySerializationMethod.UrlEncoded)] SparqlQueryRequest queryRequest
        ) where T : class, new();
    }

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