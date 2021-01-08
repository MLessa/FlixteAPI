﻿using Flixte.APIV2.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;


namespace Flixte.APIV2.Controllers
{
    [AccessControlService]
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            if (Request.Cookies[Core.Util.CookieTokenName] != null)
                ViewData[Core.Util.CookieTokenName] = Request.Cookies[Core.Util.CookieTokenName].Value;
            if (Request.QueryString["accessToken"] != null)
                ViewData[Core.Util.CookieTokenName] = Request.QueryString["accessToken"];
            else
            {
                if (!string.IsNullOrEmpty(Request.Headers["Authorization"]))
                    ViewData[Core.Util.CookieTokenName] = Request.Headers["Authorization"];
            }

        }
        public int GetUserID()
        {           
            try {
                if (Request.Cookies[Core.Util.CookieTokenName] == null || (Request.Cookies[Core.Util.CookieTokenName].Value != (string)ViewData[Core.Util.CookieTokenName] && ViewData[Core.Util.CookieTokenName]!=null))
                {
                    if (Response.Cookies[Core.Util.CookieTokenName] != null)
                        Response.Cookies.Remove(Core.Util.CookieTokenName);
                    if (ViewData[Core.Util.CookieTokenName] != null)
                        Response.Cookies.Add(new System.Web.HttpCookie(Core.Util.CookieTokenName, ViewData[Core.Util.CookieTokenName].ToString()));
                    ViewData[Core.Util.CookieTokenName] = Request.QueryString["accessToken"];
                }
                var user = Core.Util.GetUser(ViewData[Core.Util.CookieTokenName].ToString(), null);
                if (user != null)
                    return user.ID;
            }
            catch { }
            return -1;
        }
    }
}
