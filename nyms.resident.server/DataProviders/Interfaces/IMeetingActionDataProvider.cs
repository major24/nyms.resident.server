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
        IEnumerable<MeetingActionResponse> GetActionResponsesByMeetingIds(int[] meetingIds);
        IEnumerable<MeetingActionPendingJobsResponse> GetPendingActions();
        IEnumerable<MeetingActionPendingJobsResponse> GetPendingActions(int ownerId);
        IEnumerable<MeetingActionCompletedResponse> GetCompletedActions(int lastN_Rows);
        MeetingActionUpdateRequest UpdateAction(MeetingActionUpdateRequest meetingActionUpdateRequest);
        MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteRequest);
        MeetingActionAuditRequest UpdateActionAudited(MeetingActionAuditRequest meetingActionAuditRequest);

        IEnumerable<MeetingActionComment> GetComments(int[] meetingActionIds);
        MeetingActionComment InsertActionComment(MeetingActionComment meetingActionComment);
        
    }
}
