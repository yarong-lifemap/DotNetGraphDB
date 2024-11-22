using Newtonsoft.Json;

namespace DotNetGraphDB.Jena.Responses
{
    public class JenaUploadFileResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("quadCount")]
        public int QuadCount { get; set; }
        [JsonProperty("tripleCount")]
        public int TripleCount { get; set; }
    }
}
