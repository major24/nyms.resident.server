using NLog;
using nyms.resident.server.Filters;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Authentication;
using nyms.resident.server.Services.Core;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Web;
using System.Web.Http;

namespace nyms.resident.server.Controllers.Spends
{
    [UserAuthenticationFilter]
    public class SpendController : ApiController
    {
        private static Logger logger = Nlogger2.GetLogger();
        private readonly IBudgetService _budgetService;
        private readonly ISecurityService _securityService;

        public SpendController(IBudgetService budgetService,
            ISecurityService securityService)
        {
            _budgetService = budgetService ?? throw new ArgumentNullException(nameof(budgetService));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        // Spends/Expenses recored by Users and Admin
        [HttpPost]
        [Route("api/spends/user/spends")]
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
            logger.Info($"Spend created by {loggedInUser.ForeName}");
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

    }
}
