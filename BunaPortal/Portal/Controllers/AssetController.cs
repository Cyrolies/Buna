using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;
using System.Web.Routing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
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

	public class AssetController : BaseController
	{


		#region Asset Methods

		/// <summary>
		/// Asset this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult Asset()
		{
			return View();
		}

		[HttpGet]
		public ViewResult AssetLocations()
		{
			return View();
		}

		#region Dapper
		[HttpGet]
		public ActionResult GetFarmLocations()
		{
			try
			{
				if (Session != null && Session["User"] != null)
				{
					User user = (User)Session["User"];

					List<Asset> returnData = new List<Asset>();
					AssetRepository repo = new AssetRepository();
					List<Asset> resultData = repo.GetFarmLocations(user.OrgID);
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
					return View();
				}
			}
			catch (Exception ex)
			{
				return Json(new { result = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}
		#endregion



		/// <summary>
		/// Gets the Asset list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetAssetList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			//string uri = CommonHelper.BaseUri + "AssetController/Asset";
			//jQueryDataTablesModel.uri = uri;
			try
			{

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
					User user = (User)Session["User"];
					
					if (user.UserRoleID == 5)//Its a Farmer thats logged in then only show his farms
					{
						if (user.Person2 != null) //Linked to supplier entry
						{
							gridParams.ListFilterBy.Add(new FilterField() { Property = "PersonID", Operator = "=", Value = user.FarmerID });
						}
					}
								
					//Always filter by orgid
					gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });

				}
				else
				{
					RedirectToAction("Login", "Account");
				}

				List<Asset> retList = new List<Asset>();
				AssetRepository repo = new AssetRepository();
				GridResult<Asset> result = repo.GetFarmList(gridParams);
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
				//HttpResponseMessage response = GetGridData(jQueryDataTablesModel, true);
				//if (response.IsSuccessStatusCode)
				//	{

				//		var jsonString = response.Content.ReadAsStringAsync();
				//		jsonString.Wait();
				//		GridResult<Asset> result = JsonConvert.DeserializeObject<GridResult<Asset>>(jsonString.Result);
				//		List<Asset> retList = new List<Asset>();
				//		bool isFarmer = false;
				//if (Session != null && Session["User"] != null)
				//{
				//	User user = (User)Session["User"];
				//	if(user.UserRoleID == 5)
				//	{
				//		isFarmer = true;
				//	}
				//}
				foreach (Asset item in result.Items)
						{

							//if (item.StpData != null)
							//{
							//	item.AssetCategoryDesc = item.StpData.DataDescription;
							//	item.StpData = null;
							//}
							//if (item.Person != null)
							//{
							//	item.PersonDesc = item.Person.Fullname;
							//	item.Person = null;
							//}
							item.IsActiveCheckBox = item.IsActive == true ? " <span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
						
							//item.Asset1 = null;
							//item.Organization = null;
							//item.User = null;
							//item.User1 = null;

							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.AssetID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							//if (!isFarmer)
							//{
								item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.AssetID + "' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
							//}
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
					//	return Content(CommonHelper.ShowNotification(false, readAsStringAsync.Result));
					//}

				//}
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// <param name="AssetID">The Asset identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetAsset(int? id)
		{

			try
				{

					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "AssetController/Asset";
					
					if (id == null)
					{
					
					return PartialView("EditAsset", new Asset() 
						{
							CreateDateTime = SATime,
							CreatedByID = ((User)Session["User"]).UserID,
							OrgID = ((User)Session["User"]).OrgID,
							PersonID = Convert.ToInt32(((User)Session["User"]).FarmerID),
								
						});
					}
				//using (HttpClient httpClient = new HttpClient())
				//{
				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//	HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
				//	var content = response.Content.ReadAsStringAsync();
				//	if (response.IsSuccessStatusCode)
				//{
				//		var settings = new JsonSerializerSettings
				//		{
				//		NullValueHandling = NullValueHandling.Ignore,
				//		MissingMemberHandling = MissingMemberHandling.Ignore
				//		};
				//		Asset item = JsonConvert.DeserializeObject<Asset>(content.Result,settings);
							AssetRepository repo = new AssetRepository();
							Asset item = repo.GetFarm(id.ToString());
							item.ChangeDateTime = SATime;
							item.CreatedByID = ((User)Session["User"]).UserID;
							item.OrgID = ((User)Session["User"]).OrgID;
							return PartialView("EditAsset", item);
					//	}
					//else
					//{
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
		/// <param name="Asset">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditAsset(Asset newItem)
		{

			try
				{
				AssetRepository repo = new AssetRepository();
				if(newItem.AssetID == 0)
				{
					repo.Add(newItem);
					return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Added")));
				}
				else
				{
					repo.Update(newItem);
					return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Updated")));
				}
				//string uri = CommonHelper.BaseUri + "AssetController/Asset";
				//string uriAdd = CommonHelper.BaseUri + "AssetController/Asset/add";
				//string uriUpdate = CommonHelper.BaseUri + "AssetController/Asset/update";
				//Asset itemExists = null;
				//using (HttpClient httpClient = new HttpClient())
				//{

				//	httpClient.DefaultRequestHeaders.Accept.Clear();
				//	httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//	//Check exists
				//	HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.AssetID+ "/false").Result;
				//	var content = response.Content.ReadAsStringAsync();
				//	if (response.IsSuccessStatusCode)
				//	{
				//			var settings = new JsonSerializerSettings
				//			{
				//			NullValueHandling = NullValueHandling.Ignore,
				//			MissingMemberHandling = MissingMemberHandling.Ignore
				//			};
				//		itemExists = JsonConvert.DeserializeObject<Asset>(content.Result,settings);
				//	}
				//	StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
				//	//Insert
				//	if (itemExists == null)
				//	{
				//		HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
				//		var resultAdd = responseAdd.Content.ReadAsStringAsync();
				//		if (responseAdd.IsSuccessStatusCode)
				//		{
				//			return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Added")));
				//		}
				//		else
				//		{
				//			return Content(CommonHelper.ShowNotification(false, resultAdd.Result));
				//		}
				//	}
				//	else//Update
				//	{
				//		HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
				//		var resultOut = responseOut.Content.ReadAsStringAsync();
				//		if (responseOut.IsSuccessStatusCode)
				//		{
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
		/// Deletes the Asset.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteAsset(int id)
		{
			try
			{
				AssetRepository repo = new AssetRepository();
				repo.Delete(id.ToString());
				return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Deleted")));
				//string uri = CommonHelper.BaseUri + "AssetController/Asset/delete";
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
		/// <summary>
		/// <param name="dtParams">The data list.</param>
		/// <returns></returns>
		public ActionResult ExportForm(string dataList)
		{
			try
			{
			Export exportdata = new Export();
			exportdata.Controller ="Asset";
			exportdata.Entity ="Asset";
			exportdata.DatatableParams = dataList;
			return PartialView("ExportControl", exportdata);
			}
			catch (System.Exception ex)
			{
			return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}
		/// <summary>
		/// Exports the data.
		/// <summary>
		/// <param name="exportData">The export data.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">
		/// </exception>
		[HttpPost]
		public FileResult ExportData(Export exportDetails)
		{
			try
			{
				FileContentResult file = base.ExportBase<Asset>(exportDetails);
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
