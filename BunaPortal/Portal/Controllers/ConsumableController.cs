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
using CyroTechPortal.HTMLHelpers;
using System.Globalization;

namespace CyroTechPortal
{

	public class ConsumableController : BaseController
	{


		#region Consumable Methods

		/// <summary>
		/// Consumable this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult Consumable(int consumableTypeID = -1)
		{
			ViewBag.Type = consumableTypeID;
			if (consumableTypeID > 0)
			{
				@ViewBag.ShowFilter = false;
			}
			return View();
		}

			
		/// <summary>
		/// Gets the Consumable list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetConsumableList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "ConsumableController/Consumable";
			jQueryDataTablesModel.uri = uri;
			
			try
			{

				using (HttpClient httpClient = new HttpClient())
				{
					HttpResponseMessage response = GetGridData(jQueryDataTablesModel, true);
					if (response.IsSuccessStatusCode)
					{

						var jsonString = response.Content.ReadAsStringAsync();
						jsonString.Wait();
						GridResult<Consumable> result = JsonConvert.DeserializeObject<GridResult<Consumable>>(jsonString.Result);
						List<Consumable> retList = new List<Consumable>();
						foreach (Consumable item in result.Items)
						{

							if (item.StpData != null)
							{
							item.ConsumableTypeDesc = item.StpData.DataDescription;
							item.StpData = null;
							}
							if (item.StpData1 != null)
							{
							item.UnitOfMeasureDesc = item.StpData1.DataDescription;
							item.StpData1 = null;
							}
							if (item.Asset1 != null)
							{
							item.UsedOnAssetDesc = item.Asset1.Name;
							item.Asset1 = null;
							}
							if (item.Asset != null)
							{
							item.AssetDesc = item.Asset.Name;
							item.Asset = null;
							}
							if (item.StpData2 != null)
							{
							item.UOMForOpenAndCloseDesc = item.StpData2.DataDescription;
							item.StpData2 = null;
							}
							if (item.User != null)
							{
							item.CreatedByDesc = item.User.FullName;
							item.User = null;
							}
							if (item.User1 != null)
							{
							item.ChangedByDesc = item.User1.FullName;
							item.User1= null;
							}
							item.Organization = null;
							item.Asset1 = null;
						
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.ConsumableID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.ConsumableID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
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
		/// <param name="ConsumableID">The Consumable identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetConsumable(int? id,int? type)
		{

			try
				{

					
				DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "ConsumableController/Consumable";
					if (id == null)
					{
						Consumable newItem = new Consumable();
						if(type > 0)
						{
							if(type == 116 || type == 133 || type == 133) //Diesel or Water
							{
								newItem.StpUnitOfMeasureID = 128;
							}
							else if(type == 117 || type == 118 || type == 119 || type == 134) //HpFeed or Lp Feed
							{
								newItem.StpUnitOfMeasureID = 129;
							}
							
						newItem.StpConsumableTypeID = type;
							
						}
						newItem.CreateDateTime = SATime;
						newItem.CreatedByID = ((User)Session["User"]).UserID;
						newItem.OrgID = ((User)Session["User"]).OrgID;
						newItem.IsActive = true;
						return PartialView("EditConsumable", newItem);
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
							Consumable item = JsonConvert.DeserializeObject<Consumable>(content.Result,settings);
							item.ChangeDateTime = SATime;
							item.CreatedByID = ((User)Session["User"]).UserID;
							item.OrgID = ((User)Session["User"]).OrgID;
							//if (item.QuantityInUOM > 0)
							//{
							//	item.QuantityInUOM = decimal.Parse(Convert.ToDecimal(item.QuantityInUOM).ToString("##.00", CultureInfo.GetCultureInfo("de-DE")), NumberStyles.Number);
							//}
							return PartialView("EditConsumable", item);
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
		/// <param name="Consumable">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditConsumable(Consumable newItem)
		{

			try
				{

				string uri = CommonHelper.BaseUri + "ConsumableController/Consumable";
				string uriAdd = CommonHelper.BaseUri + "ConsumableController/Consumable/add";
				string uriUpdate = CommonHelper.BaseUri + "ConsumableController/Consumable/update";
				Consumable itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.ConsumableID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<Consumable>(content.Result,settings);
					}
					
					//Make quantity negative when its not a purchase 
					newItem.QuantityInUOM = newItem.IsPurchase == true ? System.Math.Abs(newItem.QuantityInUOM??0) : decimal.Negate(newItem.QuantityInUOM ?? 0);
					
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
		/// Deletes the Consumable.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteConsumable(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "ConsumableController/Consumable/delete";
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
			exportdata.Controller ="Consumable";
			exportdata.Entity ="Consumable";
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
				FileContentResult file = base.ExportBase<Consumable>(exportDetails);
				if (file != null)
				{
				return file;
				}
				else
				{
					//return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(new Exception()).CreateDetailNoHtml()));
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
