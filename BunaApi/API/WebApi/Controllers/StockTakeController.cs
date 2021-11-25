using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;
using System;

namespace Controllers
{
[RoutePrefix("StockTakeController")]

	public class StockTakeController : BaseApiController
	{

		StockTakeManager manager = new StockTakeManager();

		#region StockTake

		/// <summary>
		/// Gets all StockTakes.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StockTake")]
		[HttpPost]
		public HttpResponseMessage GetStockTakeList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<StockTake> result = new GridResult<StockTake>();
				result = manager.GetStockTake(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/activity/5
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StockTake/{id?}/{includerelations?}")]
		public HttpResponseMessage GetStockTake(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetStockTake(id, System.Convert.ToBoolean(includerelations));
				if (returnItem == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, returnItem);
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified StockTake.
		/// <param name = "StockTake" > The StockTake.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StockTake/add")]
		[HttpPost]
		public HttpResponseMessage AddStockTake([FromBody]StockTake newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					newItem.StockTakeDate = DateTime.Now;
					newItem.CreateDateTime = DateTime.Now;
					StockTake newStockTake = manager.AddReturnStockTake(newItem);
					response = Request.CreateResponse<StockTake>(HttpStatusCode.Created, newStockTake);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates StockTake.
		/// <param name = "StockTake" > The StockTake.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StockTake/update")]
		[HttpPost]
		public HttpResponseMessage UpdateStockTake([FromBody]StockTake newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					newItem.ChangeDateTime = DateTime.Now;
					int newStockTake = manager.UpdateStockTake(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.StockTakeID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes StockTake.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StockTake/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteStockTake(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteStockTake(id);
					response = Request.CreateResponse(HttpStatusCode.OK,row.ToString());
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}
		#endregion
	}
}
