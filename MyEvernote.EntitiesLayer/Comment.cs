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
    [Table("Comments")]
    public class Comment : MyBaseEntity
    {
        [Required,
            DisplayName("Text"),
            MaxLength(1000),
            MinLength(1)]
        public string Text { get; set; }
        public virtual Note Note { get; set; }
        public virtual User User { get; set; }
    }
}
