using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using DSDBLL;
using DALEFModel;

namespace Controllers
{
[RoutePrefix("AssetController")]

	public class AssetController : BaseApiController
	{

		AssetManager manager = new AssetManager();

		#region Asset

		/// <summary>
		/// Gets all Assets.
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Asset")]
		[HttpPost]
		public HttpResponseMessage GetAssetList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<Asset> result = new GridResult<Asset>();
				result = manager.GetAsset(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET Asset
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelation.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Asset/{id?}/{includerelations?}")]
		public HttpResponseMessage GetAsset(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var returnItem = manager.GetAsset(id, System.Convert.ToBoolean(includerelations));
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
		/// Puts the specified Asset.
		/// <param name = "Asset" > The Asset.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Asset/add")]
		[HttpPost]
		public HttpResponseMessage AddAsset([FromBody]Asset newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					Asset newAsset = manager.AddReturnAsset(newItem);
					response = Request.CreateResponse<Asset>(HttpStatusCode.Created, newAsset);
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Updates Asset.
		/// <param name = "Asset" > The Asset.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Asset/update")]
		[HttpPost]
		public HttpResponseMessage UpdateAsset([FromBody]Asset newItem)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (newItem != null)
				{
					int newAsset = manager.UpdateAsset(newItem);
					response = Request.CreateResponse(HttpStatusCode.Created,newItem.AssetID.ToString());
				}
				}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;
		}

		/// <summary>
		/// Deletes Asset.
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// </summary>
		[Route("Asset/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteAsset(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
					 int row = manager.DeleteAsset(id);
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
