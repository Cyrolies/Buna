using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("DocumentController")]

	public class DocumentController : BaseApiController
	{

		DocumentManager manager = new DocumentManager();

		#region Document

		/// <summary>
		/// Gets all Documents.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Document")]
		[HttpPost]
		public HttpResponseMessage GetDocumentList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Document> result = new GridResult<Document>();
				result = manager.GetDocument(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Document
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Document/{id?}/{includerelations?}")]
		public HttpResponseMessage GetDocument(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetDocument(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Document.
		/// <param name = "Document" > The Document.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Document/add")]
		[HttpPost]
		public HttpResponseMessage AddDocument([FromBody]Document newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Document newDocument = manager.AddReturnDocument(newItem);
					response = Request.CreateResponse<Document>(HttpStatusCode.Created, newDocument);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates Document.
		/// <param name = "Document" > The Document.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Document/update")]
		[HttpPost]
		public HttpResponseMessage UpdateDocument([FromBody]Document newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newDocument = manager.UpdateDocument(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.DocumentID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes Document.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Document/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteDocument(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteDocument(id);
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
