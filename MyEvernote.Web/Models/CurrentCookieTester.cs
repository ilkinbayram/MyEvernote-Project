using MyEvernote.BussinesLayer;
using MyEvernote.BussinesLayer.Managers;
using MyEvernote.EntitiesLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEvernote.Web.Models
{
    public static class CurrentCookieTester
    {
        public static bool CookieIsExist(CookieKeys key)
        {
            if (HttpContext.Current.Request.Cookies[key.ToString()] != null)
            {
                return true;
            }

            return false;
        }
        public static string GetCurrentUsername(CookieKeys key)
        { 
            UserManager _usm = new UserManager();

            if (HttpContext.Current.Request.Cookies[key.ToString()]!=null)
            {
                string token = HttpContext.Current.Request.Cookies[key.ToString()].Value;
                User user = _usm.Get(x => x.Token.ToString() == token);
                return user.Username;
            }
            return null;
        }

        public static User GetCurrentUser(CookieKeys key)
        {
            UserManager _usm = new UserManager();

            if (HttpContext.Current.Request.Cookies[key.ToString()] != null)
            {
                string token = HttpContext.Current.Request.Cookies[key.ToString()].Value;
                User user = _usm.Get(x => x.Token.ToString() == token);
                return user;
            }
            return null;
        }

        public static string GetCurrentToken(CookieKeys key)
        {
            if (HttpContext.Current.Request.Cookies[key.ToString()] != null)
            {
                string token = HttpContext.Current.Request.Cookies[key.ToString()].Value;
                return token;
            }
            return null;
        }

        public static void SetCookie(CookieKeys key, string ownedToken)
        {
            HttpCookie savingCookie = new HttpCookie(key.ToString(), ownedToken);
            HttpContext.Current.Response.Cookies.Add(savingCookie);
        }

        public static void ExpireCookie(CookieKeys key, int timeMonthAmount)
        {
            HttpContext.Current.Session.Abandon();
            if (HttpContext.Current.Request.Cookies[key.ToString()] != null)
            {
                HttpCookie currentCookie = new HttpCookie(key.ToString());
                currentCookie.Expires = DateTime.Now.AddMonths(timeMonthAmount);
                HttpContext.Current.Response.Cookies.Add(currentCookie);
            }
        }

        public static string GetCurrentUrl(CookieKeys key)
        {
            string url = "MyEvernoteHome/Index";
            if (HttpContext.Current.Request.Cookies[key.ToString()] != null)
            {
                url = HttpContext.Current.Request.Cookies[key.ToString()].Value;
            }

            return url;
        }

    }

    public enum CookieKeys
    {
        signedUserToken = 0,
        updateableUrl = 1
    }
}