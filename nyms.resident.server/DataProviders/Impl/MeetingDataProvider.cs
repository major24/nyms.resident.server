using Dapper;
using Microsoft.Ajax.Utilities;
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
    public class MeetingDataProvider : IMeetingDataProvider
    {
        private readonly string _connectionString;

        public MeetingDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public Meeting GetMeeting(Guid referenceId)
        {
            string sql = @"SELECT [id] as id
                          ,[reference_id] as referenceid
                          ,[title] as title
                          ,[meeting_date] as meetingdate
                          ,[owner_id] as ownerid
                          ,[status] as status
                          ,[created_by_id] as createdbyid
                          ,[created_date] as createddate
                        FROM [dbo].[meetings]
                        WHERE reference_id = @referenceid";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", referenceId.ToString(), DbType.String, ParameterDirection.Input);
                return conn.QuerySingle<Meeting>(sql, dp);
            }
        }

        public IEnumerable<Meeting> GetMeetings(int lastN_Rows = 20)
        {
            string sql = $@"SELECT TOP {lastN_Rows} [id] as id
                          ,[reference_id] as referenceid
                          ,[title] as title
                          ,[meeting_date] as meetingdate
                          ,[owner_id] as ownerid
                          ,[status] as status
                          ,[created_by_id] as createdbyid
                          ,[created_date] as createddate
                        FROM [dbo].[meetings]
                        ORDER BY id DESC";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                return conn.QueryAsync<Meeting>(sql).Result;
            }
        }

        public Meeting Insert(Meeting meeting)
        {
            string sql = @"INSERT INTO [dbo].[meetings]
                            ([reference_id]
                            ,[title]
                            ,[meeting_date]
                            ,[owner_id]
                            ,[created_by_id])
                            VALUES
                            (@referenceid
                            ,@title
                            ,@meetingdate
                            ,@ownerid
                            ,@createdbyid);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            string sqlInsActions = @"INSERT INTO [dbo].[meeting_actions]
                            ([meeting_id]
                            ,[meeting_category_id]
                            ,[meeting_action_item_id]
                            ,[description]
                            ,[owner_id]
                            ,[start_date]
                            ,[completion_date]
                            ,[priority]
                            ,[updated_by_id]
                            ,[updated_date])
                             VALUES
                            (@meetingid
                            ,@meetingcategoryid
                            ,@meetingactionitemid
                            ,@description
                            ,@ownerid
                            ,@startdate
                            ,@completiondate
                            ,@priority
                            ,@updatedbyid
                            ,@updateddate)";

            string sqlSelLastId = "SELECT MAX(id) FROM [dbo].[meeting_action_items]";

            string sqlInsItems = @"INSERT INTO [dbo].[meeting_action_items]
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

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", meeting.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
            dp.Add("title", meeting.Title, DbType.String, ParameterDirection.Input);
            dp.Add("meetingdate", meeting.MeetingDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
            dp.Add("ownerid", meeting.OwnerId, DbType.Int32, ParameterDirection.Input);
            dp.Add("createdbyid", meeting.CreatedById, DbType.Int32, ParameterDirection.Input);

            // Newly added action item. Find if any action item, not exists in ITEMS table? If so insert them first.
            // ID = 0, MeetingActionItemId = 0   (First insert to [Items] table and get the identity id before insert into meetingActions table
            var newActionItems = meeting.MeetingActions.Where(a => a.MeetingActionItemId == 0).DistinctBy(a => a.Name).ToArray();

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var newMeetingId = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);

                    // Add NEW items not in ITEMS table
                    newActionItems.ForEach(act =>
                    {
                        var lastId = conn.QuerySingle<int?>(sqlSelLastId, commandType: CommandType.Text, transaction: tran);
                        int nextId = lastId == null ? 1 : (int)lastId + 1;

                        DynamicParameters dpNewItem = new DynamicParameters();
                        dpNewItem.Add("id", nextId, DbType.Int32, ParameterDirection.Input);
                        dpNewItem.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dpNewItem.Add("name", act.Name, DbType.String, ParameterDirection.Input);
                        dpNewItem.Add("description", act.Description, DbType.String, ParameterDirection.Input);
                        dpNewItem.Add("isadhoc", act.IsAdhoc, DbType.String, ParameterDirection.Input);
                        dpNewItem.Add("createdbyid", act.CreatedById, DbType.Int32, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlInsItems, dpNewItem, transaction: tran);

                        act.MeetingActionItemId = nextId;
                    });

                    // Once new Items added, get the ActionItemId and assign to newly arrived data, which will be zero
                    if (newActionItems.Any())
                    {
                        newActionItems.ForEach(newAct =>
                        {
                            meeting.MeetingActions.ForEach(act =>
                            {
                                if (act.Name == newAct.Name)
                                {
                                    act.MeetingActionItemId = newAct.MeetingActionItemId;
                                }
                            });
                        });
                    }

                    DynamicParameters dp2 = new DynamicParameters();
                    meeting.MeetingActions.OrderBy(a => a.MeetingActionItemId).ForEach(act =>
                    {
                        dp2.Add("meetingid", newMeetingId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("meetingactionitemid", act.MeetingActionItemId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("description", act.Description, DbType.String, ParameterDirection.Input);
                        dp2.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("startdate", act.StartDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dp2.Add("completiondate", act.CompletionDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dp2.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        dp2.Add("updatedbyid", meeting.CreatedById, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("updateddate", DateTime.Now, DbType.String, ParameterDirection.Input);
                        var affRows2 = conn.Execute(sqlInsActions, dp2, transaction: tran);
                    });
                    tran.Commit();
                }
                return meeting;
            }
        }

        public Meeting Update(Meeting meeting)
        {
            string sqlUpdMeeting = @"UPDATE [dbo].[meetings] SET
                             [title] = @title
                            ,[meeting_date] = @meetingdate
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                          WHERE reference_id = @referenceid;";

            string sqlUpdActions = @"UPDATE [dbo].[meeting_actions] SET
                             [description] = @description
                            ,[owner_id] = @ownerid
                            ,[start_date] = @startdate
                            ,[completion_date] = @completiondate
                            ,[priority] = @priority
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                          WHERE id = @id;";

            string sqlInsActions = @"INSERT INTO [dbo].[meeting_actions]
                            ([meeting_id]
                            ,[meeting_category_id]
                            ,[meeting_action_item_id]
                            ,[description]
                            ,[owner_id]
                            ,[start_date]
                            ,[completion_date]
                            ,[priority])
                             VALUES
                            (@meetingid
                            ,@meetingcategoryid
                            ,@meetingactionitemid
                            ,@description
                            ,@ownerid
                            ,@startdate
                            ,@completiondate
                            ,@priority)";

            string sqlSelLastId = "SELECT MAX(id) FROM [dbo].[meeting_action_items]";

            string sqlInsItems = @"INSERT INTO [dbo].[meeting_action_items]
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

            // 1) Update. [Actions] table already has this item.
            //          ID > 0, MeetingActionItemId > 0
            // 2) Insert. [Actions] table does NOT have this entry. New action, but this action item already in [ITEMS] table.
            //          ID = 0, MeetingActionItemId = N
            // 3) Insert. [Actions] table does NOT have this entry. NOT in [ITEMS] table as well. Dynamically added by user.
            //          ID = 0, MeetingActionItemId = 0   (First insert to [Items] table and get the identity id before insert
            //          ID = 0, MeetingActionItemId = -1   (First insert to [Items] table and get the identity id before insert

            var existingActions = meeting.MeetingActions.Where(a => a.Id > 0).ToArray(); // MN RMV  .MeetingActionItemId
            var newActionsButItemExists = meeting.MeetingActions.Where(a => a.Id == 0 && a.MeetingActionItemId > 0).ToArray(); // MN  .MeetingActionItemId
            var newActionsAndNewItems = meeting.MeetingActions.Where(a => a.MeetingActionItemId == 0).ToArray(); // MN  .MeetingActionItemId

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", meeting.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
            dp.Add("title", meeting.Title, DbType.String, ParameterDirection.Input);
            dp.Add("meetingdate", meeting.MeetingDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
            dp.Add("updatedbyid", meeting.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("updateddate", meeting.UpdatedDate, DbType.String, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var updMeeting = conn.Execute(sqlUpdMeeting, dp, commandType: CommandType.Text, transaction: tran);

                    // Insert new ITEMS and actions. (does not exists ITEMS table yet)
                    newActionsAndNewItems.ForEach(act =>
                    {
                        var lastId = conn.QuerySingle<int?>(sqlSelLastId, commandType: CommandType.Text, transaction: tran);
                        int nextId = (int)lastId + 1;

                        DynamicParameters dpNewItem = new DynamicParameters();
                        dpNewItem.Add("id", nextId, DbType.Int32, ParameterDirection.Input);
                        dpNewItem.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dpNewItem.Add("name", act.Name, DbType.String, ParameterDirection.Input);
                        dpNewItem.Add("description", act.Description, DbType.String, ParameterDirection.Input);
                        dpNewItem.Add("isadhoc", act.IsAdhoc, DbType.String, ParameterDirection.Input);
                        dpNewItem.Add("createdbyid", meeting.UpdatedById, DbType.Int32, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlInsItems, dpNewItem, transaction: tran);

                        DynamicParameters dpNewActions = new DynamicParameters();
                        dpNewActions.Add("meetingid", meeting.Id, DbType.Int32, ParameterDirection.Input);
                        dpNewActions.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dpNewActions.Add("meetingactionitemid", nextId, DbType.Int32, ParameterDirection.Input);
                        dpNewActions.Add("description", act.Description, DbType.String, ParameterDirection.Input);
                        dpNewActions.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dpNewActions.Add("startdate", act.StartDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpNewActions.Add("completiondate", act.CompletionDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpNewActions.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        var affRows2 = conn.Execute(sqlInsActions, dpNewActions, transaction: tran);
                    });

                    // Insert new actions, but items exists in items table
                    DynamicParameters dpMoreActions = new DynamicParameters();
                    newActionsButItemExists.ForEach(act =>
                    {
                        dpMoreActions.Add("meetingid", meeting.Id, DbType.Int32, ParameterDirection.Input);
                        dpMoreActions.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dpMoreActions.Add("meetingactionitemid", act.Id, DbType.Int32, ParameterDirection.Input); // MN  .MeetingActionItemId
                        dpMoreActions.Add("description", act.Description, DbType.String, ParameterDirection.Input);
                        dpMoreActions.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dpMoreActions.Add("startdate", act.StartDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpMoreActions.Add("completiondate", act.CompletionDate, DbType.String, ParameterDirection.Input);
                        dpMoreActions.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlInsActions, dpMoreActions, transaction: tran);
                    });

                    // Update existing items
                    DynamicParameters dpExists = new DynamicParameters();
                    existingActions.ForEach(act =>
                    {
                        dpExists.Add("description", act.Description, DbType.String, ParameterDirection.Input);
                        dpExists.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dpExists.Add("startdate", act.StartDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpExists.Add("completiondate", act.CompletionDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpExists.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        dpExists.Add("id", act.Id, DbType.Int32, ParameterDirection.Input);
                        dpExists.Add("updatedbyid", act.UpdatedById, DbType.Int32, ParameterDirection.Input);
                        dpExists.Add("updateddate", DateTime.Now, DbType.String, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlUpdActions, dpExists, transaction: tran);
                    });
                  
                    tran.Commit();
                }
                return meeting;
            }
        }
    }
}