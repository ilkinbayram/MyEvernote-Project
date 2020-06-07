using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class WarningViewModel:NotifyModelBase<string>
    {
        public WarningViewModel()
        {
            Details = new List<string>();
            ColorOfNotifyBack = ColorNotify.GetColor(NameOfColors.warningLightPlus);
            AlertColor = "warning";
            RedirectSeconds = 8;
            IsRedirecting = true;
        }
    }
}