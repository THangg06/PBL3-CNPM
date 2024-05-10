//<<<<<<< HEAD
//﻿using System;
//using System.Collections.Generic;

//namespace Demo.Data;

//public partial class Account
//{
//    public string AccountId { get; set; }

//    public string? Phone { get; set; }

//    public string? Email { get; set; }

//    public string? Password { get; set; }

//    public string? Salt { get; set; }

//    public bool? Active { get; set; }

//    public int? RoleId { get; set; }

//    public DateTime? LastLogin { get; set; }

//    public DateTime? CreateDate { get; set; }

//    public virtual Role? Role { get; set; }
//}
//=======
﻿using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Account
{
    public string AccountId { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public bool? Active { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Role? Role { get; set; }
}
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
