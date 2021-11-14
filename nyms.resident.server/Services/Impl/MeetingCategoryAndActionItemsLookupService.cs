using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class MeetingCategoryAndActionItemsLookupService : IMeetingCategoryAndActionItemLookupService
    {
        private readonly IMeetingCategoryAndActionItemLookupDataProvider _meetingCategoryLookupDataProvider;
        public MeetingCategoryAndActionItemsLookupService(IMeetingCategoryAndActionItemLookupDataProvider meetingCategoryLookupDataProvider)
        {
            _meetingCategoryLookupDataProvider = meetingCategoryLookupDataProvider ?? throw new ArgumentNullException(nameof(meetingCategoryLookupDataProvider));
        }

        public IEnumerable<MeetingCategory> GetMeetingCategoriesAndActionItems()
        {
            return _meetingCategoryLookupDataProvider.GetMeetingCategoriesAndActionItems();
        }

        public MeetingCategory InsertCategoryAndActionItems(MeetingCategory meetingCategory)
        {
            return _meetingCategoryLookupDataProvider.InsertCategoryAndActionItems(meetingCategory);
        }

        public MeetingActionItem UpdateActionItem(MeetingActionItem meetingActionItem)
        {
            return _meetingCategoryLookupDataProvider.UpdateActionItem(meetingActionItem);
        }

        public MeetingActionItem InsertActionItem(MeetingActionItem meetingActionItem)
        {
            return _meetingCategoryLookupDataProvider.InsertActionItem(meetingActionItem);
        }
    }
}