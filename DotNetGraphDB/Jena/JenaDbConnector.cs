using DotNetGraphDB.Base;
using DotNetGraphDB.Jena.Requests;
using DotNetGraphDB.Sparql;
using Refit;

namespace DotNetGraphDB.Jena
{
    /// <summary>
    /// Wrapper for Apache Jena, built by inspecting requests to the Jena Fuseki server.
    /// Uses Refit for REST API calls
    /// </summary>
    public class JenaDbConnector: DbConnectorBase
    {
        private readonly IJenaApi _jenaApi;

        private const string DB_TYPE_TDB2 = "tdb2";

        public JenaDbConnector(string baseUrl, int timeoutSeconds = 60)
        {
            var settings = new RefitSettings(new NewtonsoftJsonContentSerializer());
            _jenaApi = RestService.For<IJenaApi>(new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            }, settings);
        }

        /// <summary>
        /// Get the list of datasets available on the JENA server
        /// </summary>
        public override async Task<string[]> GetDatasetNames()
        {
            var response = await _jenaApi.GetDatasets();
            if (response.IsSuccessful)
            {
                return response.Content.Datasets
                    .Select(x => x.Name.TrimStart('/'))
                    .ToArray();
            }
            else
            {
                throw new Exception($"Failed to get datasets: {response.Content}");
            }
        }

        /// <summary>
        /// Create a new repository, providing a dataset ID
        /// </summary>
        public async Task CreateRepository(string id)
        {
            var response = await _jenaApi.CreateDataset(new JenaCreateDatasetRequest()
            {
                DbName = id,
                DbType = DB_TYPE_TDB2
            });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create repository: {error}");
            }
        }

        /// <summary>
        /// Delete a repository using its dataset ID
        /// </summary>
        public async Task DeleteRepository(string repositoryId)
        {
            var response = await _jenaApi.DeleteRepository(repositoryId);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to delete repository: {error}");
            }
        }

        /// <summary>
        /// Execute a typed SPARQL query and return the results. Uses pages of 1,000.
        /// </summary>
        public async Task<IEnumerable<T>> ExecuteSparqlQuery<T>(string datasetName, string query) where T : class, new()
        {
            var request = new SparqlQueryRequest()
            {
                Query = query,
            };

            var reqResult = await _jenaApi.ExecuteSparqlQuery<T>(datasetName, request);
            return reqResult.GetResults();
        }

        /// <summary>
        /// Add data to a dataset from a given file
        /// </summary>
        public async Task ImportFile(string datasetName, string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not found", fileName);

            var response = await _jenaApi.UploadFile(
                datasetName, 
                new StreamPart(new FileStream(fileName, FileMode.Open), new FileInfo(fileName).Name, "application/octet-stream")
            );
            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to upload file: {response.Content}");
            }
        }
    }
}
