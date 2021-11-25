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

namespace CyroTechPortal
{

	public class PersonController : BaseController
	{


		#region Person Methods

		/// <summary>
		/// Person this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult Person()
		{
			return View();
		}

		/// <summary>
		/// Gets the Person list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetPersonList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			string uri = CommonHelper.BaseUri + "PersonController/Person";
			jQueryDataTablesModel.uri = uri;
			try
			{

					HttpResponseMessage response = GetGridData(jQueryDataTablesModel, true);
					if (response.IsSuccessStatusCode)
					{

						var jsonString = response.Content.ReadAsStringAsync();
						jsonString.Wait();
						GridResult<Person> result = JsonConvert.DeserializeObject<GridResult<Person>>(jsonString.Result);
						List<Person> retList = new List<Person>();
						foreach (Person item in result.Items)
						{

							if (item.StpData != null)
							{
							item.TitleDesc = item.StpData.DataDescription;
							item.StpData = null;
							}
							if (item.StpData1 != null)
							{
							item.PersonTypeDesc = item.StpData1.DataDescription;
							item.StpData1 = null;
							}
							if (item.StpData2 != null)
							{
							item.PersonCategoryDesc = item.StpData2.DataDescription;
							item.StpData2 = null;
							}
							item.Organization = null;
							item.User = null;
							item.User1 = null;
						item.StcData = null;
						item.Asset = null;
							item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.PersonID +"' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";
							item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.PersonID +"' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
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
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// <param name="PersonID">The Person identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetPerson(int? id)
		{

			try
				{

					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					string uri = CommonHelper.BaseUri + "PersonController/Person";
					if (id == null)
					{
						return PartialView("EditPerson", new Person() 
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
							Person item = JsonConvert.DeserializeObject<Person>(content.Result,settings);
							item.ChangeDateTime = SATime;
							item.CreatedByID = ((User)Session["User"]).UserID;
							item.OrgID = ((User)Session["User"]).OrgID;
							return PartialView("EditPerson", item);
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
		/// <param name="Person">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditPerson(Person newItem)
		{

			try
				{

				string uri = CommonHelper.BaseUri + "PersonController/Person";
				string uriAdd = CommonHelper.BaseUri + "PersonController/Person/add";
				string uriUpdate = CommonHelper.BaseUri + "PersonController/Person/update";
				Person itemExists = null;
				using (HttpClient httpClient = new HttpClient())
				{

					httpClient.DefaultRequestHeaders.Accept.Clear();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					//Check exists
					HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.PersonID+ "/false").Result;
					var content = response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
							var settings = new JsonSerializerSettings
							{
							NullValueHandling = NullValueHandling.Ignore,
							MissingMemberHandling = MissingMemberHandling.Ignore
							};
						itemExists = JsonConvert.DeserializeObject<Person>(content.Result,settings);
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
		/// Deletes the Person.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeletePerson(int? id)
		{
			try
			{

				string uri = CommonHelper.BaseUri + "PersonController/Person/delete";
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
			exportdata.Controller ="Person";
			exportdata.Entity ="Person";
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
				FileContentResult file = base.ExportBase<Person>(exportDetails);
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
