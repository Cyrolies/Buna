using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using iTextSharp.text;//used to create pdf for export
using NPOI.HSSF.UserModel;//used to create excel spreadsheet for export
using DALEFModel;
using Common;
using Newtonsoft.Json;
using System.Collections;
using Models;
using CyroTechPortal.HTMLHelpers;
using System.Web;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CyroTechPortal
{
	[SessionTimeout]
    /// <summary>
    /// Base Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class BaseController : Controller 
    {
       
        #region HTTP calls

        public HttpResponseMessage GetGridData(JQueryDataTablesModel jQueryDataTablesModel,bool includeRelation = false)
        {
            HttpResponseMessage response = null;
            if (Session != null && Session["User"] != null)
            {
                User user = (User)Session["User"];

                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    GridParam gridParams = new GridParam();
                    gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
                    gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
                    gridParams.Includerelations = includeRelation;
                    gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
                    gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
                    //Filter by users org Note only display data with in users organization
                    if(gridParams.ListFilterBy == null)
                    {
                        gridParams.ListFilterBy = new List<FilterField>();
                    }
                    if (jQueryDataTablesModel.uri.Contains("UserController"))
                    {
                        if (user.UserID != 4)//If Admin and user screen then show all users else filter by org
                        {
                            gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
                        }
                    }
                    else //filter all other screens by org
                    {
                        gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
                    }
                    
                    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    response = httpClient.PostAsync(jQueryDataTablesModel.uri, content).Result;
                   
                }
            }
			else
			{
                RedirectToAction("Login", "Account");
			}
            return response;
        }
        #endregion

        #region Get Data List
        public IList GetDataList<T>(string entityname, List<FilterField> filters = null)
        {
            IList result = null;
            try
            {
                
                    //TODO build GridParam based on whereclause and entityname
                    AdminController cont = new AdminController();

                    GridParam gridParams = new GridParam();
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        gridParams.EntityName = entityname;
                        gridParams.PageNo = 0;
                        gridParams.PageSize = 100000;
                        gridParams.Includerelations = false;
                        if (filters != null)
                        {
                            gridParams.ListFilterBy = filters;
                        }
                        

                        StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync(CommonHelper.BaseUri + "BaseApiController/DataList", content).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = response.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            result = JsonConvert.DeserializeObject<IList>(jsonString.Result);

                        }
                        else
                        {
                            var readAsStringAsync = response.Content.ReadAsStringAsync();
                            throw new Exception(readAsStringAsync.Result);
                        }
                    }

                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		#endregion
		#region "SelectList"
		public ActionResult PrePopulateInput(string field, string table, string where = "", string orderby = "", string direction = "ASC", bool returnList = false)
        {
            
            GridResult<SelectResult> dataList = PrePopulateData(field, table, where, orderby, direction);
            return Json(new { result = dataList.Items }, JsonRequestBehavior.AllowGet);
        }
        public IList PrePopulateInputList(string field, string table, string where = "", string orderby = "", string direction = "ASC", bool returnList = false)
        {
            GridResult<SelectResult> dataList = PrePopulateData(field, table, where,orderby, direction);
            return dataList.Items.ToList();
        }
        public GridResult<SelectResult> PrePopulateData(string field, string table, string where = "", string orderby = "", string direction = "ASC")
        {
            try
            {
               
                GridResult<SelectResult> dataList = null;
				HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
				//if (session["User"] != null)
				//{
				//	User user = (User)session["User"];
				//}
				if (field.Length > 0 && table.Length > 0)
                    {
                        string uri = CommonHelper.BaseUri + "AdminController/GetSelectList";
                    ////Filter by users org Note only display data with in users organization
                    where = where + " and OrgID = " + ((User)session["User"]).OrgID.ToString();
                    SelectQuery qry = new SelectQuery() { fields = field, table = table, where = where, orderby = orderby, direction = direction };
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            StringContent content = new StringContent(JsonConvert.SerializeObject(qry), Encoding.UTF8, "application/json");
                            HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                            var result = response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                var settings = new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    MissingMemberHandling = MissingMemberHandling.Ignore
                                };
                                dataList = JsonConvert.DeserializeObject<GridResult<SelectResult>>(result.Result, settings);
                            }

                        }

                    }
                return dataList;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        

      	#region Get Foreign Key filter data list

		public ActionResult PopulateSelect2Filter(string searchTerm, string entityfield,string entityname)
        {
            try
            {
              return Json(this.GetJsonDataList<StpData>(searchTerm, entityfield, entityname), JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Content( ExceptionHandler.Handle(ex).CreateDetailNoHtml());
            }

        }

        public string GetJsonDataList<T>(string searchTerm, string entityfield, string entityname, bool returnJson = true) where T : new()
        {
            return JsonConvert.SerializeObject(this.GetDataSelectList<T>(searchTerm, entityfield, entityname));
            
        }

        public List<SelectListItem> GetDataSelectList<T>(string searchTerm, string entityfield,string entityname) where T : new()
        {
            try
            {
                List<FilterField> filtersList = new List<FilterField>();

                EntityField entityField = GetEntityFieldListItems().Where(p => p.EntityFieldName == entityfield && p.Entity.Name == entityname).FirstOrDefault();
                if(entityField.FilterBy == null)
                {
                    throw new Exception("Filter not specified for foreign key on entity field = "+ entityField.EntityFieldName);
                }
                string[] filters = entityField.FilterBy.Split('|');
                foreach (string fielditems in filters)
                {
                    string property = string.Empty;
                    string oper = string.Empty;
                    string value = string.Empty;
                    string[] fields = fielditems.Split(',');
                    foreach (string field in fields)
                    {

                        if (field.Substring(0, field.IndexOf('=')).ToLower() == ("Property").ToLower())
                        {
                            property = field.Substring(field.IndexOf('=') + 1);
                        }
                        if (field.Substring(0, field.IndexOf('=')).ToLower() == ("Operator").ToLower())
                        {
                            oper = field.Substring(field.IndexOf('=') + 1);
                        }
                        if (field.Substring(0, field.IndexOf('=')).ToLower() == ("Value").ToLower())
                        {
                            value = field.Substring(field.IndexOf('=') + 1);
                        }

                    }
                        FilterField fil = new FilterField() { Property = property.Replace("\"", ""), Operator = oper.Replace("\"", ""), Value = value.Replace("\"", "") };

                        //TODO add this in when session user exists
                        //if(fil.Property.ToLower() != ("IsSystem").ToLower() && User != "Admin")
                        //{

                        //}

                        filtersList.Add(fil);
                    
                }
                List<SelectListItem> datareturned = new List<SelectListItem>();
                
                if (filtersList.Count > 0)
                {
                    datareturned = GetDropDownBindingFields<T>(entityField, filtersList).ToList();
                }
                else
                {
                    datareturned = GetDropDownBindingFields<T>(entityField).ToList();
                }
                if (searchTerm != null)
                {
                    datareturned = datareturned.Where(o => o.Text.ToLower().Contains(searchTerm.ToLower())).ToList<SelectListItem>();
                }

                return datareturned;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        public IEnumerable<SelectListItem> GetDropDownBindingFields<T>(EntityField field, List<FilterField> filters = null)
        {
            try
            {
                AdminController manager = new AdminController();
                //var statuslist = new[] { (int)Enumerations.SupervisionStatus.Approved, (int)Enumerations.SupervisionStatus.PendingUsable };

                //All setup data drop downs
                if (field.EntityFieldName.Substring(0, 3) == "Stp" || field.EntityFieldName.Substring(0, 3) == "STP")
                {
                    int selectedItem = 0;
                    if (field.EntityFieldName == "StpDataTypeID")
                    {
                        var list = manager.GetDataList<T>("StpDataType", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StpDataTypeID", "AppDataType", selectedItem);
                        }

                    }
                    else
                    {
                        //List<StpData> list = new List<StpData>();
                        //string fieldname = Regex.Replace(field.EntityFieldName, "ID", "", RegexOptions.IgnoreCase);
                        //int stptypeid = Enumerations.GetEnumerationValue(typeof(Enumerations.SetupDataType), fieldname.Substring(3));
                        //AdminController controller = new AdminController();
                        //list = controller.GetStpDataPopulatedList().Where(o => o.StpDataTypeID == stptypeid).ToList();
                        var list = manager.GetDataList<T>("StpData", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StpDataID", "DataDescription", selectedItem);
                        }
                    }
                }
                //All static data drop downs
                else if (field.EntityFieldName.Substring(0, 3) == "Stc" || field.EntityFieldName.Substring(0, 3) == "STC")
                {
                    int selectedItem = 0;
                    if (field.EntityFieldName == "StcDataTypeID")
                    {
                        var list = manager.GetDataList<T>("StcDataType", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StcDataTypeID", "StaticDataType", selectedItem);
                        }

                    }
                    else
                    {
                        var list = manager.GetDataList<T>("StcData", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StcDataID", "Description", selectedItem);
                        }
                    }
                }
                else
                {
                    //string name = field.EntityFieldName.Replace("OrgID", "Organization").Replace("OrgId", "Organization").Replace("BugCreatedByContactID", "Contact").Replace("AssignedContactID", "Contact").Replace("CreatedByID", "User").Replace("SupervisorID", "User").Replace("ParentID", "").Replace("ID", "");
                    string name = Common.Common.ClearEntityFieldName(field.EntityFieldName);
                    //HELL knoes why this method above doesnt return organization back : Robin
                    name = name.Replace("Org", "Organization");

                    Entity entity = manager.GetEntityName(name);
                    if (entity == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "Entity not found for Name = " + name, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    if (field.ComboBoxDisplayFieldID == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "Entityfield.ComboBoxDisplayFieldID not specified on field name : " + field.EntityFieldName, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    List<EntityField> allentityfields = manager.GetEntityFieldListItems();
                    List<EntityField> entityfields = allentityfields.Where(p => p.Entity.Name == entity.Name).ToList();
                    EntityField primaryField = entityfields.Find(o => o.IsPrimaryKey == true);
                    if (primaryField == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "No IsPrimaryKey specified in EntityFields for : " + entity.Name, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    EntityField displayField = entityfields.Find(o => o.EntityFieldID == field.ComboBoxDisplayFieldID);
                    if (displayField == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "ComboBoxDisplayFieldID not found = : " + field.ComboBoxDisplayFieldID + " on entity :" + entity.Name, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    int selected = 0;
                    //get data for drop down list
                    //Build where clause
                    StringBuilder whereCause = new StringBuilder();
                    int count = 0;
                    foreach (FilterField fd in filters)
                    {
                        if (count == 0)
                        {
                            whereCause.Append(fd.Property + " " + fd.Operator + " '" + fd.Value + "'");
                        }
                        else // add and
                        {
                            whereCause.Append(" and " + fd.Property + " " + fd.Operator + " '" + fd.Value +"'");
                        }
                        count++;
                    }

                    //var list = manager.PrePopulateInputList(primaryField.EntityFieldName + " as id," + displayField.EntityFieldName + " as description", entity.TableName, whereCause.ToString(), primaryField.EntityFieldName);
                    var list = manager.GetDataList<T>(entity.Name,filters);

                    //if (name == "Organization")
                    //{
                    //    HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                    //    if (session["User"] != null)
                    //    {
                    //        User user = (User)session["User"];
                    //        selectedItem = (int)user.OrgID;
                    //    }
                    //}

                    //if (name == "tblCodeDesc")
                    //{
                    //    tblCodeDesc codeAll = new tblCodeDesc() { CodeCategory = "TPP", CodeDesc = Localizer.Current.GetString("All"), CodeValue = "0" };
                    //    list.Add(codeAll);
                    //    HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                    //    if (session["User"] != null)
                    //    {
                    //        User user = (User)session["User"];
                    //        selectedItem = (int)user.ShippingPoint;
                    //    }
                    //}

                    if (list != null)
                    {
                        return new SelectList(list, primaryField.EntityFieldName, displayField.EntityFieldName, selected);
                    }


                }

                //return empty selectlist no records found
                SelectListItem item = new SelectListItem() { Text = Localizer.Current.GetString("No Record"), Value = "0" };
                List<SelectListItem> selectList = new List<SelectListItem>() { item };
                return new SelectList(selectList, "Value", "Text");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Get EntityField
        /// <summary>
        /// Gets the entity field list.
        /// </summary>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public List<EntityField> GetEntityFieldListItems(bool refresh = false)
        {
            string uri = CommonHelper.BaseUri + "AdminController/EntityFieldList";
            List<EntityField> fields = null;
			if (refresh)
			{
                CommonHelper.CacheRemove("EntityFieldsCache");
			}
			if (CommonHelper.CacheGet("EntityFieldsCache") != null)
			{
				fields = (List<EntityField>)CommonHelper.CacheGet("EntityFieldsCache");
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
                    gridParams.Includerelations = true;

                     StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        fields = JsonConvert.DeserializeObject<List<EntityField>>(jsonString.Result);
                        CommonHelper.CacheAdd("EntityFieldsCache",fields);
                    }
                }
            }
            return fields;

        }
        #endregion
      
        #region Get Entities
        /// <summary>
        /// Gets the entity field list.
        /// </summary>
        /// <param name="refresh">if set to <c>true</c> [refresh].</param>
        /// <returns></returns>
        public List<Entity> GetEntities(bool refresh = false)
        {
            string uri = CommonHelper.BaseUri + "AdminController/GetEntities";
            List<Entity> entities = null;
            if (refresh)
            {
                CommonHelper.CacheRemove("EntitiesCache");
            }
            if (CommonHelper.CacheGet("EntitiesCache") != null)
            {
               entities = (List<Entity>)CommonHelper.CacheGet("EntitiesCache");
            }
            else
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        entities = JsonConvert.DeserializeObject<List<Entity>>(jsonString.Result);
                        CommonHelper.CacheAdd("EntitiesCache",entities);
                    }
                }
            }
            return entities;

        }
        #endregion

        #region "Export"
        public List<T> GetExportData<T>(Export exportData) where T : class, new()
		{
            List<T> dataList = new List<T>();
            dynamic results = JsonConvert.DeserializeObject<dynamic>(exportData.DatatableParams);
            List<string> searchList = new List<string>();
            List<string> propertyList = new List<string>();
            for (int i = 0; i < (int)results["iColumns"].Value; i++)
            {
                string count = i.ToString();
                searchList.Add(results["sSearch_" + count].Value.ToString());
                propertyList.Add(results["mDataProp_" + count].Value);
            }
            var filteredColumns = new List<FilterField>();
            filteredColumns = Common.Common.GetFilters(searchList.AsReadOnly(), propertyList.AsReadOnly());

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                GridParam gridParams = new GridParam();
                gridParams.EntityName = exportData.Entity;
                gridParams.ListFilterBy = filteredColumns;
                gridParams.Includerelations = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync(CommonHelper.BaseUri + "BaseApiController/DataList", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    dataList = JsonConvert.DeserializeObject<List<T>>(jsonString.Result);
                    
                }
                else
                {
                    var readAsStringAsync = response.Content.ReadAsStringAsync();
                    throw new System.Exception(readAsStringAsync.Result);
                }
            }
            return dataList;

        }
        public FileContentResult ExportBase<T>(Export exportData) where T : class, new()
        {
            try
            {
                
                //TODO add this to common class
                StringBuilder sb = new StringBuilder();
                sb.Append(exportData.HeaderDetail + System.Environment.NewLine);

                if (exportData.ExportBlank)//Blank excel sheet with just the column names 
                {
                    MemoryStream output = new MemoryStream();
                    output = this.ExportDataHeaderToExcel<T>(exportData.ShowDBColumnNames);
                    if (output == null)
                    {
                        return null;
                    }
                    output.Close();
                    output.Dispose();
                    return File(output.ToArray(), "application/vnd.ms-excel", exportData.Filename + ".xls");
                }

                //Get data using same filters and sorting as in the datagrid
                List<T> dataList = GetExportData<T>(exportData);
                if(dataList == null)
				{
                    return null;
				}
                if (exportData.IncludeDetailsInExport)
                {
                    sb.Append(" Export details :" + System.Environment.NewLine);
                    sb.Append(" Rows Returns : " + dataList.Count() + System.Environment.NewLine);
                    dynamic results = JsonConvert.DeserializeObject<dynamic>(exportData.DatatableParams);
                   // List<string> searchList = new List<string>();
                   // List<string> propertyList = new List<string>();
                    for (int i = 0; i < (int)results["iColumns"].Value; i++)
                    {
                        string count = i.ToString();
                        sb.Append(" Filters : " + results["sSearch_" + count].Value + System.Environment.NewLine);
                      //  searchList.Add(results["sSearch_" + count].Value);
                        sb.Append(" Sort order Prop : " + results["mDataProp_" + count].Value + System.Environment.NewLine);
                      //  propertyList.Add(results["mDataProp_" + count].Value);
                    }
                    
					
					//if (exportData.CurrentPageOnly)
					//{
					//	sb.Append(" Page Number : " + Convert.ToInt32(form.GetValue("currentPage").AttemptedValue));
					//}
				}

                if (exportData.Pdf)
                {
                    MemoryStream output = new MemoryStream();
                    output = this.ExportDataToPDF<T>(dataList, exportData.IncludeDetailsInExport, sb.ToString());
                    if (output == null)
                    {
                        return null;
                    }
                    output.Close();
                    output.Dispose();

                    return File(output.ToArray(), "application/pdf", exportData.Filename + ".pdf");
                }

                if (exportData.Excel)
                {
                    MemoryStream output = new MemoryStream();
                    output = this.ExportDataToExcel<T>(dataList, exportData.IncludeDetailsInExport, sb.ToString());
                    if (output == null || dataList == null || dataList.Count() == 0)
                    {
                        return null;
                    }
                    output.Close();
                    output.Dispose();

                    return File(output.ToArray(), "application/vnd.ms-excel", exportData.Filename + ".xls");
                }
                if (exportData.Csv)
                {
                    MemoryStream output = new MemoryStream();
                    output = this.ExportDataToCSV<T>(dataList, exportData.IncludeDetailsInExport, sb.ToString());
                    if (output == null)
                    {
                        return null;
                    }
                    output.Close();
                    output.Dispose();

                    return File(output.ToArray(), "text/comma-separated-values", exportData.Filename + ".csv");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        /// <summary>
        /// Exports the data header to excel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeDetails">if set to <c>true</c> [include details].</param>
        /// <param name="details">The details.</param>
        /// <param name="exactColumnName">if set to <c>true</c> [exact column name].</param>
        /// <returns></returns>
        public MemoryStream ExportDataHeaderToExcel<T>(bool exactDBColumnName) where T : class, new()
        {
            try
            {
                //Create new Excel workbook
                var workbook = new HSSFWorkbook();

                //Create new Excel sheet
                var sheet = workbook.CreateSheet();

                //Document doc = new Document(PageSize.A4, 10, 10, 10, 10);

                MemoryStream output = new MemoryStream();

                //TODO add functionality here for any Headers or custom client PDF formating 

                //Add content to the document
                //Get EntityField details

                AdminController adminmng = new AdminController();
                List<EntityField> entityfields = adminmng.GetEntityFieldListItems();
                List<EntityField> fields = entityfields.Where<EntityField>(p => p.IsActive == true && p.Entity.Name == typeof(T).Name.Trim()).ToList();

                int numOfColumns = fields.Count();
                //iTextSharp.text.pdf.PdfPTable dataTable = new iTextSharp.text.pdf.PdfPTable(numOfColumns);

                //ADD Title
                //iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now));
                //cell.Colspan = numOfColumns;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //dataTable.AddCell(cell);
                ////----------------------
                //dataTable.HeaderRows = 1;
                //dataTable.DefaultCell.BorderWidth = 1;
                //dataTable.DefaultCell.Padding = 3;
                //string title = "Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now;
                ////Add details if required
                //if (includeDetails)
                //{
                //    title = title + " " + details;
                //}
                ////Create a Title row
                //var titleRow = sheet.CreateRow(0);
                //titleRow.CreateCell(0).SetCellValue(title);
                //TODO add formatting to title and headers
                //Create a header row
                var headerRow = sheet.CreateRow(1);
                //'Add headers
                int cellNo = 0;
                //foreach (var prop in ent.GetType().GetProperties())
                //{
                foreach (EntityField field in fields)
                {
                    //if (prop.Name.Trim() == field.EntityFieldName.Trim())
                    //{
                    if ((!field.IsPrimaryKey) && field.IsActive == true)
                    {
                        if (exactDBColumnName)
                        {
                            headerRow.CreateCell(cellNo).SetCellValue(field.EntityFieldName.Trim());
                        }
                        else
                        {
                            headerRow.CreateCell(cellNo).SetCellValue(field.DisplayName.Trim());
                        }
                        cellNo++;
                    }

                    //}
                }

                //}
                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                workbook.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Exports the data to excel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="includeDetails">if set to <c>true</c> [include details].</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        public MemoryStream ExportDataToExcel<T>(List<T> list, bool includeDetails, string header) where T : class, new()
        {
            try
            {
                //Create new Excel workbook
                var workbook = new HSSFWorkbook();

                //Create new Excel sheet
                var sheet = workbook.CreateSheet();

                //Document doc = new Document(PageSize.A4, 10, 10, 10, 10);

                MemoryStream output = new MemoryStream();

                //TODO add functionality here for any Headers or custom client PDF formating 

                //Add content to the document
                //Get EntityField details
                AdminController adminmng = new AdminController();
                List<EntityField> entityfields = adminmng.GetEntityFieldListItems();
                List<EntityField> fields = entityfields.Where(f => f.Entity.Name == typeof(T).Name.Trim() &&  f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo").ToList();

                int numOfColumns = fields.Count();
                //iTextSharp.text.pdf.PdfPTable dataTable = new iTextSharp.text.pdf.PdfPTable(numOfColumns);

                //ADD Title
                //iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now));
                //cell.Colspan = numOfColumns;
                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                //dataTable.AddCell(cell);
                ////----------------------
                //dataTable.HeaderRows = 1;
                //dataTable.DefaultCell.BorderWidth = 1;
                //dataTable.DefaultCell.Padding = 3;
                //TODO add formatting to title and headers       
                //Create a header row
                var cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                cellStyle.WrapText = true;
                
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue(header);
               // headerRow.CreateCell(0).CellStyle = cellStyle;
                //Add details if required
                if (includeDetails)
                {
                    string title = "Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now;
                    var detailRow = sheet.CreateRow(1);
                    detailRow.CreateCell(0).SetCellValue(title);
                }

                //Add column headers
                var columnheaderRow = sheet.CreateRow(2);
                int cellNo = 0;
                foreach (var prop in list[0].GetType().GetProperties())
                {
                    foreach (EntityField field in fields)
                    {

                        if (prop.Name.Trim() == field.EntityFieldName.Trim())
                        {
                            if (!field.IsPrimaryKey)
                            {
                                //if (field.EntityID == 36 && field.XMLTab != null)
                                //{
                                //    columnheaderRow.CreateCell(cellNo).SetCellValue(field.XMLTab.Trim());
                                //}
                                //else
                                //{
                                columnheaderRow.CreateCell(cellNo).SetCellValue(field.DisplayName.Trim());
                                //}
                                cellNo++;
                            }

                        }
                    }

                }
                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);
                
                int rowNumber = 3; //3rd row after header row (titlerow is 0)
                int cellsNo = 0;
                //Add Data Rows
                foreach (var item in list)
                {
                    //Create a new row
                    var row = sheet.CreateRow(rowNumber++);
                    cellsNo = 0;
                    var itemProperties = item.GetType().GetProperties();
                    string propName = string.Empty;
                    for (int i = 0; i < itemProperties.Length; i++)
                    {
                        if (itemProperties[i].PropertyType != typeof(object) || itemProperties[i].PropertyType != typeof(System.Array) || itemProperties[i].PropertyType != typeof(System.Collections.ArrayList) || itemProperties[i].PropertyType != typeof(System.Collections.Hashtable))
                        {

                            propName = itemProperties[i].Name;

                            foreach (EntityField field in fields)
                            {

                                if (propName.Trim() == field.EntityFieldName.Trim())
                                {
                                    if (!field.IsPrimaryKey)
                                    {


                                        if (itemProperties[i].GetValue(item, null) != null)
                                        {
                                            if (field.IsForeignKey)
                                            {
                                                if (field.ComboBoxDisplayFieldID != null)
                                                {
                                                    EntityField selectEntityField = entityfields.Where(o => o.EntityFieldID == field.ComboBoxDisplayFieldID).FirstOrDefault();
                                                    string selectField = selectEntityField.EntityFieldDataTypeID == 12 ? selectEntityField.DisplayName : selectEntityField.EntityFieldName;//Check if ConcatField type if true then use displayname column
                                                    string primaryField = entityfields.Where(o => o.EntityID == selectEntityField.EntityID && o.IsPrimaryKey == true).FirstOrDefault().EntityFieldName;
                                                    SelectResult foreignItem = null;
                                                   
                                                    string foreignFieldDesc = string.Empty;
                                                    if (field.EntityFieldName.Substring(0, 3).ToUpper() == "STP")
                                                    {
                                                        foreignItem = PrePopulateInputList("DataDescription as Description ", "StpData", "StpDataID = " + itemProperties[i].GetValue(item, null).ToString()).ToList2<SelectResult>().FirstOrDefault();
                                                    }
                                                    else if (field.EntityFieldName.Substring(0, 3).ToUpper() == "STC")
                                                    {
                                                        foreignItem = PrePopulateInputList("Description", "StcData", "StcDataID = " + itemProperties[i].GetValue(item, null).ToString()).ToList2<SelectResult>().FirstOrDefault();
                                                    }
                                                    else
                                                    {
                                                        foreignItem = PrePopulateData(selectField + " as Description ", field.ForeignTable, primaryField + " = " + itemProperties[i].GetValue(item, null).ToString()).Items.ToList().FirstOrDefault();
                                                    }
                                                        row.CreateCell(cellsNo).SetCellValue(foreignItem != null ? foreignItem.Description : "");

                                                        cellsNo++;
                                                }
                                                else
                                                {
                                                    row.CreateCell(cellsNo).SetCellValue("");
                                                    cellsNo++;

                                                }
                                            }
                                            else
                                            {
                                                //dataTable.AddCell(itemProperties[i].GetValue(item, null).ToString());
                                                row.CreateCell(cellsNo).SetCellValue(itemProperties[i].GetValue(item, null).ToString());
                                                cellsNo++;
                                            }
                                        }
                                        else
                                        {
                                            row.CreateCell(cellsNo).SetCellValue("");
                                            cellsNo++;
                                        }
                                    }

                                }
                            }
                        }
                    }

                }

                workbook.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exports the data to PDF.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="includeDetails">if set to <c>true</c> [include details].</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        public MemoryStream ExportDataToPDF<T>(List<T> list, bool includeDetails, string header) where T : class, new()
        {
            try
            {
                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate());
                // Document doc = new Document(new Rectangle(600, 800), 10, 10, 10, 10);

                MemoryStream output = new MemoryStream();
                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, output);
                doc.SetPageSize(new iTextSharp.text.Rectangle(600, 800));
                doc.Open();
                //TODO add functionality here for any Headers or custom client PDF formating 

                //Add content to the document
                //Get EntityField details
                AdminController adminmng = new AdminController();
                List<EntityField> entityfields = adminmng.GetEntityFieldListItems();
                List<EntityField> fields = entityfields.Where(f => f.Entity.Name == typeof(T).Name.Trim() && f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo").ToList();

                int numOfColumns = fields.Count();
                iTextSharp.text.pdf.PdfPTable dataTable = new iTextSharp.text.pdf.PdfPTable(numOfColumns);

                //ADD Title
                if (includeDetails)
                {
                    iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now));
                    cell.Colspan = numOfColumns;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    dataTable.AddCell(cell);
                }
                //----------------------
                //Add header
                iTextSharp.text.pdf.PdfPCell celldetails = new iTextSharp.text.pdf.PdfPCell(new Phrase(header));
                celldetails.Colspan = numOfColumns;
                celldetails.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                dataTable.AddCell(celldetails);
                
                //------------------------
                dataTable.HeaderRows = 3;
                dataTable.DefaultCell.BorderWidth = 1;
                dataTable.DefaultCell.Padding = 3;

                //'Add column headers
                foreach (var prop in list[0].GetType().GetProperties())
                {
                    foreach (EntityField field in fields)
                    {

                        if (prop.Name.Trim() == field.EntityFieldName.Trim())
                        {
                            if (!field.IsPrimaryKey)
                            {
                                dataTable.AddCell(field.DisplayName.Trim());
                            }

                        }
                    }

                }
                //Add Data Rows
                foreach (var item in list)
                {
                    var itemProperties = item.GetType().GetProperties();
                    string propName = string.Empty;
                    for (int i = 0; i < itemProperties.Length; i++)
                    {
                        propName = itemProperties[i].Name;
                        foreach (EntityField field in fields)
                        {

                            if (propName.Trim() == field.EntityFieldName.Trim())
                            {
                                if (!field.IsPrimaryKey)
                                {
                                    if (itemProperties[i].GetValue(item, null) != null)
                                    {
                                        if (field.IsForeignKey)
                                        {
                                            if (field.ComboBoxDisplayFieldID != null)
                                            {

                                                //EntityField selectEntityField = entityfields.Where(o => o.EntityFieldID == field.ComboBoxDisplayFieldID).FirstOrDefault();
                                                //string tableName = entityfields.Where(o => o.EntityID == selectEntityField.EntityID).FirstOrDefault().Entity.TableName;
                                                //string selectField = selectEntityField.EntityFieldName;
                                                //string primaryField = entityfields.Where(o => o.EntityID == selectEntityField.EntityID && o.IsPrimaryKey == true).FirstOrDefault().EntityFieldName;

                                                //PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                                //Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                                //string sqlQuery = "Select cast(" + selectField + " as varchar) as description from [" + tableName + "] Where " + primaryField + " = " + ID;
                                                //object[] paras = { };
                                                //TODO create a executelinqquery in API
                                                //SelectResult result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                                //dataTable.AddCell(result.Description);
                                                dataTable.AddCell(itemProperties[i].GetValue(item, null).ToString());
                                            }
                                            else
                                            {
                                                dataTable.AddCell("");
                                            }
                                        }
                                        else
                                        {
                                            dataTable.AddCell(itemProperties[i].GetValue(item, null).ToString());
                                        }
                                    }
                                    else
                                    {
                                        dataTable.AddCell("");
                                    }
                                }

                            }
                        }
                    }

                }

                //Add table to the document
                doc.Add(dataTable);

                //Close the document
                doc.Close();

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Exports the data to CSV.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="includeDetails">if set to <c>true</c> [include details].</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        public MemoryStream ExportDataToCSV<T>(List<T> list, bool includeDetails, string header) where T : class, new()
        {
            try
            {
                MemoryStream output = new MemoryStream();
                StreamWriter writer = new StreamWriter(output, Encoding.UTF8);
                //Add content to the document
                //Get EntityField details
                AdminController adminmng = new AdminController();
                List<EntityField> entityfields = adminmng.GetEntityFieldListItems();
                List<EntityField> fields = entityfields.Where(f => f.Entity.Name == typeof(T).Name.Trim() && f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo").ToList();

                int numOfColumns = fields.Count();
                string title = string.Empty;
                //ADD detials
                if (includeDetails)
                {
                    title = "Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now + System.Environment.NewLine;
                }
                //Add header if required
                title = title + header;
                
                writer.Write(title);
                writer.WriteLine();


                //'Add headers
                foreach (var prop in list[0].GetType().GetProperties())
                {
                    foreach (EntityField field in fields)
                    {
                        if (prop.Name.Trim() == field.EntityFieldName.Trim())
                        {
                            if (!field.IsPrimaryKey)
                            {
                                writer.Write(field.DisplayName.Trim() + ",");
                            }

                        }
                    }

                }
                writer.WriteLine();

                //Add Data Rows
                foreach (var item in list)
                {
                    var itemProperties = item.GetType().GetProperties();
                    string propName = string.Empty;
                    for (int i = 0; i < itemProperties.Length; i++)
                    {
                        propName = itemProperties[i].Name;
                        foreach (EntityField field in fields)
                        {
                            if (propName.Trim() == field.EntityFieldName.Trim())
                            {
                                if (!field.IsPrimaryKey)
                                {
                                    if (itemProperties[i].GetValue(item, null) != null)
                                    {
                                        if (field.IsForeignKey)
                                        {
                                            if (field.ComboBoxDisplayFieldID != null)
                                            {
                                                //EntityField selectEntityField = entityfields.Where(o => o.EntityFieldID == field.ComboBoxDisplayFieldID).FirstOrDefault();
                                                //string tableName = entityfields.Where(o => o.EntityID == selectEntityField.EntityID).FirstOrDefault().Entity.TableName;
                                                //string selectField = selectEntityField.EntityFieldName;
                                                //string primaryField = entityfields.Where(o => o.EntityID == selectEntityField.EntityID && o.IsPrimaryKey == true).FirstOrDefault().EntityFieldName;

                                                //PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                                //Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                                //string sqlQuery = "Select cast(" + selectField + " as varchar) as description from [" + tableName + "] Where " + primaryField + " = " + ID;
                                                //object[] paras = { };
                                                //TODO create API call for this
                                                //SelectResult result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                                //writer.Write(result.Description + ",");

                                            }
                                            else
                                            {
                                                writer.Write(",");
                                            }
                                        }
                                        else
                                        {
                                            writer.Write(itemProperties[i].GetValue(item, null).ToString() + ",");
                                        }
                                    }
                                    else
                                    {
                                        writer.Write(",");
                                    }
                                }

                            }
                        }
                    }

                }

                writer.WriteLine();

                writer.Flush();
                output.Position = 0;
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
