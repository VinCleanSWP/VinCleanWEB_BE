﻿using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class ProcessImage
{
    public int Id { get; set; }

    public int? ProcessId { get; set; }

    public int? OrderId { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Process? Process { get; set; }
}
