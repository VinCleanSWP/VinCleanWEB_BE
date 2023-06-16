﻿using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Process
{
    public int ProcessId { get; set; }

    public int? CustomerId { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Account? ModifiedByNavigation { get; set; }
}
