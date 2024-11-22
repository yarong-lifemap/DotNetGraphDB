namespace DotNetGraphDB.Base
{
    public abstract class DbConnectorBase
    {
        /// <summary>
        /// Check if a repository with a given name exists
        /// </summary>
        public async Task<bool> IsRepositoryExists(string name)
        {
            var repos = await GetDatasetNames();
            return repos.Any(r => r == name);
        }

        public abstract Task<string[]> GetDatasetNames();
    }
}
