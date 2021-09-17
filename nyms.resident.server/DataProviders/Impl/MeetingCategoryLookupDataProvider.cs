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
    public class MeetingCategoryLookupDataProvider : IMeetingCategoryLookupDataProvider
    {
        private readonly string _connectionString;

        public MeetingCategoryLookupDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<MeetingCategory> GetMeetingCategories()
        {
            string sql = @"SELECT 
                         id as id
                        ,name as name
                        ,description as description
                        ,active as active
                        FROM [dbo].[meeting_category]
                        WHERE active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<MeetingCategory>(sql).Result;
            }
        }

        public MeetingCategory Insert(MeetingCategory meetingCategory)
        {
            string sqlSelLastId = "SELECT MAX(id) FROM [dbo].[meeting_category]";

            string sql = @"INSERT INTO [dbo].[meeting_category]
                            ([id]
                            ,[name]
                            ,[description]
                            ,[created_by_id])
                            VALUES
                            (@id
                            ,@name
                            ,@description
                            ,@createdbyid);";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    int nextId = 1;
                    var lastId = conn.QuerySingle<int?>(sqlSelLastId, commandType: CommandType.Text, transaction: tran);
                    nextId = (lastId == null) ? nextId : (int)lastId + 1;

                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("id", nextId, DbType.Int32, ParameterDirection.Input);
                    dp.Add("name", meetingCategory.Name, DbType.String, ParameterDirection.Input);
                    dp.Add("description", meetingCategory.Description, DbType.String, ParameterDirection.Input);
                    dp.Add("createdbyid", meetingCategory.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var affRows = conn.Execute(sql, dp, transaction: tran);
                    tran.Commit();

                    meetingCategory.Id = nextId;
                }
                return meetingCategory;
            }
        }

        public MeetingCategory Update(MeetingCategory meetingCategory)
        {
            string sql = @"UPDATE [dbo].[meeting_category] SET
                        name = @name
                       ,description = @description
                        WHERE id = @id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("id", meetingCategory.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("name", meetingCategory.Name, DbType.String, ParameterDirection.Input);
            dp.Add("description", meetingCategory.Description, DbType.String, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute(sql, dp);
            }
            return meetingCategory;
        }
    }
}