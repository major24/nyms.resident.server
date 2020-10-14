using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IScheduleDataProvider
    {
        IEnumerable<ResidentScheduleEntity> GetResidentSchedules();
        IEnumerable<ResidentScheduleEntity> GetResidentSchedules(Guid referenceId);
        void UpdateScheduleEndDate(int id, DateTime scheduleEndDate);
        void CreateSchedule(SchedulePayment schedule);
        void InactivateSchedule(int id);
    }
}
