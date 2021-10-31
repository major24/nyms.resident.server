using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IMeetingCategoryAndActionItemLookupService
    {
        MeetingCategory InsertCategoryAndActionItems(MeetingCategory meetingCategory);
        IEnumerable<MeetingCategory> GetMeetingCategoriesAndActionItems();
        MeetingActionItem UpdateActionItem(MeetingActionItem meetingActionItem);
        MeetingActionItem InsertActionItem(MeetingActionItem meetingActionItem);
    }
}
