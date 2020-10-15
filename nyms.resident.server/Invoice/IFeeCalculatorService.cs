using System;

namespace nyms.resident.server.Invoice
{
    public interface IFeeCalculatorService
    {
        InvoiceResident CalculateFee(InvoiceResident resident, DateTime reportBeginDate, DateTime reportEndDate);
        int GetNumberOfDaysInMonth(DateTime beginDate, DateTime endDate);
    }
}
