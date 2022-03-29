using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using DALEFModel;
using Common;
using Models;
using BunaPortal.HTMLHelpers;
using BunaPortal.Repository;

namespace BunaPortal
{
	public class UserController : BaseController
	{

		#region User Methods

		/// <summary>
		/// User this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult User()
		{
			return View();
		}

		/// <summary>
		/// Gets the User list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The j query data tables model.</param>
		/// <returns></returns>
		public ActionResult GetUserList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			//string uri = CommonHelper.BaseUri + "UserController/GetUsers";
			//jQueryDataTablesModel.uri = uri;
			User user = null;
			try
			{

				//using (HttpClient httpClient = new HttpClient())
				//{
				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//	GridParam gridParams = new GridParam();
				//	gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
				//	gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
				//	gridParams.Includerelations = true;
				//	gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
				//	gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
				//	StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
				//	HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;

				//HttpResponseMessage response = GetGridData(jQueryDataTablesModel,true);
				//if (response.IsSuccessStatusCode)
				//{

				//	var jsonString = response.Content.ReadAsStringAsync();
				//	jsonString.Wait();
				//	GridResult<User> result = JsonConvert.DeserializeObject<GridResult<User>>(jsonString.Result);
				GridParam gridParams = new GridParam();
				gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
				gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
				gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
				gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
				if (Session != null && Session["User"] != null)
				{
					if (gridParams.ListFilterBy == null)
					{
						gridParams.ListFilterBy = new List<FilterField>();
					}
					user = (User)Session["User"];

					//Always filter by orgid
					gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
				}
				else
				{
					RedirectToAction("Login", "Account");
				}
				UserRepository repo = new UserRepository();
				GridResult<User> result = repo.GetUserList(gridParams);
					List <User> retList = new List<DALEFModel.User>();
						foreach (User item in result.Items)
						{
							//if (item.StpData != null)
							//{
							//	item.StpData = null;
							//}
							//if (item.StpData1 != null)
							//{
							//	item.StpData1 = null;
							//}
							//if (item.StpData2 != null)
							//{
							//	item.StpData2 = null;
							//}
							
							//if (item.User1 != null)
							//{
							//	item.User1 = null;
							//}
							//if (item.User2 != null)
							//{
							//	item.User2 = null;
							//}
							//if (item.User3 != null)
							//{
							//	item.User3 = null;
							//}
							//if (item.User4 != null)
							//{
							//	item.User4 = null;
							//}
							//if (item.User11 != null)
							//{
							//	item.User11 = null;
							//}
							//if (item.User12 != null)
							//{
							//	item.User12 = null;
							//}
							//if (item.UserRole != null)
							//{
							//	item.UserRoleDesc = item.UserRole.RoleName;
							//	item.UserRole = null;
							//}
							//if (item.Organization1 != null)
							//{
							//	item.Organization1 = null;
							//}
							//if (item.StcData != null)
							//{
							//	item.StcData = null;
							//}
							//if (item.Contact != null)
							//{
							//	item.Contact = null;
							//}
							item.IsActiveCheckBox = item.IsActive == true ? " <span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
							item.EditButton = "<div class='btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.UserID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.UserID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
						retList.Add(item);
						}

						return Json(new
						{
							sEcho = jQueryDataTablesModel.sEcho,
							iTotalRecords = result.TotalCount,
							iTotalDisplayRecords = result.TotalFilteredCount,
							aaData = retList
						},
						JsonRequestBehavior.AllowGet);
					//}
					//else
					//{
					//	var readAsStringAsync = response.Content.ReadAsStringAsync();
					//	return Content( readAsStringAsync.Result);
					//}

				//}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// <param name="UserID">The User identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetUser(int? id)
		{

			try
				{
				DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
				string uri = CommonHelper.BaseUri + "UserController/GetUser";
					if (id == null)
					{
						return PartialView("EditUser", new User()
						{
							CreateDateTime = SATime,
							CreatedByID = ((User)Session["User"]).UserID,
							StpThemeID = ((User)Session["User"]).StpThemeID,
							StpLanguageID = ((User)Session["User"]).StpLanguageID,
							OrgID = ((User)Session["User"]).OrgID,
							
						});
					}
				UserRepository repo = new UserRepository();
				User item = repo.GetUser(id.ToString());
				item.ChangeDateTime = SATime;
				item.ChangedByID = ((User)Session["User"]).UserID;

				return PartialView("EditUser", item);

				//                 using (HttpClient httpClient = new HttpClient())
				//                 {
				//                     httpClient.DefaultRequestHeaders.Accept.Clear();
				//                     httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				//HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/false").Result;
				//                     var content = response.Content.ReadAsStringAsync();
				//                     if (response.IsSuccessStatusCode)
				//                     {
				//                         var settings = new JsonSerializerSettings
				//                         {
				//                             NullValueHandling = NullValueHandling.Ignore,
				//                             MissingMemberHandling = MissingMemberHandling.Ignore
				//                         };
				//                         User item = JsonConvert.DeserializeObject<User>(content.Result, settings);
				//	item.ChangeDateTime = SATime;
				//	item.ChangedByID = ((User)Session["User"]).UserID;

				//	return PartialView("EditUser", item);
				//                     }
				//                     else
				//                     {
				//                           return Content(content.Result);
				//                     }
				//                 }
			}
			catch (System.Exception ex)
				{
					return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
				}

			}

		/// <summary>
		/// <param name="User">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditUser(User newItem)
		{

			try
				{
				
				UserRepository repo = new UserRepository();
				
				if (newItem.UserID == 0)
				{
					if (newItem.UserRoleID == 9 && newItem.IsActive == newItem.Registered == false) //Ext officer
					{
						newItem.Registered = true;
						Common.Common.SendEmail("", newItem.Email, "Your Buna registration has been approved", "Your login credentials have been created for you " + Environment.NewLine + " Please login using the following Username : " + newItem.UserName + " and Password  1111  " + Environment.NewLine + " You will then be asked to create your own password.");
					}
					repo.Add(newItem);
					
					return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Added")));
				}
				else
				{
					if (newItem.UserRoleID == 9 && newItem.IsActive && newItem.Registered == false) //Ext officer
					{
						newItem.Registered = true;
						Common.Common.SendEmail("", newItem.Email, "Your Buna registration has been approved", "Your login credentials have been created for you " + Environment.NewLine + " Please login using the following Username : " + newItem.UserName + " and Password  1111  " + Environment.NewLine + " You will then be asked to create your own password.");

					}
					repo.Update(newItem);
					
					return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Updated")));
				}
				//string uri = CommonHelper.BaseUri + "UserController/GetUser";
				//string uriAdd = CommonHelper.BaseUri + "UserController/User/add";
				//string uriUpdate = CommonHelper.BaseUri + "UserController/User/update";
				//User itemExists = null;
				//using (HttpClient httpClient = new HttpClient())
				//{

				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//	//Check exists
				//	HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.UserID+ "/false").Result;
				//	var content = response.Content.ReadAsStringAsync();
				//	if (response.IsSuccessStatusCode)
				//	{
				//			var settings = new JsonSerializerSettings
				//			{
				//			NullValueHandling = NullValueHandling.Ignore,
				//			MissingMemberHandling = MissingMemberHandling.Ignore
				//			};
				//		itemExists = JsonConvert.DeserializeObject<User>(content.Result,settings);
				//	}


				//	//Insert
				//	if (itemExists == null)
				//	{
				//		newItem.UserPWDHash = Common.PasswordHash.CreateHash("1111");
				//		newItem.UserName = newItem.Firstname + " " + newItem.Surname;
				//		StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
				//		HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
				//		var resultAdd = responseAdd.Content.ReadAsStringAsync();
				//		if (responseAdd.IsSuccessStatusCode)
				//		{
				//			if(newItem.StpDepartmentID == 2404 && newItem.IsActive) //Ext officer
				//			{
				//				Common.Common.SendEmail("", newItem.Email, "Your Buna registration has been approved", "Your login credentials have been created for you " + Environment.NewLine + " Please login using the following Username : " + newItem.UserName + " and Password  1111  " + Environment.NewLine + " You will then be asked to create your own password.");
				//			}
				//			return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Added")));
				//		}
				//		else
				//		{
				//			return Content(CommonHelper.ShowNotification(false, resultAdd.Result));
				//		}
				//	}
				//	else//Update
				//	{
				//		newItem.UserPWDHash = itemExists.UserPWDHash;
				//		StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
				//		HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
				//		var resultOut = responseOut.Content.ReadAsStringAsync();
				//		if (responseOut.IsSuccessStatusCode)
				//		{
				//			if (newItem.StpDepartmentID == 2404 && newItem.IsActive && itemExists.IsActive == false) //Ext officer
				//			{
				//				Common.Common.SendEmail("", newItem.Email, "Your Buna registration has been approved", "Your login credentials have been created for you " + Environment.NewLine + " Please login using the following Username : " + newItem.UserName + " and Password  1111  " + Environment.NewLine + " You will then be asked to create your own password.");
				//			}
				//			return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Updated")));
				//		}
				//		else
				//		{
				//			return Content(CommonHelper.ShowNotification(false, resultOut.Result));
				//		}
				//	}
				//}
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// Deletes the User.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteUser(int id)
		{
			try
			{
				UserRepository repo = new UserRepository();
				repo.Delete(id);
				return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Deleted")));
				//string uri = CommonHelper.BaseUri + "UserController/User/Delete";
				//if (id == null)
				//{
				//	return View();
				//}
				//using (HttpClient httpClient = new HttpClient())
				//{

				//	StringContent content1 = new StringContent(id.ToString());
				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//	HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
				//	var content = response.Content.ReadAsStringAsync();
				//	if (response.IsSuccessStatusCode)
				//	{
				//		return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Deleted")));
				//	}
				//	else
				//	{
				//		return Content(CommonHelper.ShowNotification(false, content.Result));
				//	}
				//}
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}


		}

		/// <summary>
		/// Exports the form.
		/// </summary>
		/// <param name="dataList">The data list.</param>
		/// <returns></returns>
		public ActionResult ExportUserForm(string dataList)
		{
			try
			{
				Export exportdata = new Export();
				exportdata.Controller = "User";
				exportdata.Entity = "User";
				exportdata.DatatableParams = dataList;
				return PartialView("ExportControl", exportdata);
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the data.
		/// </summary>
		/// <param name="exportData">The export data.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">
		/// </exception>
		[HttpPost]
		public FileResult ExportUserData(Export exportDetails)
		{
			try
			{
				FileContentResult file = base.ExportBase<StpData>(exportDetails);
				if (file != null)
				{
					return file;
				}
				else
				{
					throw new System.Exception(Localizer.Current.GetString("ExportError"));
				}

			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Gets the reset password.
		/// </summary>
		/// <returns></returns>
		public ActionResult GetResetPassword()
        {
            try
            {
                return PartialView("ResetUserPasswordControl", new User());

            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Resets the mobile user list.
        /// </summary>
        /// <param name="checkList">The check list.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetUserList(string[] checkList)
        {

            try
            {
                if (checkList?.Count() > 0)
                {
                    //List<User> users = new List<DALEFModel.User>();
                    //foreach (string userid in checkList)
                    //{
                    //    User newuser = new DALEFModel.User() {  UserID = Convert.ToInt32(userid) };
                    //    //Password is reset in the API save method
                    //    users.Add(newuser);
                    //}

                    string uriSave = CommonHelper.BaseUri + "UserController/ResetUsers";

                    using (HttpClient httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(checkList), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriSave, content).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
							var settings = new JsonSerializerSettings
							{
								NullValueHandling = NullValueHandling.Ignore,
								MissingMemberHandling = MissingMemberHandling.Ignore
							};
							PasswordResetList item = JsonConvert.DeserializeObject<PasswordResetList>(resultOut.Result, settings);
							StringBuilder usersList = new StringBuilder();
							foreach (User user in item.usersReset)
							{
								usersList.Append(user.Fullname + " - Email = " +user.Email + "<br>");
								Common.Common.SendEmail("", user.Email, "Buna request response", "Please login using the following pin " + item.defaultPwd + " as your password."+Environment.NewLine+" You will be requested to create a new password.");

							}
							return Content(CommonHelper.ShowNotification(true,Localizer.Current.GetString("PasswordResetMessage") +"  <b>" +  item.defaultPwd + "</b><br>" + usersList.ToString()));
                        }
                        else
                        {
                            return Content(CommonHelper.ShowNotification(false,resultOut.Result));
                        }

                    }
                }
                else
                {
                    return Content(CommonHelper.ShowNotification(false, "No users selected"));
                }
            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        #endregion

        #region UserRole Methods

        /// <summary>
        /// UserRole this instance.
        /// <summary>
        /// <returns></returns>
        [HttpGet]
		public ViewResult UserRole()
		{
			return View();
		}

		/// <summary>
		/// Gets the UserRole list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The j query data tables model.</param>
		/// <returns></returns>
		public ActionResult GetUserRoleList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "UserController/UserRole";
			try
			{

				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					GridParam gridParams = new GridParam();
					gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
					gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
					gridParams.Includerelations = false;
					gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
					gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
					StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
					HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
					if (response.IsSuccessStatusCode)
					{

						var jsonString = response.Content.ReadAsStringAsync();
						jsonString.Wait();
						GridResult<UserRole> result = JsonConvert.DeserializeObject<GridResult<UserRole>>(jsonString.Result);
						List<UserRole> retList = new List<DALEFModel.UserRole>();
						foreach (UserRole item in result.Items)
						{

							item.IsActiveCheckBox = item.IsActive == true ? " <span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.UserRoleID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.UserRoleID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
						retList.Add(item);
						}

						return Json(new
						{
							sEcho = jQueryDataTablesModel.sEcho,
							iTotalRecords = result.TotalCount,
							iTotalDisplayRecords = result.TotalFilteredCount,
							aaData = retList
						},
						JsonRequestBehavior.AllowGet);
					}
					else
					{
						var readAsStringAsync = response.Content.ReadAsStringAsync();
						return Content( readAsStringAsync.Result);
					}

				}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// <param name="UserRoleID">The UserRole identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetUserRole(int? id)
		{

			try
				{

					string uri = CommonHelper.BaseUri + "UserController/UserRole";
					if (id == null)
					{
						return PartialView("EditUserRole", new UserRole());
					}
					using (HttpClient httpClient = new HttpClient())
					{
						httpClient.DefaultRequestHeaders.Accept.Clear();
						httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
						var content = response.Content.ReadAsStringAsync();
						if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
							UserRole item = JsonConvert.DeserializeObject<UserRole>(content.Result,settings);
							return PartialView("EditUserRole", item);
						}
					else
					{
							  return Content(content.Result);
						}

					}
				}
				catch (System.Exception ex)
				{
					return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
				}

			}

		/// <summary>
		/// <param name="UserRole">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditUserRole(UserRole newItem)
		{

			try
				{

				string uri = CommonHelper.BaseUri + "UserController/UserRole";
				string uriAdd = CommonHelper.BaseUri + "UserController/UserRole/add";
				string uriUpdate = CommonHelper.BaseUri + "UserController/UserRole/update";
				UserRole itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.UserRoleID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<UserRole>(content.Result,settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							return Content(Localizer.Current.GetString("Successfully Added"));
						}
						else
						{
							return Content( resultAdd.Result);
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							return Content( Localizer.Current.GetString("Successfully Updated"));
						}
						else
						{
							return Content(resultOut.Result);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// Deletes the UserRole.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteUserRole(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "UserController/DeleteUser";
				if (id == null)
				{
					return View();
				}
				using (HttpClient httpClient = new HttpClient())
				{

					StringContent content1 = new StringContent(id.ToString());
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						return Content(Localizer.Current.GetString("Successfully Deleted"));
					}
					else
					{
						return Content( content.Result);
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the form.
		/// </summary>
		/// <param name="dataList">The data list.</param>
		/// <returns></returns>
		public ActionResult ExportUserRoleForm(string dataList)
		{
			try
			{
				Export exportdata = new Export();
				exportdata.Controller = "User";
				exportdata.Entity = "UserRole";
				exportdata.DatatableParams = dataList;
				return PartialView("ExportControl", exportdata);
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the data.
		/// </summary>
		/// <param name="exportData">The export data.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">
		/// </exception>
		[HttpPost]
		public FileResult ExportUserRoleData(Export exportDetails)
		{
			try
			{
				FileContentResult file = base.ExportBase<UserRole>(exportDetails);
				if (file != null)
				{
					return file;
				}
				else
				{
					throw new System.Exception(Localizer.Current.GetString("ExportError"));
				}

			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region UserRoleActivity Methods

		/// <summary>
		/// UserRoleActivity this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult UserRoleActivity()
		{
			return View();
		}

		/// <summary>
		/// Gets the UserRoleActivity list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The j query data tables model.</param>
		/// <returns></returns>
		public ActionResult GetUserRoleActivityList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "UserController/UserRoleActivity";
			try
			{

				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					GridParam gridParams = new GridParam();
					gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
					gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
					gridParams.Includerelations = true;
					gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
					gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
					StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
					HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
					if (response.IsSuccessStatusCode)
					{

						var jsonString = response.Content.ReadAsStringAsync();
						jsonString.Wait();
						GridResult<UserRoleActivity> result = JsonConvert.DeserializeObject<GridResult<UserRoleActivity>>(jsonString.Result);
						List<UserRoleActivity> retList = new List<DALEFModel.UserRoleActivity>();
						foreach (UserRoleActivity item in result.Items)
						{
                            if (item.Activity != null)
                            {
                                item.ActivityDesc = item.ActivityID.ToString() + " - " + item.Activity.ActivityName;
                            }
                            item.Activity = null;

                            if (item.UserRole != null)
							{
							item.UserRoleDesc = item.UserRoleID.ToString() + " - " + item.UserRole.RoleName;
							}
							item.UserRole = null;

							if (item.StcData != null)
							{
							item.PermissionDesc = item.StcPermissionID.ToString() + " - " + item.StcData.Description;
							}
							item.StcData = null;

                            if (item.StcData1 != null)
                            {
                                item.StcData1 = null;
                            }
                            if (item.Permissions != null)
                            {
                                item.Permissions = null;
                            }

                            item.IsActiveCheckBox = item.IsActive == true ? " <span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.UserRoleActivityID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.UserRoleActivityID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
						    retList.Add(item);
						}

						return Json(new
						{
							sEcho = jQueryDataTablesModel.sEcho,
							iTotalRecords = result.TotalCount,
							iTotalDisplayRecords = result.TotalFilteredCount,
							aaData = retList
						},
						JsonRequestBehavior.AllowGet);
					}
					else
					{
						var readAsStringAsync = response.Content.ReadAsStringAsync();
						return Content( readAsStringAsync.Result);
					}

				}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// <param name="UserRoleActivityID">The UserRoleActivity identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetUserRoleActivity(int? id)
		{

			try
				{

					string uri = CommonHelper.BaseUri + "UserController/UserRoleActivity";
					if (id == null)
					{
						return PartialView("EditUserRoleActivity", new UserRoleActivity());
					}
					using (HttpClient httpClient = new HttpClient())
					{
						httpClient.DefaultRequestHeaders.Accept.Clear();
						httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
						var content = response.Content.ReadAsStringAsync();
						if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
							UserRoleActivity item = JsonConvert.DeserializeObject<UserRoleActivity>(content.Result,settings);
							return PartialView("EditUserRoleActivity", item);
						}
					else
					{
							  return Content(content.Result);
						}

					}
				}
				catch (System.Exception ex)
				{
					return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
				}

			}

		/// <summary>
		/// <param name="UserRoleActivity">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditUserRoleActivity(UserRoleActivity newItem)
		{

			try
				{

				string uri = CommonHelper.BaseUri + "UserController/UserRoleActivity";
				string uriAdd = CommonHelper.BaseUri + "UserController/UserRoleActivity/add";
				string uriUpdate = CommonHelper.BaseUri + "UserController/UserRoleActivity/update";
				UserRoleActivity itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.UserRoleActivityID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<UserRoleActivity>(content.Result,settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							return Content(Localizer.Current.GetString("Successfully Added"));
						}
						else
						{
							return Content( resultAdd.Result);
						}
					}
					else//Update
					{
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							return Content( Localizer.Current.GetString("Successfully Updated"));
						}
						else
						{
							return Content(resultOut.Result);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// Deletes the UserRoleActivity.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteUserRoleActivity(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "UserController/UserRoleActivity/delete";
				if (id == null)
				{
					return View();
				}
				using (HttpClient httpClient = new HttpClient())
				{

					StringContent content1 = new StringContent(id.ToString());
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						return Content(Localizer.Current.GetString("Successfully Deleted"));
					}
					else
					{
						return Content( content.Result);
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the form.
		/// </summary>
		/// <param name="dataList">The data list.</param>
		/// <returns></returns>
		public ActionResult ExportUserRoleActivityForm(string dataList)
		{
			try
			{
				Export exportdata = new Export();
				exportdata.Controller = "User";
				exportdata.Entity = "UserRoleActivity";
				exportdata.DatatableParams = dataList;
				return PartialView("ExportControl", exportdata);
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the data.
		/// </summary>
		/// <param name="exportData">The export data.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">
		/// </exception>
		[HttpPost]
		public FileResult ExportUserRoleActivityData(Export exportDetails)
		{
			try
			{
				FileContentResult file = base.ExportBase<UserRoleActivity>(exportDetails);
				if (file != null)
				{
					return file;
				}
				else
				{
					throw new System.Exception(Localizer.Current.GetString("ExportError"));
				}

			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region Activity Methods

		/// <summary>
		/// UserRoleActivity this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult Activity()
		{
			return View();
		}

		/// <summary>
		/// Gets the UserRoleActivity list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The j query data tables model.</param>
		/// <returns></returns>
		public ActionResult GetActivityList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "UserController/Activity";
			try
			{

				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					GridParam gridParams = new GridParam();
					gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
					gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
					gridParams.Includerelations = true;
					gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
					gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
					StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
					HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
					if (response.IsSuccessStatusCode)
					{

						var jsonString = response.Content.ReadAsStringAsync();
						jsonString.Wait();
						GridResult<Activity> result = JsonConvert.DeserializeObject<GridResult<Activity>>(jsonString.Result);
						List<Activity> retList = new List<DALEFModel.Activity>();
						foreach (Activity item in result.Items)
						{
							if(item.StpData != null)
							{
								item.StpActivityGroup = item.StpData.DataDescription;
								item.StpData = null;
							}
							item.IsActiveCheckBox = item.IsActive == true ? " <span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.ActivityID + "' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.ActivityID + "' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
							retList.Add(item);
						}

						return Json(new
						{
							sEcho = jQueryDataTablesModel.sEcho,
							iTotalRecords = result.TotalCount,
							iTotalDisplayRecords = result.TotalFilteredCount,
							aaData = retList
						},
						JsonRequestBehavior.AllowGet);
					}
					else
					{
						var readAsStringAsync = response.Content.ReadAsStringAsync();
						return Content(readAsStringAsync.Result);
					}

				}
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// <param name="ActivityID">The Activity identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetActivity(int? id)
		{

			try
			{

				string uri = CommonHelper.BaseUri + "UserController/Activity";
				if (id == null)
				{
					return PartialView("EditActivity", new Activity());
				}
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						Activity item = JsonConvert.DeserializeObject<Activity>(content.Result, settings);
						return PartialView("EditActivity", item);
					}
					else
					{
						return Content(CommonHelper.ShowNotification(false, content.Result));
					}

				}
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// <param name="Activity">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditActivity(Activity newItem)
		{

			try
			{

				string uri = CommonHelper.BaseUri + "UserController/Activity";
				string uriAdd = CommonHelper.BaseUri + "UserController/Activity/add";
				string uriUpdate = CommonHelper.BaseUri + "UserController/Activity/update";
				Activity itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.ActivityID + "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var settings = new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
						};
						itemExists = JsonConvert.DeserializeObject<Activity>(content.Result, settings);
					}
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					//Insert
					if (itemExists == null)
					{
						HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
						var resultAdd = responseAdd.Content.ReadAsStringAsync();
						if (responseAdd.IsSuccessStatusCode)
						{
							return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Added")));
						}
						else
						{
							return Content(CommonHelper.ShowNotification(false, resultAdd.Result));
						}
					}
					else//Update
					{
						if(itemExists.VersionNo != newItem.VersionNo)//check concurrency
						{
							return Content(CommonHelper.ShowNotification(false, Localizer.Current.GetString("ConcurrencyFailureMessage")));
						}
						HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
						var resultOut = responseOut.Content.ReadAsStringAsync();
						if (responseOut.IsSuccessStatusCode)
						{
							return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Updated")));
						}
						else
						{
							return Content(CommonHelper.ShowNotification(false, resultOut.Result));
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		/// <summary>
		/// Deletes the Activity.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteActivity(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "UserController/Activity/delete";
				if (id == null)
				{
					return View();
				}
				using (HttpClient httpClient = new HttpClient())
				{

					StringContent content1 = new StringContent(id.ToString());
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						return Content(Localizer.Current.GetString("Successfully Deleted"));
					}
					else
					{
						return Content(content.Result);
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the form.
		/// </summary>
		/// <param name="dataList">The data list.</param>
		/// <returns></returns>
		public ActionResult ExportActivityForm(string dataList)
		{
			try
			{
				Export exportdata = new Export();
				exportdata.Controller = "User";
				exportdata.Entity = "Activity";
				exportdata.DatatableParams = dataList;
				return PartialView("ExportControl", exportdata);
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		/// <summary>
		/// Exports the data.
		/// </summary>
		/// <param name="exportData">The export data.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">
		/// </exception>
		[HttpPost]
		public FileResult ExportActivityData(Export exportDetails)
		{
			try
			{
				FileContentResult file = base.ExportBase<Activity>(exportDetails);
				if (file != null)
				{
					return file;
				}
				else
				{
					throw new System.Exception(Localizer.Current.GetString("ExportError"));
				}

			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}
		#endregion
	}
}
