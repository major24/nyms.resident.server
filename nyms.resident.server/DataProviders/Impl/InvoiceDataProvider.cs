using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/*using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;*/

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

        public IEnumerable<Schedule> GetAllSchedulesForInvoiceDate(DateTime billingStart, DateTime billingEnd)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT s.[resident_id] as residentid
                              ,s.[local_authority_id] as localauthorityid
                              ,s.[payment_type_id] as paymenttypeid
                              ,s.[schedule_begin_date] as schedulebegindate
                              ,s.[schedule_end_date] as scheduleenddate
                              ,s.[weekly_fee] weeklyfee
                              ,pt.[payment_from] as paymentfrom
                              ,pt.[description] as description
	                          ,la.[name] paymentfromname
	                          FROM schedules s
	                          LEFT JOIN local_authorities la
	                          ON s.local_authority_id = la.id
                              INNER JOIN payment_type pt
                              ON s.payment_type_id = pt.id
                              WHERE s.schedule_end_date >= @billingstart
                              AND s.schedule_begin_date <= @billingend
	                          AND s.[active] = 'Y'
                              AND s.[weekly_fee] > 0";

                // rpt begin date
                // rpt end date";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("billingstart", billingStart.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                dp.Add("billingend", billingEnd.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input, 60);
                conn.Open();
                var result = conn.QueryAsync<Schedule>(sql, dp).Result;
                return result;
            }
        }
    }
}