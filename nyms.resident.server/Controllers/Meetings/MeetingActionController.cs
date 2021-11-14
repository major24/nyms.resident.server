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
        private readonly IUserService _userService;

        public MeetingActionController(IMeetingActionService meetingActionService, IUserService userService)
        {
            _meetingActionService = meetingActionService ?? throw new ArgumentNullException(nameof(meetingActionService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [Route("api/meetings/actions/pending/owners")]
        public IHttpActionResult GetPendingActions()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingActionService.GetPendingActions();

            return Ok(actions);
        }

        [HttpGet]
        [Route("api/meetings/actions/pending/owners/{userRefId}")]
        public IHttpActionResult GetPendingActionsByOwnerId(string userRefId)
        {
            if (string.IsNullOrEmpty(userRefId))
            {
                return BadRequest("Owner ReferenceId required");
            }

            Models.User user = _userService.GetByRefereneId(new Guid(userRefId)).Result;
            if (user == null)
            {
                return BadRequest("Owner not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingActionService.GetPendingActions(user.Id);

            return Ok(actions);
        }

        // Return completed but not audited records. Remove the userid who called this ftn
        [HttpGet]
        [Route("api/meetings/actions/completed/unaudited")]
        public IHttpActionResult GetCompletedActions()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting completed actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingActionService.GetCompletedActions(5);

            var filtered = actions.Where(a => a.CompletedById != loggedInUser.Id).ToList();

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
                OwnerId = meetingActionRequestDto.OwnerId,
                StartDate = meetingActionRequestDto.StartDate,
                CompletionDate = meetingActionRequestDto.CompletionDate,
                Priority = meetingActionRequestDto.Priority
            };
        }
    }
}


