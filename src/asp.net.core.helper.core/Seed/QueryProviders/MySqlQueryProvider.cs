namespace asp.net.core.helper.core.Seed.QueryProviders
{
    internal class MySqlQueryProvider : IQueryProvider
    {
        #region IQueryProvider
        public string SeedInfoTableExists => $"SHOW TABLES LIKE '{IQueryProvider.INFO_TABLE}'";

        public string CreateSeedInfoTable => $"CREATE TABLE `{IQueryProvider.INFO_TABLE}` (" +
                                                    "`Id` INT(11) NOT NULL AUTO_INCREMENT," +
                                                    "`Key` TINYTEXT NOT NULL," +
                                                    "`Timestamp` DATETIME NOT NULL," +
                                                    "PRIMARY KEY(`Id`) USING BTREE" +
                                                ")";
        public string SeedInfoTableContainsKey => $"SELECT COUNT(*) FROM {IQueryProvider.INFO_TABLE} WHERE `Key` = '{{0}}';";

        public string SeedInfoAddKeyEntry => $"INSERT INTO `{IQueryProvider.INFO_TABLE}` (`Key`, `Timestamp`) VALUES('{{0}}', '{{1}}');";
        #endregion
    }
}
