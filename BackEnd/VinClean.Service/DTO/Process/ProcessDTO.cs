using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

// Data holder

namespace VinClean.Service.DTO
{
    public class ProcessDTO
    {
        public int ProcessId { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool isDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public TimeSpan? StartWorking { get; set; }
        public TimeSpan? EndWorking { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? StarTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
