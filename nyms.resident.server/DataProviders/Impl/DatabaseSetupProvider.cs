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
    public class DatabaseSetupProvider : IDatabaseSetupProvider
    {
        private readonly string _connectionString;

        public DatabaseSetupProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        }

        public void ClearDatabase()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sqlSelectEnquires = @"SELECT id FROM [dbo].[enquires] WHERE [forename] like 'User%'";
                string sqlSelectResidents = @"SELECT id FROM [dbo].[residents] WHERE [forename] like 'User%'";
                string sqlDeleteEnquires = @"DELETE FROM [dbo].[enquires] WHERE [forename] like 'User%'";
                string sqlDeleteResidents = @"DELETE FROM [dbo].[residents] WHERE [forename] like 'User%'";
                string sqlDeleteSchedules = @"DELETE FROM [dbo].[schedules] WHERE resident_id = @id";
                string sqlDeleteInvValidated = @"DELETE FROM [dbo].[invoices_validated] WHERE resident_id = @id";
                string sqlDeleteInvComments = @"DELETE FROM [dbo].[invoice_comments] WHERE resident_id = @id";
                string sqlDeleteNok = @"DELETE FROM [dbo].[next_of_kin] WHERE resident_id = @id";
                string sqlDeleteAddresses = @"DELETE FROM [dbo].[resident_addresses] WHERE resident_id = @id";
                string sqlDeleteContacts = @"DELETE FROM [dbo].[resident_contacts] WHERE resident_id = @id";
                string sqlDeleteSocialWorkers = @"DELETE FROM [dbo].[social_workers] WHERE resident_id = @id";

                conn.Open();
                var resEnquires = conn.Query<EnquiryEntity>(sqlSelectEnquires);
                var resResidents = conn.Query<Resident>(sqlSelectResidents);

                using (var tran = conn.BeginTransaction())
                {
                    foreach (var r in resResidents)
                    {
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("id", r.Id, DbType.Int32, ParameterDirection.Input);
                        var affRows = conn.Execute(sqlDeleteInvValidated, dp, transaction: tran);
                        affRows = conn.Execute(sqlDeleteInvComments, dp, transaction: tran);
                        affRows = conn.Execute(sqlDeleteSchedules, dp, transaction: tran);
                        affRows = conn.Execute(sqlDeleteNok, dp, transaction: tran);
                        affRows = conn.Execute(sqlDeleteAddresses, dp, transaction: tran);
                        affRows = conn.Execute(sqlDeleteContacts, dp, transaction: tran);
                        affRows = conn.Execute(sqlDeleteSocialWorkers, dp, transaction: tran);
                    }

                    var affRowsX = conn.Execute(sqlDeleteResidents, transaction: tran);
                    affRowsX = conn.Execute(sqlDeleteEnquires, transaction: tran);
                    tran.Commit();
                }
            }
        }

        public void ClearSpendsDatabase()
        {
            string sqlSelectAll = @"select cat.id as categoryid, b.id as budgetid, s.id as spendid 
                                    from spend_category cat left join spend_budgets b on cat.id = b.spend_category_id 
                                    left join spends s on b.id = s.spend_budget_id
                                    where cat.name like 'Test%'";

            string sqlDelSpends = @"DELETE FROM [dbo].[spends] WHERE [spend_budget_id] = @id";
            string sqlDelSpendsbudgetAllocations = @"DELETE FROM [dbo].[spend_budget_allocations] WHERE [spend_budget_id] = @id";
            string sqlDelSpendsbudget = @"DELETE FROM [dbo].[spend_budgets] WHERE [id] = @id";
            string sqlDelSpendRoles = @"DELETE FROM [dbo].[spend_category_roles] WHERE [spend_category_id] = @id";
            string sqlDelSpendsCategory = @"DELETE FROM [dbo].[spend_category] WHERE [id] = @id";

            List<int> CategoryIds = new List<int>();
            List<int> SpendBudgetIds = new List<int>();
            List<int> SpendIds = new List<int>();

            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlSelectAll, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    CategoryIds.Add((int)reader[0]);
                    if (reader[1] != DBNull.Value)
                    {
                        SpendBudgetIds.Add((int)reader[1]);
                    }
                    if (reader[2] != DBNull.Value)
                    {
                        SpendIds.Add((int)reader[2]);
                    }
                }
            }

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    DynamicParameters dp = new DynamicParameters();
                    SpendIds.ForEach(id => {
                        dp.Add("id", id, DbType.Int32, ParameterDirection.Input);
                        var affRowsX = conn.Execute(sqlDelSpends, dp, transaction: tran);  
                    });
                    SpendBudgetIds.ForEach(id =>
                    {
                        dp.Add("id", id, DbType.Int32, ParameterDirection.Input);
                        var affRows1 = conn.Execute(sqlDelSpendsbudgetAllocations, dp, transaction: tran);
                        var affRows2 = conn.Execute(sqlDelSpendsbudget, dp, transaction: tran);
                    });
                    CategoryIds.ForEach(id =>
                    {
                        dp.Add("id", id, DbType.Int32, ParameterDirection.Input); 
                        var affRowsX1 = conn.Execute(sqlDelSpendRoles, dp, transaction: tran);
                        var affRowsX2 = conn.Execute(sqlDelSpendsCategory, dp, transaction: tran);
                    });
                    tran.Commit();
                }
            }
        }


        public void ClearBudgetsDatabase()
        {
            string sqlSelTestBudgets = @"select id as id from spend_budgets
                                    where name like 'Test%'";

            // string sqlDelSpends = @"DELETE FROM [dbo].[spends] WHERE [spend_budget_id] = @id";
            string sqlDelSpendsbudgetAllocations = @"DELETE FROM [dbo].[spend_budget_allocations] WHERE [spend_budget_id] = @id";
            string sqlDelSpendsbudget = @"DELETE FROM [dbo].[spend_budgets] WHERE [id] = @id";

            List<int> SpendBudgetIds = new List<int>();
            // List<int> SpendIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlSelTestBudgets, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SpendBudgetIds.Add((int)reader[0]);
                }
            }

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    DynamicParameters dp = new DynamicParameters();
/*                    SpendIds.ForEach(id => {
                        dp.Add("id", id, DbType.Int32, ParameterDirection.Input);
                        var affRowsX = conn.Execute(sqlDelSpends, dp, transaction: tran);
                    });*/
                    SpendBudgetIds.ForEach(id =>
                    {
                        dp.Add("id", id, DbType.Int32, ParameterDirection.Input);
                        var affRows1 = conn.Execute(sqlDelSpendsbudgetAllocations, dp, transaction: tran);
                        var affRows2 = conn.Execute(sqlDelSpendsbudget, dp, transaction: tran);
                    });

                    tran.Commit();
                }
            }
        }

        public void ClearTestUsers()
        {
            string sqlSelTestUsers = @"select id as id from users
                                    where username like 'test%'";
            string sqlDeleteTestRoles = @"delete from users_roles where user_id = @userid";
            string sqlDeleteTestUsers = @"delete from users where id = @id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var users = conn.Query<User>(sqlSelTestUsers).ToArray();

                using (var tran = conn.BeginTransaction())
                {
                    users.ForEach(u =>
                    {
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("userid", u.Id, DbType.Int32, ParameterDirection.Input);
                        var affRows0 = conn.Execute(sqlDeleteTestRoles, dp, transaction: tran);
                    });
                    users.ForEach(u =>
                    {
                        DynamicParameters dp = new DynamicParameters();
                        dp.Add("id", u.Id, DbType.Int32, ParameterDirection.Input);
                        var affRows0 = conn.Execute(sqlDeleteTestUsers, dp, transaction: tran);
                    });
                    tran.Commit();
                }
            }
        }


        public void SeedDatabase()
        {
            throw new NotImplementedException();
        }
    }
}