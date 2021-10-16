using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace nyms.resident.server.Controllers.Meetings
{
    [AdminAuthenticationFilter]
    public class MeetingActionItemController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IMeetingActionItemLookupService _meetingActionItemService;

        public MeetingActionItemController(IMeetingActionItemLookupService meetingActionItemService)
        {
            _meetingActionItemService = meetingActionItemService ?? throw new ArgumentNullException(nameof(meetingActionItemService));
        }

        [HttpPost]
        [Route("api/meetings/action-items")]
        public IHttpActionResult InsertActionItem(MeetingActionItem meetingActionItem)
        {
            if (meetingActionItem == null)
            {
                return BadRequest("meeting meeting actionItem not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action item submitted by {loggedInUser.ForeName}");
            meetingActionItem.CreatedById = loggedInUser.Id;

            var result = _meetingActionItemService.Insert(meetingActionItem);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/meetings/action-items/{id}")]
        public IHttpActionResult UpdateActionItem(int id, MeetingActionItem meetingActionItem)
        {
            if (id <= 0)
            {
                return BadRequest("meeting meeting actionItem id not found");
            }
            if (meetingActionItem == null)
            {
                return BadRequest("meeting meeting actionItem not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action item updated by {loggedInUser.ForeName}");
            meetingActionItem.CreatedById = loggedInUser.Id;

            var result = _meetingActionItemService.Update(meetingActionItem);
            return Ok(result);
        }

    }
}
