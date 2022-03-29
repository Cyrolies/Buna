using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("ProductController")]

	public class ProductController : BaseApiController
	{

		ProductManager manager = new ProductManager();

		#region Product

		/// <summary>
		/// Gets all Products.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Product")]
		[HttpPost]
		public HttpResponseMessage GetProductList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Product> result = new GridResult<Product>();
				result = manager.GetProduct(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Product
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Product/{id?}/{includerelations?}")]
		public HttpResponseMessage GetProduct(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetProduct(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Product.
		/// <param name = "Product" > The Product.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Product/add")]
		[HttpPost]
		public HttpResponseMessage AddProduct([FromBody]Product newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Product newProduct = manager.AddReturnProduct(newItem);
					response = Request.CreateResponse<Product>(HttpStatusCode.Created, newProduct);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Updates Product.
		/// <param name = "Product" > The Product.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Product/update")]
		[HttpPost]
		public HttpResponseMessage UpdateProduct([FromBody]Product newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newProduct = manager.UpdateProduct(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.ProductID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Deletes Product.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Product/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteProduct(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteProduct(id);
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
