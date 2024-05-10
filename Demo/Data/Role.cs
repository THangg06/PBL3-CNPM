using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
