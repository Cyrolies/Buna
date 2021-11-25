using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("StudentMealController")]

	public class StudentMealController : BaseApiController
	{

		StudentMealManager manager = new StudentMealManager();

		#region StudentMeal

		/// <summary>
		/// Gets all StudentMeals.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentMeal")]
		[HttpPost]
		public HttpResponseMessage GetStudentMealList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<StudentMeal> result = new GridResult<StudentMeal>();
				result = manager.GetStudentMeal(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET StudentMeal
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentMeal/{id?}/{includerelations?}")]
		public HttpResponseMessage GetStudentMeal(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetStudentMeal(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified StudentMeal.
		/// <param name = "StudentMeal" > The StudentMeal.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentMeal/add")]
		[HttpPost]
		public HttpResponseMessage AddStudentMeal([FromBody]StudentMeal newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					StudentMeal newStudentMeal = manager.AddReturnStudentMeal(newItem);
					response = Request.CreateResponse<StudentMeal>(HttpStatusCode.Created, newStudentMeal);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates StudentMeal.
		/// <param name = "StudentMeal" > The StudentMeal.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentMeal/update")]
		[HttpPost]
		public HttpResponseMessage UpdateStudentMeal([FromBody]StudentMeal newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newStudentMeal = manager.UpdateStudentMeal(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.StudentMealID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes StudentMeal.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("StudentMeal/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteStudentMeal(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteStudentMeal(id);
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
