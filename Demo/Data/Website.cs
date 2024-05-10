//<<<<<<< HEAD
//﻿using System;
//using System.Collections.Generic;

//namespace Demo.Data;

//public partial class Website
//{
//    public int Idweb { get; set; }

//    public string? Nameweb { get; set; }

//    public string? Url { get; set; }

//    public string? StaffIdnv { get; set; }

//    public virtual Staff? StaffIdnvNavigation { get; set; }
//}
//=======
﻿using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Website
{
    public int Idweb { get; set; }

    public string? Nameweb { get; set; }

    public string? Url { get; set; }

    public string? StaffIdnv { get; set; }

    public virtual Staff? StaffIdnvNavigation { get; set; }
}

