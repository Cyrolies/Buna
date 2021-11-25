using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Collections;
using Common;
using System.Text;
using DALEFModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Security;
using Models;
using System.Text.RegularExpressions;
using CyroTechPortal.HTMLHelpers;

namespace CyroTechPortal
{
    [AllowAnonymous]
    public class AccountController : Controller 
    {
        
        #region "User"
        public ActionResult Login(string title = "Login")
        {
            ViewBag.Title = title;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOn(User model, bool saveSettings)
        {

            try { 
                //Doing this check manually and not using dataannotations because 
                //when saving a user object pwd wont be required only when loging in
                if (model.UserPWD == null)
                {
                   return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("Password is required.")));
                
                }
                //UserController manager = new UserController();
                User user = null;
                string uri = CommonHelper.BaseUri + "UserController/Login";
                
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + model.UserName.Trim() + "/true").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        user = JsonConvert.DeserializeObject<User>(content.Result, settings);
                       
                    }
                    else
                    {
                    return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("FailedLoginMessage")));
                   
                    }
                }
                if (user != null)
                {
                    if (model.UserPWD == "1111")//New user gets created with 1111
                    {
                        try
                        {
                           return JavaScript("OnPwdReset();");
                        }
                        catch(System.Exception ex)
						{
                            return Content(CommonHelper.ShowNotification(false, ex.Message));
                        }
                    }
                    if (PasswordHash.ValidatePassword(model.UserPWD, user.UserPWDHash) && user.IsReset)//Password has been reset to a temp pwd user must create new password
                    {
                        try
                        {
                           return JavaScript("OnPwdReset();");
                        }
                        catch (System.Exception ex)
                        {
                            return Content(CommonHelper.ShowNotification(false, ex.Message));
                        }
                    }
                    if (user.UserPWDHash != null)
                    {
                        if (PasswordHash.ValidatePassword(model.UserPWD, user.UserPWDHash))
                        {
                            //Only enable this if they select this on the login screen
                            //user.StpLanguageID = model.StpLanguageID;
                            //user.StpThemeID = model.StpThemeID;
                            //--------------------------------------------------------
                            if (saveSettings)
                            {
                                //Save user selections
                                //int result = manager.SaveUserSettings(userid, model.StpLanguageID, model.StpThemeID, model.OrgID);
                                //if (result == 0)
                                //{
                                //    ModelState.AddModelError("", Localizer.Current.GetString("ErrorOccured"));
                                //}
                            }
                            FormsAuthentication.SetAuthCookie(model.UserName, true);
                            //clear entityresource cache so it reloads with users orgs list 
                            CommonHelper.CacheClear();
                            //Create User Session object
                            Session["User"] = user;
                            return JavaScript("OnSuccess();");
                        }
                        else
                        {
                            return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("FailedLoginMessage")));
                        }
                    }
                    else
                    {
                        return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("FailedLoginMessage")));
                    }
                   
                }
                else
                {
                return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("FailedLoginMessage")));
                
                }

            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
           
        }
      
        public ActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult RegisterRequest(User newItem)
        //{
        //    try
        //    {
                
        //        newItem.UserPWD = "1234";
        //        newItem.UserPWDHash = Common.PasswordHash.CreateHash("1234");
        //        newItem.Registered = false;
        //        newItem.IsActive = false;
        //        newItem.StpLanguageID = 4;
        //        newItem.StpThemeID = 8;
        //        newItem.StcStatusID = (int)Enumerations.SupervisionStatus.Approved;

                
        //            string uri = BaseUri + "UserController/Login";
        //            string uriAdd = BaseUri + "UserController/AddUser";
        //            User itemExists = null;
        //            using (HttpClient httpClient = new HttpClient())
        //            {

        //                httpClient.DefaultRequestHeaders.Accept.Clear();
        //                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //                //Check exists
        //                HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.UserName.Trim() + "/false").Result;
        //                var content = response.Content.ReadAsStringAsync();
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var settings = new JsonSerializerSettings
        //                    {
        //                        NullValueHandling = NullValueHandling.Ignore,
        //                        MissingMemberHandling = MissingMemberHandling.Ignore
        //                    };
        //                    itemExists = JsonConvert.DeserializeObject<User>(content.Result, settings);
        //                }
        //                else
        //                {
        //                      return Content(content.Result);
        //                }
        //            StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
        //                //Insert
        //                if (itemExists == null)
        //                {
        //                    HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
        //                    var resultAdd = responseAdd.Content.ReadAsStringAsync();
        //                    if (responseAdd.IsSuccessStatusCode)
        //                    {
        //                         return JavaScript("OnSuccess();");
        //                    }
        //                    else
        //                    {
        //                        return Content( resultAdd.Result);
        //                    }
        //                }
        //                else//Update
        //                {
        //                    return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Username already exists")));
        //                }
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
        //        }
           
        //}

        public ActionResult LogOff(string currentTheme)
        {
            FormsAuthentication.SignOut();
            if (Session != null)
            {
                Session.Abandon();
                Session.Timeout = 1;
            }
			
            //Clear cached Items
            CommonHelper.CacheClear();
            //Add Theme back
            if (currentTheme?.Length > 0)
            {
                CommonHelper.CacheAdd("DefaultTheme", currentTheme);

            }
            return RedirectToAction("Home", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPwdToDefault(User user)
        {
            try
            {
                string uriUpdate = CommonHelper.BaseUri + "UserController/User/update";
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 
                    if (user != null)
                    {
                        user.UserPWDHash = Common.PasswordHash.CreateHash("1111");
                        user.UserPWD = "";
                        StringContent content1 = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {
                            return JavaScript("OnSuccess();");
                        }
                        else
                        {
                            return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString(" Password has been reset.Please login again with password 1111 ")));
                        }
                    }
                    else
                    {
                        return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Could not find your user profile to update")));
                    }
                }
               
            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string username)
        {
            User user = new User() { UserName = username };
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetUserPwd(User model)
        {
            try
            {
                if (model.UserPWD == null || model.ConfirmPassword == null)
                {
                    return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("Password is required")));
                }
                var regex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
                var match = Regex.Match(model.UserPWD, regex);
                if (!match.Success)
                {
                    return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("PasswordPolicy")));
                }
                //if (model.UserPWD.Length < 4)
                //{
                //   return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("Password must be 4 or less digits long")));
                //}
                if (model.UserPWD != model.ConfirmPassword)
                {
                    return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("PasswordsNotMatching")));
                }
                
                string uri = CommonHelper.BaseUri + "UserController/Login";//Get User by username
                string uriUpdate = CommonHelper.BaseUri + "UserController/User/update";
                User user = null;
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + model.UserName + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        user = JsonConvert.DeserializeObject<User>(content.Result, settings);
                    }
                    else
                    {
                          return Content(content.Result);
                    }
                    //Insert
                    if (user != null)
                    {
                        user.UserPWDHash = Common.PasswordHash.CreateHash(model.UserPWD);
                        user.UserPWD = "";
                        user.IsReset = false;
                        user.User3 = null;
                        user.User11 = null;
                        StringContent content1 = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {
                            return JavaScript("OnSuccess();");
                        }
                        else
                        {
                            return Content( resultAdd.Result);
                        }
                    }
                    else//Update
                    {
                        return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("UserNotFound")));
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }


        //public JsonResult GetUserSettingsByUsername(string username)
        //{
        //    var user = manager.Get<User>(o => o.UserName == username);
        //    if (user != null)
        //    {
        //        var result = new
        //        {
        //            LangID = user.StpLanguageID,
        //            ThemeID = user.StpThemeID,
        //            OrgID = user.OrgID
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var result = new
        //        {
        //            LangID = ConfigurationSettings.AppSettings["DefaultLanguageID"],
        //            ThemeID = ConfigurationSettings.AppSettings["DefaultThemeID"],
        //            OrgID = ConfigurationSettings.AppSettings["DefaultOrgID"]
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //}

        #endregion

     

    }
}

