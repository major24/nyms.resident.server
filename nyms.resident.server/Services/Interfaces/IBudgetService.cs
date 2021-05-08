using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IBudgetService
    {
        IEnumerable<BudgetResponse> GetBudgets();
        BudgetResponse GetBudget(Guid referenceId);
        BudgetResponse Insert(BudgetRequest budgetRequest);
        BudgetResponse Update(BudgetRequest budgetRequest);
        IEnumerable<BudgetListResponse> GetBudgetListResponses();
        BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId);

        // Spend Related
        SpendRequest CreateSpend(SpendRequest spendRequest);
    }
}
