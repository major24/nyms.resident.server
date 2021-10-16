using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Model;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingDataProvider _meetingDataProvider;
        private readonly IMeetingActionDataProvider _meetingActionDataProvider;
        public MeetingService(IMeetingDataProvider meetingDataProvider, IMeetingActionDataProvider meetingActionDataProvider)
        {
            _meetingDataProvider = meetingDataProvider ?? throw new ArgumentNullException(nameof(meetingDataProvider));
            _meetingActionDataProvider = meetingActionDataProvider ?? throw new ArgumentNullException(nameof(meetingActionDataProvider));
        }

        public Meeting GetMeeting(Guid referenceId)
        {
            return _meetingDataProvider.GetMeeting(referenceId);
        }

        public IEnumerable<Meeting> GetMeetings()
        {
            return _meetingDataProvider.GetMeetings();
        }

        public Meeting Insert(Meeting meeting)
        {
            return _meetingDataProvider.Insert(meeting);
        }

        public Meeting Update(Meeting meeting, int[] deletedIds = null)
        {
            return _meetingDataProvider.Update(meeting, deletedIds);
        }

        public IEnumerable<MeetingActionResponse> GetActions()
        {
            var actions = _meetingActionDataProvider.GetActions().ToArray();
            // get comments by meeting action ids retrieved above..
            var meetingActionIds = actions.Select(a =>
            {
                return a.Id;
            }).ToArray();

            if (meetingActionIds.Any())
            {
                var cmts = _meetingActionDataProvider.GetComments(meetingActionIds);
                actions.ForEach(a =>
                {
                    a.Comments = cmts.Where(c => c.MeetingActionId == a.Id).Select(c =>
                    {
                        return new MeetingActionComment()
                        {
                            MeetingActionId = c.MeetingActionId,
                            CommentType = c.CommentType,
                            Comment = c.Comment,
                            CreatedById = c.CreatedById
                        };
                    });
                });
            }

            return actions;
        }

        public MeetingActionCompleteRequest UpdateActionCompleted(MeetingActionCompleteRequest meetingActionCompleteRequest)
        {
            return _meetingActionDataProvider.UpdateActionCompleted(meetingActionCompleteRequest);
        }

        public MeetingActionInspectRequest UpdateActionInspected(MeetingActionInspectRequest meetingActionInspectRequest)
        {
            return _meetingActionDataProvider.UpdateActionInspected(meetingActionInspectRequest);
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