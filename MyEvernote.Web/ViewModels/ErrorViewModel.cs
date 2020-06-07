using MyEvernote.BussinesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.ViewModels
{
    public class ErrorViewModel:NotifyModelBase<BussinessError>
    {
        public ErrorViewModel()
        {
            Details = new List<BussinessError>();
            Title = "Ugursuz Emeliyyat";
            ColorOfNotifyBack = ColorNotify.GetColor(NameOfColors.dangerLightPlus);
            AlertColor = "danger";
            RedirectSeconds = 10;
            IsRedirecting = true;
        }
    }
}