using nyms.resident.server.Model;
using nyms.resident.server.Models;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IMeetingActionDataProvider
    {
        IEnumerable<MeetingActionResponse> GetActions();
        MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteOrCommentRequest);
        IEnumerable<MeetingActionComment> GetComments(int[] meetingActionIds);
        MeetingActionComment InsertActionComment(MeetingActionComment meetingActionComment);
        MeetingActionInspectRequest UpdateActionInspected(MeetingActionInspectRequest meetingActionInspectRequest);
    }
}
