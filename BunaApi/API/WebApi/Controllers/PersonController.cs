using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("PersonController")]

	public class PersonController : BaseApiController
	{

		PersonManager manager = new PersonManager();

		#region Person

		/// <summary>
		/// Gets all Persons.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Person")]
		[HttpPost]
		public HttpResponseMessage GetPersonList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Person> result = new GridResult<Person>();
				result = manager.GetPerson(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Person
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Person/{id?}/{includerelations?}")]
		public HttpResponseMessage GetPerson(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetPerson(id, System.Convert.ToBoolean(includerelations));
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
		/// <param name="newItem"></param>
		/// Puts the specified Person.
		/// <param name = "Person" > The Person.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Person/add")]
		[HttpPost]
		public HttpResponseMessage AddPerson([FromBody]Person newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Person newPerson = manager.AddReturnPerson(newItem);
					response = Request.CreateResponse<Person>(HttpStatusCode.Created, newPerson);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Updates Person.
		/// <param name = "Person" > The Person.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Person/update")]
		[HttpPost]
		public HttpResponseMessage UpdatePerson([FromBody]Person newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newPerson = manager.UpdatePerson(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.PersonID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ExceptionMessage);
			}
			return response;
		}

		/// <summary>
		/// Deletes Person.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Person/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeletePerson(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeletePerson(id);
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
