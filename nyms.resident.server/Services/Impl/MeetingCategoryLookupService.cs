using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class MeetingCategoryLookupService : IMeetingCategoryLookupService
    {
        private readonly IMeetingCategoryLookupDataProvider _meetingCategoryLookupDataProvider;
        public MeetingCategoryLookupService(IMeetingCategoryLookupDataProvider meetingCategoryLookupDataProvider)
        {
            _meetingCategoryLookupDataProvider = meetingCategoryLookupDataProvider ?? throw new ArgumentNullException(nameof(meetingCategoryLookupDataProvider));
        }

        public IEnumerable<MeetingCategory> GetMeetingCategories()
        {
            return _meetingCategoryLookupDataProvider.GetMeetingCategories();
        }

        public MeetingCategory Insert(MeetingCategory meetingCategory)
        {
            return _meetingCategoryLookupDataProvider.Insert(meetingCategory);
        }

        public MeetingCategory Update(MeetingCategory meetingCategory)
        {
            return _meetingCategoryLookupDataProvider.Update(meetingCategory);
        }
    }
}