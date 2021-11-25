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
	public class HitecJobController : BaseController
	{


		#region HitecJob Methods

		/// <summary>
		/// HitecJob this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult HitecJob()
		{
			return View();
		}

		/// <summary>
		/// Gets the HitecJob list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetHitecJobList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "HitecJobController/HitecJob";
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
						GridResult<HitecJob> result = JsonConvert.DeserializeObject<GridResult<HitecJob>>(jsonString.Result);
						List<HitecJob> retList = new List<DALEFModel.HitecJob>();
						foreach (HitecJob item in result.Items)
						{
							if (item.User != null)
							{
								item.CreatedByDesc = item.User.FullName;
							}
							item.User = null;

							if (item.User1 != null)
							{
								item.ChangedByDesc = item.User1.FullName;
							}
							item.User1 = null;

							if (item.Organization != null)
							{
								item.OrganizationName = item.Organization.OrganizationName;
							}
							item.Organization = null;

						
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.HitecJobID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.HitecJobID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
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
		/// <param name="HitecJobID">The HitecJob identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetHitecJob(int? id)
		{

			try
				{
					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "HitecJobController/HitecJob";
					if (id == null)
					{
						return PartialView("EditHitecJob", new HitecJob()
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
							HitecJob item = JsonConvert.DeserializeObject<HitecJob>(content.Result,settings);
							item.ChangeDateTime = SATime;
							item.ChangedByID = ((User)Session["User"]).UserID;
							
							return PartialView("EditHitecJob", item);
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
		/// <param name="HitecJob">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditHitecJob(HitecJob newItem)
		{

			try
				{
				
				string uri = CommonHelper.BaseUri + "HitecJobController/HitecJob";
				string uriAdd = CommonHelper.BaseUri + "HitecJobController/HitecJob/add";
				string uriUpdate = CommonHelper.BaseUri + "HitecJobController/HitecJob/update";
				HitecJob itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.HitecJobID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<HitecJob>(content.Result,settings);
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
		/// Deletes the HitecJob.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteHitecJob(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "HitecJobController/HitecJob/delete";
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
			exportdata.Controller ="HitecJob";
			exportdata.Entity ="HitecJob";
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
				FileContentResult file = base.ExportBase<HitecJob>(exportDetails);
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
