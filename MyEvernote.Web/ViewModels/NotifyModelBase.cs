using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class NotifyModelBase<T>
    {
        public string Title { get; set; }
        public string ColorOfNotifyBack { get; set; }
        public List<T> Details { get; set; }
        public string Todo { get; set; }
        public string AlertColor { get; set; }
        public bool IsRedirecting { get; set; }
        public int RedirectSeconds { get; set; }
        public string RedirectingInfo { get; set; }
        public string RedirectUrl { get; set; }
    }
}