using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IScheduleService
    {
        IEnumerable<ResidentSchedule> GetResidentSchedules();
        ResidentSchedule GetResidentSchedules(Guid referenceId);
        void UpdateScheduleEndDate(int id, DateTime scheduleEndDate);
        void CreateSchedule(ScheduleEntity schedule);
        void UpdateSchedule(ScheduleEntity schedule);
        void InactivateSchedule(int id);
        IEnumerable<PaymentProvider> GetPaymentProviders();
        IEnumerable<PaymentType> GetPaymentTypes();
    }
}