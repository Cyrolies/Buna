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
using CyroTechPortal.HTMLHelpers;

namespace CyroTechPortal
{

	public class StockTakeController : BaseController
	{


		#region StockTake Methods

		/// <summary>
		/// StockTake this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult StockTake()
		{
			return View();
		}

		/// <summary>
		/// Gets the StockTake list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetStockTakeList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "StockTakeController/StockTake";
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
						GridResult<StockTake> result = JsonConvert.DeserializeObject<GridResult<StockTake>>(jsonString.Result);
						List<StockTake> retList = new List<DALEFModel.StockTake>();
						foreach (StockTake item in result.Items)
						{

							if (item.Part != null)
							{
								item.PartDesc = item.Part.Code + " - " + item.Part.Description + " - SN = " + item.Part.SerialNo;
							}
							item.Part = null;
							if (item.User != null)
							{
								item.CreatedByDesc = item.User.FullName;
							}
							item.User = null;

							if (item.Organization != null)
							{
								item.OrganizationName = item.Organization.OrganizationName;
							}
							item.Organization = null;

							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.StockTakeID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.StockTakeID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
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
						return Content(CommonHelper.ShowNotification(false, readAsStringAsync.Result));
					}

				}
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// <param name="StockTakeID">The StockTake identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetStockTake(int? id)
		{

			try
				{
					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "StockTakeController/StockTake";
					if (id == null)
					{
						return PartialView("EditStockTake", new StockTake()
						{
							CreateDateTime = SATime,
							CreatedByID = ((User)Session["User"]).UserID,
							OrgID = ((User)Session["User"]).OrgID,
							IsActive = true,
						});
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
							StockTake item = JsonConvert.DeserializeObject<StockTake>(content.Result,settings);
							item.ChangeDateTime = SATime;
							return PartialView("EditStockTake", item);
						}
					else
					{
							return Content(CommonHelper.ShowNotification(false, content.Result));
						}

					}
				}
				catch (System.Exception ex)
				{
					return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
				}

			}

		/// <summary>
		/// <param name="StockTake">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditStockTake(StockTake newItem)
		{

			try
				{
				
				string uri = CommonHelper.BaseUri + "StockTakeController/StockTake";
				string uriAdd = CommonHelper.BaseUri + "StockTakeController/StockTake/add";
				string uriUpdate = CommonHelper.BaseUri + "StockTakeController/StockTake/update";
				StockTake itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.StockTakeID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<StockTake>(content.Result,settings);
					}
					
					//Insert
					if (itemExists == null)
					{
						
						StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
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
						StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
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
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// Deletes the StockTake.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteStockTake(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "StockTakeController/StockTake/delete";
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
						return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Deleted")));
					}
					else
					{
						return Content(CommonHelper.ShowNotification(false, content.Result));
					}
				}
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
			exportdata.Controller ="StockTake";
			exportdata.Entity ="StockTake";
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
				FileContentResult file = base.ExportBase<StockTake>(exportDetails);
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
