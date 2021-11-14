using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IMeetingService
    {
        IEnumerable<Meeting> GetMeetings(int lastN_Rows);
        Meeting GetMeeting(Guid referenceId);
        Meeting Insert(Meeting meeting);
        Meeting Update(Meeting meeting);
    }
}
