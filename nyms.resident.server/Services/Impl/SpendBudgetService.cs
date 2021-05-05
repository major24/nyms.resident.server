using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nyms.resident.server.Services.Impl
{
    public class SpendBudgetService : ISpendBudgetService
    {
        private readonly ISpendBudgetDataProvider _spendBudgetDataProvider;

        public SpendBudgetService(ISpendBudgetDataProvider spendBudgetDataProvider)
        {
            _spendBudgetDataProvider = spendBudgetDataProvider ?? throw new ArgumentNullException(nameof(spendBudgetDataProvider));
        }

        public IEnumerable<SpendBudgetResponse> GetSpendBudgets()
        {
            var budgetEntities = _spendBudgetDataProvider.GetSpendBudgets();
            IEnumerable<SpendBudgetResponse> spendBudgets = budgetEntities.Select(e =>
            {
                return ToModel(e);
            });

            return spendBudgets.ToArray();
        }

        public SpendBudgetResponse GetSpendBudget(Guid referenceId)
        {
            var budgetEntity = _spendBudgetDataProvider.GetSpendBudget(referenceId);
            return ToModel(budgetEntity);
        }

        public SpendBudgetResponse Insert(SpendBudgetRequest spendBudgetRequest)
        {
            // Request comes with ONE allocation entity. User can save ONLY ONE AMOUNT. But entity is handled as multiple (1-many)
            if (spendBudgetRequest.SpendBudgetAllocations.FirstOrDefault().Approved == "Y")
            {
                spendBudgetRequest.SpendBudgetAllocations.FirstOrDefault().ApprovedById = spendBudgetRequest.CreatedById;
                spendBudgetRequest.SpendBudgetAllocations.FirstOrDefault().ApprovedDate = DateTime.Now;
            }
            // Ensure all allocations has updated id and date..
            spendBudgetRequest.SpendBudgetAllocations.ForEach(a =>
            {
                a.UpdatedById = spendBudgetRequest.CreatedById;
            });
            var spendBudgetEnityInserted = _spendBudgetDataProvider.Insert(ToEntity(spendBudgetRequest));
            return ToModel(spendBudgetEnityInserted);
        }

        public SpendBudgetResponse Update(SpendBudgetRequest spendBudgetRequest)
        {
            // If allocation is, Approved already, do not change the amount
            // If budget Compleated || Cancelled, do not change any fields
            // Else only update, dateFrom, dateTo, desc, poPrefix, status
            var existingBudget = GetSpendBudget(spendBudgetRequest.ReferenceId);
            if (existingBudget.Status == "Cancelled" || existingBudget.Status == "Completed")
            {
                return null; // Todo: throw valid bus exception to be bubble upto UI
            }

            var unApprovedIds = existingBudget.SpendBudgetAllocations.Where(a => a.Approved != "Y").Select(a => a.Id);
            List<SpendBudgetAllocation> unApprovedList = new List<SpendBudgetAllocation>();
            unApprovedIds.ForEach(id =>
            {
                unApprovedList.Add(spendBudgetRequest.SpendBudgetAllocations.Where(a => a.Id == id).FirstOrDefault());
            });

            // With unApproved list, when a user is approving, add date and user id
            unApprovedList.ForEach(a =>
            {
                if (a.Approved == "Y")
                {
                    a.ApprovedById = spendBudgetRequest.UpdatedById;
                    a.ApprovedDate = DateTime.Now;
                }
                // Ensure updated date and updated by id is filled in
                a.UpdatedById = spendBudgetRequest.UpdatedById;
                a.UpdatedDate = DateTime.Now;
            });
            
            spendBudgetRequest.SpendBudgetAllocations = unApprovedList;

            var spendBudgetEntityUpdated =_spendBudgetDataProvider.Update(ToEntity(spendBudgetRequest));
            return ToModel(spendBudgetEntityUpdated);
        }

        public IEnumerable<SpendBudgetListResponse> GetSpendBudgetListResponses()
        {
            return _spendBudgetDataProvider.GetSpendBudgetListResponses();
        }

        public SpendBudgetListResponse GetSpendBudgetListResponseByReferenceId(Guid referenceId)
        {
            return _spendBudgetDataProvider.GetSpendBudgetListResponseByReferenceId(referenceId);
        }


        // Spend Related (expenses)
        public SpendRequest CreateSpend(SpendRequest spendRequest)
        {
            return _spendBudgetDataProvider.CreateSpend(spendRequest);
        }





        private SpendBudgetEntity ToEntity(SpendBudgetRequest spendBudgetRequest)
        {
            IEnumerable<SpendBudgetAllocationEntity> spendBudgetAllocationEntities = spendBudgetRequest.SpendBudgetAllocations.Select(a =>
            {
                return ToEntity(a);
            }).ToArray();
            /*var spendBudgetAllocationEntities = new List<SpendBudgetAllocationEntity>()
            {
                ToEntity
            }.ToArray();
*/
            return new SpendBudgetEntity()
            {
                Id = spendBudgetRequest.Id,
                ReferenceId = spendBudgetRequest.ReferenceId,
                SpendCategoryId = spendBudgetRequest.SpendCategoryId,
                CareHomeId = spendBudgetRequest.CareHomeId,
                Name = spendBudgetRequest.Name,
                DateFrom = spendBudgetRequest.DateFrom,
                DateTo = spendBudgetRequest.DateTo,
                Description = spendBudgetRequest.Description,
                PoPrefix = spendBudgetRequest.PoPrefix,
                Status = spendBudgetRequest.Status,
                CreatedById = spendBudgetRequest.CreatedById,
                UpdatedById = spendBudgetRequest.UpdatedById,
                UpdatedDate = spendBudgetRequest.UpdatedDate,
                SpendBudgetAllocations = spendBudgetAllocationEntities
            };
        }

        private SpendBudgetAllocationEntity ToEntity(SpendBudgetAllocation spendBudgetAllocation)
        {
            return new SpendBudgetAllocationEntity()
            {
                Id = spendBudgetAllocation.Id,
                SpendBudgetId = spendBudgetAllocation.SpendBudgetId,
                Amount = spendBudgetAllocation.Amount,
                Approved = spendBudgetAllocation.Approved,
                ApprovedById = spendBudgetAllocation.ApprovedById,
                ApprovedDate = spendBudgetAllocation.ApprovedDate,
                Reason = spendBudgetAllocation.Reason,
                UpdatedById = spendBudgetAllocation.UpdatedById,
                UpdatedDate = DateTime.Now
            };
        }

        private SpendBudgetResponse ToModel(SpendBudgetEntity spendBudgetEntity)
        {
            IEnumerable<SpendBudgetAllocation> spendBudgetAllocations = spendBudgetEntity.SpendBudgetAllocations.Select(a =>
            {
                return ToModel(a);
            }).ToArray();

            return new SpendBudgetResponse()
            {
                Id = spendBudgetEntity.Id,
                ReferenceId = spendBudgetEntity.ReferenceId,
                SpendCategoryId = spendBudgetEntity.SpendCategoryId,
                CareHomeId = spendBudgetEntity.CareHomeId,
                Name = spendBudgetEntity.Name,
                DateFrom = spendBudgetEntity.DateFrom,
                DateTo = spendBudgetEntity.DateTo,
                Description = spendBudgetEntity.Description,
                PoPrefix = spendBudgetEntity.PoPrefix,
                Status = spendBudgetEntity.Status,
                CreatedById = spendBudgetEntity.CreatedById,
                UpdatedById = spendBudgetEntity.UpdatedById,
                SpendBudgetAllocations = spendBudgetAllocations
            };
        }

        private SpendBudgetAllocation ToModel(SpendBudgetAllocationEntity spendBudgetAllocationEntity)
        {
            return new SpendBudgetAllocation()
            {
                Id = spendBudgetAllocationEntity.Id,
                SpendBudgetId = spendBudgetAllocationEntity.SpendBudgetId,
                Amount = spendBudgetAllocationEntity.Amount,
                Approved = spendBudgetAllocationEntity.Approved,
                ApprovedById = spendBudgetAllocationEntity.ApprovedById,
                ApprovedDate = spendBudgetAllocationEntity.ApprovedDate,
                Reason = spendBudgetAllocationEntity.Reason,
                UpdatedById = spendBudgetAllocationEntity.UpdatedById,
                UpdatedDate = spendBudgetAllocationEntity.UpdatedDate
            };
        }
    }
}