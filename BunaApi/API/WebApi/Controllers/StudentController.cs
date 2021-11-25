using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json;

namespace Controllers
{
[RoutePrefix("StudentController")]

	public class StudentController : BaseApiController
	{

		StudentManager manager = new StudentManager();

		#region Student

		[Route("GetStudentsFromEdAdmin/{school?}")]
		[HttpGet]
		public HttpResponseMessage GetStudentsFromEdAdmin(string school)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try 
			{ 
				if(manager.UpdateStudentsFromEdAdmin(school))
				{
					response =  Request.CreateResponse(HttpStatusCode.OK);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// Gets all Students.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Student")]
		[HttpPost]
		public HttpResponseMessage GetStudentList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Student> result = new GridResult<Student>();
				result = manager.GetStudent(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Student
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Student/{id?}/{includerelations?}")]
		public HttpResponseMessage GetStudent(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetStudent(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Student.
		/// <param name = "Student" > The Student.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Student/add")]
		[HttpPost]
		public HttpResponseMessage AddStudent([FromBody]Student newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Student newStudent = manager.AddReturnStudent(newItem);
					response = Request.CreateResponse<Student>(HttpStatusCode.Created, newStudent);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates Student.
		/// <param name = "Student" > The Student.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Student/update")]
		[HttpPost]
		public HttpResponseMessage UpdateStudent([FromBody]Student newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newStudent = manager.UpdateStudent(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.StudentID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes Student.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Student/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteStudent(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteStudent(id);
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
