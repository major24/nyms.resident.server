using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Impl
{
    public class BillingCycleDataProvider : IBillingCycleDataProvider
    {
        private readonly string _connectionString;

        public BillingCycleDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public Task<IEnumerable<BillingCycle>> GetBillingCycles()
        {
            string sql = @"SELECT [id] as id
                              ,[local_authority_id] as localauthorityid
                              ,[period_start] as periodstart
                              ,[period_end] as periodend
                              ,[bill_date] as billdate
                               FROM [dbo].[billing_periods]";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = conn.QueryAsync<BillingCycle>(sql).Result;
                return Task.FromResult(result);
            }
        }
    }
}