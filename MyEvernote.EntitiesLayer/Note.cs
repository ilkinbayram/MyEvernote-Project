using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.EntitiesLayer
{
    [Table("Notes")]
    public class Note : MyBaseEntity
    {
            [Required,
            DisplayName("Post"),
            MaxLength(150)]
        public string NoteTitle { get; set; }
            [Required,
            DisplayName("Post Metni"),
            MaxLength(3000)]
        public string Text { get; set; }
        [DisplayName("Qaralamadirmi ?")]
        public bool IsDraft { get; set; }
        [DisplayName("Beyenilme Sayisi")]
        public int LikeCount { get; set; }
        [DisplayName("Post Fotosu"),MaxLength(1000)]
        public string ImageCap { get; set; }

        // Relationship
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        // Constructor
        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}
