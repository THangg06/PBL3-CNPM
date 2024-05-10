<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;

namespace Demo.Data;

public partial class Category
{
    public string CatId { get; set; }

    public string? CatName { get; set; }

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public int? Levels { get; set; }

    public int? Ordering { get; set; }

    public int? Published { get; set; }

    public string? Thumb { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? MetaDesc { get; set; }

    public string? MetaKey { get; set; }

    public string? Cover { get; set; }

    public string? SchemaMarkup { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
=======
//﻿using System;
//using System.Collections.Generic;

//namespace Demo.Data;

//public partial class Category
//{
//    public string CatId { get; set; }

//    public string? CatName { get; set; }

//    public string? Description { get; set; }

//    public int? ParentId { get; set; }

//    public int? Levels { get; set; }

//    public int? Ordering { get; set; }

//    public int? Published { get; set; }

//    public string? Thumb { get; set; }

//    public string? Title { get; set; }

//    public string? Alias { get; set; }

//    public string? MetaDesc { get; set; }

//    public string? MetaKey { get; set; }

//    public string? Cover { get; set; }

//    public string? SchemaMarkup { get; set; }

//    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
//}
//>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
