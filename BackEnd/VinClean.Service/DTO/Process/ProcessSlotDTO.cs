using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Slot;

// Data holder

namespace VinClean.Service.DTO.Process
{
    public class ProcessSlotDTO
    {
        public int ProcessId { get; set; }

        public int SlotId { get; set; }

        public virtual ProcessDTO Process { get; set; }

        public virtual SlotDTO Slot { get; set; }
    }
}
