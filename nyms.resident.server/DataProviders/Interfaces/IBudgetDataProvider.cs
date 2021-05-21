using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IBudgetDataProvider
    {
        // IEnumerable<BudgetEntity> GetBudgets();
        IEnumerable<BudgetListResponse> GetBudgetListResponsesForUser(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds);
        IEnumerable<BudgetListResponse> GetBudgetListResponsesForAdmin(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds);
        BudgetEntity GetBudget(Guid referenceId);
        // BudgetEntity Insert(BudgetEntity budgetEntity);
        BudgetEntity Insert(IEnumerable<BudgetEntity> budgetEntities);
        BudgetEntity Update(BudgetEntity budgetEntity);
        // IEnumerable<BudgetListResponse> GetBudgetListResponses();

        BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId);
        BudgetEntity IncreaseBudgetAllocation(BudgetEntity budgetEntity);
        // Spend related
        SpendRequest InsertSpend(SpendRequest spendRequest);
        bool TransferSpend(TransferSpendRequest transferSpendRequest);
        IEnumerable<SpendResponse> GetSpends(int[] budgetIds);
    }
}
