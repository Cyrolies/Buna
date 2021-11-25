using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("WorkOrderPartController")]

	public class WorkOrderPartController : BaseApiController
	{

		WorkOrderPartManager manager = new WorkOrderPartManager();

		#region WorkOrderPart

		/// <summary>
		/// Gets all WorkOrderParts.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrderPart")]
		[HttpPost]
		public HttpResponseMessage GetWorkOrderPartList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<WorkOrderPart> result = new GridResult<WorkOrderPart>();
				result = manager.GetWorkOrderPart(filter);
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
		[Route("WorkOrderPart/{id?}/{includerelations?}")]
		public HttpResponseMessage GetWorkOrderPart(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetWorkOrderPart(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified WorkOrderPart.
		/// <param name = "WorkOrderPart" > The WorkOrderPart.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrderPart/add")]
		[HttpPost]
		public HttpResponseMessage AddWorkOrderPart([FromBody]WorkOrderPart newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					WorkOrderPart newWorkOrderPart = manager.AddReturnWorkOrderPart(newItem);
					response = Request.CreateResponse<WorkOrderPart>(HttpStatusCode.Created, newWorkOrderPart);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates WorkOrderPart.
		/// <param name = "WorkOrderPart" > The WorkOrderPart.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrderPart/update")]
		[HttpPost]
		public HttpResponseMessage UpdateWorkOrderPart([FromBody]WorkOrderPart newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newWorkOrderPart = manager.UpdateWorkOrderPart(newItem);
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
		/// Deletes WorkOrderPart.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("WorkOrderPart/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteWorkOrderPart(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteWorkOrderPart(id);
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
