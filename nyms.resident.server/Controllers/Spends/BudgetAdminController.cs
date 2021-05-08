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
    public class BudgetAdminController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IBudgetService _budgetService;

        public BudgetAdminController(IBudgetService budgetService)
        {
            _budgetService = budgetService ?? throw new ArgumentNullException(nameof(budgetService));
        }

        [HttpGet]
        [Route("api/spends/admin/budgets")]
        public IHttpActionResult GetBudgetsForAdmin()
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            IEnumerable <BudgetListResponse> budgetListResponses = _budgetService.GetBudgetListResponses();

            if (budgetListResponses == null || !budgetListResponses.Any())
            {
                logger.Warn($"Budgets not found");
                return NotFound();
            }

            // For Admin, no restriction on approved or status closed/cancelled, 
            // but do GroupBy. multiple allocation amounts bring > 1 records per budget
            var groupedByBudId = budgetListResponses.GroupBy(b => b.Id);
            List<BudgetListResponse> groupedResponse = new List<BudgetListResponse>();

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
        
        // Cannot use with USER. Returs alloctions to be edited
        [HttpGet]
        [Route("api/spends/admin/budgets/{referenceId}/allocations")]
        public IHttpActionResult GetBudgetByReferenceId(string referenceId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            BudgetResponse budgetResponse = _budgetService.GetBudget(new Guid(referenceId));

            if (budgetResponse == null)
            {
                logger.Warn($"Budget not found");
                return NotFound();
            }

            return Ok(budgetResponse);
        }

        [HttpPost]
        [Route("api/spends/admin/budgets")]
        public IHttpActionResult InsertBudget(BudgetRequest budgetRequest)
        {
            if (budgetRequest == null)
            {
                return BadRequest("Spend budget is null");
            }

            if (budgetRequest.SpendCategoryId <= 0 || budgetRequest.CareHomeId <= 0)
            {
                return BadRequest("Care category id and care home id is required");
            }

            if (string.IsNullOrEmpty(budgetRequest.DateFrom.ToString()) || string.IsNullOrEmpty(budgetRequest.DateTo.ToString()))
            {
                return BadRequest("Budget dates are required fields");
            }

            if (budgetRequest.BudgetAllocations.FirstOrDefault().Amount <= 0)
            {
                return BadRequest("Budget amount is required");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            budgetRequest.CreatedById = loggedInUser.Id;
            budgetRequest.UpdatedById = loggedInUser.Id;

            var result = _budgetService.Insert(budgetRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/spends/admin/budgets/{referenceId}")]
        public IHttpActionResult UpdateBudget(string referenceId, BudgetRequest budgetRequest)
        {
            if (string.IsNullOrEmpty(referenceId))
            {
                return BadRequest("referenceId not found");
            }

            if (budgetRequest == null)
            {
                return BadRequest("spendBudgetEntity not found");
            }

            if (budgetRequest.SpendCategoryId <= 0 || budgetRequest.CareHomeId <= 0)
            {
                return BadRequest("SpendCateCareHome id and amount not found");
            }

            if (string.IsNullOrEmpty(budgetRequest.DateFrom.ToString()) || string.IsNullOrEmpty(budgetRequest.DateTo.ToString()))
            {
                return BadRequest("Missing required fields");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            budgetRequest.UpdatedById = loggedInUser.Id;
            budgetRequest.UpdatedDate = DateTime.Now;

            var result = _budgetService.Update(budgetRequest);
            return Ok(result);
        }

    }
}
