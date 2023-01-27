using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(PostId), nameof(TagId))]
    public class PostTag {
        public int PostId { get; set; }
        public int TagId { get; set; }

        [ForeignKey(nameof(PostId))]
        [JsonIgnore]
        public virtual Post Post { get; set; }
        [ForeignKey(nameof(TagId))]
        [JsonIgnore]
        public virtual Tag Tag { get; set; }
    }
}
