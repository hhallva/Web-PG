using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public override bool Equals(object? obj) => (obj is Genre genre) ? genre.Id == Id : false;
}
