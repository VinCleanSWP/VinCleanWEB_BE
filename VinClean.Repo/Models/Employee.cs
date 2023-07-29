using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Status { get; set; }

    public int? AccountId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Account? Account { get; set; }
}
