using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VinClean.Repo.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }
    public virtual ICollection<Blog> BlogCreatedByNavigations { get; set; } = new List<Blog>();

    public virtual ICollection<Blog> BlogModifiedByNavigations { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> CommentCreatedByNavigations { get; set; } = new List<Comment>();

    public virtual ICollection<Comment> CommentModifiedByNavigations { get; set; } = new List<Comment>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Service> ServiceCreatedByNavigations { get; set; } = new List<Service>();

    public virtual ICollection<Service> ServiceModifiedByNavigations { get; set; } = new List<Service>();
}
