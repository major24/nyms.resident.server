using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;

namespace nyms.resident.server.Services.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentDataProvider _residentDataProvider;
        private readonly IResidentContactDataProvider _residentContactDataProvider;
        private readonly ISocialWorkerDataProvider _socialWorkerDataProvider;
        public ResidentService(IResidentDataProvider residentDataProvider,
            IResidentContactDataProvider residentContactDataProvider,
            ISocialWorkerDataProvider socialWorkerDataProvider)
        {
            _residentDataProvider = residentDataProvider ?? throw new ArgumentNullException(nameof(residentDataProvider));
            _residentContactDataProvider = residentContactDataProvider ?? throw new ArgumentNullException(nameof(residentContactDataProvider));
            _socialWorkerDataProvider = socialWorkerDataProvider ?? throw new ArgumentNullException(nameof(socialWorkerDataProvider));
        }

        public IEnumerable<Resident> GetAllResidentsByCareHomeId(int careHomeId)
        {
            return this._residentDataProvider.GetResidents().Where(res => res.CareHomeId == careHomeId);
        }

        public IEnumerable<Resident> GetActiveResidentsByCareHomeId(int careHomeId)
        {
            return this._residentDataProvider.GetResidents().Where(res => res.CareHomeId == careHomeId && res.Active == "Y");
        }

        public Resident GetResident(Guid referenceId)
        {
            return this._residentDataProvider.GetResident(referenceId);
        }

        public bool DischargeResident(Guid referenceId, DateTime dischargeDate)
        {
            return this._residentDataProvider.DischargeResident(referenceId, dischargeDate);
        }

        public bool ExitInvoiceResident(Guid referenceId, DateTime exitDate)
        {
            return this._residentDataProvider.ExitInvoiceResident(referenceId, exitDate);
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

            var residentEntityUpdated = _residentDataProvider.Create(residentEntity).Result;

            // todo: return new resident...
            var residentCreated = new Resident() { ReferenceId = residentEntity.ReferenceId };
            return Task.FromResult(residentCreated);
        }

        public Task<Resident> Update(ResidentRequest resident)
        {
            var residentExisting = GetResident(resident.ReferenceId);
            if (residentExisting == null) throw new ArgumentNullException(nameof(resident));

            var residentEntity = ConvertToResidentEntity(resident);
            residentEntity.Id = residentExisting.Id;

            // Contact Info. Issue: Contact info is separate table but Email and Phone comes as values
            // Need to find if already exists? if so update else insert..
            var existingResidentContacts = _residentContactDataProvider.GetResidentContactsByResidentId(residentEntity.Id);    // _residentDataProvider.GetResidentContactsByResidentId(residentEntity.Id);
            List<ResidentContact> rcs = new List<ResidentContact>();
            if (existingResidentContacts.Any())
            {
                // get existing email address or phone
                existingResidentContacts.ForEach((rc) =>
                {
                    if (!string.IsNullOrEmpty(rc.ContactType) && rc.ContactType == CONTACT_TYPE.email.ToString())
                    {
                        rc.Id = rc.Id;
                        rc.Data = resident.EmailAddress;
                    }
                    if (!string.IsNullOrEmpty(rc.ContactType) && rc.ContactType == CONTACT_TYPE.phone.ToString())
                    {
                        rc.Id = rc.Id;
                        rc.Data = resident.PhoneNumber;
                    }
                    rcs.Add(rc);
                });
            } 
            else
            {
                // No existing contacts found
                if (!string.IsNullOrEmpty(residentEntity.EmailAddress))
                {
                    ResidentContact rc = new ResidentContact()
                    {
                        ContactType = CONTACT_TYPE.email.ToString(),
                        Data = residentEntity.EmailAddress
                    };
                    rcs.Add(rc);
                }
                if (!string.IsNullOrEmpty(residentEntity.PhoneNumber))
                {
                    ResidentContact rc = new ResidentContact()
                    {
                        ContactType = CONTACT_TYPE.phone.ToString(),
                        Data = residentEntity.PhoneNumber
                    };
                    rcs.Add(rc);
                }
            }
            residentEntity.ResidentContacts = rcs.ToArray();

            // SocialWorker Info. Issue: SW info is separate table
            SocialWorker swToBeUpdIns = new SocialWorker();
            if (resident.SocialWorker != null && resident.SocialWorker.ForeName != "")
            {
                swToBeUpdIns.ForeName = resident.SocialWorker.ForeName;
                swToBeUpdIns.SurName = resident.SocialWorker.SurName;
                swToBeUpdIns.EmailAddress = resident.SocialWorker.EmailAddress;
                swToBeUpdIns.PhoneNumber = resident.SocialWorker.PhoneNumber;
            }
            // Need to find if already exists? if so update else insert..
            SocialWorker existingSocialWorker = _socialWorkerDataProvider.GetSocialWorkerByResidentId(residentEntity.Id);  //_residentDataProvider.GetSocialWorker(residentEntity.Id);
            if (existingSocialWorker != null)
            {
                swToBeUpdIns.Id = existingSocialWorker.Id;
            }
            residentEntity.SocialWorker = swToBeUpdIns;

            var residentEntityUpdated = _residentDataProvider.Update(residentEntity);

            // todo: return new resident...
            var residentCreated = new Resident() { ReferenceId = residentEntity.ReferenceId };
            return Task.FromResult(residentCreated);
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
                PhoneNumber = resident.PhoneNumber,
                CareHomeDivisionId = resident.CareHomeDivisionId
            };

            // fill in if resident address found?
            if (resident.Address != null && !string.IsNullOrEmpty(resident.Address.Street1))
            {
                residentEntity.Address = CreateAddress(
                    resident.Address,
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
/*                    newNok.Address = CreateAddress(
                        n.Address,
                        n.Address.RefType ?? REF_TYPE.nok.ToString(),
                        n.Address.AddrType ?? ADDRESS_TYPE.home.ToString());*/
                }
                // if nok has contact info?
                if (n.ContactInfos != null && n.ContactInfos.Any())
                {
                    var cts = n.ContactInfos.Select(c =>
                    {
                        return new ContactInfo()
                        {
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

        private Address CreateAddress(Address address, string addrType)
        {
            return new Address()
            {
                Id = address.Id,
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

