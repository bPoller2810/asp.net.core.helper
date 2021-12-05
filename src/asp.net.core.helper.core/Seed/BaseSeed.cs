using asp.net.core.helper.core.Seed.QueryProviders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace asp.net.core.helper.core.Seed
{
    public abstract class BaseSeed<TDbContext>
        where TDbContext : DbContext
    {
        #region private member
        private readonly IQueryProvider _queryProvider;
        #endregion

        #region properties
        protected TDbContext Context { get; init; }
        #endregion

        #region ctor
        protected BaseSeed(TDbContext context)
        {
            Context = context;
            _queryProvider = Context.Database.ProviderName switch
            {
                "Pomelo.EntityFrameworkCore.MySql" => new MySqlQueryProvider(),
                //TODO: mssql

                _ => throw new NotSupportedException($"Sorry we currently do not support >{Context.Database.ProviderName}< please visit https://github.com/bPoller2810/asp.net.core.helper/issues and raise a issue with the exact name and version of your provider, we might get it integrated soon"),
            };
        }
        #endregion

        #region seeding
        public void Seed()
        {
            var con = Context.Database.GetDbConnection();
            if (!SeedInfoTableExists(con))
            {//ensure the table is created
                CreateSeedInfoTable(con);
            }

            if (SeedInfoTableContainsSeed(con))
            {//this seed has been already performed
                return;
            }

            var success = PerformSeed();
            if (success)
            {
                Context.SaveChanges();
                AddSeedInfo(con);
            }
        }
        #endregion

        #region abstract
        protected abstract string Key { get; }
        protected abstract int Order { get; }
        protected abstract bool PerformSeed();
        #endregion

        #region sql helper methods
        private bool SeedInfoTableExists(DbConnection con)
        {
            try
            {
                var cmd = GetCommandAndOpenConnection(con, _queryProvider.SeedInfoTableExists);

                var data = cmd.ExecuteReader();
                return data.Read();
            }
            finally
            {
                con.Close();
            }
        }
        private void CreateSeedInfoTable(DbConnection con)
        {
            try
            {
                var cmd = GetCommandAndOpenConnection(con, _queryProvider.CreateSeedInfoTable);

                var data = cmd.ExecuteScalar();
            }
            finally
            {
                con.Close();
            }
        }
        private bool SeedInfoTableContainsSeed(DbConnection con)
        {
            try
            {
                var cmd = GetCommandAndOpenConnection(con, string.Format(_queryProvider.SeedInfoTableContainsKey, Key));

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var count = reader.GetInt32(0);
                    return count > 0;
                }
            }
            finally
            {
                con.Close();
            }
            return false;
        }
        private void AddSeedInfo(DbConnection con)
        {
            try
            {
                var cmd = GetCommandAndOpenConnection(con, string.Format(_queryProvider.SeedInfoAddKeyEntry, Key, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));

                _ = cmd.ExecuteScalar();
            }
            finally
            {
                con.Close();
            }
        }
        private static DbCommand GetCommandAndOpenConnection(DbConnection con, string seedInfoTableExists)
        {
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = seedInfoTableExists;
            return cmd;
        }
        #endregion

    }
}
