using System.Net.Mime;
using Refit;
using System.Text;
using Newtonsoft.Json;
using DotNetGraphDB.Sparql;

namespace DotNetGraphDB.Jena
{
    /// <summary>
    /// Wrapper for Apache Jena, built by inspecting requests to the Jena Fuseki server.
    /// Uses Refit for REST API calls
    /// </summary>
    public class JenaDbConnector
    {
        private readonly IJenaApi _jenaApi;

        private const string DB_TYPE_TDB2 = "tdb2";

        public JenaDbConnector(string baseUrl)
        {
            _jenaApi = RestService.For<IJenaApi>(baseUrl);
        }

        /// <summary>
        /// Create a new repository with a given ID and Title
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
        /// Delete a repository from GraphDB using its ID
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

        /*
        /// <summary>
        /// Get list of repositories in the GraphDB
        /// </summary>
        public async Task<IEnumerable<RepositoryInfo>> GetRepositoryList()
        {
            var repos = await _jenaApi.GetRepositoryList();
            return repos.GetResults();
        }

        /// <summary>
        /// Check if a repository with a given ID exists
        /// </summary>
        public async Task<bool> IsRepositoryExists(string repositoryId)
        {
            var repos = await GetRepositoryList();
            return repos.Any(r => r.Id == repositoryId);
        }

        /// <summary>
        /// Upload and import a TTL file to a repository
        /// </summary>
        public async Task ImportFile(string repositoryId, string filePath, bool stopOnError = true)
        {
            var fileName = new FileInfo(filePath).Name;

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var mimeType = MimeTypesMap.GetMimeType(fileName);
                var importSettings = new FileImportSettings() { Name = fileName };
                if (!stopOnError)
                    importSettings.ParserSettings = new ParserSettings { StopOnError = false };
                var json = JsonConvert.SerializeObject(importSettings, Formatting.Indented);

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                    await _jenaApi.UploadFile(
                        repositoryId,
                        new StreamPart(fs, fileName, mimeType),
                        new StreamPart(ms, "blob", MediaTypeNames.Application.Json)
                    );
            }
        }

        /// <summary>
        /// Check the status of file import requests (all)
        /// </summary>
        public async Task<IEnumerable<UploadFileStatus>> GetFileImportStatus(string repositoryId)
        {
            return await _jenaApi.GetImportStatus(repositoryId);
        }
        */

        /// <summary>
        /// Execute a typed SPARQL query and return the results. Uses pages of 1,000.
        /// </summary>
        public async Task<IEnumerable<T>> ExecuteSparqlQuery<T>(string repoId, string query) where T : class, new()
        {
            var request = new SparqlQueryRequest()
            {
                Query = query,
            };

            var reqResult = await _jenaApi.ExecuteSparqlQuery<T>(repoId, request);
            return reqResult.GetResults();
        }
    }
}
