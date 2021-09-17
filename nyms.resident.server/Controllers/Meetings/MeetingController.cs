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

            var result = _meetingService.GetMeetings();

            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result);
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
            /*            meeting.MeetingActions = actionsDto as MeetingAction[];

                        // If actions found, add [Checked] prop to true, to manage in UI
                        meeting.MeetingActions.ForEach(a =>
                        {
                            a.Checked = true;
                        });*/
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
            });
            actions.ForEach(a =>
            {
                a.CreatedById = loggedInUser.Id;
            });

            // meeting
            Meeting meeting = ToEntity(meetingDto);
            meeting.MeetingActions = actions;
            meeting.CreatedById = loggedInUser.Id;


            /*meeting.CreatedById = loggedInUser.Id;
            meeting.MeetingActions.ForEach(a =>
            {
                a.CreatedById = loggedInUser.Id;
            });*/

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
            });
            actions.ForEach(a =>
            {
                a.UpdatedById = loggedInUser.Id;
            });

            // meeting
            Meeting meeting = ToEntity(meetingDto);
            meeting.MeetingActions = actions;
            meeting.UpdatedById = loggedInUser.Id;

/*            IEnumerable<MeetingAction> actions = meetingDto.MeetingActions.Select(act =>
            {
                return new MeetingAction()
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
                    UpdatedById = loggedInUser.Id
                };
            }).ToArray();*/



            // meetingDto.UpdatedById = loggedInUser.Id;
            /*            meetingDto.MeetingActions.ForEach(a =>
                        {
                            a.UpdatedById = loggedInUser.Id;
                        });*/

            var result = _meetingService.Update(meeting, meetingDto.DeletedIds);
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
