using MyEvernote.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Filters
{
    public class AuthCheckAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if(CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken)!=null && CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken).IsAdmin == false)
            {
                filterContext.Result = new RedirectResult("/MyEvernoteHome/AccessDenied");
            }
        }
    }
}