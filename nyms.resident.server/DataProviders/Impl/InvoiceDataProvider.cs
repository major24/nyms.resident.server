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

        public IEnumerable<Schedule> GetAllSchedulesForInvoiceDate(DateTime billingStart, DateTime billingEnd)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT s.[id] as id
                              ,s.[resident_id] as residentid
                              ,s.[local_authority_id] as localauthorityid
                              ,s.[payment_from] paymentfrom
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
                var result = conn.QueryAsync<Schedule>(sql, dp).Result;
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

        // public Task<bool> UpdateInvoicesApproved(IEnumerable<InvoiceResident> invoices)
        public Task<bool> UpdateValidatedInvoices(IEnumerable<ValidatedInvoiceEntity> invoices)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sqlInsert = @"INSERT INTO [dbo].[validated_invoices]
                               ([local_authority_id]
                               ,[billing_cycle_id]
                               ,[resident_id]
                               ,[payment_type_id]
                               ,[amount_due]
                               ,[validated]
                               ,[transaction_amount]
                               ,[comments]
                               ,[updated_by_id])
                            VALUES
                                (@localauthorityid
                                ,@billingcycleid
                                ,@residentid
                                ,@paymenttypeid
                                ,@amountdue
                                ,@validated
                                ,@transactionamount
                                ,@comments
                                ,@updatedbyid)";

                string sqlUpdate = @"UPDATE [dbo].[validated_invoices]
                            SET [validated] = @validated
                               ,[transaction_amount] = @transactionamount
                               ,[comments] = @comments
                               ,[updated_by_id] = @updatedbyid
                               ,[updated_date] = GETDATE()
                            WHERE id = @id";

                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    foreach(var inv in invoices)
                    {
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("localauthorityid", inv.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("billingcycleid", inv.BillingCycleId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("residentid", inv.ResidentId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("paymenttypeid", inv.PaymentTypeId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("amountdue", inv.AmountDue, DbType.Decimal, ParameterDirection.Input);
                        dp.Add("validated", inv.Validated, DbType.String, ParameterDirection.Input, 10);
                        dp.Add("transactionamount", inv.TransactionAmount, DbType.Decimal, ParameterDirection.Input);
                        dp.Add("comments", inv.Comments, DbType.String, ParameterDirection.Input, 6000);
                        dp.Add("updatedbyid", inv.UpdatedById, DbType.Int32, ParameterDirection.Input);
                        dp.Add("updatedbyid", inv.UpdatedById, DbType.Int32, ParameterDirection.Input);

                        if (inv.Id == 0)
                        {
                            var affRows = conn.Execute(sqlInsert, dp, transaction: tran);
                        }
                        else
                        {
                            dp.Add("id", inv.Id, DbType.Int32, ParameterDirection.Input);
                            var affRows = conn.Execute(sqlUpdate, dp, transaction: tran);
                        }
                    }
                    tran.Commit();
                    Task.FromResult(true);
                }
            }
            return Task.FromResult(true);
        }

        public Task<IEnumerable<ValidatedInvoiceEntity>> GetValidatedInvoices(int localAuthorityId, int billingCycleId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                              ,[local_authority_id] as localauthorityid
                              ,[billing_cycle_id] as billingcycleid
                              ,[resident_id] as residentid
                              ,[payment_type_id] as paymenttypeid
                              ,[amount_due] as amountdue
                              ,[validated] as validated
                              ,[transaction_amount] as transactionamount
                              ,[comments] as comments
                               FROM [dbo].[validated_invoices]
	                           WHERE [local_authority_id] = @localauthorityid
                               AND [billing_cycle_id] = @billingcycleid";

                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("localauthorityid", localAuthorityId, DbType.Int32, ParameterDirection.Input);
                dp.Add("billingcycleid", billingCycleId, DbType.Int32, ParameterDirection.Input);
                var result = conn.QueryAsync<ValidatedInvoiceEntity>(sql, dp).Result;
                return Task.FromResult(result);
            }
        }

    }
}
