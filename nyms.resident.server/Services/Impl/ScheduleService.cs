using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleDataProvider _scheduleDataProvider;
        public ScheduleService(IScheduleDataProvider scheduleDataProvider)
        {
            _scheduleDataProvider = scheduleDataProvider ?? throw new ArgumentException(nameof(scheduleDataProvider));
        }

        public IEnumerable<ResidentSchedule> GetResidentSchedules()
        {
            // get entity result and parse to resident schedule
            var residentScheduleEntity = this._scheduleDataProvider.GetResidentSchedules();
            if (residentScheduleEntity == null || !residentScheduleEntity.Any()) return null;

            var resIdsUnique = residentScheduleEntity.Select(rs => rs.ResidentId).Distinct();
            // loop thru each uniq id and get all the schedules
            var result = resIdsUnique.Select(id =>
            {
                var resident = residentScheduleEntity.Where(r => r.ResidentId == id).FirstOrDefault();
                var schedules = residentScheduleEntity.Where(r => r.ResidentId == id).Select(s =>
                {
                    return new SchedulePayment() {
                        Id = s.ScheduleId,
                        PaymentFrom = s.PaymentFrom, 
                        PaymentTypeId = s.PaymentTypeId,
                        Description = s.Description,
                        ScheduleBeginDate = s.ScheduleBeginDate,
                        ScheduleEndDate = s.ScheduleEndDate,
                        WeeklyFee = s.WeeklyFee,
                        PaymentFromName = s.PaymentFromName,
                        LocalAuthorityId = s.LocalAuthorityId
                    };
                });
                return new ResidentSchedule()
                {
                    ReferenceId = resident.ReferenceId,
                    LocalAuthorityId = resident.LocalAuthorityId,
                    PaymentFromName = resident.PaymentFromName,
                    ForeName = resident.ForeName,
                    SurName = resident.SurName,
                    Schedules = schedules
                };
            });

            return result;
        }

        public ResidentSchedule GetResidentSchedules(Guid referenceId)
        {
            var residentScheduleEntity = this._scheduleDataProvider.GetResidentSchedules(referenceId);
            if (residentScheduleEntity == null || !residentScheduleEntity.Any()) return null;

            var schedules = residentScheduleEntity.Select(s =>
            {
                return new SchedulePayment()
                {
                    Id = s.ScheduleId,
                    PaymentFrom = s.PaymentFrom,
                    PaymentTypeId = s.PaymentTypeId,
                    Description = s.Description,
                    ScheduleBeginDate = s.ScheduleBeginDate,
                    ScheduleEndDate = s.ScheduleEndDate,
                    WeeklyFee = s.WeeklyFee,
                    PaymentFromName = s.PaymentFromName,
                    LocalAuthorityId = s.LocalAuthorityId
                };
            });

            var resident = residentScheduleEntity.FirstOrDefault();
            return new ResidentSchedule()
            {
                ReferenceId = resident.ReferenceId,
                ForeName = resident.ForeName,
                SurName = resident.SurName,
                Schedules = schedules
            };
        }

        public void CreateSchedule(SchedulePayment schedule)
        {
            this._scheduleDataProvider.CreateSchedule(schedule);
        }

        public void UpdateScheduleEndDate(int id, DateTime scheduleEndDate)
        {
            this._scheduleDataProvider.UpdateScheduleEndDate(id, scheduleEndDate);
        }

        public void InactivateSchedule(int id)
        {
            this._scheduleDataProvider.InactivateSchedule(id);
        }
    }
}