using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string Note { get; set; }

        public decimal Total { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? FinishedDate { get; set; }
    }
}
