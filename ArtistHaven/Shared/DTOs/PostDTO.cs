using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtistHaven.Shared.DTOs {
    public class PostDTO {
        public string CreatorName { get; set; } = "";
        public string Text { get; set; } = "";
        public List<MediaDTO> Media { get; set; } = new();
    }
}
