using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("ProductionController")]

	public class ProductionController : BaseApiController
	{

		ProductionManager manager = new ProductionManager();

		#region Production

		/// <summary>
		/// Gets all Productions.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Production")]
		[HttpPost]
		public HttpResponseMessage GetProductionList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Production> result = new GridResult<Production>();
				result = manager.GetProduction(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Production
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Production/{id?}/{includerelations?}")]
		public HttpResponseMessage GetProduction(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetProduction(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Production.
		/// <param name = "Production" > The Production.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Production/add")]
		[HttpPost]
		public HttpResponseMessage AddProduction([FromBody]Production newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Production newProduction = manager.AddReturnProduction(newItem);
					response = Request.CreateResponse<Production>(HttpStatusCode.Created, newProduction);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Updates Production.
		/// <param name = "Production" > The Production.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Production/update")]
		[HttpPost]
		public HttpResponseMessage UpdateProduction([FromBody]Production newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newProduction = manager.UpdateProduction(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.ProductionID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Deletes Production.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Production/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteProduction(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteProduction(id);
					response = Request.CreateResponse(HttpStatusCode.OK,row.ToString());
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}
		#endregion
	}
}
