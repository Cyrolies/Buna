using System.Web.Mvc;
using System.Net.Http;
using Common;
using System.Text;
using DALEFModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Models;
using System;
using CyroTechPortal.HTMLHelpers;
using System.Web;

namespace CyroTechPortal
{
 
    /// <summary>
    /// All web administration functionality Activity/Entity/EntityField/EntityMenu/EntityResource
    /// </summary>
    /// <seealso cref="CyroTechPortal.BaseController" />
    public class AdminController : BaseController
    {
        //[HttpGet]
        //public ActionResult GetStyle()
        //{
        //	User userSession = (User)Session["User"];
        //	var theme = userSession != null ? "~/Content/css/" + userSession.Theme + ".css" : "~/Content/css/skin-cyrotech.css";
        //	Response.ContentType = "text/css";
        //	return PartialView(theme);
        //}

       


        #region "EntityField"

        /// <summary>
        /// entityField this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult EntityField()
        {
            return View();
        }

        /// <summary>
        /// Gets the entityField list.
        /// </summary>
        /// <param name="jQueryDataTablesModel">The j query data tables model.</param>
        /// <returns></returns>
        public ActionResult GetEntityFieldList(JQueryDataTablesModel jQueryDataTablesModel)
        {
            string uri = CommonHelper.BaseUri + "AdminController/EntityField";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
                    gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;

                    //gridParams.IncludeForeignKeyValues = true;
                    gridParams.Includerelations = true;
                    gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
                    gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        GridResult<EntityField> result = JsonConvert.DeserializeObject<GridResult<EntityField>>(jsonString.Result);

                        List<EntityField> retList = new List<DALEFModel.EntityField>();
                        foreach (EntityField item in result.Items)
                        {
                            if (item.EntityFieldDataType != null)
                            {
                                item.EntityFieldDataTypeDesc = item.EntityFieldDataType.Type;
                                item.EntityFieldDataType = null;
                            }
                            if (item.StcData != null)
                            {
                                item.StcControlTypeDesc = item.StcData.Description;
                                item.StcData = null;
                            }
                            if (item.Entity != null)
                            {
                                item.EntityDesc = item.Entity.TableName;
                                item.Entity = null;
                            }
                            if (item.Activity != null)
                            { item.Activity = null; }

                            if(item.EntityField2 !=null)
                            { item.EntityField2 = null; }

                            if (item.EntityField1 != null)
                            { item.EntityField1 = null; }

                            item.IsActiveCheckBox = item.IsActive == true ? "<span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
                            item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.EntityFieldID + "' ><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></a></div>";
                            item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.EntityFieldID + "' ><i class=\"fa fa-times - square - o\" aria-hidden=\"true\"></i></a></div>";

                            retList.Add(item);
                        }

                        return Json(new
                        {
                            sEcho = jQueryDataTablesModel.sEcho,
                            iTotalRecords = result.TotalCount,
                            iTotalDisplayRecords = result.TotalFilteredCount,
                            aaData = retList
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var readAsStringAsync = response.Content.ReadAsStringAsync();
                        return Content( readAsStringAsync.Result);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Gets the entityField.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult GetEntityField(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/EntityField";
                if (id == null)
                {
                    return PartialView("EditEntityField", new EntityField());
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        EntityField item = JsonConvert.DeserializeObject<EntityField>(content.Result,settings);
                        return PartialView("EditEntityField", item);
                    }
                    else
                    {
                          return Content(content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Edits the activity.
        /// </summary>
        /// <param name="entityField">The entity field.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEntityField(EntityField entityField)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/EntityField";
                string uriAdd = CommonHelper.BaseUri + "AdminController/EntityField/add";
                string uriUpdate = CommonHelper.BaseUri + "AdminController/EntityField/update";
                EntityField itemExists = null;
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check activity exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + entityField.EntityFieldID + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        itemExists = JsonConvert.DeserializeObject<EntityField>(content.Result,settings);
                    }

                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(entityField), Encoding.UTF8, "application/json");
                    //Insert
                    if (itemExists == null)
                    {
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {

                            return Content(Localizer.Current.GetString("Successfully Added"));
                        }
                        else
                        {
                            return Content( resultAdd.Result);
                        }
                    }
                    else//Update
                    {
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
                            return Content(Localizer.Current.GetString("Successfully Updated"));
                        }
                        else
                        {
                            return Content(resultOut.Result);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Deletes the entity field.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult DeleteEntityField(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/EntityField/delete";
                if (id == null)
                {
                    return View();
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent content1 = new StringContent(id.ToString());
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return Content(Localizer.Current.GetString("Successfully Deleted"));
                    }
                    else
                    {
                        return Content( content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }
        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns></returns>
        public ActionResult ExportEntityFieldForm(string dataList)
        {
            try
            {
                Export exportdata = new Export();
                exportdata.Controller = "Admin";
                exportdata.Entity = "EntityField";
                exportdata.DatatableParams = dataList;
                return PartialView("ExportControl", exportdata);
            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        [HttpPost]
        public FileResult ExportEntityFieldData(Export exportDetails)
        {
            try
            {
                FileContentResult file = base.ExportBase<EntityField>(exportDetails);
                if (file != null)
                {
                    return file;
                }
                else
                {
                    throw new System.Exception(Localizer.Current.GetString("ExportError"));
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "Entity"

        /// <summary>
        /// Entity this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Entity()
        {
            return View();
        }

        /// <summary>
        /// Gets the Entity list.
        /// </summary>
        /// <param name="jQueryDataTablesModel">The j query data tables model.</param>
        /// <returns></returns>
        public ActionResult GetEntityList(JQueryDataTablesModel jQueryDataTablesModel)
        {
            string uri = CommonHelper.BaseUri + "AdminController/Entity";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
                    gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;

                    //gridParams.IncludeForeignKeyValues = true;
                    gridParams.Includerelations = true;
                    gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
                    gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        GridResult<Entity> result = JsonConvert.DeserializeObject<GridResult<Entity>>(jsonString.Result);

                        List<Entity> retList = new List<DALEFModel.Entity>();
                        foreach (Entity item in result.Items)
                        {
                            if(item.Activity != null)
                            {
                                item.ActivityName = item.Activity.ActivityName;
                            }
                            item.IsActiveCheckBox = item.IsActive == true ? "<span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
                            item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.EntityID + "' ><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></a></div>";
                            item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.EntityID + "' ><i class=\"fa fa-times - square - o\" aria-hidden=\"true\"></i></a></div>";

                            retList.Add(item);
                        }

                        return Json(new
                        {
                            sEcho = jQueryDataTablesModel.sEcho,
                            iTotalRecords = result.TotalCount,
                            iTotalDisplayRecords = result.TotalFilteredCount,
                            aaData = retList
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var readAsStringAsync = response.Content.ReadAsStringAsync();
                        return Content( readAsStringAsync.Result);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="entityname">The entityname.</param>
        /// <returns></returns>
        public Entity GetEntityName(string entityname)
        {
            string uri = CommonHelper.BaseUri + "AdminController/GetEntities";
            List<Entity> fields = new List<Entity>();
			if (CommonHelper.CacheGet("EntitiesCache") != null)
			{
				fields = (List<Entity>)CommonHelper.CacheGet("EntitiesCache");
			}
			else
			{
				using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        fields = JsonConvert.DeserializeObject<List<Entity>>(content.Result);
                        CommonHelper.CacheAdd("EntitiesCache",fields);
                    }
                   
                }
            }
            return fields.Where(o => o.Name == entityname).FirstOrDefault();
        }

        /// <summary>
        /// Gets the Entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult GetEntity(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/Entity";
                if (id == null)
                {
                    return PartialView("EditEntity", new Entity());
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        Entity item = JsonConvert.DeserializeObject<Entity>(content.Result,settings);
                        return PartialView("EditEntity", item);
                    }
                    else
                    {
                          return Content(content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Edits the Entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEntity(Entity entity)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/Entity";
                string uriAdd = CommonHelper.BaseUri + "AdminController/Entity/add";
                string uriUpdate = CommonHelper.BaseUri + "AdminController/Entity/update";
                Activity itemExists = null;
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check activity exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + entity.EntityID + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        itemExists = JsonConvert.DeserializeObject<Activity>(content.Result, settings);
                    }

                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                    //Insert
                    if (itemExists == null)
                    {
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {

                            return Content(Localizer.Current.GetString("Successfully Added"));
                        }
                        else
                        {
                            return Content( resultAdd.Result);
                        }
                    }
                    else//Update
                    {
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
                            return Content(Localizer.Current.GetString("Successfully Updated"));
                        }
                        else
                        {
                            return Content(resultOut.Result);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult DeleteEntity(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/Entity/delete";
                if (id == null)
                {
                    return View();
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent content1 = new StringContent(id.ToString());
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return Content(Localizer.Current.GetString("Successfully Deleted"));
                    }
                    else
                    {
                        return Content( content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns></returns>
        public ActionResult ExportEntityForm(string dataList)
        {
            try
            {
                Export exportdata = new Export();
                exportdata.Controller = "Admin";
                exportdata.Entity = "Entity";
                exportdata.DatatableParams = dataList;
                return PartialView("ExportControl", exportdata);
            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        [HttpPost]
        public FileResult ExportEntityData(Export exportDetails)
        {
            try
            {
                FileContentResult file = base.ExportBase<Entity>(exportDetails);
                if (file != null)
                {
                    return file;
                }
                else
                {
                    throw new System.Exception(Localizer.Current.GetString("ExportError"));
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "EntityResource"

        /// <summary>
        /// EntityResource this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult EntityResource()
        {
            return View();
        }

        /// <summary>
        /// Gets the entity resource list.
        /// </summary>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public List<EntityResource> GetEntityResourceItems(bool refresh = false)
        {
            string uri = CommonHelper.BaseUri + "AdminController/EntityResourceItems";
            try
            {
                List<EntityResource> fields = null;
				if (refresh)
				{
                    CommonHelper.CacheRemove("EntityResourcesCache");
				}
				if (CommonHelper.CacheGet("EntityResourcesCache") != null)
				{
					fields = (List<EntityResource>)CommonHelper.CacheGet("EntityResourcesCache");
				}
				else
				{
					using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        GridParam gridParams = new GridParam();
                        gridParams.PageNo = 1;
                        gridParams.PageSize = 100000; //fetch all of them

                        StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            fields = JsonConvert.DeserializeObject<List<EntityResource>>(jsonString.Result);
                            CommonHelper.CacheAdd("EntityResourcesCache",fields);
                        }
                    }
                }
                return fields;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the EntityResource list.
        /// </summary>
        /// <param name="jQueryDataTablesModel">The j query data tables model.</param>
        /// <returns></returns>
        public ActionResult GetEntityResourceList(JQueryDataTablesModel jQueryDataTablesModel)
        {
            string uri = CommonHelper.BaseUri + "AdminController/GetEntityResourceList";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
                    gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;

              //      gridParams.IncludeForeignKeyValues = true;
                    gridParams.Includerelations = false;
                    gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
                    gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        GridResult<EntityResource> result = JsonConvert.DeserializeObject<GridResult<EntityResource>>(jsonString.Result);

                        List<EntityResource> retList = new List<DALEFModel.EntityResource>();
                        foreach (EntityResource item in result.Items)
                        {
                            item.IsActiveCheckBox = item.IsActive == true ? "<span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
                            item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.ResourceID + "' ><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></a></div>";
                            item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.ResourceID + "' ><i class=\"fa fa-times - square - o\" aria-hidden=\"true\"></i></a></div>";

                            retList.Add(item);
                        }

                        return Json(new
                        {
                            sEcho = jQueryDataTablesModel.sEcho,
                            iTotalRecords = result.TotalCount,
                            iTotalDisplayRecords = result.TotalFilteredCount,
                            aaData = retList
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var readAsStringAsync = response.Content.ReadAsStringAsync();
                        return Content( readAsStringAsync.Result);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Gets the entity resource.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public EntityResource GetEntityResourceByKey(string key, string culture)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/GetEntityResourceListByOrg"; 
                List<EntityResource> fields = new List<EntityResource>();
                int orgID = 1;
               

                if (CommonHelper.CacheGet("EntityResourcesCache") != null)
                {
                    fields = (List<EntityResource>)CommonHelper.CacheGet("EntityResourcesCache");
                }
                else
                {
                    HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
                    if (session != null && session["User"] != null)
                    {
                        orgID = ((User)session["User"]).OrgID ?? 1;
                    }
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //                  string orgID = "1";// default to Org 1
                        //                  if (CommonHelper.CacheGet("OrgID") != null)
                        //{
                        
                        //}
                            
                        HttpResponseMessage response = httpClient.GetAsync(uri + "/" + orgID).Result;
                        var content = response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            fields = JsonConvert.DeserializeObject<List<EntityResource>>(content.Result);
                            CommonHelper.CacheAdd("EntityResourcesCache", fields);
                        }

                    }
                }
                return fields.Where(o => o.ResourceKey == key && o.ResourceCulture.Contains(culture)).FirstOrDefault();
            }catch(System.Exception ex)
			{
                throw ex;
			}
        }

        /// <summary>
        /// Gets the EntityResource.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult GetEntityResource(int? id)
        {
            try
            {
                DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
                TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));

                string uri = CommonHelper.BaseUri + "AdminController/EntityResource";
                if (id == null)
                {
                    return PartialView("EditEntityResource", new EntityResource()
                    {
                        CreateDateTime = SATime,
                      //  CreatedByID = ((User)Session["User"]).UserID,
                        OrgID = 1,//((User)Session["User"]).OrgID,
                        IsActive = true,
                    }); 
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        EntityResource item = JsonConvert.DeserializeObject<EntityResource>(content.Result,settings);
                        return PartialView("EditEntityResource", item);
                    }
                    else
                    {
                          return Content(content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Edits the EntityResource.
        /// </summary>
        /// <param name="entityResource">The entity resource.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEntityResource(EntityResource entityResource)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/EntityResource";
                string uriAdd = CommonHelper.BaseUri + "AdminController/EntityResource/add";
                string uriUpdate = CommonHelper.BaseUri + "AdminController/EntityResource/update";
                EntityResource itemExists = null;
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check EntityResource exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + entityResource.ResourceID + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        itemExists = JsonConvert.DeserializeObject<EntityResource>(content.Result,settings);
                    }
                    entityResource.OrgID = 1;
                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(entityResource), Encoding.UTF8, "application/json");
                    //Insert
                    if (itemExists == null)
                    {
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {

                            return Content(Localizer.Current.GetString("Successfully Added"));
                        }
                        else
                        {
                            return Content( resultAdd.Result);
                        }
                    }
                    else//Update
                    {
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
                            return Content(Localizer.Current.GetString("Successfully Updated"));
                        }
                        else
                        {
                            return Content(resultOut.Result);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Deletes the EntityResource.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ActionResult DeleteEntityResource(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/EntityResource/delete";
                if (id == null)
                {
                    return View();
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent content1 = new StringContent(id.ToString());
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return Content(Localizer.Current.GetString("Successfully Deleted"));
                    }
                    else
                    {
                        return Content( content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns></returns>
        public ActionResult ExportEntityResourceForm(string dataList)
        {
            try
            {
                Export exportdata = new Export();
                exportdata.Controller = "Admin";
                exportdata.Entity = "EntityResource";
                exportdata.DatatableParams = dataList;
                return PartialView("ExportControl", exportdata);
            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        [HttpPost]
        public FileResult ExportEntityResourceData(Export exportDetails)
        {
            try
            {
                FileContentResult file = base.ExportBase<EntityResource>(exportDetails);
                if (file != null)
                {
                    return file;
                }
                else
                {
                    throw new System.Exception(Localizer.Current.GetString("ExportError"));
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "Menu"
        /// <summary>
        /// Menu this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Menu()
        {
            return View();
        }
        /// <summary>
        /// Gets the menu list.
        /// </summary>
        /// <param name="jQueryDataTablesModel">The j query data tables model.</param>
        /// <returns></returns>
        public ActionResult GetMenuList(JQueryDataTablesModel jQueryDataTablesModel)
        {
            string uri = CommonHelper.BaseUri + "AdminController/Menu";
            try
            { 
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
                    gridParams.PageSize = jQueryDataTablesModel.iDisplayLength;

                    //gridParams.IncludeForeignKeyValues = true;

                    gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
                    gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
                    
                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        GridResult<EntityMenu> result = JsonConvert.DeserializeObject<GridResult<EntityMenu>>(jsonString.Result);

                        List<EntityMenu> retList = new List<EntityMenu>();
                        foreach (EntityMenu item in result.Items)
                        {
                            item.IsActiveCheckBox = item.IsActive == true ? "<span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
                            item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.EntityMenuID + "' ><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></a></div>";
                            item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.EntityMenuID + "' ><i class=\"fa fa-times - square - o\" aria-hidden=\"true\"></i></a></div>";

                            retList.Add(item);
                        }

                        return Json(new
                        {
                            sEcho = jQueryDataTablesModel.sEcho,
                            iTotalRecords = result.TotalCount,
                            iTotalDisplayRecords = result.TotalFilteredCount,
                            aaData = retList
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var readAsStringAsync = response.Content.ReadAsStringAsync();
                        return Content( readAsStringAsync.Result);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
}
        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <returns></returns>
        public List<EntityMenu> GetMenuItems(User user)
        {
            string uri = CommonHelper.BaseUri + "AdminController/Menu";
            try {
                GridResult<EntityMenu> resultList = new GridResult<EntityMenu>();
                List<EntityMenu> menuItems = new List<EntityMenu>();
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = 0;
                    gridParams.PageSize = 1000;

                    gridParams.ListFilterBy.Add(new FilterField { Property = "IsActive", Operator = "=", Value = "true" });
                    gridParams.ListFilterBy.Add(new FilterField { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
                    
                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        resultList = JsonConvert.DeserializeObject<GridResult<EntityMenu>>(jsonString.Result);

                        var items = resultList.Items;

                       // User user = (User)Request.Cookies["User"].Value;

                        //if (user.IsAdmin)
                        //{
                        //    //Get permissions
                        //    foreach (EntityMenu mnu in items)
                        //    {
                        //        mnu.Param1 = "true";
                        //        menuItems.Add(mnu);
                        //    }
                        //}
                        //else
                        //{
                        foreach (EntityMenu mnu in items)
                        {
							if (mnu.Url == null)//If empty then this is a sub menu header
							{
								//Check all menu items under this sub menu header to see if any of them do not have NONE permission if true then show sub menu header
								//Get list of menus unde rthis sub menu
								List<int?> subMenuEntityIds = items.Where(o => o.EntityMenuParentID != null && o.EntityMenuParentID == mnu.EntityMenuID).ToList().Select(o => o.EntityID).ToList();
								List<int?> linkedEntitiesToMenu = this.GetEntities().Where(o => subMenuEntityIds.Contains(o.EntityID)).ToList().Select(o => o.ActivityID).ToList();
								if (user != null && user.UserRoleActivity != null)
								{
									if (user.UserRoleActivity.Count > 0)
									{
										if (user.UserRoleActivity.Where(o => o.StcPermissionID < 24 && linkedEntitiesToMenu.Contains(o.ActivityID) ).Count() > 0)
										{
											menuItems.Add(mnu);
										}
									}
								}
							}
							else
							{
								int? ActivityID = null;
								if (mnu.EntityID != null)
								{
									ActivityID = this.GetEntities().Where(o => o.EntityID == mnu.EntityID).FirstOrDefault().ActivityID;
								}
								if (ActivityID == null)
								{
									mnu.MenuDisplayName = mnu.MenuDisplayName + "-NoActivityOnEntity";
									menuItems.Add(mnu);
								}
								else
								{
									if (user != null && user.UserRoleActivity != null)
									{
										if (user.UserRoleActivity.Count > 0)
										{
											if (user.UserRoleActivity.Where(o => o.ActivityID == ActivityID && o.StcPermissionID < 23).Count() > 0)
											{
												menuItems.Add(mnu);
											}
										}
									}
								}
							}

						}
                        //}

                        return menuItems;
                    }
                    else
                    {
                        var readAsStringAsync = response.Content.ReadAsStringAsync();
                        throw new System.Exception(readAsStringAsync.Result);
                    }
                }
            }
            catch(System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the EntityMenu.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public ActionResult GetMenu(int? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("EditMenu", new EntityMenu());
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    string uri = CommonHelper.BaseUri + "AdminController/Menu";
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        EntityMenu menu = JsonConvert.DeserializeObject<EntityMenu>(content.Result,settings);
                        return PartialView("EditMenu", menu);
                    }
                    else
                    {
                        throw new System.Exception(content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Edits the activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditMenu(EntityMenu menu)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/Menu";
                string uriAdd = CommonHelper.BaseUri + "AdminController/Menu/add";
                string uriUpdate = CommonHelper.BaseUri + "AdminController/Menu/update";
                EntityMenu menuExists = null;
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check activity exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + menu.EntityMenuID + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        menuExists = JsonConvert.DeserializeObject<EntityMenu>(content.Result,settings);
                    }

                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
                    //Insert
                    if (menuExists == null)
                    {
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {

                            return Content(Localizer.Current.GetString("Successfully Added"));
                        }
                        else
                        {
                            return Content( resultAdd.Result);
                        }
                    }
                    else//Update
                    {
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
                            return Content(Localizer.Current.GetString("Successfully Updated"));
                        }
                        else
                        {
                            return Content(resultOut.Result);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        public ActionResult DeleteMenu(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/Menu/delete";
                if (id == null)
                {
                    return View();
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent content1 = new StringContent(id.ToString());
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        int row = JsonConvert.DeserializeObject<int>(content.Result);
                        return Content(Localizer.Current.GetString("Successfully Deleted"));
                    }
                    else
                    {
                        return Content( content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns></returns>
        public ActionResult ExportEntityMenuForm(string dataList)
        {
            try
            {
                Export exportdata = new Export();
                exportdata.Controller = "Admin";
                exportdata.Entity = "EntityMenu";
                exportdata.DatatableParams = dataList;
                return PartialView("ExportControl", exportdata);
            }
            catch (System.Exception ex)
            {
                return Content(ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        [HttpPost]
        public FileResult ExportEntityMenuData(Export exportDetails)
        {
            try
            {
                FileContentResult file = base.ExportBase<EntityMenu>(exportDetails);
                if (file != null)
                {
                    return file;
                }
                else
                {
                    throw new System.Exception(Localizer.Current.GetString("ExportError"));
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "StpData"
        /// <summary>
        /// StpData this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult StpData()
        {
            return View();
        }

        /// <summary>
        /// Gets the STP data list.
        /// </summary>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public List<StpData> GetStpDataPopulatedList(bool refresh = false)
        {
            try
            { 
                string uri = CommonHelper.BaseUri + "AdminController/StpData";
                List<StpData> fields = null;
				if (refresh)
				{
                    CommonHelper.CacheRemove("StpDataCache");
				}
				if (CommonHelper.CacheGet("StpDataCache") != null)
				{
				   fields = (List<StpData>)CommonHelper.CacheGet("StpDataCache");
				}
				else
				{
					using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        GridParam gridParams = new GridParam();
                        gridParams.PageNo = 1;
                        gridParams.PageSize = 100000; //fetch all of them
                      //  gridParams.Includerelations = true;
                        StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            fields = JsonConvert.DeserializeObject<List<StpData>>(jsonString.Result);
                            CommonHelper.CacheAdd("StpDataCache" , fields);
                        }
                        else
                        {
                            var readAsStringAsync = response.Content.ReadAsStringAsync();
                            throw new System.Exception(readAsStringAsync.Result);
                        }
                    }
                }
                return fields;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
       
        /// <summary>
        /// Gets the activity list.
        /// </summary>
        /// <param name="jQueryDataTablesModel">The j query data tables model.</param>
        /// <returns></returns>
        public ActionResult GetStpDataList(JQueryDataTablesModel jQueryDataTablesModel)
        {
            string uri = CommonHelper.BaseUri + "AdminController/StpData";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
                    gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;

                   // gridParams.IncludeForeignKeyValues = true;
                    gridParams.Includerelations = true;
                    gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
                    gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        GridResult<StpData> result = JsonConvert.DeserializeObject<GridResult<StpData>>(jsonString.Result);

                        List<StpData> retList = new List<DALEFModel.StpData>();
                        foreach (StpData item in result.Items)
                        {
                            if (item.StpDataType != null)
                            {
                                item.StpDataTypeDesc = item.StpDataTypeID.ToString() + " - "+ item.StpDataType.AppDataType;
                            }
                            item.IsActiveCheckBox = item.IsActive == true ? "<span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
                            item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.StpDataID + "' ><i class=\"fa fa-pencil\" aria-hidden=\"true\"></i></a></div>";
                            item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.StpDataID + "' ><i class=\"fa fa-times - square - o\" aria-hidden=\"true\"></i></a></div>";

                            retList.Add(item);
                        }

                        return Json(new
                        {
                            sEcho = jQueryDataTablesModel.sEcho,
                            iTotalRecords = result.TotalCount,
                            iTotalDisplayRecords = result.TotalFilteredCount,
                            aaData = retList
                        },
                        JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var readAsStringAsync = response.Content.ReadAsStringAsync();
                        return Content( readAsStringAsync.Result);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        /// <summary>
        /// Gets the StpData.
        /// </summary>
        /// <param name="StpDataID">The StpData identifier.</param>
        /// <returns></returns>
        public ActionResult GetStpData(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/StpData";
                if (id == null)
                {
                    return PartialView("EditStpData", new StpData());
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        StpData item = JsonConvert.DeserializeObject<StpData>(content.Result,settings);
                        return PartialView("EditStpData", item);
                    }
                    else
                    {
                          return Content(content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Gets the StpData.
        /// </summary>
        /// <param name="StpDataID">The StpData identifier.</param>
        /// <returns></returns>
        public StpData GetStpDataByID(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/StpData";
                if (id == null)
                {
                    return null;
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + id + "/true").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        StpData item = JsonConvert.DeserializeObject<StpData>(content.Result, settings);
                        return item;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Edits the activity.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditStpData(StpData stpdata)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/StpData";
                string uriAdd = CommonHelper.BaseUri + "AdminController/StpData/add";
                string uriUpdate = CommonHelper.BaseUri + "AdminController/StpData/update";
                StpData itemExists = null;
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check activity exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + stpdata.StpDataID + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        itemExists = JsonConvert.DeserializeObject<StpData>(content.Result,settings);
                    }

                    StringContent content1 = new StringContent(JsonConvert.SerializeObject(stpdata), Encoding.UTF8, "application/json");
                    //Insert
                    if (itemExists == null)
                    {
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {

                            return Content(Localizer.Current.GetString("Successfully Added"));
                        }
                        else
                        {
                            return Content( resultAdd.Result);
                        }
                    }
                    else//Update
                    {
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
                            return Content(Localizer.Current.GetString("Successfully Updated"));
                        }
                        else
                        {
                            return Content(resultOut.Result);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }
        }

        public ActionResult DeleteStpData(int? id)
        {
            try
            {
                string uri = CommonHelper.BaseUri + "AdminController/StpData/delete";
                if (id == null)
                {
                    return View();
                }
                using (HttpClient httpClient = new HttpClient())
                {
                    StringContent content1 = new StringContent(id.ToString());
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.PostAsync(uri + "/" + id, content1).Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Activity Deleted")));
                    }
                    else
                    {
                        return Content( content.Result);
                    }

                }
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the form.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns></returns>
        public ActionResult ExportStpDataForm(string dataList)
        {
            try
            {
                Export exportdata = new Export();
                exportdata.Controller = "Admin";
                exportdata.Entity = "StpData";
                exportdata.DatatableParams = dataList;
                return PartialView("ExportControl", exportdata);
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        /// <summary>
        /// Exports the data.
        /// </summary>
        /// <param name="exportData">The export data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        [HttpPost]
        public FileResult ExportData(Export exportDetails)
        {
            try
            {
                FileContentResult file = base.ExportBase<StpData>(exportDetails);
                if (file != null)
                {
                    return file;
                }
                else
                {
                    throw new System.Exception(Localizer.Current.GetString("ExportError"));
                }
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

       
    }
}

