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
    public class SpendBudgetUserController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly ISpendBudgetService _spendBudgetService;
        private readonly ISecurityService _securityService;

        public SpendBudgetUserController(ISpendBudgetService spendBudgetService,
            ISecurityService securityService)
        {
            _spendBudgetService = spendBudgetService ?? throw new ArgumentNullException(nameof(spendBudgetService));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }


        [HttpGet]
        [Route("api/spends/user/budgets")]
        public IHttpActionResult GetBudgetsForUser()
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            IEnumerable<SpendBudgetListResponse> spendBudgetListResponses = _spendBudgetService.GetSpendBudgetListResponses();

            // Rule: Users, only get Approved and Open budgets for processing
            var filterApprovedAndOpenBudgetListResponses = spendBudgetListResponses.Where(b =>
            {
                return b.Approved == "Y" && b.Status == "Open";
            }).ToArray();

            if (filterApprovedAndOpenBudgetListResponses == null)
            {
                logger.Warn($"Budgets not found");
                return NotFound();
            }

            // Rule: Access Control based on role(s)
            var permissions = _securityService.GetRolePermissions(user.Id);
            if (IsAdmin(permissions, user.Id))
            {
                return Ok(filterApprovedAndOpenBudgetListResponses);
            }
            else
            {
                // Manager. Find by spend cate id and care home id
                var filteredBuds = FilterBudgets(permissions, filterApprovedAndOpenBudgetListResponses);
                return Ok(filteredBuds);
            }
        }

        [HttpGet]
        [Route("api/spends/user/budgets/{referenceId}/spends")]
        public IHttpActionResult GetBudgetAndSpendsByReferenceId(string referenceId)
        {
            var user = HttpContext.Current.User as SecurityPrincipal;
            var curUser = System.Threading.Thread.CurrentPrincipal;
            logger.Info($"Get budget requested by {user.ForeName}");

            var spendBudgetListResponses = _spendBudgetService.GetSpendBudgetListResponseByReferenceId(new Guid(referenceId));

            return Ok(spendBudgetListResponses);
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

            if (spendRequest.SpendBudgetId <= 0 || spendRequest.Amount <= 0)
            {
                return BadRequest("Spend budget id and or amount not found");
            }

            var loggedInUser = HttpContext.Current.User as SecurityPrincipal;
            logger.Info($"Spend Budget created by {loggedInUser.ForeName}");
            spendRequest.CreatedById = loggedInUser.Id;

            var result = _spendBudgetService.CreateSpend(spendRequest);
            return Ok(result);
        }


        private IEnumerable<SpendBudgetListResponse> FilterBudgets(IEnumerable<UserRolePermission> permissions, IEnumerable<SpendBudgetListResponse> spendBudgetListResponses)
        {
            if (permissions.Any())
            {
                List<SpendBudgetListResponse> permittedSpendBudgetListResponses = new List<SpendBudgetListResponse>();
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
                    var tmp = spendBudgetListResponses.Where(bud => bud.SpendCategoryId == d.Key);
                    // add to main list
                    tmp.ForEach(bud =>
                    {
                        permittedSpendBudgetListResponses.Add(bud);
                    });
                });

                // care home filter
                // get carehome id
                var chIds = permissions.Select(p => p.CareHomeId);
                List<SpendBudgetListResponse> permittedCareHomeSpendBudgetListResponses = new List<SpendBudgetListResponse>();
                chIds.ForEach(chId =>
                {
                    var tmp = permittedSpendBudgetListResponses.FindAll(bud => bud.CareHomeId == chId);
                    tmp.ForEach(bud =>
                    {
                        permittedCareHomeSpendBudgetListResponses.Add(bud);
                    });
                });

                return permittedCareHomeSpendBudgetListResponses;
            }

            return spendBudgetListResponses;
        }


        private bool IsAdmin(IEnumerable<UserRolePermission> permissions, int userId)
        {
            if (permissions.Any())
            {
                foreach(var p in permissions)
                {
                    if (p.RoleId == 2 || p.RoleId == 4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



    }
}
