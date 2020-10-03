using Dapper;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<Resident> GetResidentsByCareHomeId(int carehomeId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT
                              [reference_id] as referenceid
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
                              ,[care_need] as careneed
                              ,[stay_type] as staytype
                              ,[room_location] as roomlocation
                              ,[room_number] as roomnumber
                              ,[admission_date] as admissiondate
                              ,[exit_date] as exitdate
                              ,[exit_reason] as exitreason
                              ,[comments] as comments
                              ,[status] as status
                              ,[payment_category] as paymentcategory
                              ,[active] as active
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
                        FROM [dbo].[residents]
                        WHERE care_home_id = @carehomeid
                        AND active = 'Y'
                        ORDER BY fore_name";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("carehomeid", carehomeId, DbType.Int32, ParameterDirection.Input);
                conn.Open();
                var result = conn.QueryAsync<Resident>(sql, dp).Result;
                return result;
            }
        }

        public Resident GetResident(Guid referenceId)
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
                              ,[care_need] as careneed
                              ,[stay_type] as staytype
                              ,[room_location] as roomlocation
                              ,[room_number] as roomnumber
                              ,[admission_date] as admissiondate
                              ,[exit_date] as exitdate
                              ,[exit_reason] as exitreason
                              ,[comments] as comments
                              ,[status] as status
                              ,[payment_category] as paymentcategory
                              ,[active] as active
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
                        FROM [dbo].[residents]
                        WHERE reference_id = @referenceid";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input);
                conn.Open();
                var result = conn.QueryFirstOrDefault<Resident>(sql, dp);
                return result;
            }
        }

        public IEnumerable<Resident> GetResidentsForInvoice(DateTime billingStart, DateTime billingEnd)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT
                               [id] as id
                              ,[reference_id] as referenceid
                              ,[local_authority_id] as localauthorityid
                              ,[fore_name] as forename
                              ,[sur_name] as surname
                            ,[created_date] as createddate
                        FROM [dbo].[residents]
                        WHERE [exit_date] >= @billingstart             
                        AND admission_date <= @billingend
                        AND active = 'Y'";
                // rpt begin date
                // rpt end date";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("billingstart", billingStart.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dp.Add("billingend", billingEnd.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                conn.Open();
                var result = conn.QueryAsync<Resident>(sql, dp).Result;
                return result;
            }
        }

        public bool UpdateExitDate(Guid referenceId, DateTime exitDate)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE [dbo].[residents] 
                                SET exit_date = @exitdate,
                                    updated_date = GETDATE()
                        WHERE [reference_id] = @referenceId";

                string sql2 = @"UPDATE [dbo].[schedules] 
                            SET schedule_end_date = @exitdate,
                            updated_date = GETDATE()
                            WHERE resident_id = (SELECT id FROM [dbo].[residents]
                                                WHERE reference_id = @referenceid)";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("exitdate", exitDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

                DynamicParameters dp2 = new DynamicParameters();
                dp2.Add("exitdate", exitDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dp2.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

                conn.Open();
                using(var tran = conn.BeginTransaction())
                {
                    var affRows = conn.Execute(sql, dp, transaction: tran);
                    var affRows2 = conn.Execute(sql2, dp2, transaction: tran);

                    tran.Commit();
                    return true;
                }
            }
        }

        public Task<ResidentEntity> Create(ResidentEntity resident)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"INSERT INTO [dbo].[residents]
                   ([reference_id]
                   ,[care_home_id]
                   ,[local_authority_id]
                   ,[nhs_number]
                   ,[po_number]
                   ,[la_id]
                   ,[nyms_id]
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
                   ,[room_location]
                   ,[room_number]
                   ,[admission_date]
                   ,[exit_date]
                   ,[comments]
                   ,[status]
                   ,[updated_by_id])
                VALUES
                   (@referenceid
                   ,@carehomeid
                   ,@localauthorityid
                   ,@nhsnumber
                   ,@ponumber
                   ,@laid
                   ,@nymsid
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
                   ,@roomlocation
                   ,@roomnumber
                   ,@admissiondate
                   ,@exitdate
                   ,@comments
                   ,@status
                   ,@updatedbyid)";

                DynamicParameters dp = new DynamicParameters();
                conn.Open();
                dp.Add("referenceid", resident.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
                dp.Add("carehomeid", resident.CareHomeId, DbType.Int32, ParameterDirection.Input);
                dp.Add("localauthorityid", resident.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
                dp.Add("nhsnumber", resident.NhsNumber, DbType.String, ParameterDirection.Input, 20);
                dp.Add("ponumber", resident.PoNumber, DbType.String, ParameterDirection.Input, 20);
                dp.Add("laid", resident.LaId, DbType.String, ParameterDirection.Input, 20);
                dp.Add("nymsid", resident.NymsId, DbType.String, ParameterDirection.Input, 20);
                dp.Add("forename", resident.ForeName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("surname", resident.SurName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("middlename", resident.MiddleName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("dob", resident.Dob, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("gender", resident.Gender, DbType.String, ParameterDirection.Input, 80);
                dp.Add("maritalstatus", resident.MaritalStatus, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swforename", resident.SwForeName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swsurname", resident.SwSurName, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swemailaddress", resident.SwEmailAddress, DbType.String, ParameterDirection.Input, 80);
                dp.Add("swphonenumber", resident.SwPhoneNumber, DbType.String, ParameterDirection.Input, 80);
                dp.Add("carecategoryid", resident.CareCategoryId, DbType.Int32, ParameterDirection.Input);
                dp.Add("careneed", resident.CareNeed, DbType.String, ParameterDirection.Input, 80);
                dp.Add("staytype", resident.StayType, DbType.String, ParameterDirection.Input, 80);
                dp.Add("roomlocation", resident.RoomLocation, DbType.Int32, ParameterDirection.Input, 80);
                dp.Add("roomnumber", resident.RoomNumber, DbType.Int32, ParameterDirection.Input, 80);
                dp.Add("admissiondate", resident.AdmissionDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("exitdate", resident.ExitDate, DbType.Date, ParameterDirection.Input, 80);
                dp.Add("comments", resident.Comments, DbType.String, ParameterDirection.Input, 500);
                dp.Add("status", resident.Status, DbType.String, ParameterDirection.Input, 80);
                dp.Add("updatedbyid", resident.UpdatedById, DbType.Int32, ParameterDirection.Input);

                var result = conn.Execute(sql, dp, commandType: CommandType.Text);
            }
            return Task.FromResult(resident);
        }

        public Task<ResidentEntity> Update(ResidentEntity resident)
        {
            throw new NotImplementedException();
        }
    }
}