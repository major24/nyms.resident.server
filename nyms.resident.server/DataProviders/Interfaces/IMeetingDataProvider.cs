using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IMeetingDataProvider
    {
        IEnumerable<Meeting> GetMeetings();
        Meeting GetMeeting(Guid referenceId);
        Meeting Insert(Meeting meeting);
        Meeting Update(Meeting meeting, int[] deletedIds = null);
    }
}
