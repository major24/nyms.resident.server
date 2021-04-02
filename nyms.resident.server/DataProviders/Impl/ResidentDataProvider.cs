using Dapper;
using Microsoft.Ajax.Utilities;
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
    public class ResidentDataProvider : IResidentDataProvider
    {
        private readonly string _connectionString;
        private readonly IResidentContactDataProvider _residentContactDataProvider;
        private readonly ISocialWorkerDataProvider _socialWorkerDataProvider;

        public ResidentDataProvider(string connectionString,
            IResidentContactDataProvider residentContactDataProvider,
            ISocialWorkerDataProvider socialWorkerDataProvider)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _residentContactDataProvider = residentContactDataProvider ?? throw new ArgumentNullException(nameof(residentContactDataProvider));
            _socialWorkerDataProvider = socialWorkerDataProvider ?? throw new ArgumentNullException(nameof(socialWorkerDataProvider));
        }

        public IEnumerable<Resident> GetResidents()
        {
            string sql = @"SELECT
                               r.id as id
                              ,r.reference_id as referenceid
                              ,r.care_home_id as carehomeid
                              ,r.local_authority_id as localauthorityid
                              ,[nhs_number] as nhsnumber
                              ,[po_number] as ponumber
                              ,[la_id] as laid
                              ,[nyms_id] as nymsid
                              ,[forename] as forename
                              ,[surname] as surname
                              ,[middle_name] as middlename
                              ,[dob] as dob
                              ,[gender] as gender
                              ,[marital_status] as maritalstatus
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
                              ,r.active as active
                              ,[updated_by_id] as updatedbyid
                              ,[updated_date] as updateddate
							  ,[care_home_division_id] as carehomedivisionid
                              ,[discharged_from_home_date] as dischargedfromhomedate
							  ,la.name as localauthorityname
							  ,chd.name as carehomedivisionname
                        FROM [dbo].[residents] r
						INNER JOIN [dbo].[local_authorities] la
						ON la.id = r.local_authority_id
						INNER JOIN [dbo].[care_home_divisions] chd
						ON chd.id = r.care_home_division_id
                            ORDER BY forename";

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = conn.QueryAsync<Resident>(sql).Result;
                return result;
            }
        }

        public Resident GetResident(Guid referenceId)
        {
            // Get resident and other objects and assemble here...
            Resident resident = _GetResident(referenceId);
            if (resident == null) return null;

            int residentId = resident.Id;
            // Get all contacts and addresses
            var addresses = GetAddressesByResidentId(residentId);
            var residentContacts = _residentContactDataProvider.GetResidentContactsByResidentId(residentId);  //GetResidentContactsByResidentId(residentId);
            var nextofkins = GetNextOfKinsByResidentId(residentId);
            var socialWorker = _socialWorkerDataProvider.GetSocialWorkerByResidentId(residentId); // GetSocialWorker(residentId);

            // assign resident address, email and phone
            if (addresses.Any())
            {
                resident.Address = addresses.Where(a => a.ResidentId == residentId).FirstOrDefault();
            }
            else
            {
                resident.Address = new Address();
            }
            
            if (residentContacts.Any())
            {
                residentContacts.ForEach(rc =>
                {
                    if (rc.ContactType == CONTACT_TYPE.email.ToString())
                    {
                        resident.EmailAddress = rc.Data;
                    }
                    if (rc.ContactType == CONTACT_TYPE.phone.ToString())
                    {
                        resident.PhoneNumber = rc.Data;
                    }
                });
            }

            if (socialWorker != null)
            {
                resident.SocialWorker = new SocialWorker()
                {
                    ForeName = socialWorker.ForeName,
                    SurName = socialWorker.SurName,
                    EmailAddress = socialWorker.EmailAddress,
                    PhoneNumber = socialWorker.PhoneNumber
                };
            }
            return resident;
        }

        public bool DischargeResident(Guid referenceId, DateTime dischargeDate)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE [dbo].[residents] 
                                SET discharged_from_home_date = @dischargedfromhomedate,
                                    active = 'N',
                                    updated_date = GETDATE()
                        WHERE [reference_id] = @referenceId";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("dischargedfromhomedate", dischargeDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

                conn.Open();
                var affRows = conn.Execute(sql, dp);
                return true;
            }
        }

        public bool ExitInvoiceResident(Guid referenceId, DateTime exitDate)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE [dbo].[residents] 
                                SET exit_date = @exitdate,
                                    active = 'N',
                                    updated_date = GETDATE()
                        WHERE [reference_id] = @referenceId";

                string sqlSchUpdate = @"UPDATE [dbo].[schedules] 
                            SET schedule_end_date = @exitdate,
                            updated_date = GETDATE()
                            WHERE resident_id = (SELECT id FROM [dbo].[residents]
                                                WHERE reference_id = @referenceid)
                                  AND (schedule_end_date between '9999-12-31' AND '9999-12-31 23:59:59')";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("exitdate", exitDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

                DynamicParameters dpSch = new DynamicParameters();
                dpSch.Add("exitdate", exitDate.ToString("yyyy-MM-dd"), DbType.String, ParameterDirection.Input);
                dpSch.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var affRows = conn.Execute(sql, dp, transaction: tran);
                    var affRows2 = conn.Execute(sqlSchUpdate, dpSch, transaction: tran);
                    tran.Commit();
                    return true;
                }
            }
        }
        
        public bool ActivateResident(Guid referenceId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"UPDATE [dbo].[residents] 
                                SET exit_date = '9999-12-31',
                                    active = 'Y',
                                    updated_date = GETDATE()
                        WHERE [reference_id] = @referenceid";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("referenceid", referenceId, DbType.Guid, ParameterDirection.Input, 60);

                conn.Open();
                var affRows = conn.Execute(sql, dp);
                return true;
            }
        }
        
        public Task<ResidentEntity> Create(ResidentEntity residentEntity)
        {
            if (residentEntity == null)
                throw new ArgumentNullException("Resident and Enquriy entities required");

            string sql = @"INSERT INTO [dbo].[residents]
                   ([reference_id]
                   ,[care_home_id]
                   ,[local_authority_id]
                   ,[nhs_number]
                   ,[po_number]
                   ,[la_id]
                   ,[nyms_id]
                   ,[forename]
                   ,[surname]
                   ,[middle_name]
                   ,[dob]
                   ,[gender]
                   ,[marital_status]
                   ,[care_category_id]
                   ,[care_need]
                   ,[stay_type]
                   ,[room_location]
                   ,[room_number]
                   ,[admission_date]
                   ,[exit_date]
                   ,[comments]
                   ,[status]
                   ,[updated_by_id]
                   ,[enquiry_ref_id])
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
                   ,@carecategoryid
                   ,@careneed
                   ,@staytype
                   ,@roomlocation
                   ,@roomnumber
                   ,@admissiondate
                   ,@exitdate
                   ,@comments
                   ,@status
                   ,@updatedbyid
                   ,@enquiryrefid);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
            string sqlInsertAddress = @"INSERT INTO [dbo].[resident_addresses]
                                       ([resident_id]
                                       ,[addr_type]
                                       ,[street1]
                                       ,[street2]
                                       ,[city]
                                       ,[county]
                                       ,[post_code])
                                 VALUES
                                       (@residentid
                                       ,@addrtype
                                       ,@street1
                                       ,@street2
                                       ,@city
                                       ,@county
                                       ,@postcode)";
            string sqlInsertResidentContact = @"INSERT INTO [dbo].[resident_contacts]
                                       ([resident_id]
                                       ,[contact_type]
                                       ,[data])
                                 VALUES
                                       (@residentid
                                       ,@contacttype
                                       ,@data)";
            string sqlInsertNok = @"INSERT INTO [dbo].[next_of_kin]
                                       ([resident_id]
                                       ,[forename]
                                       ,[surname]
                                       ,[relationship]
                                       ,[updated_by_id])
                                 VALUES
                                       (@residentid
                                       ,@forename
                                       ,@surname
                                       ,@relationship
                                       ,@updatedbyid);
                                        SELECT CAST(SCOPE_IDENTITY() as int);";
            string sqlInsertSocialWorker = @"INSERT INTO [dbo].[social_workers]
                                       ([resident_id]
                                       ,[forename]
                                       ,[surname]
                                       ,[email_address]
                                       ,[phone_number])
                                 VALUES
                                       (@residentid
                                       ,@forename
                                       ,@surname
                                       ,@emailaddress
                                       ,@phonenumber);";
            string sqlEnqUpdate = @"UPDATE [dbo].[enquires] 
                                        SET status = @status
                                            ,[updated_by_id] = @updatedbyid
                                            ,updated_date = GETDATE()
                                        WHERE [reference_id] = @referenceid";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("referenceid", residentEntity.ReferenceId, DbType.Guid, ParameterDirection.Input, 80);
            dp.Add("carehomeid", residentEntity.CareHomeId, DbType.Int32, ParameterDirection.Input);
            dp.Add("localauthorityid", residentEntity.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
            dp.Add("nhsnumber", residentEntity.NhsNumber, DbType.String, ParameterDirection.Input, 20);
            dp.Add("ponumber", residentEntity.PoNumber, DbType.String, ParameterDirection.Input, 20);
            dp.Add("laid", residentEntity.LaId, DbType.String, ParameterDirection.Input, 20);
            dp.Add("nymsid", residentEntity.NymsId, DbType.String, ParameterDirection.Input, 20);
            dp.Add("forename", residentEntity.ForeName.Trim(), DbType.String, ParameterDirection.Input, 80);
            dp.Add("surname", residentEntity.SurName.Trim(), DbType.String, ParameterDirection.Input, 80);
            dp.Add("middlename", residentEntity.MiddleName, DbType.String, ParameterDirection.Input, 80);
            dp.Add("dob", residentEntity.Dob, DbType.Date, ParameterDirection.Input, 80);
            dp.Add("gender", residentEntity.Gender, DbType.String, ParameterDirection.Input, 80);
            dp.Add("maritalstatus", residentEntity.MaritalStatus, DbType.String, ParameterDirection.Input, 80);
            dp.Add("carecategoryid", residentEntity.CareCategoryId, DbType.Int32, ParameterDirection.Input);
            dp.Add("careneed", residentEntity.CareNeed, DbType.String, ParameterDirection.Input, 80);
            dp.Add("staytype", residentEntity.StayType, DbType.String, ParameterDirection.Input, 80);
            dp.Add("roomlocation", residentEntity.RoomLocation, DbType.Int32, ParameterDirection.Input, 80);
            dp.Add("roomnumber", residentEntity.RoomNumber, DbType.Int32, ParameterDirection.Input, 80);
            dp.Add("admissiondate", residentEntity.AdmissionDate, DbType.Date, ParameterDirection.Input, 80);
            dp.Add("exitdate", residentEntity.ExitDate, DbType.Date, ParameterDirection.Input, 80);
            dp.Add("comments", residentEntity.Comments, DbType.String, ParameterDirection.Input, 500);
            dp.Add("status", residentEntity.Status, DbType.String, ParameterDirection.Input, 80);
            dp.Add("updatedbyid", residentEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("enquiryrefid", residentEntity.EnquiryReferenceId, DbType.Guid, ParameterDirection.Input, 80);

            DynamicParameters dpAddrResident = new DynamicParameters();
            if (residentEntity.Address != null)
            {
                dpAddrResident.Add("@addrtype", residentEntity.Address.AddrType, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("@street1", residentEntity.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                dpAddrResident.Add("@street2", residentEntity.Address.Street2, DbType.String, ParameterDirection.Input, 100);
                dpAddrResident.Add("@city", residentEntity.Address.City, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("@county", residentEntity.Address.County, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("@postcode", residentEntity.Address.PostCode, DbType.String, ParameterDirection.Input, 80);
            }

            // turing enq into resident
            DynamicParameters dpEnq = new DynamicParameters();
            dpEnq.Add("referenceid", residentEntity.EnquiryReferenceId, DbType.Guid, ParameterDirection.Input, 80);
            dpEnq.Add("status", ENQUIRY_STATUS.admit.ToString(), DbType.String, ParameterDirection.Input, 80);
            dpEnq.Add("updatedbyid", residentEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var newResidentId = conn.QuerySingle<int>(sql, dp, commandType: CommandType.Text, transaction: tran);
                    // resident address..
                    if (residentEntity.Address != null)
                    {
                        dpAddrResident.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                        var affRowsAddr = conn.Execute(sqlInsertAddress, dpAddrResident, transaction: tran);
                    }
                    
                    // resident contacts..// contacts are, list of contacts for res and noks
                    // REDONE-NOW resident contact is JUST email and phone field
                    if (!string.IsNullOrEmpty(residentEntity.EmailAddress))
                    {
                        DynamicParameters dpCnt = new DynamicParameters();
                        dpCnt.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                        dpCnt.Add("contacttype", CONTACT_TYPE.email.ToString(), DbType.String, ParameterDirection.Input, 80);
                        dpCnt.Add("data", residentEntity.EmailAddress, DbType.String, ParameterDirection.Input, 100);
                        var affRowsCnt = conn.Execute(sqlInsertResidentContact, dpCnt, transaction: tran);
                    }
                    if (!string.IsNullOrEmpty(residentEntity.PhoneNumber))
                    {
                        DynamicParameters dpCnt = new DynamicParameters();
                        dpCnt.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                        dpCnt.Add("contacttype", CONTACT_TYPE.phone.ToString(), DbType.String, ParameterDirection.Input, 80);
                        dpCnt.Add("data", residentEntity.PhoneNumber, DbType.String, ParameterDirection.Input, 100);
                        var affRowsCnt = conn.Execute(sqlInsertResidentContact, dpCnt, transaction: tran);
                    }
                    // social worker
                    if (residentEntity.SocialWorker != null && !string.IsNullOrEmpty(residentEntity.SocialWorker.ForeName))
                    {
                        DynamicParameters dpSw = new DynamicParameters();
                        dpSw.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                        dpSw.Add("forename", residentEntity.SocialWorker.ForeName, DbType.String, ParameterDirection.Input, 80);
                        dpSw.Add("surname", residentEntity.SocialWorker.SurName, DbType.String, ParameterDirection.Input, 80);
                        dpSw.Add("emailaddress", residentEntity.SocialWorker.EmailAddress, DbType.String, ParameterDirection.Input, 200);
                        dpSw.Add("phonenumber", residentEntity.SocialWorker.PhoneNumber, DbType.String, ParameterDirection.Input, 40);
                        var affRowsSw = conn.Execute(sqlInsertSocialWorker, dpSw, transaction: tran);
                    }
                    // noks, list of noks
                    if (residentEntity.NextOfKins != null && residentEntity.NextOfKins.Any())
                    {
                        DynamicParameters dpNok = new DynamicParameters();
                        residentEntity.NextOfKins.ForEach(nok =>
                        {
                            int newNokId = 0;
                            dpNok.Add("forename", nok.ForeName, DbType.String, ParameterDirection.Input, 80);
                            dpNok.Add("surname", nok.SurName, DbType.String, ParameterDirection.Input, 80);
                            dpNok.Add("relationship", nok.Relationship, DbType.String, ParameterDirection.Input, 80);
                            dpNok.Add("updatedbyid", residentEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);

                            dpNok.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                            newNokId = conn.QuerySingle<int>(sqlInsertNok, dpNok, commandType: CommandType.Text, transaction: tran);
                            // add nok address to address table
                            // TODO: CREATE a new table for NOK Address and save it. Below is OLD code
/*                            if (nok.Address != null && nok.Address.Street1 != "") // ensure atleast one field is filled in?
                            {
                                DynamicParameters dpAddrNok = new DynamicParameters();
                                dpAddrNok.Add("@addrtype", nok.Address.AddrType, DbType.String, ParameterDirection.Input, 80);
                                dpAddrNok.Add("@street1", nok.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                                dpAddrNok.Add("@street2", nok.Address.Street2, DbType.String, ParameterDirection.Input, 100);
                                dpAddrNok.Add("@city", nok.Address.City, DbType.String, ParameterDirection.Input, 80);
                                dpAddrNok.Add("@county", nok.Address.County, DbType.String, ParameterDirection.Input, 80);
                                dpAddrNok.Add("@postcode", nok.Address.PostCode, DbType.String, ParameterDirection.Input, 80);
                                dpAddrNok.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                                var affRowsNokCnt = conn.Execute(sqlInsertAddress, dpAddrNok, transaction: tran);
                            }*/
                            // add nok contact to contacts table
/*                            if (nok.ContactInfos != null && nok.ContactInfos.Any())
                            {
                                DynamicParameters dpCnt = new DynamicParameters();
                                nok.ContactInfos.ForEach(nokCt =>
                                {
                                    dpCnt.Add("reftype", nokCt.RefType, DbType.String, ParameterDirection.Input, 80);
                                    dpCnt.Add("contacttype", nokCt.ContactType, DbType.String, ParameterDirection.Input, 80);
                                    dpCnt.Add("data", nokCt.Data, DbType.String, ParameterDirection.Input, 100);

                                    dpCnt.Add("nokid", newNokId, DbType.Int32, ParameterDirection.Input);
                                    dpCnt.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                                    var affRowsNokCnt = conn.Execute(sqlInsertContact, dpCnt, transaction: tran);
                                });
                            }*/
                        });
                    }

                    // update enquiry status with admit
                    var enqUpdated = conn.Execute(sqlEnqUpdate, dpEnq, commandType: CommandType.Text, transaction: tran);
                    
                    tran.Commit();
                }
                return Task.FromResult(residentEntity);
            }
        }

        public Task<ResidentEntity> Update(ResidentEntity residentEntity)
        {
            if (residentEntity == null)
                throw new ArgumentNullException("Resident and Enquriy entities required");

            string sql = @"UPDATE [dbo].[residents] SET
                    [care_home_id] = @carehomeid
                   ,[local_authority_id] = @localauthorityid
                   ,[nhs_number] = @nhsnumber
                   ,[po_number] = @ponumber
                   ,[la_id] = @laid
                   ,[nyms_id] = @nymsid
                   ,[forename] = @forename
                   ,[surname] = @surname
                   ,[middle_name] = @middlename
                   ,[dob] = @dob
                   ,[gender] = @gender
                   ,[marital_status] = @maritalstatus
                   ,[care_category_id] = @carecategoryid
                   ,[care_need] = @careneed
                   ,[stay_type] = @staytype
                   ,[room_location] = @roomlocation
                   ,[room_number] = @roomnumber
                   ,[admission_date] = @admissiondate
                   ,[exit_date] = @exitdate
                   ,[comments] = @comments
                   ,[status] = @status
                   ,[updated_by_id] = @updatedbyid
                WHERE [id] = @id";

            string sqlUpdateAddress = @"UPDATE [dbo].[resident_addresses] SET
                                        [addr_type] = @addrtype
                                       ,[street1] = @street1
                                       ,[street2] = @street2
                                       ,[city] = @city
                                       ,[county] = @county
                                       ,[post_code] = @postcode
                                    WHERE [id] = @id"; // id of address table

            string sqlInsertAddress = @"INSERT INTO [dbo].[resident_addresses]
                                       ([resident_id]
                                       ,[addr_type]
                                       ,[street1]
                                       ,[street2]
                                       ,[city]
                                       ,[county]
                                       ,[post_code])
                                 VALUES
                                       (@residentid
                                       ,@addrtype
                                       ,@street1
                                       ,@street2
                                       ,@city
                                       ,@county
                                       ,@postcode)";

            string sqlUpdateResidentContact = @"UPDATE [dbo].[resident_contacts] SET
                                            [data] = @data
                                        WHERE [id] = @id"; // id of address table

            string sqlInsertResidentContact = @"INSERT INTO [dbo].[resident_contacts]
                                       ([resident_id]
                                       ,[contact_type]
                                       ,[data])
                                 VALUES
                                       (@residentid
                                       ,@contacttype
                                       ,@data)";

            // TODO LATER
            string sqlUpdateNok = @"UPDATE [dbo].[next_of_kin] SET
                                        [forename] = @forename
                                       ,[surname] = @surname
                                       ,[relationship] = @relationship
                                       ,[updated_by_id] = @updatedbyid";

            string sqlUpdateSocialWorker = @"UPDATE [dbo].[social_workers] SET
                                        [forename] = @forename
                                       ,[surname] = @surname
                                       ,[email_address] = @emailaddress
                                       ,[phone_number] = @phonenumber
                                    WHERE [id] = @id";

            string sqlInsertSocialWorker = @"INSERT INTO [dbo].[social_workers]
                                       ([resident_id]
                                       ,[forename]
                                       ,[surname]
                                       ,[email_address]
                                       ,[phone_number])
                                 VALUES
                                       (@residentid
                                       ,@forename
                                       ,@surname
                                       ,@emailaddress
                                       ,@phonenumber)";

            DynamicParameters dp = new DynamicParameters();
            dp.Add("carehomeid", residentEntity.CareHomeId, DbType.Int32, ParameterDirection.Input);
            dp.Add("localauthorityid", residentEntity.LocalAuthorityId, DbType.Int32, ParameterDirection.Input);
            dp.Add("nhsnumber", residentEntity.NhsNumber, DbType.String, ParameterDirection.Input, 20);
            dp.Add("ponumber", residentEntity.PoNumber, DbType.String, ParameterDirection.Input, 20);
            dp.Add("laid", residentEntity.LaId, DbType.String, ParameterDirection.Input, 20);
            dp.Add("nymsid", residentEntity.NymsId, DbType.String, ParameterDirection.Input, 20);
            dp.Add("forename", residentEntity.ForeName.Trim(), DbType.String, ParameterDirection.Input, 80);
            dp.Add("surname", residentEntity.SurName.Trim(), DbType.String, ParameterDirection.Input, 80);
            dp.Add("middlename", residentEntity.MiddleName, DbType.String, ParameterDirection.Input, 80);
            dp.Add("dob", residentEntity.Dob, DbType.Date, ParameterDirection.Input, 80);
            dp.Add("gender", residentEntity.Gender, DbType.String, ParameterDirection.Input, 80);
            dp.Add("maritalstatus", residentEntity.MaritalStatus, DbType.String, ParameterDirection.Input, 80);
            dp.Add("carecategoryid", residentEntity.CareCategoryId, DbType.Int32, ParameterDirection.Input);
            dp.Add("careneed", residentEntity.CareNeed, DbType.String, ParameterDirection.Input, 80);
            dp.Add("staytype", residentEntity.StayType, DbType.String, ParameterDirection.Input, 80);
            dp.Add("roomlocation", residentEntity.RoomLocation, DbType.Int32, ParameterDirection.Input, 80);
            dp.Add("roomnumber", residentEntity.RoomNumber, DbType.Int32, ParameterDirection.Input, 80);
            dp.Add("admissiondate", residentEntity.AdmissionDate, DbType.Date, ParameterDirection.Input, 80);
            dp.Add("exitdate", residentEntity.ExitDate, DbType.Date, ParameterDirection.Input, 80);
            dp.Add("comments", residentEntity.Comments, DbType.String, ParameterDirection.Input, 500);
            dp.Add("status", residentEntity.Status, DbType.String, ParameterDirection.Input, 80);
            dp.Add("updatedbyid", residentEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);
            dp.Add("id", residentEntity.Id, DbType.Int32, ParameterDirection.Input); // for Where clause

            DynamicParameters dpAddrResident = new DynamicParameters();
            if (residentEntity.Address != null)
            {
                dpAddrResident.Add("@addrtype", residentEntity.Address.AddrType, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("@street1", residentEntity.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                dpAddrResident.Add("@street2", residentEntity.Address.Street2, DbType.String, ParameterDirection.Input, 100);
                dpAddrResident.Add("@city", residentEntity.Address.City, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("@county", residentEntity.Address.County, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("@postcode", residentEntity.Address.PostCode, DbType.String, ParameterDirection.Input, 80);
                dpAddrResident.Add("residentid", residentEntity.Id, DbType.Int32, ParameterDirection.Input); // for Where clause
            }

            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var affRowsAddr = conn.Execute(sql, dp, transaction: tran);
                    // resident address..
                    if (residentEntity.Address != null)
                    {
                        if (residentEntity.Address.Id <= 0)
                        {
                            affRowsAddr = conn.Execute(sqlInsertAddress, dpAddrResident, transaction: tran);
                        }
                        else
                        {
                            dpAddrResident.Add("id", residentEntity.Address.Id, DbType.Int32, ParameterDirection.Input); // for Where clause
                            affRowsAddr = conn.Execute(sqlUpdateAddress, dpAddrResident, transaction: tran);
                        }
                    }

                    // REDONE-NOW resident contact is JUST email and phone field
                    if (residentEntity.ResidentContacts != null)
                    {
                        residentEntity.ResidentContacts.ForEach((rc) =>
                        {
                            DynamicParameters dpCnt = new DynamicParameters();
                            
                            if (rc.Id <= 0)
                            {
                                // new rec
                                dpCnt.Add("contacttype", rc.ContactType, DbType.String, ParameterDirection.Input, 80);
                                dpCnt.Add("data", rc.Data, DbType.String, ParameterDirection.Input, 100);
                                dpCnt.Add("residentid", residentEntity.Id, DbType.Int32, ParameterDirection.Input);
                                var affRowsCnt = conn.Execute(sqlInsertResidentContact, dpCnt, transaction: tran);
                            }
                            else
                            {
                                dpCnt.Add("data", rc.Data, DbType.String, ParameterDirection.Input, 100);
                                dpCnt.Add("id", rc.Id, DbType.Int32, ParameterDirection.Input); // for Where clause
                                var affRowsCnt = conn.Execute(sqlUpdateResidentContact, dpCnt, transaction: tran);
                            }
                        });
                    }

                    // social worker
                    if (residentEntity.SocialWorker != null && !string.IsNullOrEmpty(residentEntity.SocialWorker.ForeName))
                    {
                        DynamicParameters dpSw = new DynamicParameters();
                        dpSw.Add("forename", residentEntity.SocialWorker.ForeName.Trim(), DbType.String, ParameterDirection.Input, 80);
                        dpSw.Add("surname", residentEntity.SocialWorker.SurName.Trim(), DbType.String, ParameterDirection.Input, 80);
                        dpSw.Add("emailaddress", residentEntity.SocialWorker.EmailAddress, DbType.String, ParameterDirection.Input, 200);
                        dpSw.Add("phonenumber", residentEntity.SocialWorker.PhoneNumber, DbType.String, ParameterDirection.Input, 40);
                        if (residentEntity.SocialWorker.Id <= 0)
                        {
                            // New rec
                            dpSw.Add("residentid", residentEntity.Id, DbType.Int32, ParameterDirection.Input);
                            var affRowsSw = conn.Execute(sqlInsertSocialWorker, dpSw, transaction: tran);
                        }
                        else
                        {
                            dpSw.Add("id", residentEntity.SocialWorker.Id, DbType.Int32, ParameterDirection.Input); // for Where clause
                            var affRowsSw = conn.Execute(sqlUpdateSocialWorker, dpSw, transaction: tran);
                        }

                    }
                    // noks, list of noks
                    if (residentEntity.NextOfKins != null && residentEntity.NextOfKins.Any())
                    {
                        DynamicParameters dpNok = new DynamicParameters();
                        //residentEntity.NextOfKins.ForEach(nok =>
                        //{
/*                            int newNokId = 0;
                            dpNok.Add("forename", nok.ForeName.Trim(), DbType.String, ParameterDirection.Input, 80);
                            dpNok.Add("surname", nok.SurName.Trim(), DbType.String, ParameterDirection.Input, 80);
                            dpNok.Add("relationship", nok.Relationship, DbType.String, ParameterDirection.Input, 80);
                            dpNok.Add("updatedbyid", residentEntity.UpdatedById, DbType.Int32, ParameterDirection.Input);

                            dpNok.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                            newNokId = conn.QuerySingle<int>(sqlInsertNok, dpNok, commandType: CommandType.Text, transaction: tran);*/
                            // add nok address to address table
                            // TODO: CREATE a new table for NOK Address and save it. Below is OLD code
                            /*                            if (nok.Address != null && nok.Address.Street1 != "") // ensure atleast one field is filled in?
                                                        {
                                                            DynamicParameters dpAddrNok = new DynamicParameters();
                                                            dpAddrNok.Add("@addrtype", nok.Address.AddrType, DbType.String, ParameterDirection.Input, 80);
                                                            dpAddrNok.Add("@street1", nok.Address.Street1, DbType.String, ParameterDirection.Input, 100);
                                                            dpAddrNok.Add("@street2", nok.Address.Street2, DbType.String, ParameterDirection.Input, 100);
                                                            dpAddrNok.Add("@city", nok.Address.City), DbType.String, ParameterDirection.Input, 80);
                                                            dpAddrNok.Add("@county", nok.Address.County, DbType.String, ParameterDirection.Input, 80);
                                                            dpAddrNok.Add("@postcode", nok.Address.PostCode, DbType.String, ParameterDirection.Input, 80);
                                                            dpAddrNok.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                                                            var affRowsNokCnt = conn.Execute(sqlInsertAddress, dpAddrNok, transaction: tran);
                                                        }*/
                            // add nok contact to contacts table
                            /*                            if (nok.ContactInfos != null && nok.ContactInfos.Any())
                                                        {
                                                            DynamicParameters dpCnt = new DynamicParameters();
                                                            nok.ContactInfos.ForEach(nokCt =>
                                                            {
                                                                dpCnt.Add("reftype", nokCt.RefType, DbType.String, ParameterDirection.Input, 80);
                                                                dpCnt.Add("contacttype", nokCt.ContactType, DbType.String, ParameterDirection.Input, 80);
                                                                dpCnt.Add("data", nokCt.Data, DbType.String, ParameterDirection.Input, 100);

                                                                dpCnt.Add("nokid", newNokId, DbType.Int32, ParameterDirection.Input);
                                                                dpCnt.Add("residentid", newResidentId, DbType.Int32, ParameterDirection.Input);
                                                                var affRowsNokCnt = conn.Execute(sqlInsertContact, dpCnt, transaction: tran);
                                                            });
                                                        }*/
                        //});
                    }

                    tran.Commit();
                }
                return Task.FromResult(residentEntity);
            }
        }


        private Resident _GetResident(Guid referenceId)
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
                              ,[forename] as forename
                              ,[surname] as surname
                              ,[middle_name] as middlename
                              ,[dob] as dob
                              ,[gender] as gender
                              ,[marital_status] as maritalstatus
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

        private IEnumerable<NextOfKin> GetNextOfKinsByResidentId(int residentId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT
                               [id] as id
                              ,[resident_id] as residentid
                              ,[forename] as forename
                              ,[surname] as surname
                              ,[relationship] as relationship
                        FROM [dbo].[next_of_kin]
                        WHERE [resident_id] = @residentid";          

                DynamicParameters dp = new DynamicParameters();
                dp.Add("residentid", residentId, DbType.Int32, ParameterDirection.Input);
                conn.Open();
                var result = conn.Query<NextOfKin>(sql, dp);
                return result;
            }
        }

        private IEnumerable<Address> GetAddressesByResidentId(int residentId)
        {
            using (IDbConnection conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT
                               [id] as id
                              ,[resident_id] as residentid
                              ,[addr_type] as addrtype
                              ,[street1] as street1
                              ,[street2] as street2
                              ,[city] as city
                              ,[county] as county
                              ,[post_code] as postcode
                        FROM [dbo].[resident_addresses]
                        WHERE [resident_id] = @residentid
                        AND [active] = 'Y'";

                DynamicParameters dp = new DynamicParameters();
                dp.Add("residentid", residentId, DbType.Int32, ParameterDirection.Input);
                conn.Open();
                var result = conn.Query<Address>(sql, dp);
                return result;
            }
        }

    }
}


