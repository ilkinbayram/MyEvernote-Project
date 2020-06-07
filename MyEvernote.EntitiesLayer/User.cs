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
    [Table("Users")]
    public class User : MyBaseEntity
    {
            [DisplayName("Name"),
            MinLength(1),
            MaxLength(30)]
        public string Name { get; set; }
            [DisplayName("Surname"),
            MinLength(1),
            MaxLength(40)]
        public string Surname { get; set; }
            [DisplayName("Username"),
            Required(ErrorMessage = "{0}'nizi Daxil Edin"),
            MinLength(2),
            MaxLength(50)]
        public string Username { get; set; }
            [DisplayName("Password"),
            Required(ErrorMessage = "{0}'nuzu Daxil Edin"),
            MinLength(8),
            MaxLength(200)]
        public string Password { get; set; }
            [DisplayName("Email"),
            Required(ErrorMessage ="{0}'nizi Daxil Edin"),
            MinLength(5),
            MaxLength(50),
            EmailAddress(ErrorMessage ="{0} Duzgun Formatda Deyil")]
        public string Email { get; set; }
            [DisplayName("Gender"),
            Required()]
        public bool Gender { get; set; }
        public bool IsBanned { get; set; }
        public bool IsConfirmed { get; set; }
        [Required(),ScaffoldColumn(false)]
        public Guid ConfirmCode { get; set; }
        [DisplayName("Token"),
            Required(), ScaffoldColumn(false)]
        public Guid Token { get; set; }
        [ScaffoldColumn(false),MaxLength(1000)]
        public string ImageRoad { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
