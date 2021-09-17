using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IMeetingCategoryLookupService
    {
        MeetingCategory Insert(MeetingCategory meetingCategory);
        IEnumerable<MeetingCategory> GetMeetingCategories();
        MeetingCategory Update(MeetingCategory meetingCategory);
    }
}
