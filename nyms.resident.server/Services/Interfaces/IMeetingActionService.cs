﻿using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IMeetingActionService
    {
        IEnumerable<MeetingActionResponse> GetActionsByMeetingIds(int[] meetingIds);
        IEnumerable<MeetingActionPendingJobsResponse> GetPendingActionsByOwnerId(int ownerId);
        IEnumerable<MeetingActionCompletedResponse> GetCompletedActions();
        MeetingActionUpdateRequest UpdateAction(MeetingActionUpdateRequest meetingActionUpdateRequest);
        MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteRequest);
        MeetingActionAuditRequest UpdateActionAudited(MeetingActionAuditRequest meetingActionAuditRequest);

        IEnumerable<MeetingActionComment> GetComments(int[] meetingActionIds);
        MeetingActionComment InsertActionComment(MeetingActionComment meetingActionComment);
        
    }
}
