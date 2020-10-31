using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                       e.id as id
                      ,e.reference_id as referenceid
                      ,[care_home_id] as carehomeid
                      ,[referral_agency_id] as referralagencyid
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
						LEFT JOIN [dbo].[local_authorities] la
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
                       [id] as id
                      ,[reference_id] as referenceid
                      ,[care_home_id] as carehomeid
                      ,[referral_agency_id] as referralagencyid
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


        public Task<int> Create(Enquiry enquiry)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO [dbo].[enquires]
                   ([reference_id]
                   ,[care_home_id]
                   ,[referral_agency_id]
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
                   ,[comments]
                   ,[status]
                   ,[updated_by_id])
                VALUES
                   (@referenceid
                   ,@carehomeid
                   ,@referralagencyid
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
                   ,@comments
                   ,@status
                   ,@updatedbyid);
                SELECT CAST(SCOPE_IDENTITY() as int)";



                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", enquiry.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
                dp.Add("carehomeid", enquiry.CareHomeId, DbType.Int32, ParameterDirection.Input);
                dp.Add("referralagencyid", enquiry.ReferralAgencyId, DbType.Int32, ParameterDirection.Input);
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
/*                dp.Add("street", enquiry.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                dp.Add("city", enquiry.Address.City, DbType.String, ParameterDirection.Input, 60);
                dp.Add("county", enquiry.Address.County, DbType.String, ParameterDirection.Input, 30);
                dp.Add("postcode", enquiry.Address.PostCode, DbType.String, ParameterDirection.Input, 20);
                dp.Add("nokforename", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("noksurname", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokemailaddress", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokphonenumber", "", DbType.String, ParameterDirection.Input, 20);
                dp.Add("responsedate", enquiry.ResponseDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("response", enquiry.Response, DbType.String, ParameterDirection.Input, 100);*/
                dp.Add("comments", enquiry.Comments, DbType.String, ParameterDirection.Input, 500);
                dp.Add("status", enquiry.Status, DbType.String, ParameterDirection.Input, 80);
                dp.Add("updatedbyid", enquiry.UpdatedBy, DbType.Int32, ParameterDirection.Input);

                conn.Open();
                var result = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text);
                return Task.FromResult(result);

                // previous impl of add actions along with enquiry. see below as commented out
            }
        }

        public Task<Enquiry> Update(Enquiry enquiry)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE [dbo].[enquires]
                SET [care_home_id] = @carehomeid
                   ,[referral_agency_id] = @referralagencyid
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
                dp.Add("referralagencyid", enquiry.ReferralAgencyId, DbType.Int32, ParameterDirection.Input);
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
/*                dp.Add("street", enquiry.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                dp.Add("city", enquiry.Address.City, DbType.String, ParameterDirection.Input, 60);
                dp.Add("county", enquiry.Address.County, DbType.String, ParameterDirection.Input, 30);
                dp.Add("postcode", enquiry.Address.PostCode, DbType.String, ParameterDirection.Input, 20);
                dp.Add("nokforename", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("noksurname", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokemailaddress", "", DbType.String, ParameterDirection.Input, 60);
                dp.Add("nokphonenumber", "", DbType.String, ParameterDirection.Input, 20);
                dp.Add("responsedate", enquiry.ResponseDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("response", enquiry.Response, DbType.String, ParameterDirection.Input, 100);*/
                dp.Add("comments", enquiry.Comments, DbType.String, ParameterDirection.Input, 500);
                dp.Add("status", enquiry.Status, DbType.String, ParameterDirection.Input, 80);
                dp.Add("updatedbyid", enquiry.UpdatedBy, DbType.Int32, ParameterDirection.Input);

                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
            return Task.FromResult(enquiry);
        }

        public IEnumerable<EnquiryAction> GetActions(int enquiryId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [id] as id
                              ,[enquiry_id] as enquiryid
                              ,[action] as action
                              ,[action_date] as actiondate
                              ,[response] as response
                              ,[status] as status
                              ,[updated_date] as updateddate
                              ,[created_date] as createddate
                              FROM[dbo].[enquiry_actions]
                              WHERE [enquiry_id] = @enquiryid";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("enquiryid", enquiryId, DbType.Int32, ParameterDirection.Input);

                conn.Open();
                var results = conn.Query<EnquiryAction>(sql, dp);
                return results;
            }
        }

        public void SaveActions(int enquiryId, EnquiryAction[] enquiryActions)
        {
            string sqlActInsert = @"INSERT INTO [dbo].[enquiry_actions]
                                   ([enquiry_id]
                                   ,[action]
                                   ,[action_date]
                                   ,[response]
                                   ,[status])
                                VALUES
                                   (@enquiryid
                                   ,@action
                                   ,@actiondate
                                   ,@response
                                   ,@status)";

            string sqlActUpdate = @"UPDATE [dbo].[enquiry_actions]
                                   SET [action] = @action
                                   ,[response] = @response
                                   ,[status] = @status
                                   ,[updated_date] = GETDATE()
                                    WHERE id = @id";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    foreach (var act in enquiryActions)
                    {
                        DynamicParameters dpAction = new DynamicParameters();
                        string sql = sqlActUpdate;
                        if (act.Id == 0)
                        {
                            dpAction.Add("enquiryid", act.EnquiryId, DbType.Int32, ParameterDirection.Input);
                            dpAction.Add("actiondate", act.ActionDate, DbType.DateTime, ParameterDirection.Input);
                            sql = sqlActInsert;
                        }
                        dpAction.Add("id", act.Id, DbType.Int32, ParameterDirection.Input);
                        dpAction.Add("action", act.Action, DbType.String, ParameterDirection.Input);
                        dpAction.Add("response", act.Response, DbType.String, ParameterDirection.Input);
                        dpAction.Add("status", act.Status, DbType.String, ParameterDirection.Input);
                        var result2 = conn.Execute(sql, dpAction, commandType: CommandType.Text, transaction: tran);
                    }
                    tran.Commit();
                }
            }
        }
    }
}

// get by ids
// var results = conn.Query<EnquiryAction>(sql, new { ids = residentIds });
// return results;


// add actions along with enquiry
/*                string sqlAction = @"INSERT INTO [dbo].[enquiry_actions]
                               ([enquiry_id]
                               ,[action]
                               ,[action_date]
                               ,[response]
                               ,[status])
                            VALUES
                               (@enquiryid
                               ,@action
                               ,@actiondate
                               ,@response
                               ,@status)";*/
/*          
*          using (var tran = conn.BeginTransaction())
    {
        var result = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);

        foreach (var act in enquiry.EnquiryActions)
        {
            DynamicParameters dpAction = new DynamicParameters();
            dpAction.Add("enquiryid", result, DbType.Int32, ParameterDirection.Input);
            dpAction.Add("action", act.Action, DbType.String, ParameterDirection.Input);
            dpAction.Add("actiondate", act.ActionDate, DbType.DateTime, ParameterDirection.Input);
            dpAction.Add("response", act.Response, DbType.String, ParameterDirection.Input);
            dpAction.Add("status", act.Status, DbType.String, ParameterDirection.Input);
            var result2 = conn.Execute(sqlAction, dpAction, commandType: CommandType.Text, transaction: tran);
        }
        tran.Commit();
        return Task.FromResult(result);
    }*/

