using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace nyms.resident.server.DataProviders.Impl
{
    public class ResidentContactDataProvider : IResidentContactDataProvider
    {
        private readonly string _connectionString;

        public ResidentContactDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<ResidentContact> GetResidentContactsByResidentId(int residentId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                                     ,[contact_type] as contacttype
                                     ,[data] as data
                                      FROM [dbo].[resident_contacts]
                                    WHERE [resident_id] = @residentid
                                    AND [active] = 'Y'";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("residentid", residentId, DbType.Int32, ParameterDirection.Input);
                conn.Open();
                var result = conn.Query<ResidentContact>(sql, dp);
                return result;
            }
        }
    }
}