using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Employee;

namespace VinClean.Service.DTO.WorkingBy
{
    public class WorkingBy_processDTO
    {
        public int EmployeeId { get; set; }
        public Employee_processDTO Employee { get; set; }
    }
}
