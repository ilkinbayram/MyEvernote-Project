using MyEvernote.BussinesLayer.Tools;
using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class ViewModelEach
    {
        public Note Note { get; set; }
        public List<Note> Notes { get; set; }
        public User User { get; set; }
        public List<User> Users { get; set; }
        public List<Comment> Comments { get; set; }
        public DefaultImageHelper ImageHelper { get; set; }
        public DefaultDirectoryHelper DirectoryHelper { get; set; }
    }
}