//<<<<<<< HEAD
//﻿using System;
//using System.Collections.Generic;

//namespace Demo.Data;

//public partial class Role
//{
//    public int RoleId { get; set; }

//    public string? RoleName { get; set; }

//    public string? Description { get; set; }

//    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

//    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
//}
=======
﻿using System;
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
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
