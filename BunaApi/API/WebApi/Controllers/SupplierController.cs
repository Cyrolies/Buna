using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("SupplierController")]

	public class SupplierController : BaseApiController
	{

		SupplierManager manager = new SupplierManager();

		#region Supplier

		/// <summary>
		/// Gets all Suppliers.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Supplier")]
		[HttpPost]
		public HttpResponseMessage GetSupplierList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Supplier> result = new GridResult<Supplier>();
				result = manager.GetSupplier(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Supplier
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Supplier/{id?}/{includerelations?}")]
		public HttpResponseMessage GetSupplier(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetSupplier(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Supplier.
		/// <param name = "Supplier" > The Supplier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Supplier/add")]
		[HttpPost]
		public HttpResponseMessage AddSupplier([FromBody]Supplier newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Supplier newSupplier = manager.AddReturnSupplier(newItem);
					response = Request.CreateResponse<Supplier>(HttpStatusCode.Created, newSupplier);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Updates Supplier.
		/// <param name = "Supplier" > The Supplier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Supplier/update")]
		[HttpPost]
		public HttpResponseMessage UpdateSupplier([FromBody]Supplier newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newSupplier = manager.UpdateSupplier(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.SupplierID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Deletes Supplier.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Supplier/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteSupplier(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteSupplier(id);
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
