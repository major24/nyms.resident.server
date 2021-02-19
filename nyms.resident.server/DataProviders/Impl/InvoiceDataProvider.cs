using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Impl
{
    public class InvoiceDataProvider : IInvoiceDataProvider
    {
        private readonly string _connectionString;

        public InvoiceDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        }

        public IEnumerable<SchedulePayment> GetAllSchedulesForInvoiceDate(DateTime billingStart, DateTime billingEnd)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT s.[id] as id
                              ,s.[resident_id] as residentid
                              ,s.[local_authority_id] as localauthorityid
                              ,s.[payment_provider_id] paymentproviderid
                              ,s.[payment_type_id] as paymenttypeid
                              ,s.[description] as description
                              ,s.[schedule_begin_date] as schedulebegindate
                              ,s.[schedule_end_date] as scheduleenddate
                              ,s.[weekly_fee] weeklyfee
	                          ,la.[name] paymentfromname
	                          FROM schedules s
	                          LEFT JOIN local_authorities la
	                          ON s.local_authority_id = la.id
                              WHERE s.schedule_end_date >= @billingstart
                              AND s.schedule_begin_date <= @billingend
	                          AND s.[active] = 'Y'";

                // rpt begin date
                // rpt end date";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("billingstart", billingStart.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                dp.Add("billingend", billingEnd.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                conn.Open();
                var result = conn.QueryAsync<SchedulePayment>(sql, dp).Result;
                return result;
            }
        }

        public Task<IEnumerable<BillingCycle>> GetBillingCycles()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                              ,[local_authority_id] as localauthorityid
                              ,[period_start] as periodstart
                              ,[period_end] as periodend
                              ,[bill_date] as billdate
                               FROM [dbo].[billing_periods]
	                           WHERE [active] = 'Y'";

                conn.Open();
                var result = conn.QueryAsync<BillingCycle>(sql).Result;
                return Task.FromResult(result);
            }
        }

        public Task<IEnumerable<BillingCycle>> GetBillingCycles(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT [id] as id
                              ,[local_authority_id] as localauthorityid
                              ,[period_start] as periodstart
                              ,[period_end] as periodend
                              ,[bill_date] as billdate
                               FROM [dbo].[billing_periods]
	                           WHERE period_start >= @startdate
                               AND period_end <= @enddate"; 

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("startdate", startDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                dp.Add("enddate", endDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                var result = conn.QueryAsync<BillingCycle>(sql, dp).Result;
                return Task.FromResult(result);
            }
        }

        public Task<bool> UpdateValidatedInvoices(IEnumerable<InvoiceValidatedEntity> InvoiceValidatedEntities)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sqlInsert = @"INSERT INTO [dbo].[invoices_validated]
                               ([schedule_id]
                               ,[local_authority_id]
                               ,[billing_cycle_id]
                               ,[resident_id]
                               ,[payment_type_id]
                               ,[amount_due]
                               ,[validated]
                               ,[validated_amount]
                               ,[updated_by_id])
                            VALUES
                                (@scheduleid
                                ,@localauthorityid
                                ,@billingcycleid
                                ,@residentid
                                ,@paymenttypeid
                                ,@amountdue
                                ,@validated
                                ,@validatedamount
                                ,@updatedbyid)";

                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    foreach(var inv in InvoiceValidatedEntities)
                    {
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("scheduleid", inv.ScheduleId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("localauthorityid", inv.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("billingcycleid", inv.BillingCycleId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("residentid", inv.ResidentId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("paymenttypeid", inv.PaymentTypeId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("amountdue", inv.AmountDue, DbType.Decimal, ParameterDirection.Input);
                        dp.Add("validated", inv.Validated, DbType.String, ParameterDirection.Input, 10);
                        dp.Add("validatedamount", inv.ValidatedAmount, DbType.Decimal, ParameterDirection.Input);
                        dp.Add("updatedbyid", inv.UpdatedById, DbType.Int32, ParameterDirection.Input);

                        var affRows = conn.Execute(sqlInsert, dp, transaction: tran);
                    }
                    tran.Commit();
                    Task.FromResult(true);
                }
            }
            return Task.FromResult(true);
        }

        public Task<IEnumerable<InvoiceValidatedEntity>> GetValidatedInvoices(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT ival.id as id
                              ,[schedule_id] as [scheduleid]
                              ,ival.local_authority_id as localauthorityid
                              ,[billing_cycle_id] as billingcycleid
                              ,[resident_id] as residentid
                              ,[payment_type_id] as paymenttypeid
                              ,[amount_due] as amountdue
                              ,[validated] as validated
                              ,[validated_amount] as validatedamount
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
                               FROM [dbo].[invoices_validated] ival
                               INNER JOIN [dbo].[billing_periods] bp
							   ON ival.billing_cycle_id = bp.id
                               WHERE bp.period_start >= @startdate
							   AND bp.period_end <= @enddate";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("startdate", startDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                dp.Add("enddate", endDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                var result = conn.QueryAsync<InvoiceValidatedEntity>(sql, dp).Result;
                return Task.FromResult(result);
            }
        }

        public Task<IEnumerable<InvoiceCommentsEntity>> GetInvoiceComments(DateTime startDate, DateTime endDate)
        {
            string sql = @"SELECT ic.id as id
                              ,ic.local_authority_id as localauthorityid
                              ,[billing_cycle_id] as billingcycleid
                              ,[resident_id] as residentid
                              ,[payment_type_id] as paymenttypeid
                              ,[transaction_amount] as transactionamount
                              ,[comments] as comments
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
                               FROM [dbo].[invoice_comments] ic
                               INNER JOIN [dbo].[billing_periods] bp
							   ON ic.billing_cycle_id = bp.id
                               WHERE bp.period_start >= @startdate
							   AND bp.period_end <= @enddate";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("startdate", startDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                dp.Add("enddate", endDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                var result = conn.QueryAsync<InvoiceCommentsEntity>(sql, dp).Result;
                return Task.FromResult(result);
            }
        }

        public Task<IEnumerable<InvoiceValidatedEntity>> GetValidatedInvoices(int billingCycleId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                              ,[schedule_id] as [scheduleid]
                              ,[local_authority_id] as localauthorityid
                              ,[billing_cycle_id] as billingcycleid
                              ,[resident_id] as residentid
                              ,[payment_type_id] as paymenttypeid
                              ,[amount_due] as amountdue
                              ,[validated] as validated
                              ,[validated_amount] as validatedamount
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
                               FROM [dbo].[invoices_validated]
                               WHERE [billing_cycle_id] = @billingcycleid";

                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("billingcycleid", billingCycleId, DbType.Int32, ParameterDirection.Input);
                var result = conn.QueryAsync<InvoiceValidatedEntity>(sql, dp).Result;
                return Task.FromResult(result);
            }
        }

        public Task<IEnumerable<InvoiceCommentsEntity>> GetInvoiceComments(int billingCycleId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                              ,[local_authority_id] as localauthorityid
                              ,[billing_cycle_id] as billingcycleid
                              ,[resident_id] as residentid
                              ,[payment_type_id] as paymenttypeid
                              ,[transaction_amount] as transactionamount
                              ,[comments] as comments
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
                               FROM [dbo].[invoice_comments]
                               WHERE [billing_cycle_id] = @billingcycleid";

                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("billingcycleid", billingCycleId, DbType.Int32, ParameterDirection.Input);
                var result = conn.QueryAsync<InvoiceCommentsEntity>(sql, dp).Result;
                return Task.FromResult(result);
            }
        }

        public Task<bool> InsertInvoiceComments(InvoiceCommentsEntity invoiceCommentsEntity)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO [dbo].[invoice_comments]
                               ([local_authority_id]
                               ,[billing_cycle_id]
                               ,[resident_id]
                               ,[payment_type_id]
                               ,[transaction_amount]
                               ,[comments]
                               ,[updated_by_id])
                            VALUES
                                (@localauthorityid
                                ,@billingcycleid
                                ,@residentid
                                ,@paymenttypeid
                                ,@transactionamount
                                ,@comments
                                ,@updatedbyid)";

                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("localauthorityid", invoiceCommentsEntity.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                dp.Add("billingcycleid", invoiceCommentsEntity.BillingCycleId, DbType.Int32, ParameterDirection.Input);
                dp.Add("residentid", invoiceCommentsEntity.ResidentId, DbType.Int32, ParameterDirection.Input);
                dp.Add("paymenttypeid", invoiceCommentsEntity.PaymentTypeId, DbType.Int32, ParameterDirection.Input);
                dp.Add("transactionamount", invoiceCommentsEntity.TransactionAmount, DbType.Decimal, ParameterDirection.Input);
                dp.Add("comments", invoiceCommentsEntity.Comments, DbType.String, ParameterDirection.Input);
                dp.Add("updatedbyid", invoiceCommentsEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);

                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
            return Task.FromResult(true);
        }


    }
}
