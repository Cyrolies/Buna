using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("PersonBiometricController")]

	public class PersonBiometricController : BaseApiController
	{

		PersonBiometricManager manager = new PersonBiometricManager();

		#region PersonBiometric

		/// <summary>
		/// Gets all PersonBiometrics.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("PersonBiometric")]
		[HttpPost]
		public HttpResponseMessage GetPersonBiometricList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<PersonBiometric> result = new GridResult<PersonBiometric>();
				result = manager.GetPersonBiometric(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET PersonBiometric
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("PersonBiometric/{id?}/{includerelations?}")]
		public HttpResponseMessage GetPersonBiometric(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetPersonBiometric(id, System.Convert.ToBoolean(includerelations));
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

		[Route("PersonBiometricByPersonID/{id?}/{includerelations?}")]
		public HttpResponseMessage GetPersonBiometricByPersonID(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetPersonBiometricByPersonID(id, System.Convert.ToBoolean(includerelations));
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

		[Route("GetBiometricList/{orgID?}/{biometricType?}/{includerelations?}")]
		public HttpResponseMessage GetBiometricList(int orgID,int biometricType, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetBiometricList(orgID,biometricType, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified PersonBiometric.
		/// <param name = "PersonBiometric" > The PersonBiometric.</param>
		/// <returns></returns>
		/// </summary>
		[Route("PersonBiometric/add")]
		[HttpPost]
		public HttpResponseMessage AddPersonBiometric([FromBody]PersonBiometric newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					PersonBiometric newPersonBiometric = manager.AddReturnPersonBiometric(newItem);
					response = Request.CreateResponse<PersonBiometric>(HttpStatusCode.Created, newPersonBiometric);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates PersonBiometric.
		/// <param name = "PersonBiometric" > The PersonBiometric.</param>
		/// <returns></returns>
		/// </summary>
		[Route("PersonBiometric/update")]
		[HttpPost]
		public HttpResponseMessage UpdatePersonBiometric([FromBody]PersonBiometric newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newPersonBiometric = manager.UpdatePersonBiometric(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.PersonBiometricID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes PersonBiometric.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("PersonBiometric/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeletePersonBiometric(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeletePersonBiometric(id);
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
