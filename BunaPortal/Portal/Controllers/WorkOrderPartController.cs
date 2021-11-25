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

	public class WorkOrderPartController : BaseController
	{


		#region WorkOrderPart Methods

		/// <summary>
		/// WorkOrderPart this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult WorkOrderPart()
		{
			return View();
		}

		/// <summary>
		/// Gets the WorkOrderPart list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetWorkOrderPartList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "WorkOrderPartController/WorkOrderPart";
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
						GridResult<WorkOrderPart> result = JsonConvert.DeserializeObject<GridResult<WorkOrderPart>>(jsonString.Result);
						List<WorkOrderPart> retList = new List<DALEFModel.WorkOrderPart>();
						foreach (WorkOrderPart item in result.Items)
						{

							if (item.Part != null)
							{
								item.PartDesc = item.Part.Code + " - " + item.Part.Description + " - SN = "  +item.Part.SerialNo;
							}
							item.Part = null;
							if (item.WorkOrder != null)
							{
								item.WorkOrderDesc = item.WorkOrder.WorkOrderCode +" - "+ item.WorkOrder.TechnicianDesc;
							}
							item.WorkOrder = null;

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
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.WorkOrderPartID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.WorkOrderPartID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
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
		/// <param name="WorkOrderPartID">The WorkOrderPart identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetWorkOrderPart(int? id)
		{

			try
				{
					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "WorkOrderPartController/WorkOrderPart";
					if (id == null)
					{
					return PartialView("EditWorkOrderPart", new WorkOrderPart()
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
							WorkOrderPart item = JsonConvert.DeserializeObject<WorkOrderPart>(content.Result,settings);
							item.ChangeDateTime = SATime;
							return PartialView("EditWorkOrderPart", item);
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
		/// <param name="WorkOrderPart">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditWorkOrderPart(WorkOrderPart newItem)
		{

			try
				{

				string uri = CommonHelper.BaseUri + "WorkOrderPartController/WorkOrderPart";
				string uriAdd = CommonHelper.BaseUri + "WorkOrderPartController/WorkOrderPart/add";
				string uriUpdate = CommonHelper.BaseUri + "WorkOrderPartController/WorkOrderPart/update";
				WorkOrderPart itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.WorkOrderPartID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<WorkOrderPart>(content.Result,settings);
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
		/// Deletes the WorkOrderPart.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteWorkOrderPart(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "WorkOrderPartController/WorkOrderPart/delete";
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
			exportdata.Controller ="WorkOrderPart";
			exportdata.Entity ="WorkOrderPart";
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
				FileContentResult file = base.ExportBase<WorkOrderPart>(exportDetails);
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
