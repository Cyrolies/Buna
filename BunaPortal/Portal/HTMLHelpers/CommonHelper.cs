using DALEFModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;

namespace CyroTechPortal.HTMLHelpers
{
	public static class CommonHelper
	{
        #region BaseURI
        static string baseUri = string.Empty;
        public static string BaseUri
        {
            get
            {
                baseUri = ConfigurationManager.AppSettings["DSDApiUrl"];
                return baseUri;
            }

        }
        #endregion

        #region Cache
        public static ObjectCache AppCache = MemoryCache.Default;
        public static void CacheAdd(string cacheName, object item)
        {
            if (AppCache == null)
            {
                AppCache = MemoryCache.Default;
            }
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(600000.0),

            };
            AppCache.Add(cacheName, item, cacheItemPolicy);
        }
        public static void CacheClear()
        {
            if (AppCache != null)
            {
                List<string> cacheKeys = AppCache.Select(kvp => kvp.Key).ToList();
                foreach (string cacheKey in cacheKeys)
                {
                    AppCache.Remove(cacheKey);
                }

                var field = AppCache.GetType().GetField("s_defaultCache",
                BindingFlags.Static |
                BindingFlags.NonPublic);
                field.SetValue(null, null);
            }
        }
        public static void CacheRemove(string cacheName)
        {
            if (AppCache != null && AppCache[cacheName] != null)
            {
                AppCache.Remove(cacheName);
            }
        }

        public static object CacheGet(string cacheName)
        {
            return (AppCache != null && AppCache[cacheName] != null) ? AppCache[cacheName] : null;
        }

        #endregion

        #region "Response Message"

        public static string ShowNotification(bool success, string message)
        {
            if (ConfigurationManager.AppSettings["PopupResults"].ToString() == "1")
            {
                return PopUpNotification(success, message);
            }
            else
            {
                return HeaderNotification(success, message);
            }
        }
        public static string HeaderNotification(bool success, string message)
        {

            string icon = string.Empty;
            string header = string.Empty;
            string alertClass = string.Empty;

            if (success)
            {
                header = "Success";
                alertClass = "alert-success";
                icon = "fa-check";
            }
            else
            {
                header = "Error";
                alertClass = "alert-danger";
                icon = "fa-ban";
            }

            //< div class="alert alert-success alert-dismissible">
            //            <button type = "button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
            //            <h4><i class="icon fa fa-check"></i>Header</h4>
            //            message
            //        </div>
            var html = "<div class='alert " + alertClass + " alert-dismissible'>" +
                    "<button type=button class=close data-dismiss=alert aria-hidden=true>×</button>" +
                    "<h4><i class='icon fa " + icon + "'></i> " + header + "</h4>" + message + "</div>" +
                    "<script>" +
            "$(document).ready(function() { " +
            "document.getElementById('btnclose').onclick = function () { " +
            " window.location.href = window.location.href;" +
            " };" +
            "});</script>";
            return html;

        }

        public static string PopUpNotification(bool success, string message)
        {

            string icon = string.Empty;
            string header = string.Empty;
            string alertClass = string.Empty;

            if (success)
            {
                header = "Success";
                alertClass = "box box-success";
                icon = "fa-check";
            }
            else
            {
                header = "Error";
                alertClass = "box box-danger";
                icon = "fa-ban";
            }

            var html = "<div class='modal fade' id='popupResult' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>" +
                            "<div class='popupResult'>" +
                                "<div class='" + alertClass + "' >" +
                                        "<div class='box-header'>" +
                                        " <button type = 'button' name='btncloseresult' id='btncloseresult' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button>" +
                                        "</div>" +
                                                             //  "<div class='col-md-12'>" +
                                                             //  "<div class='row'>" +
                                                             // "<div class='" + alertClass + "'>" +
                                                             "<h4><i class='icon fa " + icon + "'></i> " + header + "</h4>" + message +
                               // "</div>" +
                               //   "</div>"+
                               //  "</div>" + 
                               "</div>" +
                            "</div>" +
                      "</div>" +
            "<script>" +
             "document.getElementById('btncloseresult').onclick = function () { " +
            " window.location.href = window.location.href;" +
            " };" +

            "$(document).ready(function() { " +
            "$('#progressIcon').hide();" +
            "$('#popupResult').attr('class', 'modal fade').attr('aria-labelledby', 'myModalLabel');" +
            "$('#popupResult').modal('show');" +

            "var windowHeight = $(window).height();" +
            "var boxHeight = $('.modal-dialog').height();" +
            "$('.modal-dialog').css({ 'margin-top' : ((windowHeight - boxHeight) / 2)}); " +

            "});</script>";


            return html;

        }

        #endregion

    }
}