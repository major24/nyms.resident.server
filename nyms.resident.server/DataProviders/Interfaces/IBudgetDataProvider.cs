using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IBudgetDataProvider
    {
        IEnumerable<BudgetListResponse> GetBudgetListResponses(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds);
        IEnumerable<BudgetListResponse> GetBudgetListResponsesApprovedAndOpened(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds);
        IEnumerable<Budget> GetBudgetsApprovedAndOpened(int[] spendCategoryIds);
        BudgetListResponse GetBudgetListResponse(Guid referenceId);
        BudgetEntity Insert(IEnumerable<BudgetEntity> budgetEntities);
        BudgetEntity Update(BudgetEntity budgetEntity);
        BudgetEntity IncreaseBudgetAllocation(BudgetEntity budgetEntity);
    }
}
