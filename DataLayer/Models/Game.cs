using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Game
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int GenreId { get; set; }

    [NotMapped]
    public virtual ICollection<GameVersion?> GameVersions { get; set; } = new List<GameVersion?>();

    [NotMapped]
    public virtual Genre? Genre { get; set; } = null!;

    [NotMapped]
    public virtual ICollection<Material?> Materials { get; set; } = new List<Material?>();

    [JsonIgnore]
    [NotMapped]
    public virtual ICollection<User?> Users { get; set; } = new List<User?>();
}
