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
using Microsoft.Ajax.Utilities;
using nyms.resident.server.Model;

namespace nyms.resident.server.Controllers.Meetings
{
    [UserAuthenticationFilter]
    public class MeetingController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
        }

        [HttpGet]
        [Route("api/meetings/meetings")]
        public IHttpActionResult GetMeetings()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meetings fetched by {loggedInUser.ForeName}");

            var meetings = _meetingService.GetMeetings();

            if (meetings == null || !meetings.Any())
            {
                return NotFound();
            }

            // return meetingDto list
            IEnumerable<MeetingDto> meetingsDto = meetings.Select(meeting => {
                // return meetingDto and actionsDto insted of meeting and actions                        
                IEnumerable<MeetingActionDto> actionsDto = meeting.MeetingActions.Select(act =>
                {
                    return new MeetingActionDto()
                    {
                        Id = act.Id,
                        MeetingCategoryId = act.MeetingCategoryId,
                        MeetingActionItemId = act.MeetingActionItemId,
                        Name = act.Name,
                        Description = act.Description,
                        IsAdhoc = act.IsAdhoc,
                        OwnerId = act.OwnerId,
                        StartDate = act.StartDate,
                        CompletionDate = act.CompletionDate,
                        Priority = act.Priority,
                    };
                }).ToArray();

                return new MeetingDto()
                {
                    Id = meeting.Id,
                    ReferenceId = meeting.ReferenceId,
                    Title = meeting.Title,
                    Description = meeting.Description,
                    MeetingDate = meeting.MeetingDate,
                    OwnerId = meeting.OwnerId,
                    MeetingActions = actionsDto

                };
            }); 
            
            return Ok(meetingsDto);
        }

        [HttpGet]
        [Route("api/meetings/meetings/{referenceId}")]
        public IHttpActionResult GetMeeting(string referenceId)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                return BadRequest("meeting reference id required");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meetings fetched by {loggedInUser.ForeName}");

            var meeting = _meetingService.GetMeeting(new Guid(referenceId));

            if (meeting == null)
            {
                return NotFound();
            }
            // return meetingDto and actionsDto insted of meeting and actions                        
            IEnumerable<MeetingActionDto> actionsDto = meeting.MeetingActions.Select(act =>
            {
                return new MeetingActionDto()
                {
                    Id = act.Id,
                    MeetingCategoryId = act.MeetingCategoryId,
                    MeetingActionItemId = act.MeetingActionItemId,
                    Name = act.Name,
                    Description = act.Description,
                    IsAdhoc = act.IsAdhoc,
                    OwnerId = act.OwnerId,
                    StartDate = act.StartDate,
                    CompletionDate = act.CompletionDate,
                    Priority = act.Priority,
                };
            }).ToArray();
            // return meetingDto
            MeetingDto meetingDto = new MeetingDto()
            {
                Id = meeting.Id,
                ReferenceId = meeting.ReferenceId,
                Title = meeting.Title,
                Description = meeting.Description,
                MeetingDate = meeting.MeetingDate,
                OwnerId = meeting.OwnerId,
                MeetingActions = actionsDto

            };
            
            meetingDto.MeetingActions.ForEach(a =>
            {
                a.Checked = true;
            });

            return Ok(meetingDto);
        }

        [HttpPost]
        [Route("api/meetings/meetings")]
        public IHttpActionResult InsertMeeting(MeetingDto meetingDto)
        {
            if (meetingDto == null)
            {
                return BadRequest("meeting not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting created by {loggedInUser.ForeName}");

            // convert to meeting and meetingActions enity
            IEnumerable<MeetingAction> actions = meetingDto.MeetingActions.Select(a => {
                return ToEntity(a);
            }).ToArray();
            actions.ForEach(a =>
            {
                a.CreatedById = loggedInUser.Id;
            });

            // meeting
            Meeting meeting = ToEntity(meetingDto);
            meeting.MeetingActions = actions;
            meeting.CreatedById = loggedInUser.Id;

            meeting.ReferenceId = Guid.NewGuid();
            meeting.OwnerId = loggedInUser.Id;
            var result = _meetingService.Insert(meeting);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/meetings/meetings/{referenceId}")]
        public IHttpActionResult UpdateMeeting(string referenceId, MeetingDto meetingDto)
        {
            if (string.IsNullOrEmpty(referenceId) || meetingDto == null)
            {
                return BadRequest("meeting or reference id not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting updated by {loggedInUser.ForeName}");

            // convert to meeting and meetingActions enity
            IEnumerable<MeetingAction> actions = meetingDto.MeetingActions.Select(a => {
                return ToEntity(a);
            }).ToArray();
            actions.ForEach(a =>
            {
                a.UpdatedById = loggedInUser.Id;
            });

            // meeting
            Meeting meeting = ToEntity(meetingDto);
            meeting.MeetingActions = actions;
            meeting.UpdatedById = loggedInUser.Id;

            var result = _meetingService.Update(meeting, meetingDto.DeletedIds);
            return Ok(result);
        }


        // =========================================================
        // Actions - GET and POST status and comments...
        // =========================================================
        [HttpGet]
        [Route("api/meetings/meetings/actions/xxx")]
        public IHttpActionResult GetActionsTEMP(bool isPending, bool isCompleted, bool isInspected)
        {
            var x = isPending;
            var xx = isCompleted;
            var xxx = isInspected;
            return GetActions(isPending, isCompleted, isInspected);
        }

        [HttpGet]
        [Route("api/meetings/meetings/actions")]
        public IHttpActionResult GetActions(bool isPending, bool isCompleted, bool isInspected)
        {
            // TODO-Need find filter. Meeting date or Last 50 records etc...
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting actions fetched by {loggedInUser.ForeName}");

            var actions = _meetingService.GetActions();

            if (actions == null || !actions.Any())
            {
                return NotFound();
            }

            // filer actions
            IEnumerable<MeetingActionResponse> pendingActions = new List<MeetingActionResponse>();
            IEnumerable<MeetingActionResponse> completedActions = new List<MeetingActionResponse>();
            IEnumerable<MeetingActionResponse> inspectedActions = new List<MeetingActionResponse>();
            // isCompleted = isInspected ? false :
            if (isPending)
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
            }

            var merged = pendingActions.Concat(completedActions).Concat(inspectedActions).Distinct().ToArray();
            
            return Ok(merged);
        }

        [HttpPost]
        [Route("api/meetings/meetings/actions/complete/{id}")]
        public IHttpActionResult UpdateActionCompleted(int id, MeetingActionCompleteRequest meetingActionCompleteRequest)
        {
            if (meetingActionCompleteRequest == null || id < 0 || meetingActionCompleteRequest.Id < 0 || meetingActionCompleteRequest.Completed == "")
            {
                return BadRequest("Either action id or complete status not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action completed by {loggedInUser.ForeName}");

            meetingActionCompleteRequest.CompletedById = loggedInUser.Id;
            meetingActionCompleteRequest.CompletedDate = DateTime.Now;

            var completed = _meetingService.UpdateActionCompleted(meetingActionCompleteRequest);

            return Ok(completed);
        }

        [HttpPost]
        [Route("api/meetings/meetings/actions/inspection/{id}")]
        public IHttpActionResult UpdateActionInspected(int id, MeetingActionInspectRequest meetingActionInspectRequest)
        {
            if (meetingActionInspectRequest == null || id < 0 || meetingActionInspectRequest.Id < 0 || meetingActionInspectRequest.Inspected == "")
            {
                return BadRequest("Either action id or complete status not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting action inspected by {loggedInUser.ForeName}");

            meetingActionInspectRequest.InspectedById = loggedInUser.Id;
            meetingActionInspectRequest.InspectedDate = DateTime.Now;

            var completed = _meetingService.UpdateActionInspected(meetingActionInspectRequest);

            return Ok(completed);
        }

        [HttpPost]
        [Route("api/meetings/meetings/actions/comment/{id}")]
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

            var result = _meetingService.InsertActionComment(meetingActionComment);

            return Ok(result);
        }


        private Meeting ToEntity(MeetingDto meetingDto)
        {
            return new Meeting()
            {
                Id = meetingDto.Id,
                ReferenceId = meetingDto.ReferenceId,
                Title = meetingDto.Title,
                Description = meetingDto.Description,
                MeetingDate = meetingDto.MeetingDate,
                OwnerId = meetingDto.OwnerId
            };
        }

        private MeetingAction ToEntity(MeetingActionDto meetingActionDto)
        {
            return new MeetingAction()
            {
                Id = meetingActionDto.Id,
                MeetingCategoryId = meetingActionDto.MeetingCategoryId,
                MeetingActionItemId = meetingActionDto.MeetingActionItemId,
                Name = meetingActionDto.Name,
                Description = meetingActionDto.Description,
                IsAdhoc = meetingActionDto.IsAdhoc,
                OwnerId = meetingActionDto.OwnerId,
                StartDate = meetingActionDto.StartDate,
                CompletionDate = meetingActionDto.CompletionDate,
                Priority = meetingActionDto.Priority
            };
        }

    }
}
