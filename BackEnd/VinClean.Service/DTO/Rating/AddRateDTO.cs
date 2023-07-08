using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Rating
{
    public class AddRateDTO
    {
        public int OrderId { get; set; }
        public byte Rate { get; set; } 
        public string Comment { get; set; }
        

    }
}
