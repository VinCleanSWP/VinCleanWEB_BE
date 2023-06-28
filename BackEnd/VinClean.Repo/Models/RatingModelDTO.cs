using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models
{
    public class RatingModelDTO
    {
        public int RateId { get; set; }
        public int OrderId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public byte? Rate { get; set; }
        public string Comment { get; set; }
        public DateTime? RatedDate { get; set; }
    }
}
