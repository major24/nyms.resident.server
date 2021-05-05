using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ISpendBudgetDataProvider
    {
        IEnumerable<SpendBudgetEntity> GetSpendBudgets();
        SpendBudgetEntity GetSpendBudget(Guid referenceId);
        SpendBudgetEntity Insert(SpendBudgetEntity spendBudgetEntity);
        SpendBudgetEntity Update(SpendBudgetEntity spendBudgetEntity);
        IEnumerable<SpendBudgetListResponse> GetSpendBudgetListResponses();
        SpendBudgetListResponse GetSpendBudgetListResponseByReferenceId(Guid referenceId);

        // Spend related
        SpendRequest CreateSpend(SpendRequest spendRequest);
    }
}
