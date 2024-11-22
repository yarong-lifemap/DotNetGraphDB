using Newtonsoft.Json;
using Refit;

namespace DotNetGraphDB.Jena.Responses
{
    public class JenaGetDatasetsResponse
    {
        [JsonProperty("datasets")]
        public JenaGetDatasetsResponseDataset[] Datasets { get; set; }
    }

    public class JenaGetDatasetsResponseDataset
    {
        [JsonProperty("ds.name")]
        public string Name { get; set; }
        [JsonProperty("ds.state")]
        public bool State { get; set; }
    }
}
