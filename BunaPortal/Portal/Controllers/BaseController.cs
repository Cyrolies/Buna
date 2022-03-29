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
using BunaPortal.HTMLHelpers;
using System.Web;
using System.Collections.ObjectModel;
using System.Reflection;
using BunaPortal.Repository;

namespace BunaPortal
{
	[SessionTimeout]
    /// <summary>
    /// Base Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class BaseController : Controller 
    {
        public bool SaveUser(User newItem, out string result)
        {
            result = string.Empty;
            bool success = true;
            try
            {
                DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
                TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
                string uri = CommonHelper.BaseUri + "UserController/GetUser";//Using this to see if User already exists
                string uriAdd = CommonHelper.BaseUri + "UserController/User/add";
                string uriUpdate = CommonHelper.BaseUri + "UserController/User/update";
                User itemExists = null;
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Check exists
                    HttpResponseMessage response = httpClient.GetAsync(uri + "/" + newItem.UserID + "/false").Result;
                    var content = response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        itemExists = JsonConvert.DeserializeObject<User>(content.Result, settings);
                    }


                    //Insert
                    if (itemExists == null)
                    {
                        newItem.CreateDateTime = SATime;
                        
                        StringContent content1 = new StringContent(JsonConvert.SerializeObject(newItem), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseAdd = httpClient.PostAsync(uriAdd, content1).Result;
                        var resultAdd = responseAdd.Content.ReadAsStringAsync();
                        if (responseAdd.IsSuccessStatusCode)
                        {
                            Common.Common.SendEmail("", newItem.Email, "Your Buna registration has been approved", "Your login credentials have been created for you " + Environment.NewLine + " Please login using the following Username : " + newItem.UserName + " and Password  1111  " + Environment.NewLine + " You will then be asked to create your own password.");

                            var settings = new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            };
                            User newUser = JsonConvert.DeserializeObject<User>(resultAdd.Result, settings);
                            result = newUser.UserID.ToString();
                            success = true;
                        }
                        else
                        {
                            result = resultAdd.Result;
                            success = false;
                        }
                    }
                    else//Update
                    {
                        itemExists.IsActive = newItem.IsActive;
                        itemExists.ChangeDateTime = SATime;
                        StringContent content1 = new StringContent(JsonConvert.SerializeObject(itemExists), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseOut = httpClient.PostAsync(uriUpdate, content1).Result;
                        var resultOut = responseOut.Content.ReadAsStringAsync();
                        if (responseOut.IsSuccessStatusCode)
                        {
                            success = true;
                        }
                        else
                        {
                            result = resultOut.Result;
                            success = false;
                        }
                    }

                }

            }
            catch (System.Exception ex)
            {
                result = ExceptionHandler.Handle(ex).CreateDetailNoHtml();
            }
            return success;
        }
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
                    else if (jQueryDataTablesModel.uri.Contains("SupplierController"))
                    {
                        if (user.UserRoleID == 6)//Its a Supplier thats logged in then only show his row detail
                        {
                            gridParams.ListFilterBy.Add(new FilterField() { Property = "UserID", Operator = "=", Value = user.UserID.ToString() });
                        }
                       
                    }
                    else if (jQueryDataTablesModel.uri.Contains("ProductController"))
                    {
                        if (user.UserRoleID == 6)//Its a supplier thats logged in then only show his row detail
                        {
                            if(user.Supplier2 != null) //Linked to supplier entry
							{
                               gridParams.ListFilterBy.Add(new FilterField() { Property = "SupplierID", Operator = "=", Value = user.SupplierID.ToString() });
                            }
                        }
                        else //only show farmers in same country as champion logged in
                        {
                            gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
                        }
                    }
                    else if (jQueryDataTablesModel.uri.Contains("AssetController"))
                    {
                        if (user.UserRoleID == 5)//Its a Farmer thats logged in then only show his farms
                        {
                            if (user.Person2 != null) //Linked to supplier entry
                            {
                                gridParams.ListFilterBy.Add(new FilterField() { Property = "PersonID", Operator = "=", Value = user.Person2.FirstOrDefault().PersonID.ToString() });
                            }
                        }
                        else //only show farms in same country as champion logged in
                        {
                            gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
                        }
                    }
                    else if (jQueryDataTablesModel.uri.Contains("PersonController"))
                    {
                        if (user.UserRoleID == 5)//Its a Farmer thats logged in then only show his row detail
                        {
                            gridParams.ListFilterBy.Add(new FilterField() { Property = "UserID", Operator = "=", Value = user.UserID.ToString() });
                        }
                        else //only show farmers in same country as champion logged in
                        {
                            gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
                        }
                    }
                   
                    else if (jQueryDataTablesModel.uri.Contains("ProductionController"))
                    {
                        if (user.UserRoleID == 5)//If its a Farmer thats logged in then only show his production data
                        {
                            if (user.Person2 != null) //Linked to supplier entry
                            {
                                gridParams.ListFilterBy.Add(new FilterField() { Property = "UserID", Operator = "=", Value = user.Person2.FirstOrDefault().UserID.ToString() });
                            }
                            
                        }
                        else //only show farmers in same country as champion or ext office logged in
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
                    //using (HttpClient httpClient = new HttpClient())
                    //{
                    //    httpClient.DefaultRequestHeaders.Accept.Clear();
                    //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        gridParams.EntityName = entityname;
                        gridParams.PageNo = 0;
                        gridParams.PageSize = 100000;
                        
                        if (filters != null)
                        {
                            gridParams.ListFilterBy = filters;
                        }
                BaseRepository repo = new BaseRepository();
                switch (entityname.Trim())
                {
                    case "StpDataType":
                        {
                            return repo.GetData<StpDataType>("Select * from StpDataType ").ToList();// managerdbmng.GetList<StpDataType>(GetWhereClause<StpDataType>(filter)).ToList();
                        }
                    case "StpData":
                        {
                            return repo.GetData<StpData>("Select * from StpData ", gridParams).ToList();//managerdbmng.GetList<StpData>(GetWhereClause<StpData>(filter)).ToList();
                        }
                    case "StcDataType":
                        {
                            return repo.GetData<StcDataType>("Select * from StcDataType ").ToList();//managerdbmng.GetList<StcDataType>(GetWhereClause<StcDataType>(filter)).ToList();
                        }
                    case "StcData":
                        {
                            return repo.GetData<StcData>("Select * from StcData ", gridParams).ToList();//managerdbmng.GetList<StcData>(GetWhereClause<StcData>(filter)).ToList();
                        }
                    case "Entity":
                        {
                            return repo.GetData<Entity>("Select * from Entity ", gridParams).ToList();//managerdbmng.GetList<Entity>(GetWhereClause<Entity>(filter)).ToList();
                        }
                    case "EntityField":
                        {
                            return repo.GetData<EntityField>("Select * from EntityField ", gridParams).ToList();//managerdbmng.GetList<EntityField>(GetWhereClause<EntityField>(filter)).ToList();
                        }
                    case "EntityFieldDataType":
                        {
                            return repo.GetData<EntityFieldDataType>("Select * from EntityFieldDataType ", gridParams).ToList();// managerdbmng.GetList<EntityFieldDataType>(GetWhereClause<EntityFieldDataType>(filter)).ToList();
                        }
                    case "EntityMenu":
                        {
                            return repo.GetData<EntityMenu>("Select * from EntityMenu ", gridParams).ToList();//managerdbmng.GetList<EntityMenu>(GetWhereClause<EntityMenu>(filter)).ToList();
                        }
                    case "EntityResource":
                        {
                            return repo.GetData<EntityResource>("Select * from EntityResource ", gridParams).ToList();//managerdbmng.GetList<EntityResource>(GetWhereClause<EntityResource>(filter)).ToList();
                        }
                    case "Organization":
                        {
                            return repo.GetData<Organization>("Select * from Organization ", gridParams).ToList();//managerdbmng.GetList<Organization>(GetWhereClause<Organization>(filter)).ToList();
                        }
                    case "User":
                        {
                            return repo.GetData<User>("Select * from [User] ", gridParams).ToList();//managerdbmng.ExecuteLinqQuery<User>("select * from [user]").Where(GetWhereClause<User>(filter)).ToList();
                        }
                    case "UserRole":
                        {
                            return repo.GetData<UserRole>("Select * from UserRole ", gridParams).ToList();//managerdbmng.GetList<UserRole>(GetWhereClause<UserRole>(filter)).ToList();
                        }
                    case "UserRoleActivity":
                        {
                            return repo.GetData<UserRoleActivity>("Select * from UserRoleActivity ", gridParams).ToList();//managerdbmng.GetList<UserRoleActivity>(GetWhereClause<UserRoleActivity>(filter)).ToList();
                        }
                    case "Activity":
                        {
                            return repo.GetData<Activity>("Select * from Activity ", gridParams).ToList();//managerdbmng.GetList<Activity>(GetWhereClause<Activity>(filter)).ToList();
                        }

                    case "Application":
                        {
                            return repo.GetData<Application>("Select * from Application ", gridParams).ToList();//managerdbmng.GetList<Application>(GetWhereClause<Application>(filter)).ToList();
                        }
                    case "Contact":
                        {
                            return repo.GetData<Contact>("Select * from Contact ", gridParams).ToList();//managerdbmng.GetList<Contact>(GetWhereClause<Contact>(filter)).ToList();
                        }
                    case "Asset":
                        {
                            return repo.GetData<Asset>("Select * from Asset ", gridParams).ToList();//managerdbmng.GetList<Asset>(GetWhereClause<Asset>(filter)).ToList();
                        }
                    case "Person":
                        {
                            return repo.GetData<Person>("Select * from Person ", gridParams).ToList();//managerdbmng.GetList<Person>(GetWhereClause<Person>(filter)).ToList();
                        }
                    case "Supplier":
                        {
                            return repo.GetData<Supplier>("Select * from Supplier ", gridParams).ToList();//managerdbmng.GetList<Supplier>(GetWhereClause<Supplier>(filter)).ToList();
                        }
                    case "Consumable":
                        {
                            return repo.GetData<Consumable>("Select * from Consumable ", gridParams).ToList();//return managerdbmng.GetList<Consumable>(GetWhereClause<Consumable>(filter)).ToList();
                        }
                }
                //    StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                //    HttpResponseMessage response = httpClient.PostAsync(CommonHelper.BaseUri + "BaseApiController/DataList", content).Result;

                //    if (response.IsSuccessStatusCode)
                //    {
                //        var jsonString = response.Content.ReadAsStringAsync();
                //        jsonString.Wait();
                //        result = JsonConvert.DeserializeObject<IList>(jsonString.Result);

                //    }
                //    else
                //    {
                //        var readAsStringAsync = response.Content.ReadAsStringAsync();
                //        throw new Exception(readAsStringAsync.Result);
                //    }
                //}


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
               
                GridResult<SelectResult> dataList = new GridResult<SelectResult>();
				HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
                User user = null;
                if (session["User"] != null)
				{
					user = (User)session["User"];
				}
				
				if (field.Length > 0 && table.Length > 0)
                {
                   // string uri = CommonHelper.BaseUri + "AdminController/GetSelectList";
                    if (table.ToLower() != "userrole")//do not filter userroles by org same across all countries
                    {
                        //Filter by users org Note only display data with in users organization
                        if (where.Length > 0)
                        {
                            where = where + " and OrgID = " + ((User)session["User"]).OrgID.ToString();
                        }
                        else
						{
                            where = "  OrgID = " + ((User)session["User"]).OrgID.ToString();
                        }
                    }
                    if (table.ToLower() == "asset" && user != null)//check if farmer or not
                    {
                        //Filter by users org Note only display data with in users organization
                        if (user.UserRoleID == 5)
                        {
                            if (where.Length > 0)
                            {
                                where = where + " and PersonID = " + user.FarmerID;
                            }
                            else
                            {
                                where = "  PersonID = " + user.FarmerID;
                            }
                        }
                    }

                    SelectQuery qry = new SelectQuery() { fields = field, table = table, where = where, orderby = orderby, direction = direction };
                    BaseRepository repo = new BaseRepository();
                    dataList.Items = repo.GetData(qry.GetQuery());
                        //using (HttpClient httpClient = new HttpClient())
                        //{
                        //    httpClient.DefaultRequestHeaders.Accept.Clear();
                        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //    StringContent content = new StringContent(JsonConvert.SerializeObject(qry), Encoding.UTF8, "application/json");
                        //    HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                        //    var result = response.Content.ReadAsStringAsync();
                        //    if (response.IsSuccessStatusCode)
                        //    {
                        //        var settings = new JsonSerializerSettings
                        //        {
                        //            NullValueHandling = NullValueHandling.Ignore,
                        //            MissingMemberHandling = MissingMemberHandling.Ignore
                        //        };
                        //        dataList = JsonConvert.DeserializeObject<GridResult<SelectResult>>(result.Result, settings);
                        //    }

                    //}

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

        public List<SelectListItem> GetDataSelectList<T>(string searchTerm, string entityfield,string entityname, List<FilterField> extrafilters = null) where T : new()
        {
            try
            {
                List<FilterField> filtersList = new List<FilterField>();

                EntityField entityField = GetEntityFieldListItems().Where(p => p.EntityFieldName == entityfield && p.EntityDesc == entityname).FirstOrDefault();
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
                       
                    if(value.ToLower() == "true" || value.ToLower() == "false")
					{
                        value = "'" + value + "'";
					}
                    FilterField fil = new FilterField() { Property = property.Replace("\"", ""), Operator = oper.Replace("\"", ""), Value = value.Replace("\"", "") };
                    filtersList.Add(fil);
                    
                }
                if(extrafilters != null)
				{
                    filtersList.AddRange(extrafilters);
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
                 //   string name = Common.Common.ClearEntityFieldName(field.EntityFieldName);
                    //HELL knoes why this method above doesnt return organization back : Robin
                 //   name = name.Replace("Org", "Organization");

                    Entity entity = manager.GetEntityName(field.ForeignTable);
                    if (entity == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "Entity not found for Name = " + field.ForeignTable, Value = "0" };
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
                    List<EntityField> entityfields = allentityfields.Where(p => p.EntityDesc == entity.Name).ToList();
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
                    //StringBuilder whereCause = new StringBuilder();
                    //int count = 0;
                    //foreach (FilterField fd in filters)
                    //{
                    //    if (count == 0)
                    //    {
                    //        whereCause.Append(fd.Property + " " + fd.Operator + " '" + fd.Value + "'");
                    //    }
                    //    else // add and
                    //    {
                    //        whereCause.Append(" and " + fd.Property + " " + fd.Operator + " '" + fd.Value +"'");
                    //    }
                    //    count++;
                    //}

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
                //using (HttpClient httpClient = new HttpClient())
                //            {
                //                httpClient.DefaultRequestHeaders.Accept.Clear();
                //                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //                GridParam gridParams = new GridParam();
                //                gridParams.PageNo = 1;
                //                gridParams.PageSize = 100000; //fetch all of them
                //                gridParams.Includerelations = true;

                //                StringContent content = new StringContent(JsonConvert.SerializeObject(gridParams), Encoding.UTF8, "application/json");
                //                HttpResponseMessage response = httpClient.PostAsync(uri, content).Result;
                //                if (response.IsSuccessStatusCode)
                //                {
                //                    var jsonString = response.Content.ReadAsStringAsync();
                //                    jsonString.Wait();
                //fields = JsonConvert.DeserializeObject<List<EntityField>>(jsonString.Result);
                BaseRepository repo = new BaseRepository();
                fields = repo.GetEntityFieldList();
                CommonHelper.CacheAdd("EntityFieldsCache",fields);
     //               }
     //               else
					//{
     //                   var jsonString = response.Content.ReadAsStringAsync();
     //                   jsonString.Wait();
     //                   throw new Exception(jsonString.Result);
     //               }
      //          }
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
                BaseRepository repo = new BaseRepository();
                entities = repo.GetEntitiesList();
                CommonHelper.CacheAdd("EntitiesCache", entities);

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
                List<EntityField> fields = entityfields.Where<EntityField>(p => p.IsActive == true && p.EntityDesc == typeof(T).Name.Trim()).ToList();

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
                List<EntityField> fields = entityfields.Where(f => f.EntityDesc == typeof(T).Name.Trim() &&  f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo").ToList();

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
                List<EntityField> fields = entityfields.Where(f => f.EntityDesc == typeof(T).Name.Trim() && f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo").ToList();

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
                List<EntityField> fields = entityfields.Where(f => f.EntityDesc == typeof(T).Name.Trim() && f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo").ToList();

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
