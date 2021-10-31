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
    public class MeetingActionItemLookupDataProvider : IMeetingActionItemLookupDataProvider
    {
        private readonly string _connectionString;

/*        public MeetingActionItemLookupDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }*/

/*        public IEnumerable<MeetingActionItem> GetMeetingActionItems()
        {
            string sql = @"SELECT 
                         id as id
                        ,meeting_category_id as meetingcategoryid
                        ,name as name
                        ,description as description
                        ,is_adhoc as isadhoc
                        ,active as active
                        FROM [dbo].[meeting_action_items]
                        WHERE active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<MeetingActionItem>(sql).Result;
            }
        }*/

/*        public MeetingActionItem Insert(MeetingActionItem meetingActionItem)
        {
            string sqlSelLastId = "SELECT MAX(id) FROM [dbo].[meeting_action_items]";

            string sql = @"INSERT INTO [dbo].[meeting_action_items]
                            ([id]
                            ,[meeting_category_id]
                            ,[name]
                            ,[description]
                            ,[is_adhoc]
                            ,[created_by_id])
                            VALUES
                            (@id
                            ,@meetingcategoryid
                            ,@name
                            ,@description
                            ,@isadhoc
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
                    dp.Add("meetingcategoryid", meetingActionItem.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                    dp.Add("name", meetingActionItem.Name, DbType.String, ParameterDirection.Input);
                    dp.Add("description", meetingActionItem.Description, DbType.String, ParameterDirection.Input);
                    dp.Add("isadhoc", meetingActionItem.IsAdhoc, DbType.String, ParameterDirection.Input);
                    dp.Add("createdbyid", meetingActionItem.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var affRows = conn.Execute(sql, dp, transaction: tran);
                    tran.Commit();

                    meetingActionItem.Id = nextId;
                }
                return meetingActionItem;
            }
        }

        public MeetingActionItem Update(MeetingActionItem meetingActionItem)
        {
            string sql = @"UPDATE [dbo].[meeting_action_items] SET
                        name = @name
                        ,description = @description
                        ,is_adhoc = @isadhoc
                        WHERE id = @id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("id", meetingActionItem.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("name", meetingActionItem.Name, DbType.String, ParameterDirection.Input);
            dp.Add("description", meetingActionItem.Description, DbType.String, ParameterDirection.Input);
            dp.Add("isadhoc", meetingActionItem.IsAdhoc, DbType.String, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute(sql, dp);
            }
            return meetingActionItem;
        }*/
    }
}