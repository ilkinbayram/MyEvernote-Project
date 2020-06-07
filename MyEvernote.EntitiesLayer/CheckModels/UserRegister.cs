using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.EntitiesLayer.CheckModels
{
    public class UserRegister
    {
            [DisplayName("İstifadəçi Adı"),
            Required(ErrorMessage = "*{0} Boş Buraxıla Bilməz"),
            MinLength(1, ErrorMessage = "*{0} Min. {1} Simvol Olmalıdır"),
            MaxLength(50, ErrorMessage = "*{0} Max. {1} Simvol olmalıdır")]
        public string Username { get; set; }
            [DisplayName("Şifrə"),
            Required(ErrorMessage = "*{0} Boş Buraxıla Bilməz"),
            MinLength(8, ErrorMessage = "*{0} Min. {1} Simvol Olmalıdır"),
            MaxLength(200, ErrorMessage = "*{0} Max. {1} Simvol Olmalıdır"),
            DataType(DataType.Password)]
        public string Password { get; set; }
            [DisplayName("Şifrə (Təkrar)"),
            Required(ErrorMessage = "*{0} Boş Buraxıla Bilməz"),
            MinLength(8, ErrorMessage = "*{0} Min. {1} Simvoldan İbarət Olmalıdır"),
            MaxLength(200, ErrorMessage = "*{0} Max. {1} Simvol Olmalıdır"),
            Compare(nameof(Password), ErrorMessage ="*{0} İlə {1} Uzlaşmadı"),
            DataType(DataType.Password)]
        public string RePassword { get; set; }
            [DisplayName("E-Poçt"),
            Required(ErrorMessage = "*{0} Boş Buraxıla Bilməz"),
            MinLength(5, ErrorMessage = "*{0} Min. {1} Simvol Olmalıdır"),
            MaxLength(75, ErrorMessage = "*{0} Max. {1} Simvol Olmalıdır"),
            EmailAddress(ErrorMessage ="*Qeyd Etdiyiniz {0} Mövcud Deyil")]
        public string Email { get; set; }
            [DisplayName("Cins"),
            Required(ErrorMessage = "*{0} Boş Buraxıla Bilməz")]
        public bool Gender { get; set; }
    }
}