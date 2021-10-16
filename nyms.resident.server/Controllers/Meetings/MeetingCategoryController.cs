using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;

namespace nyms.resident.server.Controllers.Meetings
{
    [AdminAuthenticationFilter]
    public class MeetingCategoryController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IMeetingCategoryLookupService _meetingCategoryLookupService;
        private readonly IMeetingActionItemLookupService _meetingActionItemLookupService; // IMeetingAgendaLookupService _meetingAgendaLookupService;

        public MeetingCategoryController(IMeetingCategoryLookupService meetingCategoryLookupService,
            IMeetingActionItemLookupService meetingActionItemLookupService)
        {
            _meetingCategoryLookupService = meetingCategoryLookupService ?? throw new ArgumentNullException(nameof(meetingCategoryLookupService));
            _meetingActionItemLookupService = meetingActionItemLookupService ?? throw new ArgumentNullException(nameof(meetingActionItemLookupService));
        }

        [HttpGet]
        [Route("api/meetings/categories")]
        public IHttpActionResult GetMeetingCategories()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category fetched by {loggedInUser.ForeName}");

            var meetingCategories = _meetingCategoryLookupService.GetMeetingCategories();
            if (meetingCategories == null || !meetingCategories.Any())
            {
                logger.Warn($"No meeting categories found");
                return NotFound();
            }

            return Ok(meetingCategories.ToArray());
        }

        [HttpGet]
        [Route("api/meetings/categories/action-items")]
        public IHttpActionResult GetMeetingCategoriesAndAgendas()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category fetched by {loggedInUser.ForeName}");

            var meetingCategories = _meetingCategoryLookupService.GetMeetingCategories();
            if (meetingCategories == null || !meetingCategories.Any())
            {
                logger.Warn($"No meeting categories found");
                return NotFound();
            }
            // Filter DTO to send back
            var meetingCategoriesDto = meetingCategories.Select(c =>
            {
                return new MeetingCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    MeetingActionItems = null
                };
            }).ToArray();

            var meetingActionItems = _meetingActionItemLookupService.GetMeetingActionItems();
            if (meetingActionItems.Any())
            {
                // Filter action items to send back. make new dto
                var meetingActionItemsDto = meetingActionItems.Select(a =>
                {
                    return new MeetingActionItemDto
                    {
                        Id = a.Id,
                        MeetingCategoryId = a.MeetingCategoryId,
                        Name = a.Name,
                        Description = a.Description,
                        IsAdhoc = a.IsAdhoc
                    };
                }).ToArray();
                // from above list find matching items
                meetingCategoriesDto.ForEach(c =>
                {
                    c.MeetingActionItems = meetingActionItemsDto.Where(ai => ai.MeetingCategoryId == c.Id).ToArray();
                });
            }

            return Ok(meetingCategoriesDto.ToArray()); //meetingCategories.ToArray());
        }

        [HttpPost]
        [Route("api/meetings/categories")]
        public IHttpActionResult InsertCategory(MeetingCategory meetingCategory)
        {
            if (meetingCategory == null)
            {
                return BadRequest("meeting category not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category created by {loggedInUser.ForeName}");
            meetingCategory.CreatedById = loggedInUser.Id;

            var result = _meetingCategoryLookupService.Insert(meetingCategory);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/meetings/categories/{id}")]
        public IHttpActionResult UpdateCategory(int id, MeetingCategory meetingCategory)
        {
            if (id <= 0)
            {
                return BadRequest("meeting category id not found");
            }
            if (meetingCategory == null)
            {
                return BadRequest("meeting category not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category created by {loggedInUser.ForeName}");
            meetingCategory.CreatedById = loggedInUser.Id;

            var result = _meetingCategoryLookupService.Update(meetingCategory);
            return Ok(result);
        }

    }
}
