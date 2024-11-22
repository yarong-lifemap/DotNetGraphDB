using DotNetGraphDB.Jena.Requests;
using DotNetGraphDB.Jena.Responses;
using DotNetGraphDB.Sparql;
using Refit;

namespace DotNetGraphDB.Jena
{
    /// <summary>
    /// Refit interface for the GraphDB API
    /// </summary>
    internal interface IJenaApi
    {
        [Get("/$/datasets")]
        Task<IApiResponse<JenaGetDatasetsResponse>> GetDatasets();

        [Post("/$/datasets")]
        Task<HttpResponseMessage> CreateDataset(
            [Body(BodySerializationMethod.UrlEncoded)] JenaCreateDatasetRequest createRequest
        );

        [Delete("/$/datasets/{datasetName}")]
        Task<HttpResponseMessage> DeleteRepository(string datasetName);

        [Headers(
            "Accept: application/x-sparqlstar-results+json, application/sparql-results+json;q=0.9, */*;q=0.8")]
        [Post("/{datasetName}/query")]
        Task<TypedSparqlApiResult<T>> ExecuteSparqlQuery<T>(
            string datasetName,
            [Body(BodySerializationMethod.UrlEncoded)] SparqlQueryRequest queryRequest
        ) where T : class, new();

        [Multipart]
        [Post("/{datasetName}/data")]
        Task<IApiResponse<JenaUploadFileResponse>> UploadFile(
            string datasetName, 
            [AliasAs("file")] StreamPart stream
        );
    }
}