using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class MeetingActionService : IMeetingActionService
    {
        private readonly IMeetingActionDataProvider _meetingActionDataProvider;
        public MeetingActionService(IMeetingDataProvider meetingDataProvider, IMeetingActionDataProvider meetingActionDataProvider)
        {
            _meetingActionDataProvider = meetingActionDataProvider ?? throw new ArgumentNullException(nameof(meetingActionDataProvider));
        }

        public IEnumerable<MeetingActionResponse> GetActionsByMeetingIds(int[] meetingIds)
        {
            return _meetingActionDataProvider.GetActionResponsesByMeetingIds(meetingIds);
        }

        public IEnumerable<MeetingActionPendingJobsResponse> GetPendingActions()
        {
            return _meetingActionDataProvider.GetPendingActions();
        }

        public IEnumerable<MeetingActionPendingJobsResponse> GetPendingActions(int ownerId)
        {
            return _meetingActionDataProvider.GetPendingActions(ownerId);
        }

        public IEnumerable<MeetingActionCompletedResponse> GetCompletedActions(int lastN_Rows = 20)
        {
            return _meetingActionDataProvider.GetCompletedActions(lastN_Rows);
        }

        public MeetingActionUpdateRequest UpdateAction(MeetingActionUpdateRequest meetingActionUpdateRequest)
        {
            return _meetingActionDataProvider.UpdateAction(meetingActionUpdateRequest);
        }

        public MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteRequest)
        {
            return _meetingActionDataProvider.UpdateActionCompleted(meetingActionCompleteRequest);
        }

        public MeetingActionAuditRequest UpdateActionAudited(MeetingActionAuditRequest meetingActionAuditRequest)
        {
            return _meetingActionDataProvider.UpdateActionAudited(meetingActionAuditRequest);
        }



        public IEnumerable<MeetingActionComment> GetComments(int[] meetingActionIds)
        {
            return _meetingActionDataProvider.GetComments(meetingActionIds);
        }

        public MeetingActionComment InsertActionComment(MeetingActionComment meetingActionComment)
        {
            return _meetingActionDataProvider.InsertActionComment(meetingActionComment);
        }
    }
}