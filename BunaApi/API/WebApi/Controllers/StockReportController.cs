using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("StockReportController")]

	public class StockReportController : BaseApiController
	{

		StockReportManager manager = new StockReportManager();

		#region vwStockReport

		/// <summary>
		/// Gets all vwStockReports.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("GetStockReport")]
		[HttpPost]
		public HttpResponseMessage GetStockReport([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<vwStockReport> result = new GridResult<vwStockReport>();
				result = manager.GetStockReport(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		
		#endregion
	}
}
