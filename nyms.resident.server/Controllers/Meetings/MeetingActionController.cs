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
    [UserAuthenticationFilter]
    public class MeetingActionController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IMeetingActionService _meetingActionService;

        public MeetingActionController(IMeetingActionService meetingActionService)
        {
            _meetingActionService = meetingActionService ?? throw new ArgumentNullException(nameof(meetingActionService));
        }

        [HttpGet]
        [Route("api/meetings/actions/owners/{id}")]
        // public IHttpActionResult GetActions(bool isPending, bool isCompleted, bool isInspected)
        public IHttpActionResult GetPendingActionsByOwnerId(int id)
        {
            if (id == 0)
            {
                return BadRequest("Owner Id required");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingActionService.GetPendingActionsByOwnerId(id);

            return Ok(actions);
        }

        //public IEnumerable<MeetingActionReportResponse> GetCompletedActions();
        // Return completed but not audited records. Remove the userid who called this ftn
        [HttpGet]
        [Route("api/meetings/actions/completed/unaudited")]
        public IHttpActionResult GetCompletedActions()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting completed actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingActionService.GetCompletedActions();

            var filtered = actions.Where(a => a.CompletedById != loggedInUser.Id).ToArray();

            return Ok(filtered);
        }

        [HttpPost]
        [Route("api/meetings/actions/{id}")]
        public IHttpActionResult UpdateAction(int id, MeetingActionUpdateRequest meetingActionUpdateRequest)
        {
            if (id <= 0 || meetingActionUpdateRequest == null || meetingActionUpdateRequest.Id == 0)
            {
                return BadRequest("Action id or action request is missing.");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action updated by {loggedInUser.ForeName}");

            meetingActionUpdateRequest.UpdatedById = loggedInUser.Id;
            meetingActionUpdateRequest.UpdatedDate = DateTime.Now;

            _meetingActionService.UpdateAction(meetingActionUpdateRequest);

            return Ok(meetingActionUpdateRequest);
        }

        [HttpPost]
        [Route("api/meetings/actions/complete/{id}")]
        public IHttpActionResult UpdateActionCompleted(int id, MeetingActionCompleteRequest meetingActionCompleteRequest)
        {
            if (meetingActionCompleteRequest == null || id < 0 || meetingActionCompleteRequest.Id < 0)
            {
                return BadRequest("Action id required");
            }
            if (meetingActionCompleteRequest.Completed == ACTION_COMPLETED_STATUS.No && meetingActionCompleteRequest.Comment == "")
            {
                return BadRequest("Comments required");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action completed by {loggedInUser.ForeName}");

            meetingActionCompleteRequest.CompletedById = loggedInUser.Id;
            meetingActionCompleteRequest.CompletedDate = DateTime.Now;

            var completed = _meetingActionService.UpdateActionCompleted(meetingActionCompleteRequest);

            return Ok(completed);
        }

        [HttpPost]
        [Route("api/meetings/actions/audit/{id}")]
        public IHttpActionResult UpdateActionAudited(int id, MeetingActionAuditRequest meetingActionAuditRequest)
        {
            if (meetingActionAuditRequest == null || id < 0 || meetingActionAuditRequest.Id < 0)
            {
                return BadRequest("Action id not found");
            }
            if (meetingActionAuditRequest.Audited == ACTION_AUDITED_STATUS.Fail && meetingActionAuditRequest.Comment == "")
            {
                return BadRequest("Comments required");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action audited by {loggedInUser.ForeName}");

            meetingActionAuditRequest.AuditedById = loggedInUser.Id;
            meetingActionAuditRequest.AuditedDate = DateTime.Now;

            var audited = _meetingActionService.UpdateActionAudited(meetingActionAuditRequest);

            return Ok(audited);
        }

        [HttpPost]
        [Route("api/meetings/actions/comment/{id}")]
        public IHttpActionResult InsertActionComment(int id, MeetingActionComment meetingActionComment)
        {
            if (meetingActionComment == null || id < 0 || meetingActionComment.Comment == "")
            {
                return BadRequest("Either action id or comment not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action commented by {loggedInUser.ForeName}");

            meetingActionComment.MeetingActionId = id;
            meetingActionComment.CreatedById = loggedInUser.Id;
            meetingActionComment.CreatedDate = DateTime.Now;

            var result = _meetingActionService.InsertActionComment(meetingActionComment);

            return Ok(result);
        }

        private MeetingAction ToEntity(MeetingActionRequest meetingActionRequestDto)
        {
            return new MeetingAction()
            {
                Id = meetingActionRequestDto.Id,
                MeetingCategoryId = meetingActionRequestDto.MeetingCategoryId,
                MeetingActionItemId = meetingActionRequestDto.MeetingActionItemId,
                Name = meetingActionRequestDto.Name,
                Description = meetingActionRequestDto.Description,
                // IsAdhoc = meetingActionDto.IsAdhoc,
                OwnerId = meetingActionRequestDto.OwnerId,
                StartDate = meetingActionRequestDto.StartDate,
                CompletionDate = meetingActionRequestDto.CompletionDate,
                Priority = meetingActionRequestDto.Priority
            };
        }
    }
}





/*        private static Logger logger = Nlogger2.GetLogger();
        private readonly IMeetingActionItemLookupService _meetingActionItemService;*/

/*        public MeetingActionItemController(IMeetingActionItemLookupService meetingActionItemService)
        {
            _meetingActionItemService = meetingActionItemService ?? throw new ArgumentNullException(nameof(meetingActionItemService));
        }*/

/*        [HttpPost]
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
        }*/

/*        [HttpPost]
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
        }*/


/*            // TODO-Need find filter. Meeting date or Last 50 records etc...
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingActionService.GetActionsByMeetingIds(null);

            if (actions == null || !actions.Any())
            {
                return NotFound();
            }

            // filer actions
            IEnumerable<MeetingActionResponse> pendingActions = new List<MeetingActionResponse>();
            IEnumerable<MeetingActionResponse> completedActions = new List<MeetingActionResponse>();
            IEnumerable<MeetingActionResponse> inspectedActions = new List<MeetingActionResponse>();*/
// isCompleted = isInspected ? false :
/*            if (isPending)
            {
                pendingActions = actions.Where(a => a.Completed == null).ToArray();
            }
            if (isInspected)
            {
                inspectedActions = actions.Where(a => a.Inspected != null).ToArray();
            }
            if (isCompleted)
            {
                completedActions = actions.Where(a => a.Completed != null).ToArray();
            }*/

// var merged = pendingActions.Concat(completedActions).Concat(inspectedActions).Distinct().ToArray();

// return Ok(merged);
