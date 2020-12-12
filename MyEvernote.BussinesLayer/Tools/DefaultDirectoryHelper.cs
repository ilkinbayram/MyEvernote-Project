using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BussinesLayer.Tools
{
    public class DefaultDirectoryHelper
    {
        // 0 : Debug Mode
        // 1 : Production Mode

        private byte ProductionMode = 0;
        public string UserImagesDir { get; set; }
        public string NoteImagesDir { get; set; }
        public string ImagesProjectGeneralDir { get; set; }

        public DefaultDirectoryHelper()
        {
            SetDirectory();
        }

        private void SetDirectory()
        {
            if (ProductionMode == 1)
            {
                UserImagesDir = @"h:\root\home\myevernote-001\www\customgeneral\images\userimages\";
                NoteImagesDir = @"h:\root\home\myevernote-001\www\customgeneral\images\noteimages\";
                ImagesProjectGeneralDir = @"h:\root\home\myevernote-001\www\customgeneral\images\imagesprojectgeneral\";
            }
            else
            {
                UserImagesDir = "/Images/UserImages/";
                NoteImagesDir = "/Images/NoteImages/";
                ImagesProjectGeneralDir = "/Images/ImagesProjectGeneral/";
            }
        }
    }
}
