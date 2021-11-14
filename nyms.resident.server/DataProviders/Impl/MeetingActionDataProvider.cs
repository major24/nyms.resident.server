using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Model;
using nyms.resident.server.Models;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace nyms.resident.server.DataProviders.Impl
{
    public class MeetingActionDataProvider : IMeetingActionDataProvider
    {
        private readonly string _connectionString;

        public MeetingActionDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<MeetingActionResponse> GetActionResponsesByMeetingIds(int[] meetingIds)
        {
            string sqlMeetingActions = @"SELECT 
                           act.id as id
                          ,act.meeting_id as meetingid
                          ,act.meeting_category_id as meetingcategoryid
                          ,act.meeting_action_item_id as meetingactionitemid
                          ,act.description as description
                          ,act.owner_id as ownerid
                          ,act.start_date as startdate
                          ,act.completion_date as completiondate
                          ,act.priority as priority
                          ,act.completed as completed
						  ,items.name as name
                          ,items.is_adhoc as isadhoc
						  ,usr.forename as forename
                        FROM [dbo].[meeting_actions] act
						INNER JOIN [dbo].[meeting_action_items] items
						ON act.meeting_action_item_id = items.id
						LEFT JOIN [dbo].[users] usr
						ON act.owner_id = usr.id
                        WHERE meeting_id IN @meetingids
                        ORDER BY completion_date";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("meetingids", meetingIds);
                return conn.QueryAsync<MeetingActionResponse>(sqlMeetingActions, dp).Result;
            }
        }

        public IEnumerable<MeetingActionPendingJobsResponse> GetPendingActions()
        {
            return GetPendingActions(-1); // -1 is to fetch all
        }
        public IEnumerable<MeetingActionPendingJobsResponse> GetPendingActions(int ownerId)
        {
            string whereClause = ownerId == -1 ? " WHERE act.owner_id > @ownerid" : " WHERE act.owner_id = @ownerid";

            var sql = $@"SELECT act.id as id
                        ,act.description as description
                        ,act.owner_id as ownerid
                        ,act.completion_date as completiondate
                        ,act.priority as priority
                        ,itm.name as name
                        ,usr.forename as forename
                        ,cat.name as categoryname
                        ,meet.title as title
                        FROM [dbo].[meeting_actions] act
                        INNER JOIN [dbo].[meeting_action_items] itm
                        ON act.meeting_action_item_id = itm.id
                        INNER JOIN [dbo].[users] usr
                        ON act.owner_id = usr.id
                        INNER JOIN [dbo].[meeting_category] cat
                        ON act.meeting_category_id = cat.id
                        INNER JOIN [dbo].[meetings] meet
                        ON act.meeting_id = meet.id
                        {whereClause}
                        AND act.completed IS NULL
                        ORDER BY act.completion_date ASC";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("ownerid", ownerId);
                return conn.QueryAsync<MeetingActionPendingJobsResponse>(sql, dp).Result;
            }
        }

        public IEnumerable<MeetingActionCompletedResponse> GetCompletedActions(int lastN_Rows = 20)
        {
            var sql = $@"SELECT TOP {lastN_Rows} act.id as id
                        ,act.description as description
                        ,act.owner_id as ownerid
                        ,act.completion_date as completiondate
                        ,act.priority as priority
                        ,itm.name as name
                        ,usr.forename as forename
                        ,cat.name as categoryname
                        ,meet.title as title
                        ,act.completed as completed
                        ,act.completed_date as completeddate
                        ,act.completed_by_Id as completedbyid
				        ,usrcomp.forename as completedbyname
						,cmts.comment_type as commenttype
						,cmts.comments as comment
                        FROM [dbo].[meeting_actions] act
                        INNER JOIN [dbo].[meeting_action_items] itm
                        ON act.meeting_action_item_id = itm.id
                        INNER JOIN [dbo].[users] usr
                        ON act.owner_id = usr.id
                        INNER JOIN [dbo].[meeting_category] cat
                        ON act.meeting_category_id = cat.id
                        INNER JOIN [dbo].[meetings] meet
                        ON act.meeting_id = meet.id
						INNER JOIN [dbo].[meeting_action_comments] cmts
						ON act.id = cmts.meeting_action_id
						INNER JOIN [dbo].[users] usrcomp
						ON act.completed_by_id = usrcomp.id
                        WHERE act.completed IS NOT NULL
						AND cmts.comment_type = 'Owner'
						AND act.audited IS NULL
                        ORDER BY act.completion_date DESC";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                return conn.QueryAsync<MeetingActionCompletedResponse>(sql).Result;
            }
        }

        public MeetingActionUpdateRequest UpdateAction(MeetingActionUpdateRequest meetingActionUpdateRequest)
        {
            string sql = @"UPDATE [dbo].[meeting_actions] SET
                            description = @description
                            ,completion_date = @completiondate
                            ,owner_id = @ownerid
                            ,priority = @priority
                            ,updated_by_id = @updatedbyid
                            ,updated_date = @updateddate
                         WHERE id = @id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("id", meetingActionUpdateRequest.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("description", meetingActionUpdateRequest.Description, DbType.String, ParameterDirection.Input);
            dp.Add("completiondate", meetingActionUpdateRequest.CompletionDate, DbType.String, ParameterDirection.Input);
            dp.Add("ownerid", meetingActionUpdateRequest.OwnerId, DbType.Int32, ParameterDirection.Input);
            dp.Add("priority", meetingActionUpdateRequest.Priority, DbType.String, ParameterDirection.Input);
            dp.Add("updatedbyid", meetingActionUpdateRequest.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("updateddate", meetingActionUpdateRequest.UpdatedDate, DbType.String, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute(sql, dp);
                return meetingActionUpdateRequest;
            }
        }

        public MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteRequest)
        {
            var completedFlag = meetingActionCompleteRequest.Completed.ToString();  //meetingActionCompleteRequest.Completed ?? meetingActionCompleteRequest.Completed;

            string sql = @"UPDATE [dbo].[meeting_actions] SET
                        completed = @completed
                       ,completed_by_id = @completedbyid
                       ,completed_date = @completeddate
                        WHERE id = @id";

            string sqlInsCmts = @"INSERT INTO [dbo].[meeting_action_comments]
                                ([meeting_action_id]
                                ,[comment_type]
                                ,[comments]
                                ,[created_by_id])
                                VALUES(
                                 @meetingactionid
                                ,@commenttype
                                ,@comments
                                ,@createdbyid);";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("id", meetingActionCompleteRequest.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("completed", completedFlag, DbType.String, ParameterDirection.Input);
            dp.Add("completedbyid", meetingActionCompleteRequest.CompletedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("completeddate", meetingActionCompleteRequest.CompletedDate, DbType.String, ParameterDirection.Input);

            DynamicParameters dpCmts = new DynamicParameters();
            dpCmts.Add("meetingactionid", meetingActionCompleteRequest.Id, DbType.Int32, ParameterDirection.Input);
            dpCmts.Add("commenttype", ACTION_COMMENT_TYPE.Owner.ToString(), DbType.String, ParameterDirection.Input);
            dpCmts.Add("comments", meetingActionCompleteRequest.Comment, DbType.String, ParameterDirection.Input);
            dpCmts.Add("createdbyid", meetingActionCompleteRequest.CompletedById, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var updMeetingAction = conn.Execute(sql, dp, commandType: CommandType.Text, transaction: tran);
                    if (!string.IsNullOrEmpty(meetingActionCompleteRequest.Comment))
                    {
                        var updMeetingCmts = conn.Execute(sqlInsCmts, dpCmts, commandType: CommandType.Text, transaction: tran);
                    }
                    tran.Commit();
                }
            }
            return meetingActionCompleteRequest;
        }

        public MeetingActionAuditRequest UpdateActionAudited(MeetingActionAuditRequest meetingActionAuditRequest)
        {

            var auditedFlag = meetingActionAuditRequest.Audited.ToString(); // .Inspected ?? meetingActionAuditRequest.Inspected;

            string sql = @"UPDATE [dbo].[meeting_actions] SET
                        audited = @audited
                       ,audited_by_id = @auditedbyid
                       ,audited_date = @auditeddate
                        WHERE id = @id";

            string sqlInsCmts = @"INSERT INTO [dbo].[meeting_action_comments]
                                ([meeting_action_id]
                                ,[comment_type]
                                ,[comments]
                                ,[created_by_id])
                                VALUES(
                                 @meetingactionid
                                ,@commenttype
                                ,@comments
                                ,@createdbyid);";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("id", meetingActionAuditRequest.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("audited", auditedFlag, DbType.String, ParameterDirection.Input);
            dp.Add("auditedbyid", meetingActionAuditRequest.AuditedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("auditeddate", meetingActionAuditRequest.AuditedDate, DbType.String, ParameterDirection.Input);

            DynamicParameters dpCmts = new DynamicParameters();
            dpCmts.Add("meetingactionid", meetingActionAuditRequest.Id, DbType.Int32, ParameterDirection.Input);
            dpCmts.Add("commenttype", ACTION_COMMENT_TYPE.Auditor.ToString(), DbType.String, ParameterDirection.Input);
            dpCmts.Add("comments", meetingActionAuditRequest.Comment, DbType.String, ParameterDirection.Input);
            dpCmts.Add("createdbyid", meetingActionAuditRequest.AuditedById, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var updMeetingAction = conn.Execute(sql, dp, commandType: CommandType.Text, transaction: tran);
                    var updMeetingCmts = conn.Execute(sqlInsCmts, dpCmts, commandType: CommandType.Text, transaction: tran);
                    tran.Commit();
                }
            }
            return meetingActionAuditRequest;
        }

        public IEnumerable<MeetingActionComment> GetComments(int[] meetingActionIds)
        {
            string sql = @"SELECT [meeting_action_id] as meetingactionid
                            ,[comment_type] as commenttype
                            ,[comments] as comment
                          FROM [dbo].[meeting_action_comments]
                          WHERE meeting_action_id IN @meetingActionIds";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<MeetingActionComment>(sql, new { meetingActionIds }).Result;
            }
        }

        public MeetingActionComment InsertActionComment(MeetingActionComment meetingActionComment)
        {
            string sql = @"INSERT INTO [dbo].[meeting_action_comments]
                                ([meeting_action_id]
                                ,[comment_type]
                                ,[comments]
                                ,[created_by_id])
                                VALUES(
                                 @meetingactionid
                                ,@commenttype
                                ,@comments
                                ,@createdbyid);";

            DynamicParameters dpCmts = new DynamicParameters();
            dpCmts.Add("meetingactionid", meetingActionComment.MeetingActionId, DbType.Int32, ParameterDirection.Input);
            dpCmts.Add("commenttype", meetingActionComment.CommentType, DbType.String, ParameterDirection.Input);
            dpCmts.Add("comments", meetingActionComment.Comment, DbType.String, ParameterDirection.Input);
            dpCmts.Add("createdbyid", meetingActionComment.CreatedById, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var insMeetingCmts = conn.Execute(sql, dpCmts, commandType: CommandType.Text, transaction: tran);
                    tran.Commit();
                }
            }
            return meetingActionComment;
        }
    }
}