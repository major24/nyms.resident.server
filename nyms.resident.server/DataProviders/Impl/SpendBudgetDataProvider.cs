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
    public class SpendBudgetDataProvider : ISpendBudgetDataProvider
    {
        private readonly string _connectionString;

        public SpendBudgetDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<SpendBudgetEntity> GetSpendBudgets()
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
                            ,created_by_id as createdbyid
                            ,created_date as createddate
                            ,updated_by_id as updatedbyid
                            ,updated_date as updateddate
                            FROM [dbo].[spend_budgets]";
            string sqlAllocations = @"SELECT id as id
                            ,spend_budget_id as spendbudgetid
                            ,amount as amount
                            ,approved as approved
                            ,approved_by_id as approvedbyid
                            ,approved_date as approveddate
                            ,updated_by_id as updatedbyid
                            ,updated_date as updateddate
                            FROM [dbo].[spend_budget_allocations]";

            IEnumerable<SpendBudgetEntity> spendBudgetEntities;
            IEnumerable<SpendBudgetAllocationEntity> spendBudgetAllocationEntities;

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                spendBudgetEntities = conn.QueryAsync<SpendBudgetEntity>(sql).Result;
                spendBudgetAllocationEntities = conn.QueryAsync<SpendBudgetAllocationEntity>(sqlAllocations).Result;
            }

            if (spendBudgetEntities != null && spendBudgetEntities.Any())
            {
                spendBudgetEntities.ForEach(b =>
                {
                    b.SpendBudgetAllocations = spendBudgetAllocationEntities.Where(a => a.SpendBudgetId == b.Id).ToArray();
                });
            }

            return spendBudgetEntities;
        }

        public SpendBudgetEntity GetSpendBudget(Guid referenceId)
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
                            ,created_by_id as createdbyid
                            ,created_date as createddate
                            ,updated_by_id as updatedbyid
                            ,updated_date as updateddate
                            FROM [dbo].[spend_budgets]
                            WHERE reference_id = @referenceid";

            string sqlAllocations = @"SELECT a.id as id
                            ,spend_budget_id as spendbudgetid
                            ,amount as amount
                            ,approved as approved
                            ,approved_by_id as approvedbyid
                            ,approved_date as approveddate
                            ,a.updated_by_id as updatedbyid
                            ,a.updated_date as updateddate
                            FROM [dbo].[spend_budget_allocations] a
							INNER JOIN [dbo].[spend_budgets] b
							ON b.id = a.spend_budget_id
							WHERE b.reference_id = @referenceid
                            ORDER BY a.approved desc";

            SpendBudgetEntity spendBudgetEntity;
            IEnumerable<SpendBudgetAllocationEntity> spendBudgetAllocationEntities;

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);
                spendBudgetEntity = conn.QueryFirstOrDefault<SpendBudgetEntity>(sql, dp);
                spendBudgetAllocationEntities = conn.QueryAsync<SpendBudgetAllocationEntity>(sqlAllocations, dp).Result;
                spendBudgetEntity.SpendBudgetAllocations = spendBudgetAllocationEntities;
            }

            return spendBudgetEntity;
        }

        public SpendBudgetEntity Insert(SpendBudgetEntity spendBudgetEntity)
        {
            string sql = @"INSERT INTO [dbo].[spend_budgets]
                            ([spend_category_id]
                            ,[care_home_id]
                            ,[name]
                            ,[date_from]
                            ,[date_to]
                            ,[description]
                            ,[po_prefix]
                            ,[status]
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
                            ,@createdbyid);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            string sqlInsAllocations = @"INSERT INTO [dbo].[spend_budget_allocations]
                            ([spend_budget_id]
                            ,[amount]
                            ,[approved]
                            ,[approved_by_id]
                            ,[approved_date]
                            ,[reason]
                            ,[updated_by_id]
                            ,[updated_date])
                            VALUES
                            (@spendbudgetid
                            ,@amount
                            ,@approved
                            ,@approvedbyid
                            ,@approveddate
                            ,@reason
                            ,@updatedbyid
                            ,@updateddate)";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendcategoryid", spendBudgetEntity.SpendCategoryId, DbType.Int32, ParameterDirection.Input);
            dp.Add("carehomeid", spendBudgetEntity.CareHomeId, DbType.Int32, ParameterDirection.Input);
            dp.Add("name", spendBudgetEntity.Name, DbType.String, ParameterDirection.Input, 100);
            dp.Add("datefrom", spendBudgetEntity.DateFrom.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
            dp.Add("dateto", spendBudgetEntity.DateTo.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
            dp.Add("description", spendBudgetEntity.Description, DbType.String, ParameterDirection.Input, 1000);
            dp.Add("poprefix", spendBudgetEntity.PoPrefix, DbType.String, ParameterDirection.Input, 50);
            dp.Add("status", spendBudgetEntity.Status, DbType.String, ParameterDirection.Input, 50);
            dp.Add("createdbyid", spendBudgetEntity.CreatedById, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var newBudgetId = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);

                    if (spendBudgetEntity.SpendBudgetAllocations != null && spendBudgetEntity.SpendBudgetAllocations.Any())
                    {
                        spendBudgetEntity.SpendBudgetAllocations.ForEach(alloc =>
                        {
                            DynamicParameters dp2 = new DynamicParameters();
                            dp2.Add("spendbudgetid", newBudgetId, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("amount", alloc.Amount, DbType.Decimal, ParameterDirection.Input);
                            dp2.Add("approved", alloc.Approved, DbType.String, ParameterDirection.Input, 10);
                            dp2.Add("approvedbyid", alloc.ApprovedById, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("approveddate", alloc.ApprovedDate, DbType.DateTime, ParameterDirection.Input, 60);
                            dp2.Add("reason", alloc.Reason, DbType.String, ParameterDirection.Input, 1000);
                            dp2.Add("updatedbyid", alloc.UpdatedById, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("updateddate", alloc.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);

                            var affRowsX1 = conn.Execute(sqlInsAllocations, dp2, transaction: tran);
                        });
                    }
                    tran.Commit();
                }
            }
            return spendBudgetEntity;
        }

        public SpendBudgetEntity Update(SpendBudgetEntity spendBudgetEntity)
        {
            string sql = @"UPDATE [dbo].[spend_budgets] SET
                             [name] = @name
                            ,[date_from] = @datefrom
                            ,[date_to] = @dateto
                            ,[description] = @description
                            ,[po_prefix] = @poprefix
                            ,[status] = @status
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                            WHERE id = @id";

            string sqlUpdAllocations = @"UPDATE [dbo].[spend_budget_allocations] SET
                             [amount] = @amount
                            ,[approved] = @approved
                            ,[approved_by_id] = @approvedbyid
                            ,[approved_date] = @approveddate
                            ,[reason] = @reason
                            ,[updated_by_id] = @updatedbyid
                            ,[updated_date] = @updateddate
                            WHERE id = @id";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("name", spendBudgetEntity.Name, DbType.String, ParameterDirection.Input, 100);
            dp.Add("datefrom", spendBudgetEntity.DateFrom, DbType.Date, ParameterDirection.Input, 60);
            dp.Add("dateto", spendBudgetEntity.DateTo, DbType.Date, ParameterDirection.Input, 60);
            dp.Add("description", spendBudgetEntity.Description, DbType.String, ParameterDirection.Input, 1000);
            dp.Add("poprefix", spendBudgetEntity.PoPrefix, DbType.String, ParameterDirection.Input, 50);
            dp.Add("status", spendBudgetEntity.Status, DbType.String, ParameterDirection.Input, 50);
            dp.Add("updatedbyid", spendBudgetEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("updateddate", spendBudgetEntity.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);
            dp.Add("id", spendBudgetEntity.Id, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var affRowsX1 = conn.Execute(sql, dp, transaction: tran);

                    if (spendBudgetEntity.SpendBudgetAllocations != null && spendBudgetEntity.SpendBudgetAllocations.Any())
                    {
                        spendBudgetEntity.SpendBudgetAllocations.ForEach(alloc =>
                        {
                            DynamicParameters dp2 = new DynamicParameters();
                            dp2.Add("amount", alloc.Amount, DbType.Decimal, ParameterDirection.Input);
                            dp2.Add("approved", alloc.Approved, DbType.String, ParameterDirection.Input, 10);
                            dp2.Add("approvedbyid", alloc.ApprovedById, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("approveddate", alloc.ApprovedDate, DbType.DateTime, ParameterDirection.Input, 60);
                            dp2.Add("reason", alloc.Reason, DbType.String, ParameterDirection.Input, 1000);
                            dp2.Add("updatedbyid", alloc.UpdatedById, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("updateddate", alloc.UpdatedDate, DbType.DateTime, ParameterDirection.Input, 60);
                            dp2.Add("id", alloc.Id, DbType.Int32, ParameterDirection.Input);

                            var affRowsX2 = conn.Execute(sqlUpdAllocations, dp2, transaction: tran);
                        });
                    }
                    tran.Commit();
                }
            }
            return spendBudgetEntity;
        }

        // buget response for Budget-List Page
        public IEnumerable<SpendBudgetListResponse> GetSpendBudgetListResponses()
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
                        ,sqspndttl.vat_total as vattotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.spend_budget_id as bud_id, sum(a.amount) as budget_total, approved as approved FROM [dbo].[spend_budget_allocations] as a
                        GROUP BY a.spend_budget_id, a.approved
                        ) as sqttl
                        LEFT JOIN 
                        (SELECT * FROM [dbo].[spend_budgets]) as bu
                        on bu.id = sqttl.bud_id
                        LEFT JOIN 
                        (select b.spend_budget_id as bud_id, sum(b.amount) as spend_total, sum(b.vat) as vat_total FROM [dbo].[spends] as b
                        GROUP BY b.spend_budget_id
                        ) as sqspndttl
                        on bu.id = sqspndttl.bud_id
                        INNER JOIN 
                        (SELECT id, name as name FROM [dbo].[care_homes]) as ch
                        on ch.id = bu.care_home_id
                        INNER JOIN
                        (SELECT * FROM [dbo].[spend_category]) as cat
                        ON cat.id = bu.spend_category_id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<SpendBudgetListResponse>(sql).Result;
            }
        }

        public SpendBudgetListResponse GetSpendBudgetListResponseByReferenceId(Guid referenceId)
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
                        ,sqspndttl.vat_total as vattotal
                        ,ch.name as carehomename
                        ,cat.name as spendcategoryname
                        FROM (
                        SELECT a.spend_budget_id as bud_id, sum(a.amount) as budget_total, approved as approved FROM [dbo].[spend_budget_allocations] as a
                        GROUP BY a.spend_budget_id, a.approved
                        ) as sqttl
                        LEFT JOIN 
                        (SELECT * FROM [dbo].[spend_budgets]
                        WHERE reference_id = @referenceid) as bu
                        on bu.id = sqttl.bud_id
                        LEFT JOIN 
                        (select b.spend_budget_id as bud_id, sum(b.amount) as spend_total, sum(b.vat) as vat_total FROM [dbo].[spends] as b
                        GROUP BY b.spend_budget_id
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
                                    ,sp.spend_budget_id as spendbudgetid
                                    ,sp.amount as amount
                                    ,sp.vat as vat
                                    ,sp.po_number as ponumber
                                    ,sp.notes as notes
                                    ,sp.created_date as createddate
                                    ,concat(u.forename, ' ', u.surname) as createdbyname
                                    FROM [dbo].[spends] sp
                                    INNER JOIN [dbo].[users] u
                                    ON sp.created_by_id = u.id
                                    WHERE sp.spend_budget_id = @spendbudgetid";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var budget = conn.QueryFirstOrDefault<SpendBudgetListResponse>(sql, dp);
                if (budget != null && budget.Id > 0)
                {
                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("spendbudgetid", budget.Id, DbType.Int32, ParameterDirection.Input);
                    budget.SpendResponses = conn.Query<SpendResponse>(sqlSelSpends, dp2);
                }
                return budget;
            }
        }



        // Spend Related
        public SpendRequest CreateSpend(SpendRequest spendRequest)
        {
            string sql = @"INSERT INTO [dbo].[spends]
                            ([spend_budget_id]
                            ,[amount]
                            ,[vat]
                            ,[po_number]
                            ,[notes]
                            ,[created_by_id])
                            VALUES
                            (@spendbudgetid
                            ,@amount
                            ,@vat
                            ,@ponumber
                            ,@notes
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
                    // clean unq number table each time req is made
                    var affRowsX0 = conn.Execute(sqlDelUnqNbr, transaction: tran);
                    
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("createddate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    var newUnqId = conn.QuerySingle<int>(sqlInsUnqNbr, dp, commandType: CommandType.Text, transaction: tran);

                    // pad unq id to ponumber
                    spendRequest.PoNumber = spendRequest.PoNumber + "-" + newUnqId;

                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("spendbudgetid", spendRequest.SpendBudgetId, DbType.Int32, ParameterDirection.Input);
                    dp2.Add("amount", spendRequest.Amount, DbType.Decimal, ParameterDirection.Input);
                    dp2.Add("vat", spendRequest.Vat, DbType.Decimal, ParameterDirection.Input);
                    dp2.Add("ponumber", spendRequest.PoNumber, DbType.String, ParameterDirection.Input, 100);
                    dp2.Add("notes", spendRequest.Notes, DbType.String, ParameterDirection.Input, 1000);
                    dp2.Add("createdbyid", spendRequest.CreatedById, DbType.Int32, ParameterDirection.Input);

                    var affRowsX1 = conn.Execute(sql, dp2, transaction: tran);
                    tran.Commit();
                }
            }
            return spendRequest;
        }



    }
}