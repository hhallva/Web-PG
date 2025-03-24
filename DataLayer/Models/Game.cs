using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Game
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [JsonIgnore]
    public int GenreId { get; set; }

    public virtual ICollection<GameVersion> GameVersions { get; set; } = new List<GameVersion>();

    public virtual Genre Genre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
