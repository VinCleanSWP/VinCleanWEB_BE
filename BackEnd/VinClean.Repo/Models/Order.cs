using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public string? Note { get; set; }

    public decimal? Total { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? FinishedDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public DateTime? DateWork { get; set; }

    public virtual Customer? Customer { get; set; }

}
