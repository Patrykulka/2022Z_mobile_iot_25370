using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(Id))]
    public class Media {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }

        [JsonIgnore]
        public virtual List<PostMedia> Posts { get; set; } = new();
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
