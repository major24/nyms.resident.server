﻿using Dapper;
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
            // ,[meeting_category_id] as meetingcategoryid
            string sql = @"SELECT [id] as id
                          ,[reference_id] as referenceid
                          ,[title] as title
                          ,[description] as description
                          ,[meeting_date] as meetingdate
                          ,[owner_id] as ownerid
                          ,[status] as status
                          ,[created_by_id] as createdbyid
                          ,[created_date] as createddate
                        FROM [dbo].[meetings]
                        WHERE reference_id = @referenceid";

            string sqlMeetingActions = @"SELECT m.id as id
                          ,[meeting_id] as meetingid
                          ,m.meeting_category_id as meetingcategoryid
                          ,[meeting_action_item_id] as meetingactionitemid
                          ,[owner_id] as ownerid
                          ,[start_date] as startdate
                          ,[completion_date] as completiondate
                          ,[priority] as priority
                          ,[completed] as completed
                          ,[actual_completion_date] as actualcompletiondate
                          ,[status] as status
                          ,[updated_by_id] as updatedbyid
                          ,[updated_date] as updateddate
                          ,a.name as name
                          ,a.description as description
                          ,a.is_adhoc as isadhoc
                        FROM [dbo].[meeting_actions] m
                        INNER JOIN [dbo].[meeting_action_items] a
                        ON m.meeting_action_item_id = a.id
                        WHERE meeting_id = @meetingid
                        AND m.status IS NULL";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("referenceid", referenceId.ToString(), DbType.String, ParameterDirection.Input);
                    var meeting = conn.QuerySingle<Meeting>(sql, dp, commandType: CommandType.Text, transaction: tran);

                    if (meeting != null)
                    {
                        DynamicParameters dp2 = new DynamicParameters();
                        // dp2.Add("status", Constants.MEETING_ACTION_DELETED, DbType.String, ParameterDirection.Input);
                        dp2.Add("meetingid", meeting.Id, DbType.Int32, ParameterDirection.Input);
                        meeting.MeetingActions = conn.QueryAsync<MeetingAction>(sqlMeetingActions, dp2, commandType: CommandType.Text, transaction: tran).Result;
                    }
                    tran.Commit();
                    return meeting;
                }
            }
        }

        public IEnumerable<Meeting> GetMeetings()
        {
            // ,[meeting_category_id] as meetingcategoryid
            string sql = @"SELECT [id] as id
                          ,[reference_id] as referenceid
                          ,[title] as title
                          ,[description] as description
                          ,[meeting_date] as meetingdate
                          ,[owner_id] as ownerid
                          ,[status] as status
                          ,[created_by_id] as createdbyid
                          ,[created_date] as createddate
                        FROM [dbo].[meetings]";
            //todo: add where with status??

            string sqlMeetingActions = @"SELECT m.id as id
                          ,[meeting_id] as meetingid
                          ,m.meeting_category_id as meetingcategoryid
                          ,[meeting_action_item_id] as meetingactionitemid
                          ,[owner_id] as ownerid
                          ,[start_date] as startdate
                          ,[completion_date] as completiondate
                          ,[priority] as priority
                          ,[completed] as completed
                          ,[actual_completion_date] as actualcompletiondate
                          ,[status] as status
                          ,[updated_by_id] as updatedbyid
                          ,[updated_date] as updateddate
                          ,a.name as name
                          ,a.description as description
                          ,a.is_adhoc as adhoc
                        FROM [dbo].[meeting_actions] m
                        INNER JOIN [dbo].[meeting_action_items] a
                        ON m.meeting_action_item_id = a.id
                        WHERE meeting_id = @meetingid";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var meetings = conn.QueryAsync<Meeting>(sql, commandType: CommandType.Text, transaction: tran).Result;

                    DynamicParameters dp = new DynamicParameters();
                    meetings.ForEach(m =>
                    {
                        dp.Add("meetingid", m.Id, DbType.Int32, ParameterDirection.Input);
                        m.MeetingActions = conn.QueryAsync<MeetingAction>(sqlMeetingActions, dp, commandType: CommandType.Text, transaction: tran).Result;
                    });
                    tran.Commit();
                    return meetings;
                }
            }
        }

        public Meeting Insert(Meeting meeting)
        {
            string sql = @"INSERT INTO [dbo].[meetings]
                            ([reference_id]
                            ,[title]
                            ,[description]
                            ,[meeting_date]
                            ,[owner_id]
                            ,[created_by_id])
                            VALUES
                            (@referenceid
                            ,@title
                            ,@description
                            ,@meetingdate
                            ,@ownerid
                            ,@createdbyid);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            string sqlInsActions = @"INSERT INTO [dbo].[meeting_actions]
                            ([meeting_id]
                            ,[meeting_category_id]
                            ,[meeting_action_item_id]
                            ,[owner_id]
                            ,[start_date]
                            ,[completion_date]
                            ,[priority])
                             VALUES
                            (@meetingid
                            ,@meetingcategoryid
                            ,@meetingactionitemid
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

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", meeting.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
            dp.Add("title", meeting.Title, DbType.String, ParameterDirection.Input);
            dp.Add("description", meeting.Description, DbType.String, ParameterDirection.Input);
            dp.Add("meetingdate", meeting.MeetingDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
            dp.Add("ownerid", meeting.OwnerId, DbType.Int32, ParameterDirection.Input);
            dp.Add("createdbyid", meeting.CreatedById, DbType.Int32, ParameterDirection.Input);

            // Find if any NEW ITEM, not exists in ITEMS table? If so insert them first.
            // 3) Insert. [ActionsITEMS] table as. Dynamically added by user.
            //       ID = 0, MeetingActionItemId = -1   (First insert to [Items] table and get the identity id before insert
            var existingActions = meeting.MeetingActions.Where(a => a.Id > 0).ToArray(); //MN RMV  .MeetingActionItemId > 0).ToArray();
            var newActionsAndNewItems = meeting.MeetingActions.Where(a => a.Id == -1).ToArray(); // MN RMV .MeetingActionItemId == -1).ToArray();

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var newMeetingId = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);

                    // Add NEW items not in ITEMS table
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
                        dpNewItem.Add("createdbyid", act.CreatedById, DbType.Int32, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlInsItems, dpNewItem, transaction: tran);

                        act.Id = nextId; // MN  .MeetingActionItemId
                    });

                    // add actions, both brand new and existing items
                    var mergedActions = existingActions.Union(newActionsAndNewItems).ToArray();

                    DynamicParameters dp2 = new DynamicParameters();
                    mergedActions.ForEach(act =>
                    {
                        dp2.Add("meetingid", newMeetingId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("meetingactionitemid", act.Id, DbType.Int32, ParameterDirection.Input); // MN RMV .MeetingActionItemId
                        dp2.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dp2.Add("startdate", act.StartDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dp2.Add("completiondate", act.CompletionDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dp2.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        var affRows2 = conn.Execute(sqlInsActions, dp2, transaction: tran);
                    });
                    tran.Commit();
                }
                return meeting;
            }
        }

        public Meeting Update(Meeting meeting, int[] deletedIds = null)
        {
            string sqlUpdMeeting = @"UPDATE [dbo].[meetings] SET
                             [title] = @title
                            ,[description] = @description
                            ,[meeting_date] = @meetingdate
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                          WHERE reference_id = @referenceid;";

            string sqlUpdActions = @"UPDATE [dbo].[meeting_actions] SET
                             [owner_id] = @ownerid
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
                            ,[owner_id]
                            ,[start_date]
                            ,[completion_date]
                            ,[priority])
                             VALUES
                            (@meetingid
                            ,@meetingcategoryid
                            ,@meetingactionitemid
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

            string sqlDelActions = @"UPDATE [dbo].[meeting_actions] SET
                             [status] = @status
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                          WHERE id = @id;";

            // 1) Update. [Actions] table already has this item.
            //          ID = N, MeetingActionItemId = N
            // 2) Insert. [Actions] table does NOT have this entry. New action, but this action item already in [ITEMS] table.
            //          ID = 0, MeetingActionItemId = N
            // 3) Insert. [Actions] table does NOT have this entry. NOT in [ITEMS] table as well. Dynamically added by user.
            //          ID = 0, MeetingActionItemId = -1   (First insert to [Items] table and get the identity id before insert

            var existingActions = meeting.MeetingActions.Where(a => a.Id > 0 && a.Id > 0).ToArray(); // MN RMV  .MeetingActionItemId
            var newActionsButItemExists = meeting.MeetingActions.Where(a => a.Id == 0 && a.Id > 0).ToArray(); // MN  .MeetingActionItemId
            var newActionsAndNewItems = meeting.MeetingActions.Where(a => a.Id == -1).ToArray(); // MN  .MeetingActionItemId

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", meeting.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
            dp.Add("title", meeting.Title, DbType.String, ParameterDirection.Input);
            dp.Add("description", meeting.Description, DbType.String, ParameterDirection.Input);
            dp.Add("meetingdate", meeting.MeetingDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
            dp.Add("updatedbyid", meeting.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("updateddate", DateTime.Now, DbType.String, ParameterDirection.Input);

            // TODO: NEW action items creted by USER AT MEETING TIME can come in. 
            // Need to insert into act item table, then get the id and insert into meet actions table
            // Same for update as well
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // NEW thought
                    // before update anything, 1st insert NEW ITEMS and find all matching actionItemIds and map
                    // So you can update / insert later...
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
                        dpNewActions.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dpNewActions.Add("startdate", act.StartDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpNewActions.Add("completiondate", act.CompletionDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpNewActions.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        var affRows2 = conn.Execute(sqlInsActions, dpNewActions, transaction: tran);
                    });

                    // var mergedList = newActionsButItemExists.Union(newActionsAndNewItems).ToArray();

                    // Insert new actions, but items exists in items table
                    DynamicParameters dpMoreActions = new DynamicParameters();
                    newActionsButItemExists.ForEach(act =>
                    {
                        dpMoreActions.Add("meetingid", meeting.Id, DbType.Int32, ParameterDirection.Input);
                        dpMoreActions.Add("meetingcategoryid", act.MeetingCategoryId, DbType.Int32, ParameterDirection.Input);
                        dpMoreActions.Add("meetingactionitemid", act.Id, DbType.Int32, ParameterDirection.Input); // MN  .MeetingActionItemId
                        dpMoreActions.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dpMoreActions.Add("startdate", act.StartDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpMoreActions.Add("completiondate", act.CompletionDate, DbType.String, ParameterDirection.Input);
                        dpMoreActions.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlInsActions, dpMoreActions, transaction: tran);
                    });

                    // Update existing items
                    DynamicParameters dpExists = new DynamicParameters();
                    existingActions.ForEach(act =>
                    {
                        dpExists.Add("ownerid", act.OwnerId, DbType.Int32, ParameterDirection.Input);
                        dpExists.Add("startdate", act.StartDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpExists.Add("completiondate", act.CompletionDate?.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                        dpExists.Add("priority", act.Priority, DbType.String, ParameterDirection.Input);
                        dpExists.Add("id", act.Id, DbType.Int32, ParameterDirection.Input);
                        dpExists.Add("updatedbyid", act.UpdatedById, DbType.Int32, ParameterDirection.Input);
                        dpExists.Add("updateddate", DateTime.Now, DbType.String, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlUpdActions, dpExists, transaction: tran);
                    });

                    // Update del actions. Soft delete
                    DynamicParameters dpDel = new DynamicParameters();
                    deletedIds.ForEach(id =>
                    {
                        dpDel.Add("id", id, DbType.Int32, ParameterDirection.Input);
                        dpDel.Add("status", Constants.MEETING_ACTION_DELETED, DbType.String, ParameterDirection.Input);
                        dpDel.Add("updatedbyid", meeting.UpdatedById, DbType.Int32, ParameterDirection.Input);
                        dpDel.Add("updateddate", DateTime.Now, DbType.String, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlDelActions, dpDel, transaction: tran);
                        
                    });
                  
                    tran.Commit();
                }
                return meeting;
            }
        }
    }
}