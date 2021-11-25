using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("HitecJobController")]

	public class HitecJobController : BaseApiController
	{

		HitecJobManager manager = new HitecJobManager();

		#region HitecJob

		/// <summary>
		/// Gets all HitecJobs.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("HitecJob")]
		[HttpPost]
		public HttpResponseMessage GetHitecJobList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<HitecJob> result = new GridResult<HitecJob>();
				result = manager.GetHitecJob(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/activity/5
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("HitecJob/{id?}/{includerelations?}")]
		public HttpResponseMessage GetHitecJob(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetHitecJob(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified HitecJob.
		/// <param name = "HitecJob" > The HitecJob.</param>
		/// <returns></returns>
		/// </summary>
		[Route("HitecJob/add")]
		[HttpPost]
		public HttpResponseMessage AddHitecJob([FromBody]HitecJob newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					HitecJob newHitecJob = manager.AddReturnHitecJob(newItem);
					response = Request.CreateResponse<HitecJob>(HttpStatusCode.Created, newHitecJob);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Updates HitecJob.
		/// <param name = "HitecJob" > The HitecJob.</param>
		/// <returns></returns>
		/// </summary>
		[Route("HitecJob/update")]
		[HttpPost]
		public HttpResponseMessage UpdateHitecJob([FromBody]HitecJob newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newHitecJob = manager.UpdateHitecJob(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.HitecJobID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes HitecJob.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("HitecJob/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteHitecJob(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteHitecJob(id);
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
