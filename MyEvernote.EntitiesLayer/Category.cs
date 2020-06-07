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
    [Table("Categories")]
    public class Category : MyBaseEntity
    {
            [DisplayName("Kategoriya"),
            Required,
            MinLength(1),
            MaxLength(150)]
        public string CategoryName { get; set; }
            [DisplayName("Icerik"),
            MaxLength(1500)]
        public string Description { get; set; }

        // Relationship
        public virtual List<Note> Notes { get; set; }

        // Constructor
        public Category()
        {
            Notes = new List<Note>();
        }
    }
}
