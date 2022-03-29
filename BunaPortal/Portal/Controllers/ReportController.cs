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
using DALEFModel.GraphModel;
using BunaPortal.HTMLHelpers;

namespace BunaPortal
{

	public class ReportController : BaseController
	{


		#region ReportController Methods

		/// <summary>
		/// vwStockReport this instance.
		/// <summary>
		/// <returns></returns>
		//[HttpGet]
		//public ViewResult StockReport()
		//{
		//	return View();
		//}

		/// <summary>
		/// Gets the GetHitecJobGanttData list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		//public ActionResult GetHitecJobGanttData()
		//{
		//	string uri = CommonHelper.BaseUri + "ReportController/GetHitecJobGanttData";
		//	try
		//	{
		//		List<GanttItem> retList = new List<GanttItem>();
		//		using (HttpClient httpClient = new HttpClient())
		//		{
		//			httpClient.DefaultRequestHeaders.Accept.Clear();
		//			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		//			HttpResponseMessage response = httpClient.GetAsync(uri).Result;
		//			if (response.IsSuccessStatusCode)
		//			{

		//				var jsonString = response.Content.ReadAsStringAsync();
		//				jsonString.Wait();
		//				List<HitecJob> result = JsonConvert.DeserializeObject<List<HitecJob>>(jsonString.Result);
						
		//				foreach (HitecJob job in result)
		//				{
		//					GanttItem ganttItem = new GanttItem();
		//				//	ganttItem.url = job.StpCityID.ToString(); 
		//					ganttItem.id = job.HitecJobID.ToString();
		//					ganttItem.title = job.Client + "-" +job.Fullname;
		//					DateTime currentDate = DateTime.Now;
		//					if (currentDate < job.StartDate)
		//					{
		//						ganttItem.status = "0";//NotStarted - Blue
		//					}
		//					else if (job.IsInvoiced)
		//					{
		//						ganttItem.status = "2";//invoiced - Yellow
		//					}
		//					else if (job.IsComplete)
		//					{
		//						ganttItem.status = "1";//Complete - Red
		//					}
							
		//					else
		//					{
		//						ganttItem.status = "3"; //InComplete - Green
		//					}

		//					if (job.StartDate != null)
		//					{
		//						ganttItem.startdate = job.StartDate.Value.DateTime.ToString("yyyy/MM/dd");//"Date(" + ((Int32)(job.StartDate.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString() + ")";
		//					}
		//					if (job.EndDate != null)
		//					{
		//						ganttItem.enddate = job.EndDate.Value.DateTime.ToString("yyyy/MM/dd");// "Date(" + ((Int32)(job.EndDate.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString() + ")";
		//					}
		//					else // //if enddate is not specified then keep setting it to 14 days ahead
		//					{
		//						ganttItem.enddate = DateTime.Now.AddDays(14).ToString("yyyy/MM/dd");
		//					}
		//					StringBuilder displayStr = new StringBuilder();
		//					displayStr.Append("<b>Client :</b>" + job.Client + "<br>");
		//					displayStr.Append("<b>Fullname : </b>" + job.Fullname + "<br>");
		//					displayStr.Append("<b>Address :</b>" + "<br>");
		//					displayStr.Append("<span class=\"desc\">" +job.PhysicalAddressGuardPost + "</span><br>");
		//					displayStr.Append("<b>Start Date : </b>" + ganttItem.startdate + " <b>Start Time : </b>" +job.StartTime + "<br>");
		//					displayStr.Append("<b>End Date : </b>" + ganttItem.enddate + " <b>End Time : </b>" + job.EndTime + "<br>");
		//					displayStr.Append("<b>Day No: </b>" + job.NumberOfGuardsDaytime + "  Night No: </b>" + job.NumberOfGuardsNighttime + "<br>");
		//					ganttItem.desc = displayStr.ToString();
							
		//					retList.Add(ganttItem);
		//				}
						
		//			}
					
		//			return Json(new { result = retList }, JsonRequestBehavior.AllowGet);
		//		}
		//	}
		//	catch (System.Exception ex)
		//	{
		//		throw ex;
		//	}
		//}


		///// <summary>
		///// Gets the GetHitecJobMultiBarData list.
		///// <summary>
		///// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		///// <returns></returns>
		//public ActionResult GetHitecJobMultiBarData()
		//{
		//	string uri = CommonHelper.BaseUri + "ReportController/GetHitecJobMultiBarData";
		//	try
		//	{
		//		List<MultiBarItem> retList = new List<MultiBarItem>();
		//		using (HttpClient httpClient = new HttpClient())
		//		{
		//			httpClient.DefaultRequestHeaders.Accept.Clear();
		//			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		//			HttpResponseMessage response = httpClient.GetAsync(uri + "/2021-01-01/2052-12-20").Result;
		//			if (response.IsSuccessStatusCode)
		//			{

		//				var jsonString = response.Content.ReadAsStringAsync();
		//				jsonString.Wait();
		//				List<HitecJob> result = JsonConvert.DeserializeObject<List<HitecJob>>(jsonString.Result);
		//				IEnumerable<IGrouping<string, HitecJob>> groups = (IEnumerable<IGrouping<string, HitecJob>>)result.OrderBy(o=>o.CreateDateTime).GroupBy(o => o.EndDate.Value.Month.ToString());
		//				//var grouped = (from p in result
		//				//			   group p by new { month = p.CreateDateTime.Value.Month, year = p.CreateDateTime.Value.Year } into d);
		//								   //select new { dt = string.Format("{0}/{1}", d.Key.month, d.Key.year), count = d.Count() }).OrderByDescending(g => g.dt);
		//				foreach (IGrouping<string, HitecJob> grp in groups)
		//				{
		//					MultiBarItem item = new MultiBarItem();
		//					item.columnname = grp.ToList()[0].EndDate.Value.ToString("MMM");
		//					item.bar1 = grp.Where(o => o.IsComplete == false && o.StartDate < DateTime.Now).Count().ToString(); //open bar
		//					item.bar2 = grp.Where(o => o.IsComplete == true).Count().ToString(); //completed bar
		//					item.bar3 = grp.Where(o => o.IsInvoiced == true).Count().ToString(); //invoiced bar
		//					item.bar4 = grp.Where(o => o.IsComplete == false && o.StartDate > DateTime.Now).Count().ToString(); //notstarted bar
		//					retList.Add(item);
		//				}

		//			}

		//			return Json(new { result = retList }, JsonRequestBehavior.AllowGet);
		//		}
		//	}
		//	catch (System.Exception ex)
		//	{
		//		throw ex;
		//	}
		//}
		///// <summary>
		/// Exports the form.
		/// <summary>
		/// <param name="dtParams">The data list.</param>
		/// <returns></returns>
		//public ActionResult ExportForm(string dataList)
		//{
		//	try
		//	{
		//	Export exportdata = new Export();
		//	exportdata.Controller ="StockReport";
		//	exportdata.Entity ="StockReport";
		//	exportdata.DatatableParams = dataList;
		//	return PartialView("ExportControl", exportdata);
		//	}
		//	catch (System.Exception ex)
		//	{
		//	return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
		//	}
		//}
		///// <summary>
		///// Exports the data.
		///// <summary>
		///// <param name="exportData">The export data.</param>
		///// <returns></returns>
		///// <exception cref="System.Exception">
		///// </exception>
		//[HttpPost]
		//public FileResult ExportData(Export exportDetails)
		//{
		//	try
		//	{
		//		FileContentResult file = base.ExportBase<vwStockReport>(exportDetails);
		//		if (file != null)
		//		{
		//		return file;
		//		}
		//		else
		//		{
		//		throw new System.Exception(Localizer.Current.GetString("ExportError"));
		//		}
		//}
		//catch (System.Exception ex)
		//{
		//	throw ex;
		//}
		//}
		#endregion
	}
}
