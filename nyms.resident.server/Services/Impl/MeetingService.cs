using nyms.resident.server.DataProviders.Interfaces;
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
        public MeetingService(IMeetingDataProvider meetingDataProvider)
        {
            _meetingDataProvider = meetingDataProvider ?? throw new ArgumentNullException(nameof(meetingDataProvider));
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
    }
}