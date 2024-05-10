using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Staff
{
    public string Idnv { get; set; } = null!;

    public string? NameNv { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? EmailNv { get; set; }

    public decimal? Salary { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? RoleId { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Website> Websites { get; set; } = new List<Website>();
}
