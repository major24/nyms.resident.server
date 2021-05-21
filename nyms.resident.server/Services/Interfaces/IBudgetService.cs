using nyms.resident.server.Model;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IBudgetService
    {
        // IEnumerable<BudgetResponse> GetBudgets();
        BudgetResponse GetBudget(Guid referenceId);
        BudgetResponse Insert(BudgetRequest budgetRequest);
        BudgetResponse Update(BudgetRequest budgetRequest);
        // IEnumerable<BudgetListResponse> GetBudgetListResponses();
        IEnumerable<BudgetListResponse> GetBudgetListResponsesForUser(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds);
        IEnumerable<BudgetListResponse> GetBudgetListResponsesForAdmin(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds);

        BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId);
        BudgetResponse IncreaseBudgetAllocation(BudgetRequest budgetRequest);
        // Spend Related
        SpendRequest InsertSpend(SpendRequest spendRequest);
        bool TransferSpend(TransferSpendRequest transferSpendRequest);
        IEnumerable<SpendResponse> GetSpends(int[] budgetIds);
    }
}
