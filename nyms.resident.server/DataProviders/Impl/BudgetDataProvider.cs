using Dapper;
using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
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

        public IEnumerable<BudgetListResponse> GetBudgetListResponses(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.budget_type as budgettype
                        ,bu.date_from as dateFrom
                        ,bu.date_to as dateTo
                        ,bu.description as description
                        ,bu.po_prefix as poprefix
                        ,bu.status as status
                        ,sqttl.budget_total as budgettotal
                        ,sqspndttl.spend_total as spendtotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.budget_id as bud_id, sum(a.amount) as budget_total
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


        // Budget response for User page. 
        // Approved = Y 
        // Status = 'Open'
        // Dates and spend category ids
        public IEnumerable<BudgetListResponse> GetBudgetListResponsesApprovedAndOpened(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.budget_type as budgettype
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

        public IEnumerable<Budget> GetBudgetsApprovedAndOpened(int[] spendCategoryIds)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.budget_type as budgettype
                        ,convert(varchar, bu.date_from, 23) as dateFrom
                        ,convert(varchar, bu.date_to, 23) as dateTo
                        ,bu.description as description
                        ,bu.po_prefix as poprefix
                        ,bu.status as status
                        ,a.approved as approved
						FROM [dbo].[budgets] as bu
                        INNER JOIN [dbo].[budget_allocations] as a
						ON bu.id = a.budget_id
						WHERE bu.spend_category_id IN @spendcategoryids
                        AND bu.status = 'Open'
                        AND a.approved = 'Y';";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendcategoryids", spendCategoryIds);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<Budget>(sql, dp).Result;
            }
        }

        public BudgetListResponse GetBudgetListResponse(Guid referenceId)
        {
            string sql = @"SELECT 
                         bu.id as id
                        ,bu.reference_id as referenceId
                        ,bu.spend_category_id as spendcategoryid
                        ,bu.care_home_id as carehomeid
                        ,bu.name as name
                        ,bu.budget_type as budgettype
                        ,bu.date_from as dateFrom
                        ,bu.date_to as dateTo
                        ,bu.description as description
                        ,bu.po_prefix as poprefix
                        ,bu.status as status
                        ,bu.reason as reason
                        ,sqttl.budget_total as budgettotal
                        ,sqspndttl.spend_total as spendtotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.budget_id as bud_id, sum(a.amount) as budget_total
                        FROM [dbo].[budget_allocations] as a
                        GROUP BY a.budget_id
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

            string sqlSelAlloc = @"SELECT 
                                     a.id as id
                                    ,a.budget_id as budgetid
                                    ,a.amount as amount
                                    ,a.approved as approved
                                    ,a.approved_by_id as approvedbyid
                                    FROM [dbo].[budget_allocations] a
                                    WHERE a.budget_id = @budgetid";
            // todo: add order by spends query reverse order

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
                    budget.BudgetAllocations = conn.Query<BudgetAllocation>(sqlSelAlloc, dp2);
                }
                return budget;
            }
        }

        public BudgetEntity Insert(IEnumerable<BudgetEntity> budgetEntities)
        {
            string sql = @"INSERT INTO [dbo].[budgets]
                            ([spend_category_id]
                            ,[care_home_id]
                            ,[name]
                            ,[budget_type]
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
                            ,@budgettype
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
                        dp.Add("budgettype", budgetEntity.BudgetType.ToString(), DbType.String, ParameterDirection.Input, 100);
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
                            ,[budget_type] = @budgettype
                            ,[date_from] = @datefrom
                            ,[date_to] = @dateto
                            ,[description] = @description
                            ,[po_prefix] = @poprefix
                            ,[status] = @status
                            ,[reason] = [reason] + '| ' + @reason
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
            dp.Add("budgettype", budgetEntity.BudgetType.ToString(), DbType.String, ParameterDirection.Input, 100);
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


    }
}