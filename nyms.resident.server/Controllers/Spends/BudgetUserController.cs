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
using System.Net;
using System.Net.Http;
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
        [Route("api/spends/user/budgets")]
        public IHttpActionResult GetBudgetsForUser()
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            IEnumerable<BudgetListResponse> budgetListResponses = _budgetService.GetBudgetListResponses();

            // Rule1: Users, only get Approved and Open budgets for processing
            var filterApprovedAndOpenBudgetListResponses = budgetListResponses.Where(b =>
            {
                return b.Approved == Constants.BUDGET_APPROVED && b.Status == Constants.BUDGET_STATUS_OPEN;
            }).ToArray();

            if (filterApprovedAndOpenBudgetListResponses == null)
            {
                logger.Warn($"Budgets not found");
                return NotFound();
            }

            var groupedByBudId = filterApprovedAndOpenBudgetListResponses.GroupBy(b => b.Id);
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

            // Rule2: Access Control based on role(s)
            var permissions = _securityService.GetRolePermissions(user.Id);
            if (IsAdmin(permissions))
            {
                return Ok(groupedResponse);
            }
            else
            {
                // Manager. Find by spend cate id and care home id
                var filteredBuds = FilterBudgets(permissions, groupedResponse);
                return Ok(filteredBuds);
            }
        }

        [HttpGet]
        [Route("api/spends/user/budgets/{referenceId}/spends")]
        public IHttpActionResult GetBudgetAndSpendsByReferenceId(string referenceId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            var budgetListResponses = _budgetService.GetBudgetListResponseByReferenceId(new Guid(referenceId));

            return Ok(budgetListResponses);
        }


        // Spends/Expenses recored by Users and Admin
        // spends/users/spends
        [HttpPost]
        [Route("api/spends/user/spends")]
        public IHttpActionResult CreateSpend(SpendRequest spendRequest)
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

            var result = _budgetService.CreateSpend(spendRequest);
            return Ok(result);
        }


        private IEnumerable<BudgetListResponse> FilterBudgets(IEnumerable<UserRolePermission> permissions, IEnumerable<BudgetListResponse> budgetListResponses)
        {
            if (permissions.Any())
            {
                List<BudgetListResponse> permittedBudgetListResponses = new List<BudgetListResponse>();
                // Make distinct spend_category_id
                Dictionary<int, int> dicSpendCategoryIds = new Dictionary<int, int>();
                permissions.ForEach(p =>
                {
                    if (!dicSpendCategoryIds.ContainsKey(p.SpendCategoryId))
                    {
                        dicSpendCategoryIds.Add(p.SpendCategoryId, p.SpendCategoryId);
                    }
                });
                dicSpendCategoryIds.ForEach(d => {
                    var tmp = budgetListResponses.Where(bud => bud.SpendCategoryId == d.Key);
                    // add to main list
                    tmp.ForEach(bud =>
                    {
                        permittedBudgetListResponses.Add(bud);
                    });
                });

                // care home filter
                // get carehome id
                var chIds = permissions.Select(p => p.CareHomeId).Distinct<int>();
                List<BudgetListResponse> permittedCareHomeBudgetListResponses = new List<BudgetListResponse>();
                chIds.ForEach(chId =>
                {
                    var tmp = permittedBudgetListResponses.FindAll(bud => bud.CareHomeId == chId);
                    tmp.ForEach(bud =>
                    {
                        permittedCareHomeBudgetListResponses.Add(bud);
                    });
                });

                return permittedCareHomeBudgetListResponses;
            }

            return budgetListResponses;
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
