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

        public List<Comment> Comments { get; set; }
    }
}