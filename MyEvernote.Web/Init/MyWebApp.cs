﻿using MyEvernote.CommonLayer;
using MyEvernote.BussinesLayer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyEvernote.Web.Models;

namespace MyEvernote.Web.Init
{
    public class MyWebApp : ICommon
    {
        public string GetCurrentUsername()
        {
            string resultMain;
            UserManager usm = new UserManager();

            if(CurrentCookieTester.CookieIsExist(CookieKeys.signedUserToken)==true)
            {
                resultMain = usm.CheckCookieThenGetResponse(CurrentCookieTester.GetCurrentToken(CookieKeys.signedUserToken));
                if (resultMain == null)
                {
                    CurrentCookieTester.ExpireCookie(CookieKeys.signedUserToken, 6);
                }
            }
            else
            {
                DefaultCommon defaultCommon = new DefaultCommon();
                resultMain = DefaultCommon.UserName;
            }

            return resultMain;
        }
    }
}