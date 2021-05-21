using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Model;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
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

/*        public IEnumerable<BudgetResponse> GetBudgets()
        {
            var budgetEntities = _spendBudgetDataProvider.GetBudgets();
            IEnumerable<BudgetResponse> spendBudgets = budgetEntities.Select(e =>
            {
                return ToModel(e);
            });

            return spendBudgets.ToArray();
        }*/

        public BudgetResponse GetBudget(Guid referenceId)
        {
            var budgetEntity = _spendBudgetDataProvider.GetBudget(referenceId);
            return ToModel(budgetEntity);
        }

        public BudgetResponse Insert(BudgetRequest budgetRequest)
        {
            // For Inserts, request comes with ONE allocation entity. User can save ONLY ONE AMOUNT. But entity is handled as multiple (1-many)
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
            //var spendBudgetEnityInserted = _spendBudgetDataProvider.Insert(ToEntity(budgetRequest));
            //return ToModel(spendBudgetEnityInserted);

            // Recurring budgets. Month starts at 1. 1=Jan. If less than Zero, than no recurring budgets. 
            BudgetEntity inserted = null;
            if (budgetRequest.Recurrence == null || budgetRequest.Recurrence.StartMonth <= 0)
            {
                // Standard insert. ONE budget entity
                List<BudgetEntity> budgetEntities = new List<BudgetEntity>()
                {
                    ToEntity(budgetRequest)
                };
                inserted = _spendBudgetDataProvider.Insert(budgetEntities.ToArray());
            }
            else
            {
                var budgetEnitites = CreateRecurringBudgets(budgetRequest);
                inserted = _spendBudgetDataProvider.Insert(budgetEnitites);
            }

            return ToModel(inserted);
        }

        public BudgetResponse Update(BudgetRequest budgetRequest)
        {
            // If allocation is, Approved already, do not change the amount
            // If budget Compleated do not change any fields
            // Else only update, dateFrom, dateTo, desc, poPrefix, status
            var existingBudget = GetBudget(budgetRequest.ReferenceId);
            if (existingBudget.Status == Constants.BUDGET_STATUS_COMPLETED)
            {
                return null; // Todo: throw valid bus exception to be bubble upto UI
            }

            var unApprovedIds = existingBudget.BudgetAllocations.Where(a => a.Approved != "Y").Select(a => a.Id);
            List<BudgetAllocation> unApproved = new List<BudgetAllocation>();
            unApprovedIds.ForEach(id =>
            {
                unApproved.Add(budgetRequest.BudgetAllocations.Where(a => a.Id == id).FirstOrDefault());
            });

            // With unApproved list, when a user is approving, add date and user id
            unApproved.ForEach(a =>
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
            
            budgetRequest.BudgetAllocations = unApproved;

            var spendBudgetEntityUpdated =_spendBudgetDataProvider.Update(ToEntity(budgetRequest));
            return ToModel(spendBudgetEntityUpdated);
        }

        // public IEnumerable<BudgetListResponse> GetBudgetListResponses()
        // IEnumerable<BudgetListResponse> GetBudgetListResponses(int[] spendCategoryIds, string status = "Open")
        public IEnumerable<BudgetListResponse> GetBudgetListResponsesForUser(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds)
        {
            return _spendBudgetDataProvider.GetBudgetListResponsesForUser(dateFrom, dateTo, spendCategoryIds);
        }

        public IEnumerable<BudgetListResponse> GetBudgetListResponsesForAdmin(DateTime dateFrom, DateTime dateTo, int[] spendCategoryIds)
        {
            return _spendBudgetDataProvider.GetBudgetListResponsesForAdmin(dateFrom, dateTo, spendCategoryIds);
        }

        public BudgetListResponse GetBudgetListResponseByReferenceId(Guid referenceId)
        {
            return _spendBudgetDataProvider.GetBudgetListResponseByReferenceId(referenceId);
        }

        public BudgetResponse IncreaseBudgetAllocation(BudgetRequest budgetRequest)
        {
            BudgetAllocation allocation = budgetRequest.BudgetAllocations.FirstOrDefault();
            if (allocation.Approved == Constants.BUDGET_APPROVED)
            {
                allocation.ApprovedById = budgetRequest.UpdatedById;
                allocation.ApprovedDate = DateTime.Now;
            }
            allocation.UpdatedById = budgetRequest.UpdatedById;
            allocation.UpdatedDate = DateTime.Now;

            var updated = _spendBudgetDataProvider.IncreaseBudgetAllocation(ToEntity(budgetRequest));
            return ToModel(updated);
        }


        // Spend Related (expenses)
        public SpendRequest InsertSpend(SpendRequest spendRequest)
        {
            return _spendBudgetDataProvider.InsertSpend(spendRequest);
        }

        public bool TransferSpend(TransferSpendRequest transferSpendRequest)
        {
            return _spendBudgetDataProvider.TransferSpend(transferSpendRequest);
        }

        public IEnumerable<SpendResponse> GetSpends(int[] budgetIds)
        {
            return _spendBudgetDataProvider.GetSpends(budgetIds);
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
                Reason = budgetRequest.Reason,
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
                Reason = budgetEntity.Reason,
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
                UpdatedById = budgetAllocationEntity.UpdatedById,
                UpdatedDate = budgetAllocationEntity.UpdatedDate
            };
        }


        // Create recurring budgets based on start month and number of months
        private IEnumerable<DatePair> GenerateMonthStartEndDates(int startMonth, int numOfMonths)
        {
            List<DatePair> dates = new List<DatePair>();
            var year = DateTime.Now.Year;
            for (int i = 0; i < numOfMonths; i++)
            {
                var date1 = new DateTime(year, startMonth, 1);
                var lastDayOfMth = DateTime.DaysInMonth(year, startMonth);
                var date2 = new DateTime(year, startMonth, lastDayOfMth);
                DatePair datePair = new DatePair() { StartDate = date1, EndDate = date2 };
                dates.Add(datePair);

                startMonth++;
                if (startMonth == 13)
                {
                    year++;
                    startMonth = 1;
                }
            }
            return dates.ToArray();
        }

        private IEnumerable<BudgetEntity> CreateRecurringBudgets(BudgetRequest budgetRequest)
        {
            var startMonth = budgetRequest.Recurrence.StartMonth;
            var numOfMonths = budgetRequest.Recurrence.NumberOfMonths;

            var listOfDates = GenerateMonthStartEndDates(startMonth, numOfMonths).ToArray();

            // create n number of reqs entities
            List<BudgetEntity> list = new List<BudgetEntity>();
            for (int i = 0; i < numOfMonths; i++)
            {
                var temp = ToEntity(budgetRequest);
                temp.DateFrom = listOfDates[i].StartDate;
                temp.DateTo = listOfDates[i].EndDate;
                list.Add(temp);
            }
            return list.ToArray();
        }

    }
}