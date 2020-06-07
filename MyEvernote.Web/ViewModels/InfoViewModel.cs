using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class InfoViewModel:NotifyModelBase<string>
    {
        public InfoViewModel()
        {
            Details = new List<string>();
            ColorOfNotifyBack = ColorNotify.GetColor(NameOfColors.successLightPlus);
            AlertColor = "info";
            RedirectSeconds = 10;
            IsRedirecting = true;
        }
    }
}