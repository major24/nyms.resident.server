using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace nyms.resident.server.Models
{
    public class TransferSpendRequest
    {
        public int TransferFromSpendId { get; set; }
        public Guid TransferToBudgetReferenceId { get; set; }
        public string Notes { get; set; }
    }
}