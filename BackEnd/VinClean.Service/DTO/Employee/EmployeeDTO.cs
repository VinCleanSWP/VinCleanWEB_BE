using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Employee
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "FirstName can not be less than 2 characters")]
        [MaxLength(20, ErrorMessage = "FirstName too long")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "LastName can not be less than 2 characters")]
        [MaxLength(20, ErrorMessage = "LastName too long")]
        public string LastName { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Phone number can not less than 10")]
        [MaxLength(20, ErrorMessage = "Phone number too long")]
        public string Phone { get; set; }

        public string Status { get; set; }

        public int AccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
