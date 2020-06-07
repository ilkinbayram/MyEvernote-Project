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
    public abstract class MyBaseEntity
    {
            [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            [DisplayName("Hazirlanma/Tarixi"),Required, ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
            [DisplayName("Deyistirilme/Tarixi"),Required, ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }
            [DisplayName("Deyistirdi/Istifadeci"),Required, ScaffoldColumn(false),MaxLength(100)]
        public string ModifiedUsername { get; set; }
            [DisplayName("SilindiMi"), ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }
    }
}
