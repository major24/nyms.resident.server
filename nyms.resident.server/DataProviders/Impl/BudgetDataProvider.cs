using Dapper;
using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace nyms.resident.server.DataProviders.Impl
{
    public class BudgetDataProvider : IBudgetDataProvider
    {
        private readonly string _connectionString;

        public BudgetDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

/*        public IEnumerable<BudgetEntity> GetBudgets()
        {
            string sql = @"SELECT id as id
                            ,reference_id as referenceid
                            ,spend_category_id as spendcategoryid
                            ,care_home_id as carehomeid
                            ,name as name
                            ,date_from as datefrom
                            ,date_to as dateto
                            ,description as description
                            ,po_prefix as poprefix
                            ,status as status
                            ,reason as reason
                            ,created_by_id as createdbyid
                            ,created_date as createddate
                            ,updated_by_id as updatedbyid
                            ,updated_date as updateddate
                            FROM [dbo].[budgets]";
            string sqlAllocations = @"SELECT id as id
                            ,budget_id as budgetid
                            ,amount as amount
                            ,approved as approved
                            ,approved_by_id as approvedbyid
                            ,approved_date as approveddate
                            ,updated_by_id as updatedbyid
                            ,updated_date as updateddate
                            FROM [dbo].[budget_allocations]";

            IEnumerable<BudgetEntity> budgetEntities;
            IEnumerable<BudgetAllocationEntity> budgetAllocationEntities;

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                budgetEntities = conn.QueryAsync<BudgetEntity>(sql).Result;
                budgetAllocationEntities = conn.QueryAsync<BudgetAllocationEntity>(sqlAllocations).Result;
            }

            if (budgetEntities != null && budgetEntities.Any())
            {
                budgetEntities.ForEach(b =>
                {
                    b.BudgetAllocations = budgetAllocationEntities.Where(a => a.BudgetId == b.Id).ToArray();
                });
            }

            return budgetEntities;
        }*/

        public BudgetEntity GetBudget(Guid referenceId)
        {
            string sql = @"SELECT id as id
                            ,reference_id as referenceid
                            ,spend_category_id as spendcategoryid
                            ,care_home_id as carehomeid
                            ,name as name
                            ,date_from as datefrom
                            ,date_to as dateto
                            ,description as description
                            ,po_prefix as poprefix
                            ,status as status
                            ,reason as reason
                            ,created_by_id as createdbyid
                            ,created_date as createddate
                            ,updated_by_id as updatedbyid
                            ,updated_date as updateddate
                            FROM [dbo].[budgets]
                            WHERE reference_id = @referenceid";

            string sqlAllocations = @"SELECT a.id as id
                            ,budget_id as budgetid
                            ,amount as amount
                            ,approved as approved
                            ,approved_by_id as approvedbyid
                            ,approved_date as approveddate
                            ,a.updated_by_id as updatedbyid
                            ,a.updated_date as updateddate
                            FROM [dbo].[budget_allocations] a
							INNER JOIN [dbo].[budgets] b
							ON b.id = a.budget_id
							WHERE b.reference_id = @referenceid
                            ORDER BY a.approved desc";

            BudgetEntity budgetEntity;
            IEnumerable<BudgetAllocationEntity> budgetAllocationEntities;

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);
                budgetEntity = conn.QueryFirstOrDefault<BudgetEntity>(sql, dp);
                budgetAllocationEntities = conn.QueryAsync<BudgetAllocationEntity>(sqlAllocations, dp).Result;
                budgetEntity.BudgetAllocations = budgetAllocationEntities;
            }

            return budgetEntity;
        }

        public BudgetEntity Insert(IEnumerable<BudgetEntity> budgetEntities)
        {
            string sql = @"INSERT INTO [dbo].[budgets]
                            ([spend_category_id]
                            ,[care_home_id]
                            ,[name]
                            ,[date_from]
                            ,[date_to]
                            ,[description]
                            ,[po_prefix]
                            ,[status]
                            ,[reason]
                            ,[created_by_id])
                            VALUES
                            (@spendcategoryid
                            ,@carehomeid
                            ,@name
                            ,@datefrom
                            ,@dateto
                            ,@description
                            ,@poprefix
                            ,@status
                            ,@reason
                            ,@createdbyid);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            string sqlInsAllocations = @"INSERT INTO [dbo].[budget_allocations]
                            ([budget_id]
                            ,[amount]
                            ,[approved]
                            ,[approved_by_id]
                            ,[approved_date]
                            ,[updated_by_id]
                            ,[updated_date])
                            VALUES
                            (@budgetid
                            ,@amount
                            ,@approved
                            ,@approvedbyid
                            ,@approveddate
                            ,@updatedbyid
                            ,@updateddate)";



            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    foreach(var budgetEntity in budgetEntities)
                    {
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("spendcategoryid", budgetEntity.SpendCategoryId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("carehomeid", budgetEntity.CareHomeId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("name", budgetEntity.Name, DbType.String, ParameterDirection.Input, 100);
                        dp.Add("datefrom", budgetEntity.DateFrom.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                        dp.Add("dateto", budgetEntity.DateTo.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                        dp.Add("description", budgetEntity.Description, DbType.String, ParameterDirection.Input, 1000);
                        dp.Add("poprefix", budgetEntity.PoPrefix, DbType.String, ParameterDirection.Input, 50);
                        dp.Add("status", budgetEntity.Status, DbType.String, ParameterDirection.Input, 50);
                        dp.Add("reason", budgetEntity.Reason, DbType.String, ParameterDirection.Input, 3000);
                        dp.Add("createdbyid", budgetEntity.CreatedById, DbType.Int32, ParameterDirection.Input);

                        var newBudgetId = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);

                        if (budgetEntity.BudgetAllocations != null && budgetEntity.BudgetAllocations.Any())
                        {
                            budgetEntity.BudgetAllocations.ForEach(alloc =>
                            {
                                DynamicParameters dp2 = new DynamicParameters();
                                dp2.Add("budgetid", newBudgetId, DbType.Int32, ParameterDirection.Input);
                                dp2.Add("amount", alloc.Amount, DbType.Decimal, ParameterDirection.Input);
                                dp2.Add("approved", alloc.Approved, DbType.String, ParameterDirection.Input, 10);
                                dp2.Add("approvedbyid", alloc.ApprovedById, DbType.Int32, ParameterDirection.Input);
                                dp2.Add("approveddate", alloc.ApprovedDate, DbType.DateTime, ParameterDirection.Input, 60);
                                dp2.Add("updatedbyid", alloc.UpdatedById, DbType.Int32, ParameterDirection.Input);
                                dp2.Add("updateddate", alloc.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);

                                var affRowsX1 = conn.Execute(sqlInsAllocations, dp2, transaction: tran);
                            });
                        }
                    }
                    tran.Commit();
                }
            }
            return budgetEntities.FirstOrDefault();
        }

        public BudgetEntity Update(BudgetEntity budgetEntity)
        {
            string sql = @"UPDATE [dbo].[budgets] SET
                             [name] = @name
                            ,[date_from] = @datefrom
                            ,[date_to] = @dateto
                            ,[description] = @description
                            ,[po_prefix] = @poprefix
                            ,[status] = @status
                            ,[reason] = @reason
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                            WHERE id = @id";

            string sqlUpdAllocations = @"UPDATE [dbo].[budget_allocations] SET
                             [amount] = @amount
                            ,[approved] = @approved
                            ,[approved_by_id] = @approvedbyid
                            ,[approved_date] = @approveddate
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                            WHERE id = @id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("name", budgetEntity.Name, DbType.String, ParameterDirection.Input, 100);
            dp.Add("datefrom", budgetEntity.DateFrom, DbType.Date, ParameterDirection.Input, 60);
            dp.Add("dateto", budgetEntity.DateTo, DbType.Date, ParameterDirection.Input, 60);
            dp.Add("description", budgetEntity.Description, DbType.String, ParameterDirection.Input, 1000);
            dp.Add("poprefix", budgetEntity.PoPrefix, DbType.String, ParameterDirection.Input, 50);
            dp.Add("status", budgetEntity.Status, DbType.String, ParameterDirection.Input, 50);
            dp.Add("reason", budgetEntity.Reason, DbType.String, ParameterDirection.Input, 3000);
            dp.Add("updatedbyid", budgetEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("updateddate", budgetEntity.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);
            dp.Add("id", budgetEntity.Id, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var affRowsX1 = conn.Execute(sql, dp, transaction: tran);

                    if (budgetEntity.BudgetAllocations != null && budgetEntity.BudgetAllocations.Any())
                    {
                        budgetEntity.BudgetAllocations.ForEach(alloc =>
                        {
                            DynamicParameters dp2 = new DynamicParameters();
                            dp2.Add("amount", alloc.Amount, DbType.Decimal, ParameterDirection.Input);
                            dp2.Add("approved", alloc.Approved, DbType.String, ParameterDirection.Input, 10);
                            dp2.Add("approvedbyid", alloc.ApprovedById, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("approveddate", alloc.ApprovedDate, DbType.DateTime, ParameterDirection.Input, 60);
                            dp2.Add("updatedbyid", alloc.UpdatedById, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("updateddate", alloc.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);

                            dp2.Add("id", alloc.Id, DbType.Int32, ParameterDirection.Input);
                            var affRowsX2 = conn.Execute(sqlUpdAllocations, dp2, transaction: tran);
                        });
                    }
                    tran.Commit();
                }
            }
            return budgetEntity;
        }

        public BudgetEntity IncreaseBudgetAllocation(BudgetEntity budgetEntity)
        {
            string sql = @"INSERT INTO [dbo].[budget_allocations]
                  ([budget_id]
                  ,[amount]
                  ,[approved]
                  ,[approved_by_id]
                  ,[approved_date]
                  ,[updated_by_id]
                  ,[updated_date])
                  VALUES
                  (@budgetid
                  ,@amount
                  ,@approved
                  ,@approvedbyid
                  ,@approveddate
                  ,@updatedbyid
                  ,@updateddate)";

            // reason is appened with existing ones
            string newReason = "<br/>" + budgetEntity.Reason;
            string sqlUpdBudgetReason = @"UPDATE [dbo].[budgets] SET
                   [reason] = [reason] + @reason
                  ,[updated_by_id] = @updatedbyid
                  ,[updated_date] = @updateddate
                    WHERE id = @id";

            var budgetAllocation = budgetEntity.BudgetAllocations.FirstOrDefault();

            DynamicParameters dp = new DynamicParameters();
            dp.Add("budgetid", budgetEntity.Id, DbType.Int32, ParameterDirection.Input);   //budgetAllocation.BudgetId, DbType.Int32, ParameterDirection.Input);
            dp.Add("amount", budgetAllocation.Amount, DbType.Decimal, ParameterDirection.Input);
            dp.Add("approved", budgetAllocation.Approved, DbType.String, ParameterDirection.Input, 10);
            dp.Add("approvedbyid", budgetAllocation.ApprovedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("approveddate", budgetAllocation.ApprovedDate, DbType.DateTime, ParameterDirection.Input, 60);
            dp.Add("updatedbyid", budgetAllocation.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("updateddate", budgetAllocation.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var affRowsX1 = conn.Execute(sql, dp, transaction: tran);
                    // Add reason to the parent table (budget)
                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("id", budgetEntity.Id, DbType.Int32, ParameterDirection.Input);
                    dp2.Add("reason", newReason, DbType.String, ParameterDirection.Input);
                    dp2.Add("updatedbyid", budgetAllocation.UpdatedById, DbType.Int32, ParameterDirection.Input);
                    dp2.Add("updateddate", budgetAllocation.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);

                    var affRowsX2 = conn.Execute(sqlUpdBudgetReason, dp2, transaction: tran);
                    tran.Commit();
                }
            }
            return budgetEntity;
        }

        // Budget response for User page. 
        // Approved = Y 
        // Status = 'Open'
        // Dates and spend category ids
        public IEnumerable<BudgetListResponse> GetBudgetListResponsesForUser(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.date_from as dateFrom
                        ,bu.date_to as dateTo
                        ,bu.description as description
                        ,bu.po_prefix as poprefix
                        ,bu.status as status
                        ,sqttl.budget_total as budgettotal
                        ,sqttl.approved as approved
                        ,sqspndttl.spend_total as spendtotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.budget_id as bud_id, sum(a.amount) as budget_total, approved as approved FROM [dbo].[budget_allocations] as a
                        GROUP BY a.budget_id, a.approved
                        ) as sqttl
                        LEFT JOIN 
                        (SELECT * FROM [dbo].[budgets]) as bu
                        on bu.id = sqttl.bud_id
                        LEFT JOIN 
                        (select b.budget_id as bud_id, sum(b.amount) as spend_total FROM [dbo].[spends] as b
                        GROUP BY b.budget_id
                        ) as sqspndttl
                        on bu.id = sqspndttl.bud_id
                        INNER JOIN 
                        (SELECT id, name as name FROM [dbo].[care_homes]) as ch
                        on ch.id = bu.care_home_id
                        INNER JOIN
                        (SELECT * FROM [dbo].[spend_category]) as cat
                        ON cat.id = bu.spend_category_id
                        WHERE bu.spend_category_id IN @spendcategoryids
                        AND bu.status = @status
                        AND approved = 'Y'
						AND bu.date_from >= @datefrom
						AND bu.date_to <= @dateto";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendcategoryids", spendCategoryIds);
            dp.Add("status", Constants.BUDGET_STATUS_OPEN, DbType.String);
            dp.Add("datefrom", dateFrom.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
            dp.Add("dateto", dateTo.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<BudgetListResponse>(sql, dp).Result;
            }
        }

        // Budget response for Admin page. 
        // Dates and spend category ids only
        public IEnumerable<BudgetListResponse> GetBudgetListResponsesForAdmin(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.date_from as dateFrom
                        ,bu.date_to as dateTo
                        ,bu.description as description
                        ,bu.po_prefix as poprefix
                        ,bu.status as status
                        ,sqttl.budget_total as budgettotal
                        --,sqttl.approved as approved
                        ,sqspndttl.spend_total as spendtotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.budget_id as bud_id, sum(a.amount) as budget_total
                            --, approved as approved 
                        FROM [dbo].[budget_allocations] as a
                        GROUP BY a.budget_id --, a.approved
                        ) as sqttl
                        LEFT JOIN 
                        (SELECT * FROM [dbo].[budgets]) as bu
                        on bu.id = sqttl.bud_id
                        LEFT JOIN 
                        (select b.budget_id as bud_id, sum(b.amount) as spend_total FROM [dbo].[spends] as b
                        GROUP BY b.budget_id
                        ) as sqspndttl
                        on bu.id = sqspndttl.bud_id
                        INNER JOIN 
                        (SELECT id, name as name FROM [dbo].[care_homes]) as ch
                        on ch.id = bu.care_home_id
                        INNER JOIN
                        (SELECT * FROM [dbo].[spend_category]) as cat
                        ON cat.id = bu.spend_category_id
                        WHERE bu.spend_category_id IN @spendcategoryids
						AND bu.date_from >= @datefrom
						AND bu.date_to <= @dateto";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendcategoryids", spendCategoryIds);
            dp.Add("datefrom", dateFrom.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
            dp.Add("dateto", dateTo.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<BudgetListResponse>(sql, dp).Result;
            }
        }

        public BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.date_from as dateFrom
                        ,bu.date_to as dateTo
                        ,bu.description as description
                        ,bu.po_prefix as poprefix
                        ,bu.status as status
                        ,sqttl.budget_total as budgettotal
                        --,sqttl.approved as approved
                        ,sqspndttl.spend_total as spendtotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.budget_id as bud_id, sum(a.amount) as budget_total
                        --,approved as approved 
                        FROM [dbo].[budget_allocations] as a
                        WHERE a.approved = 'Y'
                        GROUP BY a.budget_id
                        -- , a.approved
                        ) as sqttl
                        LEFT JOIN 
                        (SELECT * FROM [dbo].[budgets]
                        WHERE reference_id = @referenceid) as bu
                        on bu.id = sqttl.bud_id
                        LEFT JOIN 
                        (select b.budget_id as bud_id, sum(b.amount) as spend_total FROM [dbo].[spends] as b
                        GROUP BY b.budget_id
                        ) as sqspndttl
                        on bu.id = sqspndttl.bud_id
                        INNER JOIN 
                        (SELECT id, name as name FROM [dbo].[care_homes]) as ch
                        on ch.id = bu.care_home_id
                        INNER JOIN
                        (SELECT * FROM [dbo].[spend_category]) as cat
                        ON cat.id = bu.spend_category_id";

            string sqlSelSpends = @"SELECT 
                                     sp.id as id
                                    ,sp.budget_id as budgetid
                                    ,sp.amount as amount
                                    ,sp.po_number as ponumber
                                    ,sp.notes as notes
                                    ,sp.created_date as createddate
                                    ,concat(u.forename, ' ', u.surname) as createdbyname
                                    FROM [dbo].[spends] sp
                                    INNER JOIN [dbo].[users] u
                                    ON sp.created_by_id = u.id
                                    WHERE sp.budget_id = @budgetid";

            // todo: add order by spends reverse order

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var budget = conn.QueryFirstOrDefault<BudgetListResponse>(sql, dp);
                if (budget != null && budget.Id > 0)
                {
                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("budgetid", budget.Id, DbType.Int32, ParameterDirection.Input);
                    budget.SpendResponses = conn.Query<SpendResponse>(sqlSelSpends, dp2);
                }
                return budget;
            }
        }



        // Spend Related
        public SpendRequest InsertSpend(SpendRequest spendRequest)
        {
            string sql = @"INSERT INTO [dbo].[spends]
                            ([budget_id]
                            ,[amount]
                            ,[po_number]
                            ,[notes]
                            ,[tran_type]
                            ,[created_by_id])
                            VALUES
                            (@budgetid
                            ,@amount
                            ,@ponumber
                            ,@notes
                            ,@trantype
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
                    dp2.Add("notes", spendRequest.Notes, DbType.String, ParameterDirection.Input);
                    dp2.Add("trantype", spendRequest.TranType, DbType.String, ParameterDirection.Input);
                    dp2.Add("createdbyid", spendRequest.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var affRowsX1 = conn.Execute(sql, dp2, transaction: tran);
                    tran.Commit();
                }
            }
            return spendRequest;
        }

        public bool TransferSpend(TransferSpendRequest transferSpendRequest)
        {
            //string newReason = "<br/>" + budgetEntity.Reason;
            string sql = @"UPDATE [dbo].[spends] SET
                        budget_id = (select id from budgets where reference_id = @transfertoreferenceid)
                        , notes = notes + '<br /> TRF: ' + @notes
                        WHERE id = @spendid";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("transfertoreferenceid", transferSpendRequest.TransferToBudgetReferenceId, DbType.Guid, ParameterDirection.Input, 60);
            dp.Add("notes", transferSpendRequest.Notes, DbType.String, ParameterDirection.Input);
            dp.Add("spendid", transferSpendRequest.TransferFromSpendId, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute(sql, dp);
                return true;
            }
        }

        public IEnumerable<SpendResponse> GetSpends(int[] budgetIds)
        {
            string sql = @"SELECT 
                        sp.id as id
                        ,sp.budget_id as budgetid
                        ,sp.amount as amount
                        ,sp.po_number as ponumber
                        ,sp.notes as notes
                        ,sp.tran_type as trantype
                        ,sp.created_date as createddate
                        FROM [dbo].[spends] sp
                        WHERE sp.budget_id IN @budgetids
                        ORDER BY sp.budget_id, sp.po_number, sp.created_by_id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("budgetids", budgetIds);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<SpendResponse>(sql, dp).Result;
            }
        }


    }
}