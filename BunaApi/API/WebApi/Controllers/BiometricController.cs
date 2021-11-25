using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("BiometricController")]

	public class BiometricController : BaseApiController
	{

		BiometricManager manager = new BiometricManager();

		#region Biometric

		/// <summary>
		/// Gets all Biometrics.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Biometric")]
		[HttpPost]
		public HttpResponseMessage GetBiometricList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Biometric> result = new GridResult<Biometric>();
				result = manager.GetBiometric(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Biometric
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Biometric/{id?}/{includerelations?}")]
		public HttpResponseMessage GetBiometric(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetBiometric(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Biometric.
		/// <param name = "Biometric" > The Biometric.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Biometric/add")]
		[HttpPost]
		public HttpResponseMessage AddBiometric([FromBody]Biometric newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Biometric newBiometric = manager.AddReturnBiometric(newItem);
					response = Request.CreateResponse<Biometric>(HttpStatusCode.Created, newBiometric);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates Biometric.
		/// <param name = "Biometric" > The Biometric.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Biometric/update")]
		[HttpPost]
		public HttpResponseMessage UpdateBiometric([FromBody]Biometric newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newBiometric = manager.UpdateBiometric(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.BiometricID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes Biometric.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Biometric/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteBiometric(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteBiometric(id);
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
