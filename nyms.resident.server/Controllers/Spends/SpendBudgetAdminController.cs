using Microsoft.Ajax.Utilities;
using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace nyms.resident.server.Controllers.Spends
{
    [AdminAuthenticationFilter]
    public class SpendBudgetAdminController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly ISpendBudgetService _spendBudgetService;

        public SpendBudgetAdminController(ISpendBudgetService spendBudgetService)
        {
            _spendBudgetService = spendBudgetService ?? throw new ArgumentNullException(nameof(spendBudgetService));
        }

        [HttpGet]
        [Route("api/spends/admin/budgets")]
        public IHttpActionResult GetBudgetsForAdmin()
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            IEnumerable <SpendBudgetListResponse> spendBudgetListResponses = _spendBudgetService.GetSpendBudgetListResponses();

            if (spendBudgetListResponses == null || !spendBudgetListResponses.Any())
            {
                logger.Warn($"Budgets not found");
                return NotFound();
            }

            // For Admin, oo restriction approved or status, but do GroupBy. multiple allocation amounts bring > 1 records per budget
            var groupedByBudId = spendBudgetListResponses.GroupBy(b => b.Id); //.Select(b => b).ToArray();
            List<SpendBudgetListResponse> groupedResponse = new List<SpendBudgetListResponse>();

            groupedByBudId.ForEach(g =>
            {
                var curBudget = g.FirstOrDefault();
                decimal bTotal = 0;
                g.ForEach(b =>
                {
                    bTotal += b.BudgetTotal;
                });
                curBudget.BudgetTotal = bTotal;
                groupedResponse.Add(curBudget);
            });

            return Ok(groupedResponse);
        }
        
        // Cannot use with USER. Returs alloction to be edited
        [HttpGet]
        [Route("api/spends/admin/budgets/{referenceId}")]
        public IHttpActionResult GetBudgetByReferenceId(string referenceId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            SpendBudgetResponse spendBudgetResponse = _spendBudgetService.GetSpendBudget(new Guid(referenceId));

            if (spendBudgetResponse == null)
            {
                logger.Warn($"Budget not found");
                return NotFound();
            }

            return Ok(spendBudgetResponse);
        }

        [HttpPost]
        [Route("api/spends/admin/budgets")]
        public IHttpActionResult InsertBudget(SpendBudgetRequest spendBudgetRequest)
        {
            if (spendBudgetRequest == null)
            {
                return BadRequest("Spend budget is null");
            }

            if (spendBudgetRequest.SpendCategoryId <= 0 || spendBudgetRequest.CareHomeId <= 0)
            {
                return BadRequest("Care category id and care home id is required");
            }

            if (string.IsNullOrEmpty(spendBudgetRequest.DateFrom.ToString()) || string.IsNullOrEmpty(spendBudgetRequest.DateTo.ToString()))
            {
                return BadRequest("Budget dates are required fields");
            }

            if (spendBudgetRequest.SpendBudgetAllocations.FirstOrDefault().Amount <= 0)
            {
                return BadRequest("Budget amount is required");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            spendBudgetRequest.CreatedById = loggedInUser.Id;
            spendBudgetRequest.UpdatedById = loggedInUser.Id;

            var result = _spendBudgetService.Insert(spendBudgetRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/spends/admin/budgets/{referenceId}")]
        public IHttpActionResult UpdateBudget(string referenceId, SpendBudgetRequest spendBudgetRequest)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                return BadRequest("referenceId not found");
            }

            if (spendBudgetRequest == null)
            {
                return BadRequest("spendBudgetEntity not found");
            }

            if (spendBudgetRequest.SpendCategoryId <= 0 || spendBudgetRequest.CareHomeId <= 0)
            {
                return BadRequest("SpendCateCareHome id and amount not found");
            }

            if (string.IsNullOrEmpty(spendBudgetRequest.DateFrom.ToString()) || string.IsNullOrEmpty(spendBudgetRequest.DateTo.ToString()))
            {
                return BadRequest("Missing required fields");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            spendBudgetRequest.UpdatedById = loggedInUser.Id;
            spendBudgetRequest.UpdatedDate = DateTime.Now;

            var result = _spendBudgetService.Update(spendBudgetRequest);
            return Ok(result);
        }






    }
}
