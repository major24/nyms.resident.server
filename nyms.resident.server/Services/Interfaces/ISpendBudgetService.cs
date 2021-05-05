using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface ISpendBudgetService
    {
        IEnumerable<SpendBudgetResponse> GetSpendBudgets();
        SpendBudgetResponse GetSpendBudget(Guid referenceId);
        SpendBudgetResponse Insert(SpendBudgetRequest spendBudgetRequest);
        SpendBudgetResponse Update(SpendBudgetRequest spendBudgetRequest);
        IEnumerable<SpendBudgetListResponse> GetSpendBudgetListResponses();
        SpendBudgetListResponse GetSpendBudgetListResponseByReferenceId(Guid referenceId);

        // Spend Related
        SpendRequest CreateSpend(SpendRequest spendRequest);
    }
}
