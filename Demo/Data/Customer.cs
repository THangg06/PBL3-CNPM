//<<<<<<< HEAD
//﻿using System;
//using System.Collections.Generic;

//namespace Demo.Data;

//public partial class Customer
//{
//    public string CustomerId { get; set; }

//    public string? FullName { get; set; }

//    public DateTime? Birthday { get; set; }

//    public string? Avatar { get; set; }

//    public string? Address { get; set; }

//    public string? Email { get; set; }
//    public string? Phone { get; set; }
//    public DateTime? CreateDate { get; set; }

//    public string? Password { get; set; }

//    public string? Salt { get; set; }

//    public DateTime? LastLogin { get; set; }

//    public bool? Active { get; set; }
//    //public object Randomkey { get; internal set; }
//    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
//}

﻿using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Customer
{
    public string CustomerId { get; set; }

    public string? FullName { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? RoleId { get; set; }
    public string? Password { get; set; }

    public string? Salt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? Active { get; set; }
    //public object Randomkey { get; internal set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
<<<<<<< HEAD
    
}

=======

}
>>>>>>> 06235904bb357cf68a24e145336d06811074051c
