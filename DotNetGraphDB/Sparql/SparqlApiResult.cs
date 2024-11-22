namespace DotNetGraphDB.Sparql
{
    public class SparqlApiResult
    {
        public SparqlApiResultHead Head { get; set; }
        public SparqlApiResultResults Results { get; set; }

        public class SparqlApiResultHead
        {
            public List<string> Vars { get; set; }
        }

        public class SparqlApiResultResults
        {
            public List<Dictionary<string, SparqlApiResultBinding>> Bindings { get; set; }
        }

        public class SparqlApiResultBinding
        {
            public Uri? DataType { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
