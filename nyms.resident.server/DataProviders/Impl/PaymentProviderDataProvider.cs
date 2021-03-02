using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace nyms.resident.server.DataProviders.Impl
{
    public class PaymentProviderDataProvider : IPaymentProviderDataProvider
    {
        private readonly string _connectionString;

        public PaymentProviderDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<PaymentProvider> GetPaymentProviders()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                            ,[name] as name
                            FROM[dbo].[payment_providers]
                            WHERE active = 'Y'";

                conn.Open();
                var result = conn.Query<PaymentProvider>(sql);
                return result;
            }
        }
    }
}