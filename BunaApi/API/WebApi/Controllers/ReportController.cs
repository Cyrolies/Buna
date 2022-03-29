using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;
using System.Collections.Generic;
using System;

namespace Controllers
{
[RoutePrefix("ReportController")]


	public class ReportController : BaseApiController
	{

		ReportManager manager = new ReportManager();

		#region Hitec

		/// <summary>
		/// GetHitecJobGanttData.
		/// <returns>List HitecJobs</returns>
		/// </summary>
		//[Route("GetHitecJobGanttData")]
		//[HttpGet]
		//public HttpResponseMessage GetHitecJobGanttData()
		//{
		//	HttpResponseMessage response = new HttpResponseMessage();
		//	try
		//	{
		//		List<HitecJob> result = new List<HitecJob>();
		//		result = manager.GetHitecJobGanttData();
		//		response = Request.CreateResponse(HttpStatusCode.OK, result);
		//	}
		//	catch (System.Exception ex)
		//	{
		//		response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
		//	}
		//	return response;
		//}

		///// <summary>
		///// GetHitecJobMultiBarData.
		///// <returns>List HitecJobs</returns>
		///// </summary>
		//[Route("GetHitecJobMultiBarData/{startDate}/{endDate}")]
		//[HttpGet]
		//public HttpResponseMessage GetHitecJobMultiBarData(string startDate,string endDate)
		//{
		//	HttpResponseMessage response = new HttpResponseMessage();
		//	try
		//	{
		//		DateTime startdt = DateTime.Parse(startDate);
		//		DateTime enddt = DateTime.Parse(endDate);
		//		List<HitecJob> result = new List<HitecJob>();
		//		result = manager.GetHitecJobMultiBarData(startdt, enddt);
		//		response = Request.CreateResponse(HttpStatusCode.OK, result);
		//	}
		//	catch (System.Exception ex)
		//	{
		//		response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
		//	}
		//	return response;
		//}

		#endregion
	}
}
