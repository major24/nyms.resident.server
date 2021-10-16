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

        public IEnumerable<MeetingActionResponse> GetActions()
        {
            string sql = @"SELECT a.id as id
                          ,a.meeting_id as meetingid
                          ,a.meeting_category_id as meetingcategoryid
                          ,a.meeting_action_item_id as meetingactionitemid
                          ,a.owner_id as ownerid
                          ,a.start_date as startdate
                          ,a.completion_date as completiondate
                          ,a.priority as priority
                          ,a.completed as completed
                          ,a.completed_date as completeddate
                          ,a.status as status
                          ,a.updated_by_id as updatedbyid
                          ,a.updated_date as updateddate
                          ,a.inspected as inspected
                          ,a.inspected_by_id as inspectedbyid
                          ,a.inspected_date as inspecteddate
                          ,i.name as name
                          ,i.description as description
                          ,i.is_adhoc as isadhoc
						  ,a.status as action_status
		                  ,usr_owner.forename as ownerforename
                          ,usr_owner.surname as ownersurname
		                  ,usr_coml.forename as completedbyforename
                          ,usr_coml.surname as completedbysurname
                          ,usr_ins.forename as inspectedbyforename
		                  ,usr_ins.surname as inspectedbysurname
                        FROM [dbo].[meeting_actions] a
                        INNER JOIN [dbo].[meeting_action_items] i
                        ON a.meeting_action_item_id = i.id
                        LEFT JOIN [dbo].[users] usr_owner
	                    ON usr_owner.id = a.owner_id
	                    LEFT JOIN [dbo].[users] usr_coml
	                    ON usr_coml.id = a.completed_by_id
		                LEFT JOIN [dbo].[users] usr_ins
	                    ON usr_ins.id = a.inspected_by_id
	                    ORDER BY a.id desc";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<MeetingActionResponse>(sql).Result;
            }
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

        public MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteRequest)
        {
            var completedFlag = meetingActionCompleteRequest.Completed ?? meetingActionCompleteRequest.Completed;

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
            dpCmts.Add("commenttype", meetingActionCompleteRequest.MeetingActionComment.CommentType, DbType.String, ParameterDirection.Input);
            dpCmts.Add("comments", meetingActionCompleteRequest.MeetingActionComment.Comment, DbType.String, ParameterDirection.Input);
            dpCmts.Add("createdbyid", meetingActionCompleteRequest.CompletedById, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var updMeetingAction = conn.Execute(sql, dp, commandType: CommandType.Text, transaction: tran);
                    if (!string.IsNullOrEmpty(meetingActionCompleteRequest.MeetingActionComment.Comment))
                    {
                        var updMeetingCmts = conn.Execute(sqlInsCmts, dpCmts, commandType: CommandType.Text, transaction: tran);
                    }
                    tran.Commit();
                }
            }
            return meetingActionCompleteRequest;
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

        public MeetingActionInspectRequest UpdateActionInspected(MeetingActionInspectRequest meetingActionInspectRequest)
        {

            var inspectedFlag = meetingActionInspectRequest.Inspected ?? meetingActionInspectRequest.Inspected;

            string sql = @"UPDATE [dbo].[meeting_actions] SET
                        inspected = @inspected
                       ,inspected_by_id = @inspectedbyid
                       ,inspected_date = @inspecteddate
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
            dp.Add("id", meetingActionInspectRequest.Id, DbType.Int32, ParameterDirection.Input);
            dp.Add("inspected", inspectedFlag, DbType.String, ParameterDirection.Input);
            dp.Add("inspectedbyid", meetingActionInspectRequest.InspectedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("inspecteddate", meetingActionInspectRequest.InspectedDate, DbType.String, ParameterDirection.Input);

            DynamicParameters dpCmts = new DynamicParameters();
            dpCmts.Add("meetingactionid", meetingActionInspectRequest.Id, DbType.Int32, ParameterDirection.Input);
            dpCmts.Add("commenttype", meetingActionInspectRequest.MeetingActionComment.CommentType, DbType.String, ParameterDirection.Input);
            dpCmts.Add("comments", meetingActionInspectRequest.MeetingActionComment.Comment, DbType.String, ParameterDirection.Input);
            dpCmts.Add("createdbyid", meetingActionInspectRequest.InspectedById, DbType.Int32, ParameterDirection.Input);

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
            return meetingActionInspectRequest;
        }
    }
}