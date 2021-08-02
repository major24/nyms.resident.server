using System;

namespace nyms.resident.server.Models
{
    public class TransferSpendRequest
    {
        public int TransferFromSpendId { get; set; }
        public Guid TransferToBudgetReferenceId { get; set; }
        public string Comments { get; set; }
    }
}