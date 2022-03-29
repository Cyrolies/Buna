using System.Web.Mvc;
using BunaPortal.HTMLHelpers;
using System.Web;
using System;
using DALEFModel;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Common;
using System.Text;
using System.Collections.Generic;
using BunaPortal.Repository;
using DALEFModel.GraphModel;
using BunaPortal.Models.ChartModel;
using System.Configuration;

namespace BunaPortal
{
	//[Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Home()
        {
            CommonHelper.CacheClear();
            CommonHelper.CacheAdd("DefaultTheme", "~/Content/css/BunaGrey.css");
            return View();
        }

		
		public ActionResult WeatherWindow()
		{
			return PartialView("WeatherWindow");
		}

		public ActionResult GetTandC()
		{
			return PartialView("DisclaimerText");
		}

		public ViewResult DisclaimerPage()
		{
			return View();
		}

		public ViewResult ContactUs()
		{
			return View(new ContactUs());
		}

		public ActionResult Dashboard()
		{
			return View("Dashboard");
		}
		
		#region Farmer

		[HttpGet]
		public ViewResult FarmerInfo()
		{
			return View();
		}

		[HttpGet]
		public ViewResult MeetTheTeam()
		{
			return View();
		}

		[HttpGet]
		public ViewResult Manual()
		{
			return View();
		}

		public ActionResult RegisterFarmer(string countryID)
		{

			try
			{

				DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
				TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
				if (countryID == "3")//Malawi
				{
					return PartialView("RegisterFarmerMalawi", new Person()
					{
						CreateDateTime = SATime,
						IsActive = false,
						OrgID = 3,
						StpPersonTypeID = 122,
					});
				}
				if (countryID == "4")//Zambia
				{
					return PartialView("RegisterFarmerZambia", new Person()
					{
						CreateDateTime = SATime,
						IsActive = false,
						OrgID = 4,
						StpPersonTypeID = 122,
					});
				}
				return View("Home", "Home");
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}

		}
		
		public ActionResult PopiMessage(string regType)
		{
			try
			{
				TempData["RegistrationType"] = regType;
				return View("PopUpPopiWindow");
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}

		}
		[HttpPost]
		public ActionResult SaveFarmerRegistration(Person newItem)
		{
			try
			{
				string uriAdd = CommonHelper.BaseUri + "PersonController/Person/add";
				using (HttpClient httpClient = new HttpClient())
				{
					newItem.StpPersonTypeID = 122;
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
					var resultAdd = responseAdd.Content.ReadAsStringAsync();
					if (responseAdd.IsSuccessStatusCode)
					{
						string message = "Saved Successfully" + Environment.NewLine + " Once verified an Email will be send with your login credentials" + Environment.NewLine +
							"You can then login and add your farm details and production data etc. ";
						return Content(CommonHelper.ShowNotification(true, message));
					}
					else
					{
						return Content(CommonHelper.ShowNotification(false, resultAdd.Result));
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		#endregion

		#region Supplier
		[HttpGet]
        public ViewResult SupplierInfo()
        {
            return View();
        }
		public ActionResult RegisterSupplier()
		{

			try
			{
				DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
				TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
				string uri = CommonHelper.BaseUri + "SupplierController/Supplier";
				return PartialView("RegisterSupplier", new Supplier()
				{
					CreateDateTime = SATime,
					IsActive = false,
				});


			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}

		}
		/// <summary>
		/// <param name="Supplier">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult SaveSupplier(Supplier newItem)
		{

			try
			{
				string uriAdd = CommonHelper.BaseUri + "SupplierController/Supplier/add";
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					
					StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
					
					HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
					var resultAdd = responseAdd.Content.ReadAsStringAsync();
					if (responseAdd.IsSuccessStatusCode)
					{
						string message = "Saved Successfully" + Environment.NewLine +" Once verified an Email will be send with your login credentials" + Environment.NewLine + 
							"You can then login and edit your details by adding an image and product details"+Environment.NewLine +
							"Your details will then be available and visible to all farmers using Buna";
						return Content(CommonHelper.ShowNotification(true, message));
					}
					else
					{
						return Content(CommonHelper.ShowNotification(false, resultAdd.Result));
					}
				}
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}


		#endregion

		#region Ext Officer

		
		public ActionResult RegisterExtOfficer(string countryID)
		{
			try
			{

				DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
				TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
				int department = 0;
				if (countryID == "3")//Malawi
				{
					department = 2404;
				}
				else//Zambia
				{
					department = 2426;
				}
				return PartialView("RegisterExtOfficer", new User()
				{
					CreateDateTime = SATime,
					IsActive = false,
					OrgID = Convert.ToInt32(countryID),
					StpDepartmentID = department,
					Registered = false,//this gets set to true once approved by champion
				});

			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult SaveExtOfficer(User newItem)
		{
			try
			{
				newItem.UserPWD = "1111";
				newItem.UserPWDHash = Common.PasswordHash.CreateHash("1111");
				newItem.UserName = newItem.Firstname + " " + newItem.Surname; 
				newItem.Registered = false;
				newItem.IsActive = false;
				newItem.StpLanguageID = 4;
				newItem.StpThemeID = 6;
				newItem.UserRoleID = 9;
				newItem.StcStatusID = (int)Enumerations.SupervisionStatus.Approved;

				UserRepository repo = new UserRepository();
				repo.Add(newItem);
				string message = "Saved Successfully" + Environment.NewLine + " Once verified an Email will be send with your login credentials";
				return Content(CommonHelper.ShowNotification(true, message));
				//string uri = CommonHelper.BaseUri + "UserController/Login";
				//string uriAdd = CommonHelper.BaseUri + "UserController/User/add";
				//User itemExists = null;
				//using (HttpClient httpClient = new HttpClient())
				//{

				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//	//Check exists
				//	HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.UserName.Trim() + "/true/false").Result;
				//	var content = response.Content.ReadAsStringAsync();
				//	if (response.IsSuccessStatusCode)
				//	{
				//		var settings = new JsonSerializerSettings
				//		{
				//			NullValueHandling = NullValueHandling.Ignore,
				//			MissingMemberHandling = MissingMemberHandling.Ignore
				//		};
				//		itemExists = JsonConvert.DeserializeObject<User>(content.Result, settings);
				//	}
				//	else
				//	{
				//		return Content(content.Result);
				//	}
				//	StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
				//	//Insert
				//	if (itemExists == null)
				//	{
				//		HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
				//		var resultAdd = responseAdd.Content.ReadAsStringAsync();
				//		if (responseAdd.IsSuccessStatusCode)
				//		{
				//			string message = "Saved Successfully" + Environment.NewLine + " Once verified an Email will be send with your login credentials";
				//			return Content(CommonHelper.ShowNotification(true, message));
				//		}
				//		else
				//		{
				//			return Content(resultAdd.Result);
				//		}
				//	}
				//	else//Update
				//	{
				//		return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Someone with this name already exists")));
				//	}
			//}
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}

		}

		#endregion

		[HttpPost]
		public ActionResult GetProductionData(string period, string prodtype, string fishtype, string farmid, string year)
		{
			try
			{
				if (Session != null && Session["User"] != null)
				{
					User user = (User)Session["User"];
					List<MultiBarItem> resultData = new List<MultiBarItem>();
					List <MultiBarItem> returnData = new List<MultiBarItem>();
					DashboardRepository repo = new DashboardRepository();
					if (user.UserRoleID == 5) //Farmer
					{
						resultData = repo.GetProductionData(user.OrgID, period, prodtype, fishtype, user.UserID.ToString(), farmid, year);
					}
					else
					{
						resultData = repo.GetProductionData(user.OrgID, period, prodtype, fishtype, "0", farmid, year);
					}

					//if (searchTxt != null)
					//{
					//	if (!(bool)chkTitles)
					//	{
					//		resultData = resultData.Where(o => o.DiscussionTitle.Contains(searchTxt)).ToList();
					//	}
					//	else
					//	{
					//		resultData = resultData.Where(o => o.DiscussionTitle.Contains(searchTxt) || o.Message.Contains(searchTxt)).ToList();
					//	}
					//}
					//IEnumerable<IGrouping<string, Forum>> discussionsList = resultData.OrderBy(n => n.CreateDateTime).GroupBy(o => o.DiscussionIdentity.ToString());
					//foreach (IGrouping<string, Forum> discussion in discussionsList)
					//{
					//	Forum forum = discussion.FirstOrDefault();
					//	forum.DiscussionMsgCount = discussion.Count().ToString();
					//	returnData.Add(forum);
					//}
					

					return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return View("Home", "Home");
				}
			}
			catch (Exception ex)
			{
				return Json(new { result = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}
		[HttpPost]
		public ActionResult GetHorizontalBarProdData(string period, string prodtype, string fishtype, string farmid, string year)
		{
			try
			{
				if (Session != null && Session["User"] != null)
				{
					User user = (User)Session["User"];
					List<BarHorizontalItem> resultData = new List<BarHorizontalItem>();
				
					DashboardRepository repo = new DashboardRepository();
					if (user.UserRoleID == 5) //Farmer
					{
						resultData = repo.GetBarHorizontalProdData(user.OrgID, prodtype, fishtype, user.UserID.ToString(), farmid, year);
					}
					else
					{
						resultData = repo.GetBarHorizontalProdData(user.OrgID, prodtype, fishtype, "0", farmid, year);
					}
					return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return View("Home", "Home");
				}
			}
			catch (Exception ex)
			{
				return Json(new { result = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}
		[HttpPost]
		public ActionResult GetGridProductionData(string period, string type)
		{
			try
			{
				if (Session != null && Session["User"] != null)
				{
					User user = (User)Session["User"];

					List<GridChartItem> returnData = new List<GridChartItem>();
					DashboardRepository repo = new DashboardRepository();
					List<GridChartItem> resultData = repo.GetGridProductionData(user.OrgID, period, type);
					//if (searchTxt != null)
					//{
					//	if (!(bool)chkTitles)
					//	{
					//		resultData = resultData.Where(o => o.DiscussionTitle.Contains(searchTxt)).ToList();
					//	}
					//	else
					//	{
					//		resultData = resultData.Where(o => o.DiscussionTitle.Contains(searchTxt) || o.Message.Contains(searchTxt)).ToList();
					//	}
					//}
					//IEnumerable<IGrouping<string, Forum>> discussionsList = resultData.OrderBy(n => n.CreateDateTime).GroupBy(o => o.DiscussionIdentity.ToString());
					int count = 1;
					foreach (GridChartItem item in resultData)
					{
						GridChartItem it = new GridChartItem();
						it.Region = item.Region;
						it.Jan = item.Jan + 7 + count;
						it.Feb = item.Feb +3 +count;
						it.Mar = item.Mar +5 +count;
						it.Apr = item.Apr +7 +count;
						it.May = item.May +2 +count;
						it.Jun = item.Jun +7 +count;
						it.Jul = item.Jul + 9+count;
						it.Aug = item.Aug +11 +count;
						it.Sep = item.Sep +14 +count;
						it.Oct = item.Oct +2 +count;
						it.Nov = item.Nov +8 +count;
						it.Dec = item.Dec +10 +count;
						it.Total = it.Jan + it.Feb + it.Mar + it.Apr + it.May + it.Jun + it.Jul + it.Aug + it.Sep + it.Oct + it.Nov + it.Dec;
						count++;
						returnData.Add(it);
					}


					return Json(new { result = returnData }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return View("Home", "Home");
				}
			}
			catch (Exception ex)
			{
				return Json(new { result = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public ActionResult GetFarmersData(string period, string region)
		{
			try
			{
				if (Session != null && Session["User"] != null)
				{
					User user = (User)Session["User"];

					List<MultiBarItem> returnData = new List<MultiBarItem>();
					DashboardRepository repo = new DashboardRepository();
					List<MultiBarItem> resultData = repo.GetFarmersData(user.OrgID, period, region);
					//if (searchTxt != null)
					//{
					//	if (!(bool)chkTitles)
					//	{
					//		resultData = resultData.Where(o => o.DiscussionTitle.Contains(searchTxt)).ToList();
					//	}
					//	else
					//	{
					//		resultData = resultData.Where(o => o.DiscussionTitle.Contains(searchTxt) || o.Message.Contains(searchTxt)).ToList();
					//	}
					//}
					//IEnumerable<IGrouping<string, Forum>> discussionsList = resultData.OrderBy(n => n.CreateDateTime).GroupBy(o => o.DiscussionIdentity.ToString());
					//foreach (IGrouping<string, Forum> discussion in discussionsList)
					//{
					//	Forum forum = discussion.FirstOrDefault();
					//	forum.DiscussionMsgCount = discussion.Count().ToString();
					//	returnData.Add(forum);
					//}


					return Json(new { result = resultData }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return View("Home", "Home");
				}
			}
			catch (Exception ex)
			{
				return Json(new { result = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult GetIconData(string iconType)
		{
			try
			{
				User user = null;
				HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
				if (session["User"] == null)
				{
					return null;
				}
				else
				{
					user = (User)session["User"];
					if(user.StpDepartmentID != 96 && user.StpDepartmentID != 2404)//Management/Champions and Ext Officers only
					{
						return null;
					}
					
				}
				
					GridResult<SelectResult> dataList = new GridResult<SelectResult>();

					string field = string.Empty; string table = string.Empty; string where = ""; string orderby = ""; string direction = "ASC";
					if (iconType.Equals("farmers", StringComparison.OrdinalIgnoreCase))
					{
						field = "Count(PersonID) as ID,\'\' as Description";
						table = "Person";
						where = " StpPersonTypeID = 122 AND IsActive = 0 ";
						orderby = "ID";
					}
					else if (iconType.Equals("suppliers", StringComparison.OrdinalIgnoreCase))
					{
						field = "Count(SupplierID) as ID,\'\' as Description";
						table = "Supplier";
						where = " IsActive = 0 ";
					orderby = "ID";
					}
					else if (iconType.Equals("officers", StringComparison.OrdinalIgnoreCase))
					{
						field = "Count(UserID) as ID,\'\' as Description";
						table = "User";
						where = " StpDepartmentID = 2404 AND IsActive = 0 ";
						orderby = "ID";
					}

					if (field.Length > 0 && table.Length > 0)
					{
						string uri = CommonHelper.BaseUri + "AdminController/GetSelectList";
						////Filter by users org Note only display data with in users organization
						where = where + " and OrgID = " + ((User)session["User"]).OrgID.ToString();
						SelectQuery qry = new SelectQuery() { fields = field, table = table, where = where, orderby = orderby, direction = direction };
					BaseRepository repo = new BaseRepository();
					dataList.Items = repo.GetData(qry.GetQuery());
					//using (HttpClient httpClient = new HttpClient())
					//{
					//	httpClient.DefaultRequestHeaders.Accept.Clear();
					//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//	StringContent content = new StringContent(JsonConvert.SerializeObject(qry), Encoding.UTF8, "application/json");
					//	HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
					//	var result = response.Content.ReadAsStringAsync();
					//	if (response.IsSuccessStatusCode)
					//	{
					//		var settings = new JsonSerializerSettings
					//		{
					//			NullValueHandling = NullValueHandling.Ignore,
					//			MissingMemberHandling = MissingMemberHandling.Ignore
					//		};
					//		dataList = JsonConvert.DeserializeObject<GridResult<SelectResult>>(result.Result, settings);
					//	}
					//	else
					//	{
					//		return Json(new { result = result.Result }, JsonRequestBehavior.AllowGet);
					//	}
					//}

					}
					return Json(new { result = dataList.Items }, JsonRequestBehavior.AllowGet);
				

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public ActionResult GetStpDataList(string orgID,string dataCode,string stpDataType)
		{
			//string uri = CommonHelper.BaseUri + "AdminController/StpData";
			try
			{
				GridResult<StpData> result = new GridResult<StpData>();
				BaseRepository repo = new BaseRepository();
				result.Items = repo.GetStpData(orgID, dataCode, stpDataType);
				//using (HttpClient httpClient = new HttpClient())
				//{
				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				//	GridParam gridParams = new GridParam();
				//	gridParams.PageNo = 0;
				//	gridParams.PageSize = 100;

				//	// gridParams.IncludeForeignKeyValues = true;
				//	gridParams.Includerelations = true;
				//	gridParams.ListOrderBy = new System.Collections.Generic.List<FilterField>();
				//	gridParams.ListOrderBy.Add(new FilterField() { Property = "DataDescription", Operator = "=", Value = "Asc" });
				//	gridParams.ListFilterBy = new System.Collections.Generic.List<FilterField>();
				//	if (orgID.Length > 0)
				//	{
				//		gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = orgID });
				//	}
				//	if (stpDataType.Length > 0)
				//	{
				//		gridParams.ListFilterBy.Add(new FilterField() { Property = "StpDataTypeID", Operator = "=", Value = stpDataType });
				//	}
				//	if (dataCode.Length > 0)
				//	{
				//		gridParams.ListFilterBy.Add(new FilterField() { Property = "DataCode", Operator = "=", Value = dataCode });
				//	}
				//	StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
				//	HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
				//	if (response.IsSuccessStatusCode)
				//	{
				//		var jsonString = response.Content.ReadAsStringAsync();
				//		jsonString.Wait();
				//		GridResult<StpData> result = JsonConvert.DeserializeObject<GridResult<StpData>>(jsonString.Result);

				//		List<StpData> retList = new List<DALEFModel.StpData>();
				//		foreach (StpData item in result.Items)
				//		{
				//			retList.Add(item);
				//		}
				//		//return JsonConvert.SerializeObject(new SelectList(retList, "StpDataID", "DataDescription", 0));
						return Json(new { result = result.Items }, JsonRequestBehavior.AllowGet);

				//	}
				//	else
				//	{
				//		var readAsStringAsync = response.Content.ReadAsStringAsync();
				//		return Content(readAsStringAsync.Result);
				//	}
				//}
			}
			catch (System.Exception ex)
			{
				return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
		}

		[HttpPost]
		public ActionResult SendContactUsMessage(ContactUs form)
		{
			try
			{
				string adminEmails = ConfigurationManager.AppSettings["AdminEmail"];
				Common.Common.SendEmail("info@buna.africa", adminEmails, "Contact Us meesage from Buna africa", "Contact us request from " + form.Firstname + " " + form.Surname + " Email : " + form.Email+ " Message = " + form.Message);
				return Content(CommonHelper.ShowNotification(true, "Your message was send successfully. We will contact you as soon as possible."));
			}
			catch(Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).ExceptionMessage));
			}
		}
	}
}
