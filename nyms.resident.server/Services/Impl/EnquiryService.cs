using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return _enquiryDataProvider.GetEnquires();
        }

        public Task<Enquiry> GetByReferenceId(Guid referenceId)
        {
            var entity = _enquiryDataProvider.GetEnquiry(referenceId).Result;
            if (entity == null) return null;

            // else, convert db entity to web enquiry
            Enquiry enquiry = null;
            if (entity != null)
            {
                var actions = _enquiryDataProvider.GetActions(entity.Id);

                SocialWorker sw = new SocialWorker() { ForeName = entity.SwForeName, SurName = entity.SwSurName, EmailAddress = entity.SwEmailAddress, PhoneNumber = entity.SwPhoneNumber };
                enquiry = new Enquiry()
                {
                    ReferenceId = entity.ReferenceId,
                    CareHomeId = entity.CareHomeId,
                    ReferralAgencyId = entity.ReferralAgencyId,
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
                    RoomLocation = entity.RoomLocation,
                    RoomNumber = entity.RoomNumber,
                    Comments = entity.Comments,
                    Status = entity.Status,
                    UpdatedBy = entity.UpdatedBy,
                    UpdatedDate = entity.UpdatedDate,
                    EnquiryActions = actions
                };
            }

            return Task.FromResult(enquiry);
        }

        public Task<Enquiry> Create(Enquiry enquiry)
        {
            // assign guid and necessary values
            enquiry.ReferenceId = Guid.NewGuid();
            enquiry.Status = ENQUIRY_STATUS.active.ToString();

            var result = this._enquiryDataProvider.Create(enquiry);
            return Task.FromResult(enquiry);
        }

        public Task<Enquiry> Update(Enquiry enquiry)
        {
            _enquiryDataProvider.Update(enquiry);
            return Task.FromResult(enquiry);
        }

        public IEnumerable<EnquiryAction> GetActions(Guid referenceId)
        {
            var entity = _enquiryDataProvider.GetEnquiry(referenceId).Result;
            IEnumerable<EnquiryAction> actions = new List<EnquiryAction>();

            if (entity != null)
            {
                actions = _enquiryDataProvider.GetActions(entity.Id);
            }
            return actions;
        }

        public void SaveActions(Guid referenceId, EnquiryAction[] enquiryActions)
        {
            var entity = _enquiryDataProvider.GetEnquiry(referenceId).Result;
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