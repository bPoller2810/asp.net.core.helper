namespace asp.net.core.helper.core.Seed.QueryProviders
{
    internal interface IQueryProvider
    {
        #region const
        internal const string INFO_TABLE = "_seed";
        #endregion

        string SeedInfoTableExists { get; }
        string CreateSeedInfoTable { get; }
        string SeedInfoTableContainsKey { get; }
        string SeedInfoAddKeyEntry { get; }

    }
}
