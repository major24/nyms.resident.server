﻿using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IInvoiceDataProvider
    {
        IEnumerable<Schedule> GetAllSchedulesForInvoiceDate(DateTime billingStart, DateTime billingEnd);
    }
}