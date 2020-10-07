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
                string status = EnquiryStatus.active.ToString();
                IEnumerable<Enquiry> enquiries = new List<Enquiry>();
                using (IDbConnection conn = new SqlConnection(_connectionString))
                {
                    string sql = @"SELECT
                       e.reference_id as referenceid
                      ,[care_home_id] as carehomeid
                      ,[local_authority_id] as localauthorityid
                      ,[forename] as forename
                      ,[surname] as surname
                      ,[middle_name] as middlename
                      ,[dob] as dob
                      ,[gender] as gender
                      ,[marital_status] as maritalstatus
                      ,[sw_forename] as swforname
                      ,[sw_surname] as swsurname
                      ,[sw_email_address] as swemailaddress
                      ,[sw_phone_number] as swphonenumber
                      ,[care_category_id] as carecategoryid
                      ,[care_need] as careneed
                      ,[stay_type] as staytype
                      ,[move_in_date] as moveindate
                      ,[reserved_room_location] as reservedroomlocation
                      ,[reserved_room_number] as reservedroomnumber
                      ,[status] as status
                      ,[updated_date] as updateddate
					  ,cc.name as carecategoryname
					  ,la.name as localauthorityname
                        FROM [dbo].[enquires] e
						INNER JOIN [dbo].[local_authorities] la
						ON e.local_authority_id = la.id
						LEFT JOIN [dbo].[care_categories] cc
						ON e.care_category_id = cc.id
                        WHERE status = @status";

                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("status", status, DbType.String, ParameterDirection.Input, 60);
                    conn.Open();
                    var result = conn.QueryAsync<Enquiry>(sql, dp).Result;
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
                      ,[forename] as forename
                      ,[surname] as surname
                      ,[middle_name] as middlename
                      ,[dob] as dob
                      ,[gender] as gender
                      ,[marital_status] as maritalstatus
                      ,[sw_forename] as swforename
                      ,[sw_surname] as swsurname
                      ,[sw_email_address] as swemailaddress
                      ,[sw_phone_number] as swphonenumber
                      ,[care_category_id] as carecategoryid
                      ,[care_need] as careneed
                      ,[stay_type] as staytype
                      ,[move_in_date] as moveindate
                      ,[family_home_visit_date] as familyhomevisitdate
                      ,[reserved_room_location] as reservedroomlocation
                      ,[reserved_room_number] as reservedroomnumber
                      ,[street] as street
                      ,[city] as city
                      ,[county] as county
                      ,[postcode] as postcode
                      ,[nok_forename] as nok_forename
                      ,[nok_surname] as nok_surname
                      ,[nok_email_address] as nok_email_address
                      ,[nok_phone_number] as nok_phone_number
                      ,[response_date] as responsedate
                      ,[response] as response
                      ,[comments] as comments
                      ,[status] as status
                      ,[updated_date] as updateddate
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
                   ,[forename]
                   ,[surname]
                   ,[middle_name]
                   ,[dob]
                   ,[gender]
                   ,[marital_status]
                   ,[sw_forename]
                   ,[sw_surname]
                   ,[sw_email_address]
                   ,[sw_phone_number]
                   ,[care_category_id]
                   ,[care_need]
                   ,[stay_type]
                   ,[move_in_date]
                   ,[family_home_visit_date]
                   ,[reserved_room_location]
                   ,[reserved_room_number]
                   ,[street]
                   ,[city]
                   ,[county]
                   ,[postcode]
                   ,[nok_forename]
                   ,[nok_surname]
                   ,[nok_email_address]
                   ,[nok_phone_number]
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
                   ,@street
                   ,@city
                   ,@county
                   ,@postcode
                   ,@nokforename
                   ,@noksurname
                   ,@nokemailaddress
                   ,@nokphonenumber
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
                dp.Add("street", enquiry.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                dp.Add("city", enquiry.Address.City, DbType.String, ParameterDirection.Input, 60);
                dp.Add("county", enquiry.Address.County, DbType.String, ParameterDirection.Input, 30);
                dp.Add("postcode", enquiry.Address.PostCode, DbType.String, ParameterDirection.Input, 20);
                dp.Add("nokforename", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("noksurname", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokemailaddress", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokphonenumber", "", DbType.String, ParameterDirection.Input, 20);
                dp.Add("responsedate", enquiry.ResponseDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("response", enquiry.Response, DbType.String, ParameterDirection.Input, 100);
                dp.Add("comments", enquiry.Comments, DbType.String, ParameterDirection.Input, 500);
                dp.Add("status", enquiry.Status, DbType.String, ParameterDirection.Input, 80);
                dp.Add("updatedbyid", enquiry.UpdatedBy, DbType.Int32, ParameterDirection.Input);
    
                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
            return Task.FromResult(enquiry);
        }

        public Task<Enquiry> Update(Enquiry enquiry)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE [dbo].[enquires]
                SET [care_home_id] = @carehomeid
                   ,[local_authority_id] = @localauthorityid
                   ,[forename] = @forename
                   ,[surname] = @surname
                   ,[middle_name] = @middlename
                   ,[dob] = @dob
                   ,[gender] = @gender
                   ,[marital_status] = @maritalstatus
                   ,[sw_forename] = @swforename
                   ,[sw_surname] = @swsurname
                   ,[sw_email_address] = @swemailaddress
                   ,[sw_phone_number] = @swphonenumber
                   ,[care_category_id] = @carecategoryid
                   ,[care_need] = @careneed
                   ,[stay_type] = @staytype
                   ,[move_in_date] = @moveindate
                   ,[family_home_visit_date] = @familyhomevisitdate
                   ,[reserved_room_location] = @reservedroomlocation
                   ,[reserved_room_number] = @reservedroomnumber
                   ,[street] = @street
                   ,[city] = @city
                   ,[county] = @county
                   ,[postcode] = @postcode
                   ,[nok_forename] = @nokforename
                   ,[nok_surname] = @noksurname
                   ,[nok_email_address] = @nokemailaddress
                   ,[nok_phone_number] = @nokphonenumber
                   ,[response_date] = @responsedate
                   ,[response] = @response
                   ,[comments] = @comments
                   ,[status] = @status
                   ,[updated_by_id] = @updatedbyid
                   ,[updated_date] = GETDATE()
                WHERE [reference_id] = @referenceid";

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
                dp.Add("street", enquiry.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                dp.Add("city", enquiry.Address.City, DbType.String, ParameterDirection.Input, 60);
                dp.Add("county", enquiry.Address.County, DbType.String, ParameterDirection.Input, 30);
                dp.Add("postcode", enquiry.Address.PostCode, DbType.String, ParameterDirection.Input, 20);
                dp.Add("nokforename", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("noksurname", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokemailaddress", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokphonenumber", "", DbType.String, ParameterDirection.Input, 20);
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


