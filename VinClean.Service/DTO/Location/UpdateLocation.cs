using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.WorkingBy
{
    public class UpdateLocation
    {
        public int OrderId { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }
    }
}
