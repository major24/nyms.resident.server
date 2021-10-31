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
    public class MeetingCategoryAndActionItemLookupDataProvider : IMeetingCategoryAndActionItemLookupDataProvider
    {
        private readonly string _connectionString;

        public MeetingCategoryAndActionItemLookupDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<MeetingCategory> GetMeetingCategoriesAndActionItems()
        {
            string sql = @"SELECT 
                         id as id
                        ,name as name
                        ,active as active
                        FROM [dbo].[meeting_category]
                        WHERE active = 'Y'";

            string sqlActItems = @"SELECT 
                         id as id
                        ,meeting_category_id as meetingcategoryid
                        ,name as name
                        ,description as description
                        ,is_adhoc as isadhoc
                        FROM [dbo].[meeting_action_items]
                        WHERE meeting_category_id IN @meetingcategoryids 
                        AND active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var meetingCategories = conn.QueryAsync<MeetingCategory>(sql).Result.ToList();
                if (meetingCategories == null) return null;

                int[] meetingCategoryIds = meetingCategories.Select(c => c.Id).ToArray();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("meetingcategoryids", meetingCategoryIds);

                var meetingActionItems = conn.QueryAsync<MeetingActionItem>(sqlActItems, dp).Result;

                meetingCategories.ForEach(c =>
                {
                    c.MeetingActionItems = meetingActionItems.Where(a => a.MeetingCategoryId == c.Id).ToArray();
                });

                return meetingCategories;
            }
        }

        public MeetingCategory InsertCategoryAndActionItems(MeetingCategory meetingCategory)
        {
            string sqlSelLastIdCategory = "SELECT MAX(id) FROM [dbo].[meeting_category]";

            string sqlInsCategory = @"INSERT INTO [dbo].[meeting_category]
                            ([id]
                            ,[name]
                            ,[created_by_id])
                            VALUES
                            (@id
                            ,@name
                            ,@createdbyid);";

            string sqlSelLastIdActItems = "SELECT MAX(id) FROM [dbo].[meeting_action_items]";

            string sqlInsActItems = @"INSERT INTO [dbo].[meeting_action_items]
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
                    int nextCatId = 1;
                    var lastId = conn.QuerySingle<int?>(sqlSelLastIdCategory, commandType: CommandType.Text, transaction: tran);
                    nextCatId = (lastId == null) ? nextCatId : (int)lastId + 1;

                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("id", nextCatId, DbType.Int32, ParameterDirection.Input);
                    dp.Add("name", meetingCategory.Name, DbType.String, ParameterDirection.Input);
                    dp.Add("createdbyid", meetingCategory.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var affRows = conn.Execute(sqlInsCategory, dp, transaction: tran);

                    if (meetingCategory.MeetingActionItems != null && meetingCategory.MeetingActionItems.Any())
                    {
                        var meetingActionItems = meetingCategory.MeetingActionItems.ToList();
                        meetingActionItems.ForEach(meetingActionItem =>
                        {
                            int nextId = 1;
                            lastId = conn.QuerySingle<int?>(sqlSelLastIdActItems, commandType: CommandType.Text, transaction: tran);
                            nextId = (lastId == null) ? nextId : (int)lastId + 1;

                            DynamicParameters dp2 = new DynamicParameters();
                            dp2.Add("id", nextId, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("meetingcategoryid", nextCatId, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("name", meetingActionItem.Name, DbType.String, ParameterDirection.Input);
                            dp2.Add("description", meetingActionItem.Description, DbType.String, ParameterDirection.Input);
                            dp2.Add("isadhoc", meetingActionItem.IsAdhoc, DbType.String, ParameterDirection.Input);
                            dp2.Add("createdbyid", meetingActionItem.CreatedById, DbType.Int32, ParameterDirection.Input);

                            var affRows2 = conn.Execute(sqlInsActItems, dp2, transaction: tran);
                        });
                    }

                    tran.Commit();
                    meetingCategory.Id = nextCatId;
                }
                return meetingCategory;
            }
        }

        public MeetingActionItem UpdateActionItem(MeetingActionItem meetingActionItem)
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
                conn.Execute(sql, dp);
                return meetingActionItem;
            }
        }

        public MeetingActionItem InsertActionItem(MeetingActionItem meetingActionItem)
        {
            string sqlSelLastIdActItems = "SELECT MAX(id) FROM [dbo].[meeting_action_items]";

            string sqlInsActItems = @"INSERT INTO [dbo].[meeting_action_items]
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
                    var lastId = conn.QuerySingle<int?>(sqlSelLastIdActItems, commandType: CommandType.Text, transaction: tran);
                    nextId = (lastId == null) ? nextId : (int)lastId + 1;

                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("id", nextId, DbType.Int32, ParameterDirection.Input);
                    dp.Add("meetingcategoryid", meetingActionItem.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                    dp.Add("name", meetingActionItem.Name, DbType.String, ParameterDirection.Input);
                    dp.Add("description", meetingActionItem.Description, DbType.String, ParameterDirection.Input);
                    dp.Add("isadhoc", meetingActionItem.IsAdhoc, DbType.String, ParameterDirection.Input);
                    dp.Add("createdbyid", meetingActionItem.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var affRows = conn.Execute(sqlInsActItems, dp, transaction: tran);

                    tran.Commit();
                    meetingActionItem.Id = nextId;
                }
                return meetingActionItem;
            }
        }



        /*        public MeetingCategory GetMeetingCategoryAndActionItems(int id)
        {
            string sql = @"SELECT 
                         id as id
                        ,name as name
                        ,active as active
                        FROM [dbo].[meeting_category]
                        WHERE id = @id";

            string sqlActItems = @"SELECT 
                         id as id
                        ,meeting_category_id as meetingcategoryid
                        ,name as name
                        ,description as description
                        ,is_adhoc as isadhoc
                        FROM [dbo].[meeting_action_items]
                        WHERE meeting_category_id = @meetingcategoryid 
                        AND active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("id", id);
                var meetingCategory = conn.QueryFirstOrDefault<MeetingCategory>(sql, dp);
                if (meetingCategory == null) return null;

                DynamicParameters dp2 = new DynamicParameters();
                dp2.Add("meetingcategoryid", id);

                meetingCategory.MeetingActionItems = conn.QueryAsync<MeetingActionItem>(sqlActItems, dp2).Result.ToArray();

                return meetingCategory;
            }
        }*/

        // TODO: Not sure we want to update meeting category now. only update ActionItem 
        /*        public MeetingCategory Update(MeetingCategory meetingCategory)
                {
                    string sqlUpdCategory = @"UPDATE [dbo].[meeting_category] SET
                                name = @name
                                WHERE id = @id";

                    string sqlUpdActItems = @"UPDATE [dbo].[meeting_action_items] SET
                                name = @name
                                ,description = @description
                                ,is_adhoc = @isadhoc
                                WHERE id = @id";

                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("id", meetingCategory.Id, DbType.Int32, ParameterDirection.Input);
                    dp.Add("name", meetingCategory.Name, DbType.String, ParameterDirection.Input);

                    using (IDbConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var tran = conn.BeginTransaction())
                        {
                            conn.Execute(sqlUpdCategory, dp, transaction: tran);

                            if (meetingCategory.MeetingActionItems != null && meetingCategory.MeetingActionItems.Any())
                            {
                                var meetingActionItems = meetingCategory.MeetingActionItems.ToList();
                                meetingActionItems.ForEach(meetingActionItem =>
                                {
                                    DynamicParameters dp2 = new DynamicParameters();
                                    dp2.Add("id", meetingActionItem.Id, DbType.Int32, ParameterDirection.Input);
                                    dp2.Add("name", meetingActionItem.Name, DbType.String, ParameterDirection.Input);
                                    dp2.Add("description", meetingActionItem.Description, DbType.String, ParameterDirection.Input);
                                    dp2.Add("isadhoc", meetingActionItem.IsAdhoc, DbType.String, ParameterDirection.Input);
                                    conn.Execute(sqlUpdActItems, dp2, transaction: tran);
                                });
                            }
                            tran.Commit();
                        }
                    }
                    return meetingCategory;
                }*/
    }
}