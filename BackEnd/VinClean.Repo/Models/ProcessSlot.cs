using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class ProcessSlot
{
    public int? ProcessId { get; set; }

    public int? SlotId { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Slot? Slot { get; set; }
}
