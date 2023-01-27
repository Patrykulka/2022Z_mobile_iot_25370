using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtistHaven.API.Models {
    [PrimaryKey(nameof(PostId), nameof(MediaId))]
    public class PostMedia {
        public int PostId { get; set; }
        public int MediaId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }
        [ForeignKey(nameof(MediaId))]
        public Media Media { get; set; }
    }
}
