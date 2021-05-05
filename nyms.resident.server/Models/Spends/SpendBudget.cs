using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class SpendBudget
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int SpendCategoryId { get; set; }
        public int CareHomeId { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public string PoPrefix { get; set; }
        public string Status { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}