using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("WorkOrderController")]

	public class WorkOrderController : BaseApiController
	{

		WorkOrderManager manager = new WorkOrderManager();

		#region WorkOrder

		/// <summary>
		/// Gets all WorkOrders.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrder")]
		[HttpPost]
		public HttpResponseMessage GetWorkOrderList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<WorkOrder> result = new GridResult<WorkOrder>();
				result = manager.GetWorkOrder(filter);
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
		[Route("WorkOrder/{id?}/{includerelations?}")]
		public HttpResponseMessage GetWorkOrder(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetWorkOrder(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified WorkOrder.
		/// <param name = "WorkOrder" > The WorkOrder.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrder/add")]
		[HttpPost]
		public HttpResponseMessage AddWorkOrder([FromBody]WorkOrder newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					WorkOrder newWorkOrder = manager.AddReturnWorkOrder(newItem);
					response = Request.CreateResponse<WorkOrder>(HttpStatusCode.Created, newWorkOrder);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates WorkOrder.
		/// <param name = "WorkOrder" > The WorkOrder.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrder/update")]
		[HttpPost]
		public HttpResponseMessage UpdateWorkOrder([FromBody]WorkOrder newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newWorkOrder = manager.UpdateWorkOrder(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.WorkOrderID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes WorkOrder.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrder/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteWorkOrder(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteWorkOrder(id);
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
