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
        private readonly IMeetingActionService _meetingActionService;

        public MeetingController(IMeetingService meetingService, IMeetingActionService meetingActionService)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
            _meetingActionService = meetingActionService ?? throw new ArgumentNullException(nameof(meetingActionService));
        }

        [HttpGet]
        [Route("api/meetings/meetings")]
        public IHttpActionResult GetMeetings()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meetings fetched by {loggedInUser.ForeName}");

            var meetings = _meetingService.GetMeetings(5); // Default max num of rows

            if (meetings == null || !meetings.Any())
            {
                return NotFound();
            }

            // convert Meeting to MeetingResponse obj
            var meetingsResp = meetings.Select(m => {
                return new MeetingResponse()
                {
                    Id = m.Id,
                    ReferenceId = m.ReferenceId,
                    Title = m.Title,
                    MeetingDate = m.MeetingDate,
                    OwnerId = m.OwnerId
                };
            }).ToArray();

            var meetingIds = meetingsResp.Select(m => m.Id).ToArray();

            var actions = _meetingActionService.GetActionsByMeetingIds(meetingIds);

            // assign actions to respective meeting obj
            meetingsResp.ForEach(m =>
            {
                m.MeetingActions = actions.Where(a => a.MeetingId == m.Id).ToArray();
            });
            
            return Ok(meetingsResp);
        }

        // TODO. NOT sure below is done. Remove if not need when developing UI
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
            IEnumerable<MeetingActionRequest> actionsDto = meeting.MeetingActions.Select(act =>
            {
                return new MeetingActionRequest()
                {
                    Id = act.Id,
                    MeetingCategoryId = act.MeetingCategoryId,
                    MeetingActionItemId = act.MeetingActionItemId,
                    Name = act.Name,
                    Description = act.Description,
/*                    IsAdhoc = act.IsAdhoc,*/
                    OwnerId = act.OwnerId,
                    StartDate = act.StartDate,
                    CompletionDate = act.CompletionDate,
                    Priority = act.Priority,
                };
            }).ToArray();
            // return meetingDto
            MeetingRequest meetingDto = new MeetingRequest()
            {
                Id = meeting.Id,
                ReferenceId = meeting.ReferenceId,
                Title = meeting.Title,
                // Description = meeting.Description,
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
        public IHttpActionResult InsertMeeting(MeetingRequest meetingRequest)
        {
            if (meetingRequest == null)
            {
                return BadRequest("Meeting not found");
            }

            if (meetingRequest.MeetingActions == null || !meetingRequest.MeetingActions.Any())
            {
                return BadRequest("Meeting actions not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting created by {loggedInUser.ForeName}");

            List<MeetingActionRequest> tempActions = meetingRequest.MeetingActions.ToList();

            // Repetitive actions for fortnight or weekly
            // Weekly = Every 7 days / Fortnight = Every 14 days / Monthly = Once a month
            var frequentActions = meetingRequest.MeetingActions.Where(a => a.Frequency != ACTION_FREQUENCY.None).ToArray();    //!string.IsNullOrEmpty(a.Frequency));
            
            if (frequentActions.Any())
            {
                frequentActions.ForEach(a =>
                {
                    var dates = CreateFutureDates(a.CompletionDate, (int)a.Frequency, a.Repetitive).ToList();
                    for(int i = 1; i < dates.Count; i++) // skip the first date
                    {
                        var action = Clone(a);
                        action.CompletionDate = dates[i];
                        tempActions.Add(action);
                    }
                });
            }

            var actions = tempActions.Select(a =>
            {
                return ToEntity(a);
            }).ToArray();
            actions.ForEach(a =>
            {
                a.CreatedById = loggedInUser.Id;
            });

            Meeting meeting = ToEntity(meetingRequest);
            meeting.MeetingActions = actions;
            meeting.CreatedById = loggedInUser.Id;

            meeting.ReferenceId = Guid.NewGuid();
            meeting.OwnerId = loggedInUser.Id;
            var result = _meetingService.Insert(meeting);

            return Ok(result);
        }


        private Meeting ToEntity(MeetingRequest meetingRequest)
        {
            return new Meeting()
            {
                Id = meetingRequest.Id,
                ReferenceId = meetingRequest.ReferenceId,
                Title = meetingRequest.Title,
                MeetingDate = meetingRequest.MeetingDate,
                OwnerId = meetingRequest.OwnerId
            };
        }

        private MeetingAction ToEntity(MeetingActionRequest meetingActionRequest)
        {
            return new MeetingAction()
            {
                Id = meetingActionRequest.Id,
                MeetingCategoryId = meetingActionRequest.MeetingCategoryId,
                MeetingActionItemId = meetingActionRequest.MeetingActionItemId,
                Name = meetingActionRequest.Name,
                Description = meetingActionRequest.Description,
                OwnerId = meetingActionRequest.OwnerId,
                StartDate = meetingActionRequest.StartDate,
                CompletionDate = meetingActionRequest.CompletionDate,
                Priority = meetingActionRequest.Priority
            };
        }

        private MeetingActionRequest Clone(MeetingActionRequest meetingActionRequest)
        {
            return new MeetingActionRequest()
            {
                Id = meetingActionRequest.Id,
                MeetingCategoryId = meetingActionRequest.MeetingCategoryId,
                MeetingActionItemId = meetingActionRequest.MeetingActionItemId,
                Name = meetingActionRequest.Name,
                Description = meetingActionRequest.Description,
                OwnerId = meetingActionRequest.OwnerId,
                StartDate = meetingActionRequest.StartDate,
                CompletionDate = meetingActionRequest.CompletionDate,
                Priority = meetingActionRequest.Priority,
                Frequency = meetingActionRequest.Frequency,
                Repetitive = meetingActionRequest.Repetitive,
                Checked = meetingActionRequest.Checked
            };
        }

        private IEnumerable<DateTime> CreateFutureDates(DateTime startDate, int stepDays, int repetitive)
        {
            List<DateTime> dates = new List<DateTime>() { startDate };
            var tmp = startDate;
            for (int i = 0; i < repetitive; i++)
            {
                tmp = tmp.AddDays(stepDays);
                dates.Add(tmp);
            }
            return dates.ToArray();
        }

    }
}
