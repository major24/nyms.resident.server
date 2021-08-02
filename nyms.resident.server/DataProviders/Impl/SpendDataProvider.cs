using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace nyms.resident.server.DataProviders.Impl
{
    public class SpendDataProvider : ISpendDataProvider
    {
        private readonly string _connectionString;

        public SpendDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public SpendRequest InsertSpend(SpendRequest spendRequest)
        {
            string sql = @"INSERT INTO [dbo].[spends]
                            ([budget_id]
                            ,[amount]
                            ,[po_number]
                            ,[tran_type]
                            ,[created_by_id])
                            VALUES
                            (@budgetid
                            ,@amount
                            ,@ponumber
                            ,@trantype
                            ,@createdbyid);
                         SELECT CAST(SCOPE_IDENTITY() as int);";

            string sqlInsComments = @"INSERT INTO [dbo].[spend_comments_statuses]
                           ([spend_id]
                           ,[comments]
                           ,[status]
                           ,[created_by_id])
                            VALUES
                            (@spendid
                           ,@comments
                           ,@status
                           ,@createdbyid)";

            string sqlInsUnqNbr = @"INSERT INTO [dbo].[unq_number] 
                                    VALUES (@createddate);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";

            string sqlDelUnqNbr = @"DELETE FROM [dbo].[unq_number] WHERE id > 0";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    if (spendRequest.TranType == Constants.SPEND_TRAN_TYPE_DEBIT)
                    {
                        // clean unq number table each time req is made
                        var affRowsX0 = conn.Execute(sqlDelUnqNbr, transaction: tran);

                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("createddate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                        var newUnqId = conn.QuerySingle<int>(sqlInsUnqNbr, dp, commandType: CommandType.Text, transaction: tran);

                        // pad unq id to ponumber
                        spendRequest.PoNumber = spendRequest.PoNumber + "-" + DateTime.Now.ToString("MMyy") + "-" + newUnqId;
                    }

                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("budgetid", spendRequest.BudgetId, DbType.Int32, ParameterDirection.Input);
                    dp2.Add("amount", spendRequest.Amount, DbType.Decimal, ParameterDirection.Input);
                    dp2.Add("ponumber", spendRequest.PoNumber, DbType.String, ParameterDirection.Input, 100);
                    dp2.Add("trantype", spendRequest.TranType, DbType.String, ParameterDirection.Input);
                    dp2.Add("createdbyid", spendRequest.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var newSpendId = conn.QuerySingle<int>(sql, dp2, commandType: CommandType.Text, transaction: tran);

                    DynamicParameters dp3 = new DynamicParameters();
                    dp3.Add("spendid", newSpendId, DbType.Int32, ParameterDirection.Input);
                    dp3.Add("comments", spendRequest.SpendComments.Comments, DbType.String, ParameterDirection.Input);
                    dp3.Add("status", spendRequest.SpendComments.Status.ToString(), DbType.String, ParameterDirection.Input);
                    dp3.Add("createdbyid", spendRequest.CreatedById, DbType.Int32, ParameterDirection.Input);
                    var affRowsX2 = conn.Execute(sqlInsComments, dp3, transaction: tran);

                    tran.Commit();
                }
            }
            return spendRequest;
        }

        public bool TransferSpend(TransferSpendRequest transferSpendRequest)
        {
            string sql = @"UPDATE [dbo].[spends] SET
                        budget_id = (select id from budgets where reference_id = @transfertoreferenceid)
                        WHERE id = @spendid";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("transfertoreferenceid", transferSpendRequest.TransferToBudgetReferenceId, DbType.Guid, ParameterDirection.Input, 60);
            dp.Add("spendid", transferSpendRequest.TransferFromSpendId, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute(sql, dp);
                return true;
            }
        }

        public IEnumerable<Spend> GetSpends(int[] budgetIds)
        {
            string sql = @"SELECT 
                        sp.id as id
                        ,sp.budget_id as budgetid
                        ,sp.amount as amount
                        ,sp.po_number as ponumber
                        ,sp.tran_type as trantype
                        ,sp.created_date as createddate
                        FROM [dbo].[spends] sp
                        WHERE sp.budget_id IN @budgetids
                        ORDER BY sp.budget_id, sp.id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("budgetids", budgetIds);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<Spend>(sql, dp).Result;
            }
        }

        public IEnumerable<SpendComments> GetSpendComments(int[] spendIds)
        {
            string sql = @"SELECT spcmt.id as id
                          ,[spend_id] as spendid
                          ,[comments] as comments
                          ,[status] as status
                          ,[created_by_id] as createdbyid
                          ,spcmt.created_date as createddate
	                      ,forename  as createdbyname
                      FROM [dbo].[spend_comments_statuses] as spcmt
                      INNER JOIN [dbo].[users] as u
                      ON spcmt.created_by_id = u.id
                      WHERE spcmt.spend_id IN @spendids
                      ORDER BY spcmt.spend_id, spcmt.id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendids", spendIds);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<SpendComments>(sql, dp).Result;
            }
        }

        public void InsertSpendComment(SpendComments spendComments)
        {
            string sql = @"INSERT INTO [dbo].[spend_comments_statuses]
                           ([spend_id]
                           ,[comments]
                           ,[status]
                           ,[created_by_id])
                            VALUES
                            (@spendid
                           ,@comments
                           ,@status
                           ,@createdbyid)";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendid", spendComments.SpendId, DbType.Int32, ParameterDirection.Input);
            dp.Add("comments", spendComments.Comments, DbType.String, ParameterDirection.Input);
            dp.Add("status", spendComments.Status.ToString(), DbType.String, ParameterDirection.Input);
            dp.Add("createdbyid", spendComments.CreatedById, DbType.Int32, ParameterDirection.Input);
            
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var affRowsX2 = conn.Execute(sql, dp);
            }
        }
    }
}