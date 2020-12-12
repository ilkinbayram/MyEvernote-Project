using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer.Tools
{
    public class DefaultImageHelper
    {
        public string AdminBoyProfilePhoto { get; set; }
        public string AdminGirlProfilePhoto { get; set; }
        public string UserBoyProfilePhoto { get; set; }
        public string UserGirlProfilePhoto { get; set; }
        public string PostCaptionPhoto { get; set; }

        public string sl1 { get; set; }
        public string sl2 { get; set; }
        public string sl3 { get; set; }
        public string sl4 { get; set; }
        public string sl5 { get; set; }
        public string sl6 { get; set; }

        public DefaultImageHelper()
        {
            SetDefaultProfilePhoto();
        }


        private void SetDefaultProfilePhoto()
        {
            AdminBoyProfilePhoto = "admin_profile_default_photo_last.png";
            AdminGirlProfilePhoto = "admin_girl_profilePhoto.png";
            PostCaptionPhoto = "DefaultNoteProfilePhoto.jpg";
            UserBoyProfilePhoto = "boy_profilePhoto.png";
            UserGirlProfilePhoto = "girl_profilePhoto.png";

            sl1 = "slide1_Default_Image.jpg";
            sl2 = "slide2_Default_Image.jpg";
            sl3 = "slide3_Default_Image.jpg";
            sl4 = "slide4_Default_Image.jpg";
            sl5 = "slide5_Default_Image.jpg";
            sl6 = "slide6_Default_Image.jpg";
        }
    }
}
