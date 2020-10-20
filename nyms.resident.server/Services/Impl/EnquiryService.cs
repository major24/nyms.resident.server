using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Impl
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryDataProvider _enquiryDataProvider;
        private readonly IResidentService _residentService;

        public EnquiryService(IEnquiryDataProvider enquiryDataProvider, IResidentService residentService)
        {
            _enquiryDataProvider = enquiryDataProvider ?? throw new ArgumentNullException(nameof(enquiryDataProvider));
            _residentService = residentService ?? throw new ArgumentNullException(nameof(residentService));
        }

        public IEnumerable<Enquiry> GetAll()
        {
            return _enquiryDataProvider.GetAll();
        }

        public Task<Enquiry> GetByReferenceId(Guid referenceId)
        {
            var entity = _enquiryDataProvider.GetByReferenceId(referenceId).Result;
            Enquiry enquiry = null;
            // convert db entity to web enquiry
            if (entity != null)
            {
                var actions = _enquiryDataProvider.GetActions(entity.Id);

                SocialWorker sw = new SocialWorker() { ForeName = entity.SwForeName, SurName = entity.SwSurName, Email = entity.SwEmailAddress, PhoneNumber = entity.SwPhoneNumber };
                Address address = new Address() { Street1 = entity.Street, City = entity.City, County = entity.County, PostCode = entity.Postcode };
                enquiry = new Enquiry()
                {
                    ReferenceId = entity.ReferenceId,
                    CareHomeId = entity.CareHomeId,
                    LocalAuthorityId = entity.LocalAuthorityId,
                    ForeName = entity.ForeName,
                    SurName = entity.SurName,
                    MiddleName = entity.MiddleName,
                    Dob = entity.Dob,
                    Gender = entity.Gender,
                    MaritalStatus = entity.MaritalStatus,
                    SocialWorker = sw,
                    CareCategoryId = entity.CareCategoryId,
                    CareNeed = entity.CareNeed,
                    StayType = entity.StayType,
                    MoveInDate = entity.MoveInDate,
                    FamilyHomeVisitDate = entity.FamilyHomeVisitDate,
                    ReservedRoomLocation = entity.ReservedRoomLocation,
                    ReservedRoomNumber = entity.ReservedRoomNumber,
                    ResponseDate = entity.ResponseDate,
                    Response = entity.Response,
                    Comments = entity.Comments,
                    Status = entity.Status,
                    UpdatedBy = entity.UpdatedBy,
                    UpdatedDate = entity.UpdatedDate,
                    Address = address,
                    EnquiryActions = actions
                };
            }

            return Task.FromResult(enquiry);
        }

        public Task<Enquiry> Create(Enquiry enquiry)
        {
            // assign guid and necessary values
            enquiry.ReferenceId = Guid.NewGuid();
            enquiry.Status = EnquiryStatus.active.ToString();

            var result = this._enquiryDataProvider.Create(enquiry);
            return Task.FromResult(enquiry);
        }

        public Task<Enquiry> Update(Enquiry enquiry)
        {
            // TODO: Q: do we want to save first and let resident edit?
            // OR from UI, show both go to res edit and save??????? for now just update
            if (enquiry.Status == EnquiryStatus.admit.ToString())
            {
                enquiry.UpdatedBy = enquiry.UpdatedBy;
                _residentService.ConvertEnquiryToResident(enquiry);
                
                // update enquiry status to [admit] as well
                this._enquiryDataProvider.Update(enquiry);

                return Task.FromResult(enquiry);
            }
            else
            {
                return this._enquiryDataProvider.Update(enquiry);
            }
        }

        public IEnumerable<EnquiryAction> GetActions(Guid referenceId)
        {
            var entity = _enquiryDataProvider.GetByReferenceId(referenceId).Result;
            IEnumerable<EnquiryAction> actions = new List<EnquiryAction>();

            if (entity != null)
            {
                actions = _enquiryDataProvider.GetActions(entity.Id);
            }
            return actions;
        }

        public void SaveActions(Guid referenceId, EnquiryAction[] enquiryActions)
        {
            var entity = _enquiryDataProvider.GetByReferenceId(referenceId).Result;
            // add enquiry id to actions (fkey)
            enquiryActions.ForEach(en => en.EnquiryId = entity.Id);

            // only save new and updated actions. 
            // 1) new when id is zero. 
            // 2) when checkbox is checked (status is '_completing')
            // 3) if response comment is changed
            var existingActions = _enquiryDataProvider.GetActions(entity.Id);
            List<EnquiryAction> updatedOrNewActions = new List<EnquiryAction>();
            foreach (var act in enquiryActions)
            {
                if (act.Id == 0 || act.Status == Constants.ENQUIRY_ACTION_STATUS_COMPLETING)
                {
                    // mark if "_completing" to "completed"
                    if (act.Status == Constants.ENQUIRY_ACTION_STATUS_COMPLETING)
                        act.Status = Constants.ENQUIRY_ACTION_STATUS_COMPLETED;
                    // ensure actionDate is valid or make it null
                    if (act.ActionDate <= DateTime.MinValue) act.ActionDate = null;

                    updatedOrNewActions.Add(act);
                }
                else
                {
                    // check if resonse comment changed?
                    var existingResponse = existingActions.Where(ea => ea.Id == act.Id).FirstOrDefault().Response;
                    if (existingResponse != act.Response)
                    {
                        updatedOrNewActions.Add(act);
                    }
                }
            }
            if (updatedOrNewActions.Any())
            {
                this._enquiryDataProvider.SaveActions(entity.Id, updatedOrNewActions.ToArray());
            }
        }
    }
}