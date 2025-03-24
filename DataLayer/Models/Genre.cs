using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
