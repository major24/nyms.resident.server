using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nyms.resident.server.Services.Impl
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetDataProvider _spendBudgetDataProvider;

        public BudgetService(IBudgetDataProvider spendBudgetDataProvider)
        {
            _spendBudgetDataProvider = spendBudgetDataProvider ?? throw new ArgumentNullException(nameof(spendBudgetDataProvider));
        }

        public IEnumerable<BudgetResponse> GetBudgets()
        {
            var budgetEntities = _spendBudgetDataProvider.GetBudgets();
            IEnumerable<BudgetResponse> spendBudgets = budgetEntities.Select(e =>
            {
                return ToModel(e);
            });

            return spendBudgets.ToArray();
        }

        public BudgetResponse GetBudget(Guid referenceId)
        {
            var budgetEntity = _spendBudgetDataProvider.GetBudget(referenceId);
            return ToModel(budgetEntity);
        }

        public BudgetResponse Insert(BudgetRequest budgetRequest)
        {
            // Request comes with ONE allocation entity. User can save ONLY ONE AMOUNT. But entity is handled as multiple (1-many)
            if (budgetRequest.BudgetAllocations.FirstOrDefault().Approved == Constants.BUDGET_APPROVED)
            {
                budgetRequest.BudgetAllocations.FirstOrDefault().ApprovedById = budgetRequest.CreatedById;
                budgetRequest.BudgetAllocations.FirstOrDefault().ApprovedDate = DateTime.Now;
            }
            // Ensure all allocations has updated id and date..
            budgetRequest.BudgetAllocations.ForEach(a =>
            {
                a.UpdatedById = budgetRequest.CreatedById;
            });
            var spendBudgetEnityInserted = _spendBudgetDataProvider.Insert(ToEntity(budgetRequest));
            return ToModel(spendBudgetEnityInserted);
        }

        public BudgetResponse Update(BudgetRequest budgetRequest)
        {
            // If allocation is, Approved already, do not change the amount
            // If budget Compleated do not change any fields
            // Else only update, dateFrom, dateTo, desc, poPrefix, status
            var existingBudget = GetBudget(budgetRequest.ReferenceId);
            if (existingBudget.Status == Constants.BUDGET_STATUS_COMPLETED) // existingBudget.Status == "Cancelled"
            {
                return null; // Todo: throw valid bus exception to be bubble upto UI
            }

            var unApprovedIds = existingBudget.BudgetAllocations.Where(a => a.Approved != "Y").Select(a => a.Id);
            List<BudgetAllocation> unApprovedOrNewAmountList = new List<BudgetAllocation>();
            unApprovedIds.ForEach(id =>
            {
                unApprovedOrNewAmountList.Add(budgetRequest.BudgetAllocations.Where(a => a.Id == id).FirstOrDefault());
            });

            // With unApproved list, when a user is approving, add date and user id
            unApprovedOrNewAmountList.ForEach(a =>
            {
                if (a.Approved == Constants.BUDGET_APPROVED)
                {
                    a.ApprovedById = budgetRequest.UpdatedById;
                    a.ApprovedDate = DateTime.Now;
                }
                // Ensure updated date and updated by id is filled in
                a.UpdatedById = budgetRequest.UpdatedById;
                a.UpdatedDate = DateTime.Now;
            });

            // find any new amount has been sent?
            budgetRequest.BudgetAllocations.ForEach(a =>
            {
                if (a.Id == 0)
                {
                    // Ensure updated date and updated by id is filled in for new amounts
                    a.UpdatedById = budgetRequest.UpdatedById;
                    a.UpdatedDate = DateTime.Now;
                    unApprovedOrNewAmountList.Add(a);
                }
            });
            
            budgetRequest.BudgetAllocations = unApprovedOrNewAmountList;

            var spendBudgetEntityUpdated =_spendBudgetDataProvider.Update(ToEntity(budgetRequest));
            return ToModel(spendBudgetEntityUpdated);
        }

        public IEnumerable<BudgetListResponse> GetBudgetListResponses()
        {
            return _spendBudgetDataProvider.GetBudgetListResponses();
        }

        public BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId)
        {
            return _spendBudgetDataProvider.GetBudgetListResponseByReferenceId(referenceId);
        }


        // Spend Related (expenses)
        public SpendRequest CreateSpend(SpendRequest spendRequest)
        {
            return _spendBudgetDataProvider.CreateSpend(spendRequest);
        }





        private BudgetEntity ToEntity(BudgetRequest budgetRequest)
        {
            IEnumerable<BudgetAllocationEntity> spendBudgetAllocationEntities = budgetRequest.BudgetAllocations.Select(a =>
            {
                return ToEntity(a);
            }).ToArray();

            return new BudgetEntity()
            {
                Id = budgetRequest.Id,
                ReferenceId = budgetRequest.ReferenceId,
                SpendCategoryId = budgetRequest.SpendCategoryId,
                CareHomeId = budgetRequest.CareHomeId,
                Name = budgetRequest.Name,
                DateFrom = budgetRequest.DateFrom,
                DateTo = budgetRequest.DateTo,
                Description = budgetRequest.Description,
                PoPrefix = budgetRequest.PoPrefix,
                Status = budgetRequest.Status,
                CreatedById = budgetRequest.CreatedById,
                UpdatedById = budgetRequest.UpdatedById,
                UpdatedDate = budgetRequest.UpdatedDate,
                BudgetAllocations = spendBudgetAllocationEntities
            };
        }

        private BudgetAllocationEntity ToEntity(BudgetAllocation budgetAllocation)
        {
            return new BudgetAllocationEntity()
            {
                Id = budgetAllocation.Id,
                BudgetId = budgetAllocation.BudgetId,
                Amount = budgetAllocation.Amount,
                Approved = budgetAllocation.Approved,
                ApprovedById = budgetAllocation.ApprovedById,
                ApprovedDate = budgetAllocation.ApprovedDate,
                Reason = budgetAllocation.Reason,
                UpdatedById = budgetAllocation.UpdatedById,
                UpdatedDate = DateTime.Now
            };
        }

        private BudgetResponse ToModel(BudgetEntity budgetEntity)
        {
            IEnumerable<BudgetAllocation> budgetAllocations = budgetEntity.BudgetAllocations.Select(a =>
            {
                return ToModel(a);
            }).ToArray();

            return new BudgetResponse()
            {
                Id = budgetEntity.Id,
                ReferenceId = budgetEntity.ReferenceId,
                SpendCategoryId = budgetEntity.SpendCategoryId,
                CareHomeId = budgetEntity.CareHomeId,
                Name = budgetEntity.Name,
                DateFrom = budgetEntity.DateFrom,
                DateTo = budgetEntity.DateTo,
                Description = budgetEntity.Description,
                PoPrefix = budgetEntity.PoPrefix,
                Status = budgetEntity.Status,
                CreatedById = budgetEntity.CreatedById,
                UpdatedById = budgetEntity.UpdatedById,
                BudgetAllocations = budgetAllocations
            };
        }

        private BudgetAllocation ToModel(BudgetAllocationEntity budgetAllocationEntity)
        {
            return new BudgetAllocation()
            {
                Id = budgetAllocationEntity.Id,
                BudgetId = budgetAllocationEntity.BudgetId,
                Amount = budgetAllocationEntity.Amount,
                Approved = budgetAllocationEntity.Approved,
                ApprovedById = budgetAllocationEntity.ApprovedById,
                ApprovedDate = budgetAllocationEntity.ApprovedDate,
                Reason = budgetAllocationEntity.Reason,
                UpdatedById = budgetAllocationEntity.UpdatedById,
                UpdatedDate = budgetAllocationEntity.UpdatedDate
            };
        }
    }
}