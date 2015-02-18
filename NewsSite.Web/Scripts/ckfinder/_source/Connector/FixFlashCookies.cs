/*
 * CKFinder
 * ========
 * http://cksource.com/ckfinder
 * Copyright (C) 2007-2014, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 */

using System;
using System.Reflection;
using System.Web;

namespace CKFinder.Utils
{
    /// <summary> 
    /// Fix for the Flash Player Cookie bug in Non-IE browsers.
    /// </summary> 
    public class FixFlashCookiesModule : IHttpModule
    {
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie cookie;
            string cookie_prefix = "ckfcookie_";
            string cookie_name;
            string cookie_value;
            string command = HttpContext.Current.Request.QueryString["command"];

            if (command == null || command != "FileUpload")
                return;

            try
            {
                foreach (string formKey in HttpContext.Current.Request.Form.AllKeys)
                {
                    if (formKey.StartsWith(cookie_prefix))
                    {
                        cookie_name = formKey.Replace(cookie_prefix, "");
                        cookie_value = HttpContext.Current.Request.Form[formKey];

                        cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
                        if (cookie == null)
                        {
                            cookie = new HttpCookie(cookie_name);
                            HttpContext.Current.Request.Cookies.Add(cookie);
                        }
                        cookie.Value = cookie_value;
                        HttpContext.Current.Request.Cookies.Set(cookie);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }
    }
}
