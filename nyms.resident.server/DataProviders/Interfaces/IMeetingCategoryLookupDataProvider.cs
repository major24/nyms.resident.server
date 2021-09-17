using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IMeetingCategoryLookupDataProvider
    {
        MeetingCategory Insert(MeetingCategory meetingCategory);
        IEnumerable<MeetingCategory> GetMeetingCategories();
        MeetingCategory Update(MeetingCategory meetingCategory);
    }
}