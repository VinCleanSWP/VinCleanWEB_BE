using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class OrderDetail
{
    public int? OrderId { get; set; }

    public int? ServiceId { get; set; }

    public byte? Slot { get; set; }

    public decimal? Total { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Service? Service { get; set; }
}
