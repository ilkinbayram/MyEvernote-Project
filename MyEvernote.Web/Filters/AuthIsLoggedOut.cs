using MyEvernote.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.Web.Filters
{
    public class AuthIsLoggedOut : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentCookieTester.GetCurrentUser(CookieKeys.signedUserToken) == null)
            {
                filterContext.Result = new RedirectResult("/MyEvernoteHome/Login");
            }
        }
    }
}