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
    public class ResidentDataProvider : IResidentDataProvider
    {
        private readonly string _connectionString;

        public ResidentDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<Resident> GetAll()
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT
                               [id] as id
                              ,[reference_id] as referenceid
                              ,[care_home_id] as carehomeid
                              ,[local_authority_id] as localauthorityid
                              ,[nhs_number] as nhsnumber
                              ,[po_number] as ponumber
                              ,[la_id] as laid
                              ,[nyms_id] as nymsid
                              ,[fore_name] as forename
                              ,[sur_name] as surname
                              ,[middle_name] as middlename
                              ,[dob] as dob
                              ,[gender] as gender
                              ,[marital_status] as maritalstatus
                              ,[sw_fore_name] as swforename
                              ,[sw_sur_name] as swsurname
                              ,[sw_email_address] as sweamiladdress
                              ,[sw_phone_number] as swphonenumber
                              ,[care_category_id] carecategoryid
                              ,[care_needs] as careneeds
                              ,[stay_type] as staytype
                              ,[room_location] as roomlocation
                              ,[room_number] as roomnumber
                              ,[adminition_date] as adminitiondate
                              ,[exit_date] as exitdate
                              ,[exit_reason] as exitreason
                              ,[comments] as comments
                              ,[status] as status
                              ,[payment_category] as paymentcategory
                              ,[active] as active
                              ,[updated_by] as updatedby
                              ,[updated_date] as updateddate
                              ,[created_date] as createddate
                        FROM [dbo].[residents]
                        WHERE active = 'Y'";

                conn.Open();
                var result = conn.QueryAsync<Resident>(sql).Result;
                return result;
            }
        }
    }
}