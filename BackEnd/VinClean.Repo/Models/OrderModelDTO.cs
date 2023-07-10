using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models
{
    public class OrderModelDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerImage { get; set; }
        public string CustomerName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; } 
        public int TypeId { get; set; }
        public string Type { get; set; } 
        public DateTime? DateWork { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? StartWorking { get; set; }
        public TimeSpan? EndWorking { get; set; }
        public decimal? Total { get; set; }
        public int PonitUsed { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImg { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
    }
}
