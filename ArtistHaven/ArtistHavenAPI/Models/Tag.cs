using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(Id))]
    [Index(nameof(Name), IsUnique = true)]
    public class Tag {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<PostTag> Posts { get; set; }
    }
}
