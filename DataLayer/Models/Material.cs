using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Material
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string Name { get; set; } = null!;

    public string? Path { get; set; }

    public string? Type { get; set; }

    public long? Size { get; set; }

    [JsonIgnore]
    [NotMapped]
    public virtual Game? Game { get; set; } = null!;
}
