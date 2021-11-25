using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DSDBLL;
using Common;
using DALEFModel;
using System.Threading.Tasks;

namespace Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("BaseApiController")]
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [Route("DataList")]
        [HttpPost]
        public HttpResponseMessage GetDataList([FromBody] GridParam filter)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                BaseManager manager = new BaseManager();
                var list = manager.GetDataByEntityName(filter);
                if (list == null)
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ExceptionHandler.Handle(new Exception("Entity not in Switch selection in API method OR No filter has been provided add filterby to foreign key entity field")).ErrorDetail);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (System.Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
            }
            return response;
        }

        /// <summary>Gets the tx item file.</summary>
        /// <param name="txItemId">The tx item identifier.</param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("GetApkFile")]
        //public async Task<HttpResponseMessage> GetApkFile()
        //{
        //    HttpResponseMessage response;
        //    try
        //    {
        //        byte[] fileContent = _manager.GetTXItemFile(txItemId);
        //        if (fileContent == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, "No file found");
        //        }
        //        response = Request.CreateResponse(HttpStatusCode.OK);
        //        response.Content = new ByteArrayContent(fileContent);
        //        switch (fileExt.ToLower())
        //        {
        //            case ".xlsx":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

        //                break;
        //            case ".txt":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        //                break;
        //            case ".html":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        //                break;
        //            case ".csv":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
        //                break;
        //            case ".json":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //                break;
        //            case ".xls":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType =
        //                    new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //                break;
        //            case ".xml":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
        //                break;
        //            case ".pdf":
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //                break;
        //            default:
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        //                break;
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.Debug("Error retrieving tx item file = " + ex.Message);
        //        response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message, ex);
        //    }
        //    return response;
        //}
    }
}