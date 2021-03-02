using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nyms.resident.server.DataProviders.Impl
{
    public class PaymentTypeDataProvider : IPaymentTypeDataProvider
    {
        private readonly string _connectionString;

        public PaymentTypeDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                                ,[name] as name
                                FROM[dbo].[payment_types]
                                WHERE active = 'Y'";

                conn.Open();
                var result = conn.Query<PaymentType>(sql);
                return result;
            }
        }
    }
}