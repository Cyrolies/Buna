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

	public class WorkOrderController : BaseController
	{


		#region WorkOrder Methods

		/// <summary>
		/// WorkOrder this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult WorkOrder()
		{
			return View();
		}

		/// <summary>
		/// Gets the WorkOrder list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetWorkOrderList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "WorkOrderController/WorkOrder";
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
						GridResult<WorkOrder> result = JsonConvert.DeserializeObject<GridResult<WorkOrder>>(jsonString.Result);
						List<WorkOrder> retList = new List<DALEFModel.WorkOrder>();
						foreach (WorkOrder item in result.Items)
						{

							if (item.StpData != null)
							{
							item.WorkOrderTypeDesc = item.StpData.DataDescription;
							}
							item.StpData = null;
							if (item.StpData1 != null)
							{
								item.ClientDesc = item.StpData1.DataDescription;
							}
							item.StpData1 = null;
							if (item.User1 != null)
							{
							item.TechnicianDesc = item.User1.FullName;
							}
							item.User1 = null;
							
							if (item.User != null)
							{
							item.CreatedByDesc =  item.User.FullName;
							}
							item.User = null;

							if (item.Organization != null)
							{
								item.OrganizationName = item.Organization.OrganizationName;
							}
							item.Organization = null;

							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.WorkOrderID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.WorkOrderID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
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
		/// <param name="WorkOrderID">The WorkOrder identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetWorkOrder(int? id)
		{

			try
				{
					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "WorkOrderController/WorkOrder";
					if (id == null)
					{
						return PartialView("EditWorkOrder", new WorkOrder()
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
							WorkOrder item = JsonConvert.DeserializeObject<WorkOrder>(content.Result,settings);
							item.ChangeDateTime = SATime;
							return PartialView("EditWorkOrder", item);
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
		/// <param name="WorkOrder">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditWorkOrder(WorkOrder newItem)
		{

			try
				{

				string uri = CommonHelper.BaseUri + "WorkOrderController/WorkOrder";
				string uriAdd = CommonHelper.BaseUri + "WorkOrderController/WorkOrder/add";
				string uriUpdate = CommonHelper.BaseUri + "WorkOrderController/WorkOrder/update";
				WorkOrder itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.WorkOrderID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<WorkOrder>(content.Result,settings);
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
		/// Deletes the WorkOrder.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeleteWorkOrder(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "WorkOrderController/WorkOrder/delete";
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
			exportdata.Controller ="WorkOrder";
			exportdata.Entity ="WorkOrder";
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
				FileContentResult file = base.ExportBase<WorkOrder>(exportDetails);
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
