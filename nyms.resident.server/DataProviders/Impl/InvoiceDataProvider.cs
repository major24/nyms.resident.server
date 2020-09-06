using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nyms.resident.server.DataProviders.Impl
{
    public class InvoiceDataProvider : IInvoiceDataProvider
    {
        private readonly string _connectionString;

        public InvoiceDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        }

        public IEnumerable<Schedule> GetAllSchedules()
        {
            // IEnumerable<Schedule> schedules = new List<Schedule>();
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT s.[resident_id] as residentid
                              ,s.[local_authority_id] as localauthorityid
                              ,s.[payment_type] as paymenttype
                              ,s.[payment_from_code] as paymentfromcode
                              ,s.[description] as description
                              ,s.[schedule_begin_date] as schedulebegindate
                              ,s.[schedule_end_date] as scheduleenddate
                              ,s.[weekly_fee] weeklyfee
	                          ,la.[name] paymentfromdesc
	                          FROM schedules s
	                          LEFT JOIN local_authorities la
	                          ON s.local_authority_id = la.id
	                          WHERE s.[active] = 'Y'
                              AND s.[weekly_fee] > 0";

                conn.Open();
                var result = conn.QueryAsync<Schedule>(sql).Result;
                return result;
            }
        }
    }
}