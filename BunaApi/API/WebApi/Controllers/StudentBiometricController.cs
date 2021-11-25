using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("StudentBiometricController")]

	public class StudentBiometricController : BaseApiController
	{

		StudentBiometricManager manager = new StudentBiometricManager();

		#region StudentBiometric

		/// <summary>
		/// Gets all StudentBiometrics.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentBiometric")]
		[HttpPost]
		public HttpResponseMessage GetStudentBiometricList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<StudentBiometric> result = new GridResult<StudentBiometric>();
				result = manager.GetStudentBiometric(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET StudentBiometric
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentBiometric/{id?}/{includerelations?}")]
		public HttpResponseMessage GetStudentBiometric(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetStudentBiometric(id, System.Convert.ToBoolean(includerelations));
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

		[Route("GetStudentBiometricByStudentID/{id?}/{includerelations?}")]
		public HttpResponseMessage GetStudentBiometricByStudentID(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetStudentBiometricByStudentID(id, System.Convert.ToBoolean(includerelations));
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

		[Route("GetStudentBiometricList/{orgID?}/{biometricType?}/{includerelations?}")]
		public HttpResponseMessage GetStudentBiometricList(int orgID, int biometricType, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetStudentBiometricList(orgID, biometricType, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified StudentBiometric.
		/// <param name = "StudentBiometric" > The StudentBiometric.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentBiometric/add")]
		[HttpPost]
		public HttpResponseMessage AddStudentBiometric([FromBody]StudentBiometric newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					StudentBiometric newStudentBiometric = manager.AddReturnStudentBiometric(newItem);
					response = Request.CreateResponse<StudentBiometric>(HttpStatusCode.Created, newStudentBiometric);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates StudentBiometric.
		/// <param name = "StudentBiometric" > The StudentBiometric.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentBiometric/update")]
		[HttpPost]
		public HttpResponseMessage UpdateStudentBiometric([FromBody]StudentBiometric newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newStudentBiometric = manager.UpdateStudentBiometric(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.StudentBiometricID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes StudentBiometric.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentBiometric/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteStudentBiometric(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteStudentBiometric(id);
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
