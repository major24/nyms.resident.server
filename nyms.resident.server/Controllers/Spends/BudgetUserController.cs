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
    [UserAuthenticationFilter]
    public class BudgetUserController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IBudgetService _budgetService;
        private readonly ISecurityService _securityService;

        public BudgetUserController(IBudgetService budgetService,
            ISecurityService securityService)
        {
            _budgetService = budgetService ?? throw new ArgumentNullException(nameof(budgetService));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        [HttpGet]
        [Route("api/user/budgets/{dateFrom}/{dateTo}")]
        public IHttpActionResult GetBudgetsForUser(string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(dateFrom)) throw new ArgumentNullException(nameof(dateFrom));
            if (string.IsNullOrEmpty(dateTo)) throw new ArgumentNullException(nameof(dateTo));

            DateTime.TryParse(dateFrom, out DateTime dataFromInput);
            DateTime.TryParse(dateTo, out DateTime dateToInput);

            if (dataFromInput == null || dateToInput == null) throw new ArgumentException("Invalid dates");

            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            // For user find allowed spend category ids
            var spendCategoryIdsAllowed = _securityService.GetSpendCategoryRoleIds(user.Id).ToArray();

            // Rule1: Select by allowed spend category ids only
            IEnumerable<BudgetListResponse> budgetListResponses = _budgetService.GetBudgetListResponsesApprovedAndOpened(dataFromInput,
                                                                                                        dateToInput,
                                                                                                        spendCategoryIdsAllowed);

            // Rule2: Access Control based on care home role(s)
            var permissions = _securityService.GetRolePermissions(user.Id);
            if (IsAdmin(permissions))
            {
                return Ok(budgetListResponses);
            }
            else
            {
                // Manager. Find by spend cate id and care home id
                var temp = budgetListResponses.Where(r => r.CareHomeId == permissions.FirstOrDefault().CareHomeId);
                return Ok(temp);
            }
        }

        [HttpGet]
        [Route("api/user/budgetnames/{budgetType}")]
        public IHttpActionResult GetBudgetNamesForUser(BudgetType budgetType = BudgetType.Project)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget names requested by {user.ForeName}");

            // For user find allowed spend category ids
            var spendCategoryIdsAllowed = _securityService.GetSpendCategoryRoleIds(user.Id).ToArray();

            // Rule1: Select by allowed spend category ids only
            IEnumerable<Budget> budgets = _budgetService.GetBudgetsApprovedAndOpened(spendCategoryIdsAllowed);

            var budgetFilterByType = budgets.Where(b => b.BudgetType == budgetType);

            // Rule2: Access Control based on care home role(s)
            var permissions = _securityService.GetRolePermissions(user.Id);
            if (IsAdmin(permissions))
            {
                return Ok(budgetFilterByType);
            }
            else
            {
                // Manager. Find by spend cate id and care home id
                var temp = budgetFilterByType.Where(r => r.CareHomeId == permissions.FirstOrDefault().CareHomeId);
                return Ok(temp);
            }
        }

        [HttpGet]
        [Route("api/user/budgets/{referenceId}")]
        public IHttpActionResult GetBudgetAndSpendsByReferenceId(string referenceId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            var budgetListResponses = _budgetService.GetBudgetListResponse(new Guid(referenceId));

            return Ok(budgetListResponses);
        }


        // Spends/Expenses recored by Users and Admin
        [HttpPost]
        [Route("api/user/spends")]
        public IHttpActionResult InsertSpend(SpendRequest spendRequest)
        {
            if (spendRequest == null)
            {
                return BadRequest("spend request not found");
            }

            if (spendRequest.BudgetId <= 0 || spendRequest.Amount <= 0)
            {
                return BadRequest("Spend budget id and or amount not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            spendRequest.CreatedById = loggedInUser.Id;

            // Most of the spends are money spend, so [Debit] 
            // Admin may credit all or some amount, will be [Credit]. 
            // UI should the flag for Credit.
            if (string.IsNullOrEmpty(spendRequest.TranType))
            {
                spendRequest.TranType = Constants.SPEND_TRAN_TYPE_DEBIT;
            }
            
            var result = _budgetService.InsertSpend(spendRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("api/user/spends/comments")]
        public IHttpActionResult InsertSpendComment(SpendComments spendComments)
        {
            if (spendComments == null)
            {
                return BadRequest("spend comments not found");
            }

            if (spendComments.SpendId <= 0 || string.IsNullOrEmpty(spendComments.Comments))
            {
                return BadRequest("Spend id and or comments not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            spendComments.CreatedById = loggedInUser.Id;

            _budgetService.InsertSpendComment(spendComments);
            return Ok(spendComments);
        }

        private bool IsAdmin(IEnumerable<UserRolePermission> permissions)
        {
            if (permissions.Any())
            {
                foreach(var p in permissions)
                {
                    if (p.RoleId == (int)USER_ROLE.Admin || p.RoleId == (int)USER_ROLE.FinanceAdmin)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
