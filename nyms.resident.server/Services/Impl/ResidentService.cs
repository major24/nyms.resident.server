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
            throw new NotImplementedException();
            // return this._residentDataProvider.Create(resident);
        }

        public Task<Resident> Update(Resident resident)
        {
            throw new NotImplementedException();
        }

        public Task<Resident> ConvertEnquiryToResident(Enquiry enquiry)
        {
            var residentEntity = ConvertToResident(enquiry);
            residentEntity.ReferenceId = Guid.NewGuid();

            var newEntity = _residentDataProvider.Create(residentEntity).Result;

            // todo: need to convert the full object
            var result = new Resident() { ReferenceId = newEntity.ReferenceId };

            return Task.FromResult(result);
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
                Dob = enquiry.Dob,
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
                AdmissionDate = System.DateTime.UtcNow,  // todo: add contorl in UI
                Comments = enquiry.Comments,
                UpdatedById = enquiry.UpdatedBy,
                ExitDate = Convert.ToDateTime("9999-12-31"),
            };
            return residentEntity;
        }
    }
}