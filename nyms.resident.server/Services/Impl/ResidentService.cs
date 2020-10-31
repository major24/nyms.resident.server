using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentDataProvider _residentDataProvider;
        public ResidentService(IResidentDataProvider residentDataProvider)
        {
            _residentDataProvider = residentDataProvider ?? throw new ArgumentException(nameof(residentDataProvider));
        }

        public IEnumerable<Resident> GetResidentsByCareHomeId(int careHomeId)
        {
            return this._residentDataProvider.GetResidentsByCareHomeId(careHomeId);
        }

        public Resident GetResident(Guid referenceId)
        {
            return this._residentDataProvider.GetResident(referenceId);
        }
        public bool UpdateExitDate(Guid referenceId, DateTime exitDate)
        {
            return this._residentDataProvider.UpdateExitDate(referenceId, exitDate);
        }

        public Task<Resident> Create(Resident resident)
        {
            var residentEntity = ConvertToResident(resident);
            return Create(residentEntity);
        }

        public Task<Resident> Create(Enquiry enquiry)
        {
            var residentEntity = ConvertToResident(enquiry);
            return Create(residentEntity);
        }

        private Task<Resident> Create(ResidentEntity residentEntity)
        {
            // Add required values here and below conversions
            residentEntity.ReferenceId = Guid.NewGuid();
            var newEntity = _residentDataProvider.Create(residentEntity).Result;
            // todo: need to convert the full object
            var result = new Resident() { ReferenceId = newEntity.ReferenceId };

            return Task.FromResult(result);
        }

        public Task<Resident> Update(Resident resident)
        {
            throw new NotImplementedException();
        }

        private ResidentEntity ConvertToResident(Enquiry enquiry)
        {
            ResidentEntity residentEntity = new ResidentEntity()
            {
                CareHomeId = enquiry.CareHomeId,
                LocalAuthorityId = enquiry.LocalAuthorityId,
                NhsNumber = "",
                PoNumber = "",
                LaId = "",
                NymsId = "",
                ForeName = enquiry.ForeName,
                SurName = enquiry.SurName,
                MiddleName = enquiry.MiddleName,
                Dob = enquiry.Dob ?? Convert.ToDateTime("1800-01-01"),
                Gender = enquiry.Gender,
                MaritalStatus = enquiry.MaritalStatus,
                SwForeName = enquiry.SocialWorker.ForeName,
                SwSurName = enquiry.SocialWorker.SurName,
                SwEmailAddress = enquiry.SocialWorker.Email,
                SwPhoneNumber = enquiry.SocialWorker.PhoneNumber,
                CareCategoryId = enquiry.CareCategoryId,
                CareNeed = enquiry.CareNeed,
                StayType = enquiry.StayType,
                RoomLocation = enquiry.ReservedRoomLocation,
                RoomNumber = enquiry.ReservedRoomNumber,
                AdmissionDate = enquiry.AdmissionDate,
                Comments = enquiry.Comments,
                UpdatedById = enquiry.UpdatedBy,
                ExitDate = Convert.ToDateTime("9999-12-31"),
            };
            return residentEntity;
        }

        private ResidentEntity ConvertToResident(Resident resident)
        {
            ResidentEntity residentEntity = new ResidentEntity()
            {
                CareHomeId = resident.CareHomeId,
                LocalAuthorityId = resident.LocalAuthorityId,
                NhsNumber = "",
                PoNumber = "",
                LaId = "",
                NymsId = "",
                ForeName = resident.ForeName,
                SurName = resident.SurName,
                MiddleName = resident.MiddleName,
                Dob = resident.Dob ?? Convert.ToDateTime("1800-01-01"),
                Gender = resident.Gender,
                MaritalStatus = resident.MaritalStatus,
                SwForeName = resident.SwForeName,
                SwSurName = resident.SwSurName,
                SwEmailAddress = resident.SwEmailAddress,
                SwPhoneNumber = resident.SwPhoneNumber,
                CareCategoryId = resident.CareCategoryId,
                CareNeed = resident.CareNeed,
                StayType = resident.StayType,
                RoomLocation = resident.RoomLocation,
                RoomNumber = resident.RoomNumber,
                AdmissionDate = resident.AdmissionDate,
                Comments = resident.Comments,
                UpdatedById = resident.UpdatedBy,
                ExitDate = Convert.ToDateTime("9999-12-31"),
            };
            return residentEntity;
        }
    }
}