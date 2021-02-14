using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using WebGrease.Css.Extensions;

namespace nyms.resident.server.Services.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentDataProvider _residentDataProvider;
        public ResidentService(IResidentDataProvider residentDataProvider)
        {
            _residentDataProvider = residentDataProvider ?? throw new ArgumentException(nameof(residentDataProvider));
        }

        public IEnumerable<Resident> GetAllResidentsByCareHomeId(int careHomeId)
        {
            return this._residentDataProvider.GetAllResidentsByCareHomeId(careHomeId);
        }

        public IEnumerable<Resident> GetActiveResidentsByCareHomeId(int careHomeId)
        {
            return this._residentDataProvider.GetActiveResidentsByCareHomeId(careHomeId);
        }

        public Resident GetResident(Guid referenceId)
        {
            return this._residentDataProvider.GetResident(referenceId);
        }
        public bool DischargeResident(Guid referenceId, DateTime exitDate)
        {
            return this._residentDataProvider.DischargeResident(referenceId, exitDate);
        }

        public bool ActivateResident(Guid referenceId)
        {
            return this._residentDataProvider.ActivateResident(referenceId);
        }
        public Task<Resident> AdmitEnquiry(ResidentRequest resident)
        {
            var residentEntity = ConvertToResidentEntity(resident);
            // Add required values for creation
            residentEntity.ReferenceId = Guid.NewGuid();

            EnquiryEntity enquiryEntity = new EnquiryEntity()
            {
                ReferenceId = resident.EnquiryReferenceId,
                Status = ENQUIRY_STATUS.admit.ToString(),
                UpdatedBy = resident.UpdatedBy
            };
            var residentEntityUpdated = _residentDataProvider.Create(residentEntity, enquiryEntity).Result;

            // todo: return new resident...
            var residentCreated = new Resident() { ReferenceId = residentEntity.ReferenceId };
            return Task.FromResult(residentCreated);
        }

        public Task<Resident> Update(Resident resident)
        {
            throw new NotImplementedException();
        }

        private ResidentEntity ConvertToResidentEntity(ResidentRequest resident)
        {
            ResidentEntity residentEntity = new ResidentEntity()
            {
                CareHomeId = resident.CareHomeId,
                LocalAuthorityId = resident.LocalAuthorityId,
                NhsNumber = resident.NhsNumber,
                PoNumber = resident.PoNumber,
                LaId = resident.LaId,
                NymsId = resident.NymsId,
                ForeName = resident.ForeName,
                SurName = resident.SurName,
                MiddleName = resident.MiddleName,
                Dob = resident.Dob ?? Convert.ToDateTime("1800-01-01"),
                Gender = resident.Gender,
                MaritalStatus = resident.MaritalStatus,
                CareCategoryId = resident.CareCategoryId,
                CareNeed = resident.CareNeed,
                StayType = resident.StayType,
                RoomLocation = resident.RoomLocation,
                RoomNumber = resident.RoomNumber,
                AdmissionDate = (DateTime)resident.AdmissionDate,
                Comments = resident.Comments,
                UpdatedById = resident.UpdatedBy,
                ExitDate = Convert.ToDateTime("9999-12-31"),
                EnquiryReferenceId = resident.ReferenceId,
                Address = null,
                NextOfKins = null,
                EmailAddress = resident.EmailAddress,
                PhoneNumber = resident.PhoneNumber
            };

            // fill in if resident address found?
            if (resident.Address != null && !string.IsNullOrEmpty(resident.Address.Street1))
            {
                residentEntity.Address = CreateAddress(
                    resident.Address,
                    resident.Address.RefType ?? REF_TYPE.resident.ToString(),
                    resident.Address.AddrType ?? ADDRESS_TYPE.home.ToString());
            }

            // fill in if nok found?
            if (resident.NextOfKins != null && resident.NextOfKins.Any())
            {
                // safeguard. ensure atleast the 1st record has forename
                var fn = resident.NextOfKins.FirstOrDefault().ForeName;
                if (!string.IsNullOrEmpty(fn))
                    residentEntity.NextOfKins = CreateNextOfKinList(resident);
            }

            if (resident.SocialWorker != null && !string.IsNullOrEmpty(resident.SocialWorker.ForeName))
            {
                residentEntity.SocialWorker = resident.SocialWorker;
            }

            return residentEntity;
        }

        private IEnumerable<NextOfKin> CreateNextOfKinList(ResidentRequest resident)
        {
            List<NextOfKin> noks = new List<NextOfKin>();
            resident.NextOfKins.ForEach(n =>
            {
                // create next of kin obj
                var newNok = new NextOfKin()
                {
                    ForeName = n.ForeName,
                    SurName = n.SurName,
                    Relationship = n.Relationship,
                };
                // if nok has address?
                if (n.Address != null && n.Address.Street1 != "")
                {
                    newNok.Address = CreateAddress(
                        n.Address,
                        n.Address.RefType ?? REF_TYPE.nok.ToString(),
                        n.Address.AddrType ?? ADDRESS_TYPE.home.ToString());
                }
                // if nok has contact info?
                if (n.ContactInfos != null && n.ContactInfos.Any())
                {
                    var cts = n.ContactInfos.Select(c =>
                    {
                        return new ContactInfo()
                        {
                            RefType = c.RefType,
                            ContactType = c.ContactType,
                            Data = c.Data
                        };
                    });
                    newNok.ContactInfos = cts.ToArray();
                }
                noks.Add(newNok);
            });
            return noks.ToArray();
        }

        private Address CreateAddress(Address address, string refType, string addrType)
        {
            return new Address()
            {
                RefType = refType,
                AddrType = addrType,
                Street1 = address.Street1,
                Street2 = address.Street2,
                City = address.City,
                County = address.County,
                PostCode = address.PostCode
            };
        }

    }
}

