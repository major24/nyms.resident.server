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

        public IEnumerable<Meeting> GetMeetings(int lastN_Rows = 20)
        {
            return _meetingDataProvider.GetMeetings(lastN_Rows);
        }

        public Meeting Insert(Meeting meeting)
        {
            return _meetingDataProvider.Insert(meeting);
        }

        public Meeting Update(Meeting meeting)
        {
            return _meetingDataProvider.Update(meeting);
        }

    }
}