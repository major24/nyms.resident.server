﻿using Dapper;
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

            DynamicParameters dp = new DynamicParameters();
            dp.Add("userid", userId, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<UserRolePermission>(sql, dp).Result;
            }
        }

        public IEnumerable<int> GetSpendCategoryRoleIds(int userId)
        {
            string sql = @"SELECT DISTINCT scr.spend_category_id as spendcategoryid 
                            FROM [dbo].[users] u 
                            INNER JOIN [dbo].[users_roles] ur
                            ON u.id = ur.user_id
                            INNER JOIN [dbo].[spend_category_roles] scr
                            ON scr.role_id = ur.role_id
                            WHERE u.active = 'Y'
                            AND u.id = @userid";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("userid", userId, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<int>(sql, dp).Result;
            }
        }

        public IEnumerable<UserRoleAccess> GetUserRoleAccesses()
        {
            string sql = @"SELECT u.id as userid
                            ,u.reference_id as referenceid
                            ,u.forename as forename
                            ,u.surname as surname
                            ,u.active as activeuser
                            ,r.id as roleid
                            ,ur.care_home_id as carehomeid
                            ,r.name as rolename
                            ,r.active as activerole
                            FROM [dbo].[users] u 
                            INNER JOIN [dbo].[users_roles] ur
                            ON u.id = ur.user_id
                            INNER JOIN [dbo].[roles] r
                            ON r.id = ur.role_id
                            ORDER BY u.id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                return conn.QueryAsync<UserRoleAccess>(sql).Result;
            }
        }

    }
}