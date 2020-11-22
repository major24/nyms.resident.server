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

namespace nyms.resident.server.DataProviders.Impl
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly string _connectionString;

        public UserDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
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

        public IEnumerable<User> GetUsers()
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

