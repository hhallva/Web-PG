using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class GameVersion
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string Version { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    [JsonIgnore]
    public virtual Game Game { get; set; } = null!;
}
