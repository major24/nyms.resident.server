using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IMeetingCategoryAndActionItemLookupDataProvider
    {
        IEnumerable<MeetingCategory> GetMeetingCategoriesAndActionItems();
        MeetingCategory InsertCategoryAndActionItems(MeetingCategory meetingCategory);
        MeetingActionItem UpdateActionItem(MeetingActionItem meetingActionItem);
        MeetingActionItem InsertActionItem(MeetingActionItem meetingActionItem);
    }
}