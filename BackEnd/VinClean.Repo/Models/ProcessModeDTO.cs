﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models
{
    public class ProcessModeDTO
    {
        public int ProcessId { get; set; }
        public int CustomerId { get; set; }
        public int? AccountId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool? IsDeleted { get; set; }
        public TimeSpan? StartWorking { get; set; }
        public TimeSpan? EndWorking { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public decimal? CostPerSlot { get; set; }
        public int? MinimalSlot { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? TotalMoney { get; set; }
        public int? TotalPoint { get; set; }
        public string AccountImage { get; set; }
        public string EmployeeImage { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmployeeAccountId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeEmail { get; set; }
    }
}
