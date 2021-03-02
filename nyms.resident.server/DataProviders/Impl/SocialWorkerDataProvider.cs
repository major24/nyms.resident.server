using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;

namespace nyms.resident.server.DataProviders.Impl
{
    public class SocialWorkerDataProvider : ISocialWorkerDataProvider
    {
        private readonly string _connectionString;

        public SocialWorkerDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public SocialWorker GetSocialWorkerByResidentId(int residentId)
        {
            string sql = @"SELECT [id] as id
                          ,[resident_id] as residentid
                          ,[forename] as forename
                          ,[surname] as surname
                          ,[email_address] as emailaddress
                          ,[phone_number] as phonenumber
                      FROM [dbo].[social_workers]
                      WHERE [resident_id] = @residentid";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("residentid", residentId, DbType.Int32, ParameterDirection.Input);
                conn.Open();
                var result = conn.QueryFirstOrDefault<SocialWorker>(sql, dp);
                return result;
            }
        }
    }
}