using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class OkViewModel:NotifyModelBase<string>
    {
        public OkViewModel()
        {
            Details = new List<string>();
            Title = "Ugurlu Emeliyyat";
            ColorOfNotifyBack = ColorNotify.GetColor(NameOfColors.successLightPlus);
            AlertColor = "primary";
            RedirectSeconds = 10;
            IsRedirecting = true;
        }
    }
}