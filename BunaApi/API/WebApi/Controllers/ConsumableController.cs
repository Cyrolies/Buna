using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("ConsumableController")]

	public class ConsumableController : BaseApiController
	{

		ConsumableManager manager = new ConsumableManager();

		#region Consumable

		/// <summary>
		/// Gets all Consumables.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Consumable")]
		[HttpPost]
		public HttpResponseMessage GetConsumableList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Consumable> result = new GridResult<Consumable>();
				result = manager.GetConsumable(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Consumable
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Consumable/{id?}/{includerelations?}")]
		public HttpResponseMessage GetConsumable(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetConsumable(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Consumable.
		/// <param name = "Consumable" > The Consumable.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Consumable/add")]
		[HttpPost]
		public HttpResponseMessage AddConsumable([FromBody]Consumable newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Consumable newConsumable = manager.AddReturnConsumable(newItem);
					response = Request.CreateResponse<Consumable>(HttpStatusCode.Created, newConsumable);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates Consumable.
		/// <param name = "Consumable" > The Consumable.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Consumable/update")]
		[HttpPost]
		public HttpResponseMessage UpdateConsumable([FromBody]Consumable newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newConsumable = manager.UpdateConsumable(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.ConsumableID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes Consumable.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Consumable/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteConsumable(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteConsumable(id);
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
