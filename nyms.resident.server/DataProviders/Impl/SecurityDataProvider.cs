using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace nyms.resident.server.DataProviders.Impl
{
    public class SecurityDataProvider : ISecurityDataProvider
    {
        private readonly string _connectionString;

        public SecurityDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<Role> GetRoles()
        {
            string sql = @"SELECT id as id, name as name FROM [dbo].[roles] 
                            WHERE [id] > 1 
                            AND [active] = 'Y'";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<Role>(sql).Result;
            }
        }

        public IEnumerable<UserRolePermission> GetRolePermissions(int userId)
        {
            string sql = @"SELECT 
                        ur.user_id as userid 
                        ,ur.role_id as roleid 
                        ,ur.care_home_id as carehomeid
                        ,sr.spend_category_id as spendcategoryid
                        FROM [dbo].[users_roles] ur 
                        INNER JOIN [dbo].[spend_category_roles] sr
                        ON ur.role_id = sr.role_id 
                        WHERE ur.user_id = @userid";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("userid", userId, DbType.Int32, ParameterDirection.Input);
                return conn.QueryAsync<UserRolePermission>(sql, dp).Result;
            }
        }

    }
}