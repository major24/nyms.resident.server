using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Impl
{
    public class EnquiryDataProvider : IEnquiryDataProvider
    {
        private readonly string _connectionString;

        public EnquiryDataProvider(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        }
        public IEnumerable<Enquiry> GetAll()
        {
            // For list page, no need all the fields, 
            // when req for specific enq, should bring all fields
            try
            {
                IEnumerable<Enquiry> enquiries = new List<Enquiry>();
                using (IDbConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"SELECT
                       [reference_id] as referenceid
                      ,[care_home_id] as carehomeid
                      ,[local_authority_id] as localauthorityid
                      ,[fore_name] as forename
                      ,[sur_name] as surname
                      ,[middle_name] as middlename
                      ,[dob] as dob
                      ,[gender] as gender
                      ,[marital_status] as maritalstatus
                      ,[sw_fore_name] as swforname
                      ,[sw_sur_name] as swsurname
                      ,[sw_email_address] as swemailaddress
                      ,[sw_phone_number] as swphonenumber
                      ,[care_category_id] as carecategoryid
                      ,[care_need] as careneed
                      ,[stay_type] as staytype
                      ,[move_in_date] as moveindate
                      ,[reserved_room_location] as reservedroomlocation
                      ,[reserved_room_number] as reservedroomnumber
                      ,[status] as status
                        FROM [dbo].[enquires]
                        WHERE status = 'admit'";

                    conn.Open();
                    var result = conn.QueryAsync<Enquiry>(sql).Result;
                    // todo: create sworker obj and add to enq object...
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching enquires: " + ex.Message);
            }
        }

        public Task<EnquiryEntity> GetByReferenceId(Guid referenceId)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"SELECT
                       [reference_id] as referenceid
                      ,[care_home_id] as carehomeid
                      ,[local_authority_id] as localauthorityid
                      ,[fore_name] as forename
                      ,[sur_name] as surname
                      ,[middle_name] as middlename
                      ,[dob] as dob
                      ,[gender] as gender
                      ,[marital_status] as maritalstatus
                      ,[sw_fore_name] as swforename
                      ,[sw_sur_name] as swsurname
                      ,[sw_email_address] as swemailaddress
                      ,[sw_phone_number] as swphonenumber
                      ,[care_category_id] as carecategoryid
                      ,[care_need] as careneed
                      ,[stay_type] as staytype
                      ,[move_in_date] as moveindate
                      ,[family_home_visit_date] as familyhomevisitdate
                      ,[reserved_room_location] as reservedroomlocation
                      ,[reserved_room_number] as reservedroomnumber
                      ,[response_date] as responsedate
                      ,[response] as response
                      ,[comments] as comments
                      ,[status] as status
                        FROM [dbo].[enquires]
                        WHERE reference_id = @referenceid";

                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);
                    conn.Open();
                    var result = conn.QuerySingleAsync<EnquiryEntity>(sql, dp).Result;
                    return Task.FromResult(result);
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching enquires: " + ex.Message);
            }
        }


        public Task<Enquiry> Create(Enquiry enquiry)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO [dbo].[enquires]
                   ([reference_id]
                   ,[care_home_id]
                   ,[local_authority_id]
                   ,[fore_name]
                   ,[sur_name]
                   ,[middle_name]
                   ,[dob]
                   ,[gender]
                   ,[marital_status]
                   ,[sw_fore_name]
                   ,[sw_sur_name]
                   ,[sw_email_address]
                   ,[sw_phone_number]
                   ,[care_category_id]
                   ,[care_need]
                   ,[stay_type]
                   ,[move_in_date]
                   ,[family_home_visit_date]
                   ,[reserved_room_location]
                   ,[reserved_room_number]
                   ,[response_date]
                   ,[response]
                   ,[comments]
                   ,[status]
                   ,[updated_by_id])
                VALUES
                   (@referenceid
                   ,@carehomeid
                   ,@localauthorityid
                   ,@forename
                   ,@surname
                   ,@middlename
                   ,@dob
                   ,@gender
                   ,@maritalstatus
                   ,@swforename
                   ,@swsurname
                   ,@swemailaddress
                   ,@swphonenumber
                   ,@carecategoryid
                   ,@careneed
                   ,@staytype
                   ,@moveindate
                   ,@familyhomevisitdate
                   ,@reservedroomlocation
                   ,@reservedroomnumber
                   ,@responsedate
                   ,@response
                   ,@comments
                   ,@status
                   ,@updatedbyid)";

                DynamicParameters dp = new DynamicParameters();
                conn.Open();
                dp.Add("referenceid", enquiry.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
                dp.Add("carehomeid", enquiry.CareHomeId, DbType.Int32, ParameterDirection.Input);
                dp.Add("localauthorityid", enquiry.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                dp.Add("forename", enquiry.ForeName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("surname", enquiry.SurName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("middlename", enquiry.MiddleName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("dob", enquiry.Dob, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("gender", enquiry.Gender, DbType.String, ParameterDirection.Input, 80);
                dp.Add("maritalstatus", enquiry.MaritalStatus, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swforename", enquiry.SocialWorker.ForeName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swsurname", enquiry.SocialWorker.SurName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swemailaddress", enquiry.SocialWorker.Email, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swphonenumber", enquiry.SocialWorker.PhoneNumber, DbType.String, ParameterDirection.Input, 80);
                dp.Add("carecategoryid", enquiry.CareCategoryId, DbType.Int32, ParameterDirection.Input);
                dp.Add("careneed", enquiry.CareNeed, DbType.String, ParameterDirection.Input, 80);
                dp.Add("staytype", enquiry.StayType, DbType.String, ParameterDirection.Input, 80);
                dp.Add("moveindate", enquiry.MoveInDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("familyhomevisitdate", enquiry.FamilyHomeVisitDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("reservedroomlocation", enquiry.ReservedRoomLocation, DbType.Int32, ParameterDirection.Input, 80);
                dp.Add("reservedroomnumber", enquiry.ReservedRoomNumber, DbType.Int32, ParameterDirection.Input, 80);
                dp.Add("responsedate", enquiry.ResponseDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("response", enquiry.Response, DbType.String, ParameterDirection.Input, 100);
                dp.Add("comments", enquiry.Comments, DbType.String, ParameterDirection.Input, 500);
                dp.Add("status", enquiry.Status, DbType.String, ParameterDirection.Input, 80);
                dp.Add("updatedbyid", enquiry.UpdatedBy, DbType.Int32, ParameterDirection.Input);
    
                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
            return Task.FromResult(enquiry);
        }


    }
}














/*
 SELECT
                      [reference_id] as referenceid
                      ,[care_home_id] as carehomeid
                      ,[local_authority_id] as localauthorityid
                      ,[fore_name] as forename
                      ,[sur_name] as surname
                      ,[middle_name] as middlename
                      ,[dob] as dob
                      ,[gender] as gender
                      ,[marital_status] as maritalstatus
                      ,[sw_fore_name] as swforname
                      ,[sw_sur_name] as swsurname
                      ,[sw_email_address] as swemailaddress
                      ,[sw_phone_number] as swphonenumber
                      ,[care_category_id] as carecategoryid
                      ,[care_needs] as careneeds
                      ,[stay_type] as staytype
                      ,[move_in_date] as moveindate
                      ,[family_home_visit_date] as familyhomevisitdate
                      ,[reserved_room_location] as reservedroomlocation
                      ,[reserved_room_number] as reservedroomnumber
                      ,[status] as status
                        FROM [dbo].[enquires]
                        WHERE status = 'admit'*/
