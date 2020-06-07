using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Filters
{
    public class OwnExc : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            filterContext.Controller.TempData["Last-Error"] = filterContext.Exception;

            filterContext.Result = new RedirectResult("/MyEvernoteHome/ErrorTurned");
        }
    }
}