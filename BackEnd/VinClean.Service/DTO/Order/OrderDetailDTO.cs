using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Order
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }

        public int ServiceId { get; set; }

        public int RateId { get; set; }

        public byte Slot { get; set; }

        public decimal Total { get; set; }

        public TimeSpan StartWorking { get; set; }

        public TimeSpan EndWorking { get; set; }

    }
}
