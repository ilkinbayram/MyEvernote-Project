using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.EntitiesLayer.CheckModels
{
    public class UserLogin
    {
            [DisplayName("İstifadəçi Adı"),
            Required(ErrorMessage ="*{0} Boş Buraxıla Bilməz"),
            MinLength(1,ErrorMessage ="*{0} Min. {1} Simvol Olmalıdır"),
            MaxLength(50,ErrorMessage = "*{0} Max. {1} Simvol Olmalıdır")]
        public string Username { get; set; }
            [DisplayName("Şifrə"),
            Required(ErrorMessage = "*{0} Boş Buraxıla Bilməz"),
            MinLength(8, ErrorMessage = "*{0} Min. {1} Simvol Olmalıdır"),
            MaxLength(200, ErrorMessage = "*{0} Max. {1} Simvol olmalıdır"),
            DataType(DataType.Password)]
        public string Password { get; set; }
    }
}