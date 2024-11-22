using System.Reflection;

namespace DotNetGraphDB.Sparql
{
    /// <summary>
    /// Represents a typed result of a SPARQL API query.
    /// </summary>
    /// <typeparam name="T">The type of the result object.</typeparam>
    public class TypedSparqlApiResult<T> : SparqlApiResult where T : class, new()
    {
        private static Uri DATA_TYPE_BOOLEAN = new Uri("http://www.w3.org/2001/XMLSchema#boolean");

        public IEnumerable<T> GetResults()
        {
            // Build mappings
            Dictionary<string, PropertyInfo> properties = typeof(T).GetProperties()
                .ToDictionary(
                    x => x.Name,
                    x => x,
                    StringComparer.InvariantCultureIgnoreCase
                );

            // Parse results and use setters to bind object values (currently supports string literals and booleans)
            foreach (var results in Results.Bindings)
            {
                var result = new T();
                foreach (var (key, value) in results)
                {
                    if (properties.TryGetValue(key, out var setter))
                    {
                        if (value == null)
                            continue;

                        if (value?.DataType?.AbsoluteUri == DATA_TYPE_BOOLEAN.AbsoluteUri)
                        {
                            setter.SetValue(result, value.Value == "true");
                        }
                        else
                        {
                            setter.SetValue(result, value.Value);
                        }
                    }
                }
                yield return result;
            }
        }
    }
}
