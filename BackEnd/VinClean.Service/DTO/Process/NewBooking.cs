using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Process
{
    public class NewBooking
    {
        [Required]
        public int ProcessId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public TimeSpan? StarTime { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public int ServiceId {get; set; }
        public string Note { get; set; } 

    }
}
