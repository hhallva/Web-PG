using System;
using System.Collections.Generic;

namespace DataLayer.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Phone { get; set; } = null!;
    
    // public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    public virtual List<Game> Games { get; set; } = new();
}
