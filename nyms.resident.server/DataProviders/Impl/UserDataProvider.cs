using nyms.resident.server.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using nyms.resident.server.DataProviders.Interfaces;
using System.Collections;
using System.Collections.Generic;
using WebGrease.Css.Extensions;

namespace nyms.resident.server.DataProviders.Impl
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly string _connectionString;

        public UserDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<User> GetUsers()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT id as id, reference_id as referenceid, forename as forename, surname as surname
                                                            FROM [users]";

                conn.Open();
                var result = conn.QueryAsync<User>(sql).Result;
                return result;
            }
        }

        public Task<User> GetById(int id)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT id as id, reference_id as referenceid, forename as forename, surname as surname, password as password
                                                FROM [users]
                                                WHERE id = @id";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("id", id, DbType.Int32, ParameterDirection.Input, 60);

                conn.Open();
                var result = conn.QueryFirstOrDefault<User>(sql, dp);
                return Task.FromResult(result);
            }
        }

        public Task<User> GetUserByReferenceId(Guid referenceId)
        {
            User user = new User();
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sqlUser = @"SELECT u.id as id, u.reference_id as referenceid, u.forename as forename, u.surname as surname 
                                    FROM [users] u
                                    WHERE u.reference_id = @referenceid";

                string sqlCareHomeRoles = @"SELECT
                                        r.name as name, ch.name as carehomename, ch.reference_id as carehomereferenceid
                                    FROM [users] u
                                    INNER JOIN [users_roles] ur
                                    ON u.id = ur.user_id
                                    INNER JOIN [roles] r
                                    ON ur.role_id = r.id
                                    LEFT JOIN [care_homes] ch
                                    ON ur.care_home_id = ch.id
                                    WHERE u.active = 'Y'
                                    AND r.active = 'Y'
                                    AND u.reference_id = @referenceid";

                var queries = $"{sqlUser} {sqlCareHomeRoles}";

                conn.Open();
                using (var multi = conn.QueryMultiple(queries, new { referenceid = referenceId }))
                {
                    user = multi.Read<User>().FirstOrDefault();
                    var careHomeRoles = multi.Read<CareHomeRole>();
                    if (user != null)
                        user.CareHomeRoles = careHomeRoles.ToArray();
                }
                return Task.FromResult(user);
            }
        }

        public Task<User> GetUserByUserNamePassword(string userName, string password)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                // todo: remove pwd  // 
                string sql = @"SELECT id as id, reference_id as referenceid, forename as forename, surname as surname, password as password
                                                            FROM [users]
                                                            WHERE username = @username";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("username", userName, DbType.String, ParameterDirection.Input, 60);

                conn.Open();
                var result = conn.QueryFirstOrDefault<User>(sql, dp); 
                return Task.FromResult(result);
            }
        }

        public void CreateUser(string userName, string password, User user)
        {
            string sqlCount = @"SELECT MAX(id) as newid FROM [dbo].[users];";
            string sqlInsert = @"INSERT INTO [dbo].[users] 
                                (id
                                ,username
                                ,forename
                                ,surname
                                ,email_address
                                ,password
                                ,active)
                                VALUES
                                (@id
                                ,@username
                                ,@forename
                                ,@surname
                                ,@emailaddress
                                ,@password
                                ,@active);";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("username", userName.Trim(), DbType.String, ParameterDirection.Input, 60);
            dp.Add("password", password, DbType.String, ParameterDirection.Input, 100);
            dp.Add("forename", user.ForeName.Trim(), DbType.String, ParameterDirection.Input, 100);
            dp.Add("surname", user.SurName.Trim(), DbType.String, ParameterDirection.Input, 100);
            dp.Add("emailaddress", user.Email, DbType.String, ParameterDirection.Input, 200);
            dp.Add("active", "Y", DbType.String, ParameterDirection.Input, 10);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    // add id (NOT Identity)
                    var maxId = conn.QuerySingle<int>(sqlCount, commandType: CommandType.Text, transaction: tran);
                    dp.Add("id", (maxId + 1), DbType.Int32, ParameterDirection.Input);
                    var affRowsX = conn.Execute(sqlInsert, dp, transaction: tran);
                    tran.Commit();                
                }
            }
        }

        public void AssignRoles(int userId, IEnumerable<UserRolePermission> userRolePermissions)
        {
            string sqlDelExistingRoles = @"DELETE FROM [dbo].[users_roles] WHERE user_id = @userid";
            string sqlInsert = @"INSERT INTO [dbo].[users_roles] 
                            (user_id
                            ,role_id
                            ,care_home_id)
                            VALUES
                            (@userid
                            ,@roleid
                            ,@carehomeid)";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("userid", userId, DbType.Int32, ParameterDirection.Input);
                    var affRowsX = conn.Execute(sqlDelExistingRoles, dp, transaction: tran);

                    userRolePermissions.ForEach(r =>
                    {
                        dp = new DynamicParameters();
                        dp.Add("userid", userId, DbType.Int32, ParameterDirection.Input);
                        dp.Add("roleid", r.RoleId, DbType.Int32, ParameterDirection.Input);
                        if (r.CareHomeId > 0)
                        {
                            dp.Add("carehomeid", r.CareHomeId, DbType.Int32, ParameterDirection.Input);
                        }
                        else
                        {
                            dp.Add("carehomeid", DBNull.Value, DbType.Int32, ParameterDirection.Input);
                        }
                        var affRowsX0 = conn.Execute(sqlInsert, dp, commandType: CommandType.Text, transaction: tran);
                    });
                    tran.Commit();
                }
            }
        }

        public void SetPassword(Guid referenceId, string password)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sqlUpdate = @"UPDATE [users]
                                     SET password = @password,
                                        updated_date = SYSDATETIME()
                                     WHERE reference_id = @referenceid";

                DynamicParameters dp = new DynamicParameters();
                conn.Open();
                dp.Add("password", password, DbType.String, ParameterDirection.Input);
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);
                var result = conn.Execute(sqlUpdate, dp, commandType: CommandType.Text);
            }
        }

    }
}

