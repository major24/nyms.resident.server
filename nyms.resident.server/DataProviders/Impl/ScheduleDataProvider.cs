using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nyms.resident.server.DataProviders.Impl
{
    public class ScheduleDataProvider : IScheduleDataProvider
    {
        private readonly string _connectionString;

        public ScheduleDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<ResidentScheduleEntity> GetResidentSchedules()
        {
            string sql = @"SELECT
                                r.id as residentid
                                , r.reference_id referenceid
                                , r.forename as forename
                                , r.surname as surname
                                , s.id as scheduleid
                                , s.payment_provider_id as paymentproviderid
                                , s.payment_type_id as paymenttypeid
                                , s.description as description
                                , s.schedule_begin_date as schedulebegindate
                                , s.schedule_end_date as scheduleenddate
                                , s.weekly_fee as weeklyfee
                                , s.updated_date as updateddate
                                , s.active as active
								, la.id as localauthorityid
								, la.name as paymentfromname
                            FROM [dbo].[residents] r 
							INNER JOIN [dbo].local_authorities la
							ON r.local_authority_id = la.id
                            LEFT JOIN [dbo].schedules s
                            ON s.resident_id = r.id
                            WHERE r.active = 'Y'
                            ORDER BY r.forename";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = conn.QueryAsync<ResidentScheduleEntity>(sql).Result;
                return result;
            }
        }

        public IEnumerable<ResidentScheduleEntity> GetResidentSchedules(Guid referenceId)
        {
            string sql = @"SELECT
                                r.id as residentid
                                , r.reference_id referenceid
                                , r.forename as forename
                                , r.surname as surname
                                , s.id as scheduleid
                                , s.payment_provider_id as paymentproviderid
                                , s.payment_type_id as paymenttypeid
                                , s.description as description
                                , s.schedule_begin_date as schedulebegindate
                                , s.schedule_end_date as scheduleenddate
                                , s.weekly_fee as weeklyfee
                                , s.updated_date as updateddate
								, la.id as localauthorityid
								, la.name as paymentfromname
                            FROM [dbo].[residents] r 
                            LEFT JOIN [dbo].schedules s
                            ON s.resident_id = r.id
							LEFT JOIN [dbo].local_authorities la
							ON s.local_authority_id = la.id
                            WHERE r.reference_id = @referenceid
                            AND s.active = 'Y'
                            AND r.active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);
                conn.Open();
                var result = conn.QueryAsync<ResidentScheduleEntity>(sql, dp).Result;
                return result;
            }
        }

        public void UpdateScheduleEndDate(int id, DateTime scheduleEndDate)
        {
            string sql = @"UPDATE [dbo].[schedules] 
                            SET schedule_end_date = @scheduleenddate,
                            updated_date = GETDATE()
                            WHERE id = @id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("scheduleenddate", scheduleEndDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dp.Add("id", id, DbType.Int32, ParameterDirection.Input);

                conn.Open();
                var affRows = conn.Execute(sql, dp);
            }
        }

        public void CreateSchedule(ScheduleEntity schedule)
        {
            string sql = @"INSERT INTO [dbo].[schedules]
                           ([resident_id]
                           ,[local_authority_id]
                           ,[payment_provider_id]
                           ,[payment_type_id]
                           ,[description]
                           ,[schedule_begin_date]
                           ,[schedule_end_date]
                           ,[weekly_fee])
                     VALUES
                           (@residentid
                           ,@localauthorityid
                           ,@paymentproviderid
                           ,@paymenttypeid
                           ,@description
                           ,@schedulebegindate
                           ,@scheduleenddate
                           ,@weeklyfee)";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                conn.Open();
                dp.Add("residentid", schedule.ResidentId, DbType.Int32, ParameterDirection.Input);
                dp.Add("localauthorityid", schedule.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                dp.Add("paymentproviderid", schedule.PaymentProviderId, DbType.String, ParameterDirection.Input, 80);
                dp.Add("paymenttypeid", schedule.PaymentTypeId, DbType.String, ParameterDirection.Input, 80);
                dp.Add("description", schedule.Description.Trim(), DbType.String, ParameterDirection.Input, 200);
                dp.Add("schedulebegindate", schedule.ScheduleBeginDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("scheduleenddate", schedule.ScheduleEndDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("weeklyfee", schedule.WeeklyFee, DbType.Decimal, ParameterDirection.Input, 80);

                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
        }

        public void UpdateSchedule(ScheduleEntity schedule)
        {
            string sql = @"UPDATE [dbo].[schedules] 
                            SET
                            [local_authority_id] = @localauthorityid
                           ,[payment_provider_id] = @paymentproviderid
                           ,[payment_type_id] = @paymenttypeid
                           ,[description] = @description
                           ,[schedule_begin_date] = @schedulebegindate
                           ,[schedule_end_date] = @scheduleenddate
                           ,[weekly_fee] = @weeklyfee
                           ,[updated_date] = GETDATE()
                            WHERE id = @id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                conn.Open();
                dp.Add("id", schedule.Id, DbType.Int32, ParameterDirection.Input);
                dp.Add("localauthorityid", schedule.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                dp.Add("paymentproviderid", schedule.PaymentProviderId, DbType.String, ParameterDirection.Input, 80);
                dp.Add("paymenttypeid", schedule.PaymentTypeId, DbType.String, ParameterDirection.Input, 80);
                dp.Add("description", schedule.Description.Trim(), DbType.String, ParameterDirection.Input, 200);
                dp.Add("schedulebegindate", schedule.ScheduleBeginDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("scheduleenddate", schedule.ScheduleEndDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("weeklyfee", schedule.WeeklyFee, DbType.Decimal, ParameterDirection.Input, 80);

                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
        }

        public void InactivateSchedule(int id)
        {
            string sql = @"DELETE FROM [dbo].[schedules] WHERE id = @id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("id", id, DbType.Int32, ParameterDirection.Input);

                conn.Open();
                var affRows = conn.Execute(sql, dp);
            }
        }

    }
}