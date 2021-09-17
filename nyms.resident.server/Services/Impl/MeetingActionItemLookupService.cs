using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class MeetingActionItemLookupService : IMeetingActionItemLookupService
    {
        private readonly IMeetingActionItemLookupDataProvider _meetingActionItemLookupDataProvider;
        public MeetingActionItemLookupService(IMeetingActionItemLookupDataProvider meetingActionItemLookupDataProvider)
        {
            _meetingActionItemLookupDataProvider = meetingActionItemLookupDataProvider ?? throw new ArgumentNullException(nameof(meetingActionItemLookupDataProvider));

        }
        public IEnumerable<MeetingActionItem> GetMeetingActionItems()
        {
            return _meetingActionItemLookupDataProvider.GetMeetingActionItems();
        }

        public MeetingActionItem Insert(MeetingActionItem meetingActionItem)
        {
            return _meetingActionItemLookupDataProvider.Insert(meetingActionItem);
        }

        public MeetingActionItem Update(MeetingActionItem meetingActionItem)
        {
            return _meetingActionItemLookupDataProvider.Update(meetingActionItem);
        }
    }
}