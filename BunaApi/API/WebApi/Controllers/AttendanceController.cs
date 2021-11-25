using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("AttendanceController")]

	public class AttendanceController : BaseApiController
	{

		AttendanceManager manager = new AttendanceManager();

		#region Attendance

		/// <summary>
		/// Gets all Attendances.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Attendance")]
		[HttpPost]
		public HttpResponseMessage GetAttendanceList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Attendance> result = new GridResult<Attendance>();
				result = manager.GetAttendance(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Attendance
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Attendance/{id?}/{includerelations?}")]
		public HttpResponseMessage GetAttendance(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetAttendance(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Attendance.
		/// <param name = "Attendance" > The Attendance.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Attendance/add")]
		[HttpPost]
		public HttpResponseMessage AddAttendance([FromBody]Attendance newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Attendance newAttendance = manager.AddReturnAttendance(newItem);
					response = Request.CreateResponse<Attendance>(HttpStatusCode.Created, newAttendance);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates Attendance.
		/// <param name = "Attendance" > The Attendance.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Attendance/update")]
		[HttpPost]
		public HttpResponseMessage UpdateAttendance([FromBody]Attendance newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newAttendance = manager.UpdateAttendance(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.AttendanceID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes Attendance.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Attendance/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteAttendance(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteAttendance(id);
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
