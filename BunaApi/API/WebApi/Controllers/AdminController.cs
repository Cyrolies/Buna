using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DSDBLL;
using Common;
using DALEFModel;
using System;
using System.Text;

namespace Controllers
{
    /// <summary>
    /// All web administration functionality Activity/Entity/EntityField/EntityMenu/EntityResource
    /// </summary>
    /// <seealso cref="Controllers.BaseApiController" />
    /// <seealso cref="System.Web.Http.ApiController" />
	[RoutePrefix("AdminController")]
    public class AdminController : BaseApiController
    {
        AdminManager manager = new AdminManager();

		#region "SelectList"
		/// <summary>
		/// Gets a list of a particular table column.
		/// </summary>
		/// <param name="selectQuery"></param>
		/// <returns>List<string></string></returns>
		[Route("GetSelectList")]
		[HttpPost]
		public HttpResponseMessage GetSelectList([FromBody]  SelectQuery selectQuery)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				object[] paras = { };
				GridResult<SelectResult> result = new GridResult<SelectResult>();
				result.Items = manager.ExecuteLinqQuery<SelectResult>(selectQuery.GetQuery(), paras);

				response = Request.CreateResponse(HttpStatusCode.OK, result);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		#endregion

		#region "Entity"
		/// <summary>
		/// Gets all entities.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("Entity")]
		[HttpPost]
		public HttpResponseMessage GetEntityList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				GridResult<Entity> entities = manager.GetEntityList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, entities);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// Gets all entities.
		/// </summary>
		/// <returns></returns>
		[Route("GetEntities")]
		[HttpGet]
		public HttpResponseMessage GetEntities()
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				List<Entity> entities = manager.GetEntities();
				response = Request.CreateResponse(HttpStatusCode.OK, entities);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// GET api/Entity/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("Entity/{id?}/{includerelations?}")]
		public HttpResponseMessage GetEntity(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				var activity = manager.GetEntity(id, System.Convert.ToBoolean(includerelations));
				if (activity == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, activity);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		[Route("Entity/{name?}")]
		public HttpResponseMessage GetEntity(string name)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				var entity = manager.GetEntity(name, false);
				if (entity == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, entity);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified Entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		[Route("Entity/add")]
		[HttpPost]
		public HttpResponseMessage AddEntity([FromBody] Entity entity)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entity != null)
				{

					Entity newentity = manager.AddReturnEntity(entity);
					response = Request.CreateResponse<Entity>(HttpStatusCode.Created, newentity);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Updates the entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		[Route("Entity/update")]
		[HttpPost]
		public HttpResponseMessage UpdateEntity([FromBody] Entity entity)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entity != null)
				{

					int newactivity = manager.UpdateEntity(entity);
					response = Request.CreateResponse(HttpStatusCode.Created, entity.EntityID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the entity.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("Entity/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteEntity(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteEntity(id);
				response = Request.CreateResponse(HttpStatusCode.OK, row.ToString());
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		#endregion

		#region "EntityField"
		/// <summary>
		/// Gets all EntityFields.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("EntityField")]
		[HttpPost]
		public HttpResponseMessage GetEntityFieldList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				GridResult<EntityField> entities = manager.GetEntityFieldList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, entities);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Gets the entity fields as list.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("EntityFieldList")]
		[HttpPost]
		public HttpResponseMessage GetEntityFieldsAsList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				IEnumerable<EntityField> entities = manager.GetEntityFieldsAsList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, entities);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/EntityField/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("EntityField/{id?}/{includerelations?}")]
		public HttpResponseMessage GetEntityField(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				var activity = manager.GetEntityField(id, System.Convert.ToBoolean(includerelations));
				if (activity == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, activity);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified EntityField.
		/// </summary>
		/// <param name="entityfield">The entityfield.</param>
		/// <returns></returns>
		[Route("EntityField/add")]
		[HttpPost]
		public HttpResponseMessage AddEntityField([FromBody] EntityField entityfield)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entityfield != null)
				{

					EntityField newentityfield = manager.AddReturnEntityField(entityfield);
					response = Request.CreateResponse<EntityField>(HttpStatusCode.Created, newentityfield);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}


		/// <summary>
		/// Updates the EntityField.
		/// </summary>
		/// <param name="entityfield">The entityfield.</param>
		/// <returns></returns>
		[Route("EntityField/update")]
		[HttpPost]
		public HttpResponseMessage UpdateEntityField([FromBody] EntityField entityfield)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entityfield != null)
				{

					int newactivity = manager.UpdateEntityField(entityfield);
					response = Request.CreateResponse(HttpStatusCode.Created, entityfield.EntityFieldID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "hello id =" + entityfield.EntityFieldID.ToString() + ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the entity.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("EntityField/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteEntityField(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteEntityField(id);
				response = Request.CreateResponse(HttpStatusCode.OK, row.ToString());
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		#endregion

		#region "EntityMenu"
		/// <summary>
		/// Gets all entity menu.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("Menu")]
		[HttpPost]
		public HttpResponseMessage GetEntityMenuList([FromBody] GridParam filter)
		{

			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				GridResult<EntityMenu> menus = manager.GetEntityMenuList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, menus);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/entitymenu/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("Menu/{id?}/{includerelations?}")]
		public HttpResponseMessage GetEntityMenu(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				var activity = manager.GetEntityMenu(id, System.Convert.ToBoolean(includerelations));
				if (activity == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, activity);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified entitymenu.
		/// </summary>
		/// <param name="entitymenu">The entitymenu.</param>
		/// <returns></returns>
		[Route("Menu/add")]
		[HttpPost]
		public HttpResponseMessage AddEntityMenu([FromBody] EntityMenu entitymenu)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entitymenu != null)
				{

					EntityMenu newentitymenu = manager.AddReturnEntityMenu(entitymenu);
					response = Request.CreateResponse<EntityMenu>(HttpStatusCode.Created, newentitymenu);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}


		/// <summary>
		/// Updates the entitymenu.
		/// </summary>
		/// <param name="entitymenu">The entitymenu.</param>
		/// <returns></returns>
		[Route("Menu/update")]
		[HttpPost]
		public HttpResponseMessage UpdateEntityMenu([FromBody] EntityMenu entitymenu)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entitymenu != null)
				{

					int newactivity = manager.UpdateEntityMenu(entitymenu);
					response = Request.CreateResponse(HttpStatusCode.Created, entitymenu.EntityMenuID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the menu.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("Menu/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteMenu(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteEntityMenu(id);
				response = Request.CreateResponse(HttpStatusCode.OK, row.ToString());
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		#endregion

		#region "EntityResource"
		
		/// <summary>
		/// Gets the entity resource list.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("GetEntityResourceList")]
		[HttpPost]
		public HttpResponseMessage GetEntityResourceList([FromBody] GridParam filter)
		{

			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				GridResult<EntityResource> resources = manager.GetEntityResourceList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, resources);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Gets the entity resource list.
		/// </summary>
		/// <param name="OrgID">The OrgID.</param>
		/// <returns></returns>
		[Route("GetEntityResourceListByOrg/{OrgID}")]
		[HttpGet]
		public HttpResponseMessage GetEntityResourceListbyOrg(string OrgID)
		{

			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				List<EntityResource> resources = manager.GetEntityResourceList(Convert.ToInt32(OrgID));
				response = Request.CreateResponse(HttpStatusCode.OK, resources);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// GET api/EntityResource/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("EntityResource/{id}/{includerelations}")]
		public HttpResponseMessage GetEntityResource(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				EntityResource resource = new EntityResource();
				//if (id != null)
				//{
				resource = manager.GetEntityResource((int)id, System.Convert.ToBoolean(includerelations));
				//}
				//else
				//{
				//    resource = adminmng.GetEntityResource(key, culture, false);
				//}
				if (resource == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, resource);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Gets the entity resource.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="culture">The culture.</param>
		/// <returns></returns>
		[Route("EntityResourceByKey/{key}/{culture}")]
		public HttpResponseMessage GetEntityResourceByKey(string key, string culture)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				var resource = manager.GetEntityResource(key, culture, false);
				if (resource == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, resource);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified EntityResource.
		/// </summary>
		/// <param name="entityResource">The entity resource.</param>
		/// <returns></returns>
		[Route("EntityResource/Add")]
		[HttpPost]
		public HttpResponseMessage AddEntityResource([FromBody] EntityResource entityResource)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entityResource != null)
				{

					EntityResource newentityresource = manager.AddReturnEntityResource(entityResource);
					response = Request.CreateResponse<EntityResource>(HttpStatusCode.Created, newentityresource);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}


		/// <summary>
		/// Updates the EntityResource.
		/// </summary>
		/// <param name="entityResource">The entity resource.</param>
		/// <returns></returns>
		[Route("EntityResource/Update")]
		[HttpPost]
		public HttpResponseMessage UpdateEntityResource([FromBody] EntityResource entityResource)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (entityResource != null)
				{

					int newactivity = manager.UpdateEntityResource(entityResource);
					response = Request.CreateResponse(HttpStatusCode.Created, entityResource.ResourceID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "hello id =" + entityResource.ResourceID.ToString() + ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;

		}

		/// <summary>
		/// Deletes the entity resource.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("EntityResource/Delete/{id}")]
		[HttpPost]
		public HttpResponseMessage DeleteEntityResource(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteEntityResource(id);
				response = Request.CreateResponse(HttpStatusCode.OK, row.ToString());
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}


		#endregion

		#region "StpData"

		/// <summary>
		/// Gets all setup data.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		[Route("StpData")]
		[HttpPost]
		public HttpResponseMessage GetStpDataList([FromBody] GridParam filter)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				GridResult<StpData> data = manager.GetSTPDataList(filter);
				response = Request.CreateResponse(HttpStatusCode.OK, data);
			}
			catch (HttpResponseException ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}
		/// <summary>
		/// GET api/StpData/5
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="includerelations">The includerelations.</param>
		/// <returns></returns>
		[Route("StpData/{id?}/{includerelations?}")]
		public HttpResponseMessage GetStpData(int id, string includerelations)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				var stpdata = manager.GetStpData(id, System.Convert.ToBoolean(includerelations));
				if (stpdata == null)
				{
					response = Request.CreateResponse(HttpStatusCode.NotFound);
				}
				response = Request.CreateResponse(HttpStatusCode.OK, stpdata);
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		/// <summary>
		/// Puts the specified activity.
		/// </summary>
		/// <param name="stpData">The STP data.</param>
		/// <returns></returns>
		[Route("StpData/add")]
		[HttpPost]
		public HttpResponseMessage AddStpData([FromBody] StpData stpData)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (stpData != null)
				{

					StpData newstpData = manager.AddReturnStpData(stpData);
					response = Request.CreateResponse<StpData>(HttpStatusCode.Created, newstpData);
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;

		}

		/// <summary>
		/// Updates the activity.
		/// </summary>
		/// <param name="stpData">The STP data.</param>
		/// <returns></returns>
		[Route("StpData/update")]
		[HttpPost]
		public HttpResponseMessage UpdateStpData([FromBody] StpData stpData)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				if (stpData != null)
				{

					int newdata = manager.UpdateStpData(stpData);
					response = Request.CreateResponse(HttpStatusCode.Created, stpData.StpDataID.ToString());
				}
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).CreateDetailNoHtml());
			}
			return response;

		}

		/// <summary>
		/// Deletes the STP data.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[Route("StpData/delete/{id?}")]
		[HttpPost]
		public HttpResponseMessage DeleteStpData(int id)
		{
			HttpResponseMessage response = new HttpResponseMessage();
			try
			{
				int row = manager.DeleteStpData(id);
				response = Request.CreateResponse(HttpStatusCode.OK, row.ToString());
			}
			catch (System.Exception ex)
			{
				response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ExceptionHandler.Handle(ex).ErrorDetail);
			}
			return response;
		}

		#endregion

	}
}
