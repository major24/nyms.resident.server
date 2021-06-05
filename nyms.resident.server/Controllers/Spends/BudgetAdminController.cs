using Microsoft.Ajax.Utilities;
using NLog;
using nyms.resident.server.Core;
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
        private readonly ISecurityService _securityService;

        public BudgetAdminController(IBudgetService budgetService,
            ISecurityService securityService)
        {
            _budgetService = budgetService ?? throw new ArgumentNullException(nameof(budgetService));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        [HttpGet]
        [Route("api/spends/admin/budgets/{dateFrom}/{dateTo}")]
        public IHttpActionResult GetBudgetsForAdmin(string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(dateFrom)) throw new ArgumentNullException(nameof(dateFrom));
            if (string.IsNullOrEmpty(dateTo)) throw new ArgumentNullException(nameof(dateTo));

            DateTime.TryParse(dateFrom, out DateTime dateFromInput);
            DateTime.TryParse(dateTo, out DateTime dateToInput);

            if (dateFromInput == null || dateToInput == null) throw new ArgumentException("Invalid dates");

            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            // For user find allowed spend category ids
            var spendCategoryIdsAllowed = _securityService.GetSpendCategoryRoleIds(user.Id).ToArray();

            IEnumerable <BudgetListResponse> budgetListResponses = _budgetService.GetBudgetListResponsesForAdmin(dateFromInput,
                                                                                                        dateToInput,
                                                                                                        spendCategoryIdsAllowed);
            if (budgetListResponses == null || !budgetListResponses.Any())
            {
                logger.Warn($"Budgets not found");
                return NotFound();
            }

            return Ok(budgetListResponses);
        }

        [HttpGet]
        [Route("api/spends/admin/budgets/{dateFrom}/{dateTo}/spendsreport")]
        public IHttpActionResult GetBudgetsForSummryReport(string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(dateFrom)) throw new ArgumentNullException(nameof(dateFrom));
            if (string.IsNullOrEmpty(dateTo)) throw new ArgumentNullException(nameof(dateTo));

            DateTime.TryParse(dateFrom, out DateTime dateFromInput);
            DateTime.TryParse(dateTo, out DateTime dateToInput);

            if (dateFromInput == null || dateToInput == null) throw new ArgumentException("Invalid dates");

            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            // For user find allowed spend category ids
            var spendCategoryIdsAllowed = _securityService.GetSpendCategoryRoleIds(user.Id).ToArray();

            IEnumerable<BudgetListResponse> budgetListResponses = _budgetService.GetBudgetListResponsesForAdmin(dateFromInput,
                                                                                                        dateToInput,
                                                                                                        spendCategoryIdsAllowed);
            var budgetIds = budgetListResponses.Select(b => b.Id).Distinct().ToArray();

            var sps = _budgetService.GetSpends(budgetIds);
            budgetListResponses.ForEach(b =>
            {
                b.SpendResponses = sps.Where(s => s.BudgetId == b.Id).ToArray();
            });

            if (budgetListResponses == null || !budgetListResponses.Any())
            {
                logger.Warn($"Budgets not found");
                return NotFound();
            }

            return Ok(budgetListResponses);
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

        [HttpPost]
        [Route("api/spends/admin/budgets/{referenceId}/allocations")]
        public IHttpActionResult IncrementBudgetAllocationAmount(string referenceId, BudgetRequest budgetRequest)
        {
            if (string.IsNullOrEmpty(referenceId) || budgetRequest == null)
            {
                return BadRequest("Budget reference id or allocation is missing.");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            budgetRequest.UpdatedById = loggedInUser.Id;
            budgetRequest.UpdatedDate = DateTime.Now;

            var inserted = _budgetService.IncreaseBudgetAllocation(budgetRequest);

            return Ok(inserted);
        }

        [HttpPost]
        [Route("api/spends/admin/spends/creditnote")]
        public IHttpActionResult IssueCreditNote(SpendRequest spendRequest)
        {
            if (spendRequest == null || spendRequest.BudgetId <= 0)
            {
                return BadRequest("Budget id or spend is missing.");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            spendRequest.CreatedById = loggedInUser.Id;

            // Note: Same ftn as user does spend, except this req should all necessary values populated UI
            var inserted = _budgetService.InsertSpend(spendRequest);

            return Ok(inserted);
        }

        [HttpPost]
        [Route("api/spends/admin/spends/tranferspend")]
        public IHttpActionResult TransferSpend(TransferSpendRequest transferSpendRequest)
        {
            if (!GuidConverter.IsValid(transferSpendRequest.TransferToBudgetReferenceId.ToString()))
            {
                return BadRequest("From budget referenceid is missing.");
            }
            if (transferSpendRequest.TransferFromSpendId <= 0)
            {
                return BadRequest("To spend id is missing.");
            }

            // Ensure we are not tranfering within the SAME budget
            var budgetTo = _budgetService.GetBudgetListResponseByReferenceId(transferSpendRequest.TransferToBudgetReferenceId);
            var found = budgetTo.SpendResponses.Where(sp => sp.Id == transferSpendRequest.TransferFromSpendId).FirstOrDefault();
            if (found != null)
            {
                return BadRequest("Invalid request. You are trying to transer withing the same budget.");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend is transfered from spend id {transferSpendRequest.TransferFromSpendId} to budget {transferSpendRequest.TransferToBudgetReferenceId} by {loggedInUser.ForeName}");

            var inserted = _budgetService.TransferSpend(transferSpendRequest);

            return Ok(inserted);
        }

    }
}
