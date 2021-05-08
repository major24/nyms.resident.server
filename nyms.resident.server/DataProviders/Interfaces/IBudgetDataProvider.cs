using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IBudgetDataProvider
    {
        IEnumerable<BudgetEntity> GetBudgets();
        BudgetEntity GetBudget(Guid referenceId);
        BudgetEntity Insert(BudgetEntity budgetEntity);
        BudgetEntity Update(BudgetEntity budgetEntity);
        IEnumerable<BudgetListResponse> GetBudgetListResponses();
        BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId);

        // Spend related
        SpendRequest CreateSpend(SpendRequest spendRequest);
    }
}
