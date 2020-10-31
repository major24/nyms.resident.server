﻿using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
                    }

                    var affRowsX = conn.Execute(sqlDeleteResidents, transaction: tran);
                    affRowsX = conn.Execute(sqlDeleteEnquires, transaction: tran);
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