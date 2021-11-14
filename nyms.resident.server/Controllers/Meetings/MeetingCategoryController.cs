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
        private readonly IMeetingCategoryAndActionItemLookupService _meetingCategoryLookupService;

        public MeetingCategoryController(IMeetingCategoryAndActionItemLookupService meetingCategoryAndActionItemsLookupService)
        {
            _meetingCategoryLookupService = meetingCategoryAndActionItemsLookupService ?? throw new ArgumentNullException(nameof(meetingCategoryAndActionItemsLookupService));
        }

        [HttpGet]
        [Route("api/meetings/categories-and-actions-items")]
        public IHttpActionResult GetMeetingCategoriesAndActionItems()
        {
            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category fetched by {loggedInUser.ForeName}");

            var meetingCategories = _meetingCategoryLookupService.GetMeetingCategoriesAndActionItems();
            if (meetingCategories == null || !meetingCategories.Any())
            {
                logger.Warn($"No meeting categories found");
                return NotFound();
            }

            var meetingCategoriesResponse = meetingCategories.Select(c =>
            {
                var meetingActionItemsResponse = c.MeetingActionItems.Select(a =>
                {
                    return new MeetingActionItemResponse()
                    {
                        Id = a.Id,
                        MeetingCategoryId = a.MeetingCategoryId,
                        Name = a.Name,
                        Description = a.Description,
                        IsAdhoc = a.IsAdhoc == "Y" ? true : false
                    };                  
                });

                return new MeetingCategoryResponse()
                {
                    Id = c.Id,
                    Name = c.Name,
                    MeetingActionItems = meetingActionItemsResponse
                };
            }).ToArray();

            return Ok(meetingCategoriesResponse);
        }

        [HttpPost]
        [Route("api/meetings/categories-and-actions-items")]
        public IHttpActionResult InsertCategoryAndActionItems(MeetingCategory meetingCategory)
        {
            if (meetingCategory == null)
            {
                return BadRequest("meeting category not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category created by {loggedInUser.ForeName}");
            meetingCategory.CreatedById = loggedInUser.Id;

            meetingCategory.MeetingActionItems.ForEach(a =>
            {
                a.IsAdhoc = a.IsAdhoc == "true" ? "Y" : "N";
                a.CreatedById = loggedInUser.Id;
            });

            var result = _meetingCategoryLookupService.InsertCategoryAndActionItems(meetingCategory);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/meetings/categories/action-items/{id}")]
        public IHttpActionResult UpdateActionItem(int id, MeetingActionItem meetingActionItem)
        {
            if (id <= 0)
            {
                return BadRequest("meeting action item id not found");
            }
            if (meetingActionItem == null)
            {
                return BadRequest("meeting action item not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category created by {loggedInUser.ForeName}");
            meetingActionItem.CreatedById = loggedInUser.Id;

            meetingActionItem.IsAdhoc = (meetingActionItem.IsAdhoc == "true") ? "Y" : "N";

            var result = _meetingCategoryLookupService.UpdateActionItem(meetingActionItem);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/meetings/categories/action-items")]
        public IHttpActionResult InsertActionItem(MeetingActionItem meetingActionItem)
        {
            if (meetingActionItem == null)
            {
                return BadRequest("meeting action item not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Meeting Category created by {loggedInUser.ForeName}");
            meetingActionItem.CreatedById = loggedInUser.Id;

            meetingActionItem.IsAdhoc = (meetingActionItem.IsAdhoc == "true") ? "Y" : "N";

            var result = _meetingCategoryLookupService.InsertActionItem(meetingActionItem);
            return Ok(result);
        }

    }
}
