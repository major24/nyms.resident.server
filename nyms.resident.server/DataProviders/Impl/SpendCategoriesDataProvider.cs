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
    public class SpendCategoriesDataProvider : ISpendCategoriesDataProvider
    {
        private readonly string _connectionString;

        public SpendCategoriesDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<SpendMasterCategory> GetSpendMasterCategories()
        {
            string sql = @"SELECT id as id, name as name, active as active
                            FROM [dbo].[spend_master_category]
                            WHERE active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = conn.QueryAsync<SpendMasterCategory>(sql).Result;
                return result;
            }
        }

        public IEnumerable<SpendCategoryEntity> GetSpendCategoryEntities()
        {
            string sql = @"SELECT id as id
                            ,spend_master_category_id as spendmastercategoryid 
                            ,name as name
                            ,cat_code as catcode
                            ,active as active
                            FROM [dbo].[spend_category]
                            WHERE active = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = conn.QueryAsync<SpendCategoryEntity>(sql).Result;
                return result;
            }
        }

        public IEnumerable<SpendCategoryRoleEntity> GetSpendCategoryRoleEntities()
        {
            string sql = @"SELECT sr.spend_category_id as spendcategoryid
                            ,r.id as roleid
                            ,r.name as rolename
                            FROM [dbo].[spend_category_roles] sr
                            INNER JOIN [dbo].[roles] r
                            ON sr.role_id = r.id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<SpendCategoryRoleEntity>(sql).Result;
            }
        }


        public SpendCategoryEntity Insert(SpendCategoryEntity spendCategoryEntity)
        {
            string sql = @"INSERT INTO [dbo].[spend_category]
                                    ([spend_master_category_id]
                                    ,[name]
                                    ,[cat_code])
                                    VALUES 
                                    (@spendmastercategoryid
                                    ,@name
                                    ,@catcode);
                                    SELECT CAST(SCOPE_IDENTITY() as int)";

            string sqlInsRoles = @"INSERT INTO [dbo].[spend_category_roles]
                                    ([role_id]
                                    ,[spend_category_id])
                                    VALUES 
                                    (@roleid
                                    ,@spendcategoryid)";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendmastercategoryid", spendCategoryEntity.SpendMasterCategoryId, DbType.Int32, ParameterDirection.Input);
            dp.Add("name", spendCategoryEntity.Name.Trim(), DbType.String, ParameterDirection.Input, 100);
            dp.Add("catcode", spendCategoryEntity.CatCode, DbType.String, ParameterDirection.Input, 60);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var spendCateId = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);
                    if (spendCategoryEntity.SpendCategoryRoleEntities.Any())
                    {
                        DynamicParameters dp2 = new DynamicParameters();
                        spendCategoryEntity.SpendCategoryRoleEntities.ForEach(catRole =>
                        {
                            dp2.Add("roleid", catRole.RoleId, DbType.Int32, ParameterDirection.Input);
                            dp2.Add("spendcategoryid", spendCateId, DbType.Int32, ParameterDirection.Input);
                            var affRowsX = conn.Execute(sqlInsRoles, dp2, transaction: tran);
                        });
                    }
                    tran.Commit();
                }
            }
            return spendCategoryEntity;
        }

        public SpendCategoryEntity Update(SpendCategoryEntity spendCategoryEntity)
        {
            string sql = @"UPDATE [dbo].[spend_category] SET
                                    [spend_master_category_id] = @spendmastercategoryid
                                   ,[name] = @name
                                   ,[cat_code] = @catcode
                                    WHERE [id] = @id";

            string sqlDelRoles = @"DELETE FROM [dbo].[spend_category_roles] WHERE [spend_category_id] = @spendcategoryid";

            string sqlInsRoles = @"INSERT INTO [dbo].[spend_category_roles]
                                    ([role_id]
                                    ,[spend_category_id])
                                    VALUES 
                                    (@roleid
                                    ,@spendcategoryid)";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("spendmastercategoryid", spendCategoryEntity.SpendMasterCategoryId, DbType.Int32, ParameterDirection.Input);
            dp.Add("name", spendCategoryEntity.Name.Trim(), DbType.String, ParameterDirection.Input, 100);
            dp.Add("catcode", spendCategoryEntity.CatCode, DbType.String, ParameterDirection.Input, 60);
            dp.Add("id", spendCategoryEntity.Id, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // update spend category table
                    int spendCateId = spendCategoryEntity.Id;
                    var affRows0 = conn.Execute(sql, dp, transaction: tran);

                    // delete  all roles irrespective and create new roles if any
                    DynamicParameters dp2 = new DynamicParameters();
                    dp2.Add("spendcategoryid", spendCateId, DbType.Int32, ParameterDirection.Input);
                    var affRows1 = conn.Execute(sqlDelRoles, dp2, transaction: tran);
                    
                    if (spendCategoryEntity.SpendCategoryRoleEntities.Any())
                    {
                        spendCategoryEntity.SpendCategoryRoleEntities.ForEach(catRole =>
                        {
                            DynamicParameters dp3 = new DynamicParameters();
                            dp3.Add("roleid", catRole.RoleId, DbType.Int32, ParameterDirection.Input);
                            dp3.Add("spendcategoryid", spendCateId, DbType.Int32, ParameterDirection.Input);
                            var affRows2 = conn.Execute(sqlInsRoles, dp3, transaction: tran);
                        });
                    }
                    tran.Commit();
                }
            }

            return spendCategoryEntity;
        }

        public IEnumerable<SpendCategory> GetSpendCategoryAndRoles()
        {
            throw new NotImplementedException();
        }

    }
}