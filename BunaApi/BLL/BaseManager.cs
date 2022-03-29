using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Reflection;
using DALEFModel;
using DALBase;
using DALEFBase;
using Common;
using iTextSharp.text;//used to create pdf for export
using NPOI.HSSF.UserModel;//used to create excel spreadsheet for export
using NPOI.HPSF;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.HSSF.Model;


namespace DSDBLL
{
    public class BaseManager 
    {
        private IRepository repositoryBase;
        public int shippingPointID;
        public string shippingPoint = "%";
        public BaseManager()
        {
              //   shippingPointID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("ShippingPoint"));
            //if (shippingPointID > 0)
            //{
            //    ShipmentManager mng = new ShipmentManager();
            //    shippingPoint = mng.Get<CodeDesc>(o => o.CodeCategory == "TPP" && o.Id == shippingPointID).CodeDescription;
            //}
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public Enumerations.ModalContext Model {get;set;}

        public int OrganizationID { get; set; }
        
        public int SetSupervisionStatus(Enumerations.RepositoryAction action,bool isPendingUsable)
        {
                if (Convert.ToBoolean(ConfigurationSettings.AppSettings["SupervisionOn"]))
                {
                    if(action == Enumerations.RepositoryAction.Add || action == Enumerations.RepositoryAction.Update)
                    {
                        //Check to see if this entity must be set to pending usable
                        if(isPendingUsable)
                        {
                            return 347;//Pending Usable
                        }
                        else
                        {
                            return 2;//Pending
                        }
                    }
                    else
                    {
                        return 348;//Pending Deletion
                    }
                }
                else
                {
                    
                    return 1;//Approved
                }
        }
        /// <summary>
        /// Gets or sets the Repository.
        /// </summary>
        /// <value>
        /// The Repository
        /// </value>
        public IRepository Repository 
        { 
            get
            {
                    IDALFactory factory = new DALFactory(Model);
                    IUnitOfWork unitOfWork = factory.CurrentUoW;
                    IRepository repository = new RepositoryBase(unitOfWork);
                    repositoryBase = repository;
               
                return repositoryBase;
                
            }
        }


        #region "Create filter where clause"
        //public IQueryable<T> Get(bool includeRelations, GridParam filters)
        //{

        //    Expression<Func<T, bool>> where = null;
        //    foreach (FilterField field in filters.ListFilterBy)
        //    {
        //        if (where == null)
        //        {
        //            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<T>(field.Property, field.Operator, field.Value);
        //        }
        //        else
        //        {
        //            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<T>(field.Property, field.Operator, field.Value, where);
        //        }

        //    }

        //    try
        //    {
        //        IQueryable<T> list;

        //        if (includeRelations)// Add includepaths into method 
        //        {
        //            //Add Includes here example below
        //            //STARTCUSTOMCODE
        //            List<Expression<Func<T, object>>> includepaths = new List<Expression<Func<T, object>>>();
        //            includepaths.Add(p => p.StcData);
        //            //ENDCUSTOMCODE

        //            //APPLY FILTERS
        //            if (where != null)
        //            {
        //                list = Repository.GetList<T>(where);
        //            }
        //            else
        //            {
        //                list = Repository.GetList<T, string>(includepaths);
        //            }

        //        }
        //        else
        //        {
        //            if (where != null)
        //            {
        //                list = Repository.GetList<T>(where);
        //            }
        //            else
        //            {
        //                list = Repository.GetList<T>();
        //            }

        //        }

        //        //APPLY ALL SORTING

        //        if (filters.ListOrderBy != null && filters.ListOrderBy.Count() > 0)
        //        {
        //            foreach (var sort in filters.ListOrderBy)
        //            {
        //                list = list.OrderBy(sort.Key, sort.Value);
        //            }
        //        }
        //        else
        //        {
        //            list = list.OrderBy(o => o.MenuDisplayName);
        //        }
        //        //APPLY PAGE SIZE
        //        if (filters.PageNo > 0 && filters.PageSize > 0)
        //        {
        //            list = list.Skip((filters.PageNo - 1) * filters.PageSize).Take(filters.PageSize);
        //        }


        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public Expression FilterDevices(IEnumerable<Device> devices, IEnumerable<Func<Device, string>> filters, string filterValue)
        //{
        //    foreach (var filter in filters)
        //    {
        //        devices = devices.Where(d => filter(d).Equals(filterValue));
        //    }

        //    return devices;
        //}
        //public Expression GetFilterExpression(FilterField filterField)
        //{
        //    Expression filterExpression = null;
        //    Expression comparison = null;
        //    //if (filterField.Value.GetType() != typeof(JArray))
        //    //{
        //    //    filterExpression = CompileComparison(filterField.Property, filterField.Value, filterField.Operator, filterField.exactMatch);
        //    //}
        //    //else
        //    //{
        //        foreach (dynamic filterValue in filterField.Value)
        //        {
        //            dynamic value = Convert.ChangeType(filterValue.Value, property.Type);
        //            comparison = CompileComparison(filterField.Property, value, filterField.Operator, filterField.exactMatch);

        //            if (filterExpression == null)
        //            {
        //                filterExpression = comparison;
        //            }
        //            else
        //            {
        //                if (filterField.Operator == "in" || filterField.Operator == "like")
        //                    filterExpression = Expression.OrElse(filterExpression, comparison);
        //                else
        //                    filterExpression = Expression.AndAlso(filterExpression, comparison);
        //            }
        //        }
        //    //}

        //    return filterExpression;
        //}

        //private Expression CompileComparison(Expression property, dynamic value, string @operator, bool exactMatch)
        //{
        //    Expression comparison = null;
        //    Expression constant = Expression.Convert(Expression.Constant(value), property.Type);
        //    MethodInfo method = null;

        //    switch (@operator)
        //    {
        //        case "<":
        //            comparison = Expression.LessThan(property, constant);
        //            break;
        //        case "<=":
        //            comparison = Expression.LessThanOrEqual(property, constant);
        //            break;
        //        case "=":
        //            comparison = Expression.Equal(property, constant);
        //            break;
        //        case "&gt;=":
        //            comparison = Expression.GreaterThanOrEqual(property, constant);
        //            break;
        //        case "&gt;":
        //            comparison = Expression.GreaterThan(property, constant);
        //            break;
        //        case "!=":
        //            comparison = Expression.NotEqual(property, constant);
        //            break;
        //        case "in":
        //            comparison = Expression.Equal(property, constant);
        //            break;
        //        case "like":
        //            if (value.GetType() == typeof(String))
        //            {
        //                method = typeof(ExtensionMethods).GetMethod("Contains");
        //                comparison = Expression.Call(null, method, property, Expression.Constant(value), Expression.Constant(StringComparison.OrdinalIgnoreCase));
        //            }
        //            else
        //            {
        //                comparison = Expression.Equal(property, constant);
        //            }
        //            break;
        //        default:
        //            if (!exactMatch && value.GetType() == typeof(String))
        //            {
        //                method = typeof(ExtensionMethods).GetMethod("Contains");
        //                comparison = Expression.Call(null, method, property, Expression.Constant(value.ToString()), Expression.Constant(StringComparison.OrdinalIgnoreCase));
        //            }
        //            else
        //            {
        //                comparison = Expression.Equal(property, constant);
        //            }
        //            break;
        //    }

        //    return comparison;
        //}

        //private Expression GetDeepPropertyExpression(Expression initialInstance, IList<string> property)
        //{
        //    Expression result = null;
        //    foreach (string propertyName in property)
        //    {
        //        Expression instance = result;
        //        if (instance == null)
        //            instance = initialInstance;
        //        result = Expression.Property(instance, propertyName);
        //    }

        //    return result;
        //}
        #endregion

        #region Permissions

        public bool getFieldPermission(int? fieldActivityID, User usr, string entityname, out bool noPermission)
        {
            bool editable = true;
            noPermission = false;
            User user = usr;
            Entity ent = this.GetEntity(entityname, false);
            
            foreach (UserRoleActivity userActivity in user.UserRoleActivity)
            {
                if (fieldActivityID != null)
                {
                    if (fieldActivityID == userActivity.ActivityID)
                    {
                        if (userActivity.StcPermissionID == 24)//No Permission hide control
                        {
                            noPermission = true;
                        }
                        if (userActivity.StcPermissionID == 11)//View only
                        {
                            editable = false;
                        }
                    }
                }
                else
                {
                    if (ent.ActivityID == userActivity.ActivityID)
                    {
                        if (userActivity.StcPermissionID == 24)//No Permission hide control
                        {
                            noPermission = true;
                        }
                        if (userActivity.StcPermissionID == 11)//View only on Entity overrides any permission on any field lower 
                        {
                            editable = false;
                        }
                    }
                }

            }

            return editable;
        }

        public string getMenuPermission(int? fieldActivityID, User usr, string entityname)
        {
            string editable = "true";

            User user = usr;
            Entity ent = this.GetEntity(entityname, false);

            foreach (UserRoleActivity userActivity in user.UserRoleActivity)
            {
                if (fieldActivityID != null)
                {
                    if (fieldActivityID == userActivity.ActivityID)
                    {
                        if (userActivity.StcPermissionID == 24)//None
                        {
                            editable = "none";
                        }
                    }
                }
                else
                {
                    if (ent.ActivityID == userActivity.ActivityID)
                    {
                        if (userActivity.StcPermissionID == 24)//None
                        {
                            editable = "none";
                        }
                    }
                }

            }

            return editable;
        }

        public Dictionary<string, object> getButtonHtmlAttributes(string buttonType, string tablename, string culture)
        {
            HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
            string attributes = string.Empty;
            Dictionary<string, object> htmlattributes = new Dictionary<string, object>();
            if (session["User"] != null)
            {
                if (buttonType != "EditForm" && buttonType != "HideSaveButton")
                {
                    if (!getButtonPermission(buttonType, tablename, (User)session["User"]))
                    {
                        if (buttonType != "Edit")
                        {

                            string message = "No" + buttonType + "Permission";
                            EntityResource resource = this.Get<EntityResource>(p => p.ResourceKey == message && p.ResourceCulture == culture);
                            if (resource == null)
                            {
                                message = "No" + buttonType + "PermissionNOR";
                            }
                            else
                            {
                                message = resource.ResourceValue;
                            }
                            htmlattributes.Add("title", message);
                            htmlattributes.Add("disabled", "disabled");
                            htmlattributes.Add("class", "readOnly");
                            htmlattributes.Add("readonly", "read-only");
                        }
                        else//If edit button then just show message that they cant save anything once the edit form opens
                        {
                            string message = "No" + buttonType + "Permission";
                            EntityResource resource = this.Get<EntityResource>(p => p.ResourceKey == message && p.ResourceCulture == culture);
                            if (resource == null)
                            {
                                message = "No" + buttonType + "PermissionNOR";
                            }
                            else
                            {
                                message = resource.ResourceValue;
                            }
                            htmlattributes.Add("title", message);

                        }


                    }
                    else
                    {
                        string message = buttonType;
                        EntityResource resource = this.Get<EntityResource>(p => p.ResourceKey == buttonType && p.ResourceCulture == culture);
                        if (resource == null)
                        {
                            message = "buttonTypeNOR";
                        }
                        else
                        {
                            message = resource.ResourceValue;
                        }
                        htmlattributes.Add("title", message);
                    }
                }
                //Disabling the saving on Edit Form
                if (buttonType == "EditForm")
                {
                    if (!getButtonPermission("Edit", tablename, (User)session["User"]))//If Edit is disabled then disable edit form saving
                    {
                        string message = "No" + buttonType + "Permission";
                        EntityResource resource = this.Get<EntityResource>(p => p.ResourceKey == message && p.ResourceCulture == culture);
                        if (resource == null)
                        {
                            message = "No" + buttonType + "PermissionNOR";
                        }
                        else
                        {
                            message = resource.ResourceValue;
                        }
                        htmlattributes.Add("onClick", "alert(\"" + message + "\");");
                        htmlattributes.Add("disabled", "disabled");
                        htmlattributes.Add("class", "readOnly");
                        htmlattributes.Add("readonly", "read-only");
                        htmlattributes.Add("method", "");
                        htmlattributes.Add("action", "");
                    }
                }
                //Hide the save button onn Edit Form
                if (buttonType == "HideSaveButton")
                {
                    htmlattributes.Add("id", "uploadForm");
                    htmlattributes.Add("enctype", "multipart/form-data");
                    htmlattributes.Add("method", "Post");


                }
            }
            return htmlattributes;
        }

        public bool getButtonPermission(string buttonType, string tablename, User usr)
        {
            return getButtonPermission(buttonType, tablename, usr, null);
        }

        public bool getButtonPermission(string buttonType, string tablename, User usr, long? recordId)
        {
            bool enable = true;
            User user = usr;
            Entity ent = this.GetEntity(tablename, true);
            if (buttonType == "Edit")//Check EntityFields for any different activities set on a field other than on the entity
            {
                foreach (UserRoleActivity userActivity in user.UserRoleActivity)
                {

                    EntityField field = ent.EntityField.FirstOrDefault(o => o.ActivityID == userActivity.ActivityID);
                    if (field != null)
                    {
                        if (userActivity.StcPermissionID == 9 || userActivity.StcPermissionID == 10 || userActivity.StcPermissionID == 8)//Add,Delete or Edit permission 
                        {
                            return true;
                        }
                        else if (userActivity.StcPermissionID == 11)//View permission only on this control
                        {
                            return false;
                        }
                    }
                }
            }
            //If no activity found on a entityfield above then use the activity set on the entity
            foreach (UserRoleActivity userActivity in user.UserRoleActivity)
            {
                if (ent.ActivityID == userActivity.ActivityID)
                {
                    if (userActivity.StcPermissionID == 8)//Delete permission Can Add/Edit and Delete
                    {
                        break;
                    }
                    else if (userActivity.StcPermissionID == 9)//Edit permission Can only Add and Edit 
                    {
                        if (buttonType == "Delete")
                        {
                            enable = false;
                            break;
                        }

                    }
                    else if (userActivity.StcPermissionID == 10)//Add permission Can only Add
                    {
                        if (buttonType == "Delete" || buttonType == "Edit")
                        {
                            enable = false;
                            break;
                        }
                    }
                    else if (userActivity.StcPermissionID == 11)//View permission Can only View all controls disabled on edit form
                    {
                        if (buttonType == "Add" || buttonType == "Delete" || buttonType == "Edit")
                        {
                            enable = false;
                            break;
                        }
                    }

                }

            }

            return enable;
        }

        #endregion

        #region ezLogger
        //public int AddezLogger(ezLogger newEntity)
        //{
        //    try
        //    {
        //        return Repository.UoW.Add<ezLogger>(newEntity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region ImportResult
        //public int AddImportResult(sapImportResult newEntity)
        //{
        //    try
        //    {
        //        return Repository.UoW.Add<sapImportResult>(newEntity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region Entity Data Methods

        public Entity GetEntity(int id, bool includeRelations)
        {

            if (includeRelations)
            {
                List<Expression<Func<Entity, object>>> includepaths = new List<Expression<Func<Entity, object>>>();
                includepaths.Add(p => p.EntityField);
                includepaths.Add(p => p.Organization);
                includepaths.Add(p => p.Activity);

                Entity data = Repository.Get<Entity>(includepaths, p => p.EntityID == id);
                return data;
            }
            else
            {
                return Repository.Get<Entity>(null, p => p.EntityID == id);
            }
        }

        public Entity GetEntity(string tablename, bool includeRelations)
        {

            if (includeRelations)
            {
                List<Expression<Func<Entity, object>>> includepaths = new List<Expression<Func<Entity, object>>>();
                includepaths.Add(p => p.EntityField);
               // includepaths.Add(p => p.Organization);
               // includepaths.Add(p => p.Activity);

                Entity data = Repository.Get<Entity>(includepaths, p => p.TableName == tablename);
                return data;
            }
            else
            {
                return Repository.Get<Entity>(null, p => p.TableName == tablename);
            }
        }

        public GridResult<Entity> GetEntityList(GridParam filters)
        {
            try
            {
                GridResult<Entity> result = new GridResult<Entity>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<Entity>().Count();
                Expression<Func<Entity, bool>> where = null;
                if (filters.ListFilterBy != null)
                {
                    foreach (FilterField field in filters.ListFilterBy)
                    {
                        if (field.Property.Length == 0 || field.Operator.Length == 0 || field.Value.Length == 0)
                        {
                            throw new Exception("A Filter field has not been specified properly.");
                        }
                        if (where == null)
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Entity>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Entity>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<Entity> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<Entity, object>>> includepaths = new List<Expression<Func<Entity, object>>>();
                    //includepaths.Add(p => p.EntityField);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<Entity>(where);
                    }
                    else
                    {
                        list = Repository.GetList<Entity, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<Entity>(where);
                    }
                    else
                    {
                        list = Repository.GetList<Entity>();
                    }

                }

                //APPLY ALL SORTING

                if (filters.ListOrderBy != null && filters.ListOrderBy.Count() > 0)
                {
                    foreach (var sort in filters.ListOrderBy)
                    {
                        if (sort.Property.Length == 0 || sort.Value.Length == 0)
                        {
                            throw new Exception("A sort field has not been specified properly.");
                        }
                        list = list.OrderBy(sort.Property, sort.Value);
                    }
                }
                else
                {
                    list = list.OrderBy(o => o.Name);
                }
                //APPLY PAGE SIZE
                //Get total filtered rows before paging is applied 
                result.TotalFilteredCount = list.Count();
                list = list.Skip(filters.PageNo).Take(filters.PageSize);

                if (filters.Includerelations)// Add includepaths into method 
                {
                    List<Entity> returnlist = new List<Entity>();
                    foreach (Entity ent in list)
                    {
                        ent.Activity = this.Repository.Get<Activity>(o => o.ActivityID == ent.ActivityID);
                        returnlist.Add(ent);
                    }

                    result.Items = returnlist.AsQueryable();
                }
                else
                {
                    result.Items = list;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IQueryable<Entity> GetEntityList(bool includeRelations, Expression<Func<Entity, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
        //{

        //    try
        //    {
        //        IQueryable<Entity> list;

        //        if (includeRelations)// Add includepaths into method 
        //        {
        //            //Add Includes here example below
        //            //STARTCUSTOMCODE
        //            List<Expression<Func<Entity, object>>> includepaths = new List<Expression<Func<Entity, object>>>();
        //            includepaths.Add(p => p.EntityField);
        //            includepaths.Add(p => p.Organization);
        //            includepaths.Add(p => p.Activity);
        //            //ENDCUSTOMCODE

        //            //APPLY FILTERS
        //            if (where != null)
        //            {
        //                list = Repository.GetList<Entity, string>(includepaths, where);
        //            }
        //            else
        //            {
        //                list = Repository.GetList<Entity, string>(includepaths);
        //            }

        //        }
        //        else
        //        {
        //            if (where != null)
        //            {
        //                list = Repository.GetList<Entity>(where);
        //            }
        //            else
        //            {
        //                list = Repository.GetList<Entity>();
        //            }

        //        }
        //        //resultList = list;
        //        ////APPLY ALL SORTING
        //        //List<User> newList = resultList.ToList();
        //        if (listOrderBy != null && listOrderBy.Count() > 0)
        //        {
        //            foreach (var sort in listOrderBy)
        //            {
        //                list = list.OrderBy(sort.Key, sort.Value);
        //            }
        //        }
        //        else
        //        {
        //            list = list.OrderBy(o => o.TableName);
        //        }
        //        //APPLY PAGE SIZE
        //        if (page > 0 && size > 0)
        //        {
        //            list = list.Skip((page - 1) * size).Take(size);
        //        }


        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<Entity> GetEntities()
        {

            List<Expression<Func<Entity, object>>> includepaths = new List<Expression<Func<Entity, object>>>();
            includepaths.Add(p => p.EntityField);
            includepaths.Add(p => p.Organization);
            includepaths.Add(p => p.Activity);

            return Repository.GetList<Entity, string>(includepaths, e => e.Name).ToList();


        }

        public int AddEntity(Entity newEntity)
        {
            try
            {
                int result = 0;
                result = Repository.UoW.Add<Entity>(newEntity);
                if (result > 0)
                {
                    //Check if exists else Create default EntityResource language entry
                    EntityResource resource = Get<EntityResource>(r => r.ResourceKey == newEntity.Name);
                    if (resource == null)
                    {
                        EntityResource newresource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = newEntity.Name, ResourceValue = newEntity.Name };
                        Repository.UoW.Add<EntityResource>(newresource);
                    }
                    else
                    {
                        resource.ResourceValue = newEntity.Name;
                        Repository.UoW.Update<EntityResource>(resource);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Entity AddReturnEntity(Entity newEntity)
        {
            try
            {
                newEntity.StcStatusID = this.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<Entity>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateEntity(Entity editEntity)
        {
            try
            {
                int result = 0;
                result = Repository.UoW.Update<Entity>(editEntity);
                if (result > 0)
                {
                    //Check if exists else Create default EntityResource language entry
                    EntityResource resource = Get<EntityResource>(r => r.ResourceKey == editEntity.Name);
                    if (resource == null)
                    {
                        EntityResource newresource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = editEntity.Name, ResourceValue = editEntity.Name };
                        Repository.UoW.Add<EntityResource>(newresource);
                    }
                    else
                    {
                        resource.ResourceValue = editEntity.Name;
                        Repository.UoW.Update<EntityResource>(resource);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int DeleteEntity(Entity deleteEntity)
        {
            return Repository.UoW.Delete<Entity>(deleteEntity);
        }

        public int DeleteEntity(int id)
        {
            try
            {
                Entity entity = this.GetEntity(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Entity>(entity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region EntityFields Methods

        public EntityField GetEntityField(int id, bool includeRelations)
        {

            if (includeRelations)
            {
                //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                //includepaths.Add(p => p.StcData);
                //includepaths.Add(p => p.Municipality);

                EntityField data = Repository.Get<EntityField>(null, p => p.EntityFieldID == id);
                return data;
            }
            else
            {
                return Repository.Get<EntityField>(null, p => p.EntityFieldID == id);
            }
        }

        public EntityField GetEntityField(string entityfieldname, bool includeRelations)
        {

            if (includeRelations)
            {
                //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                //includepaths.Add(p => p.StcData);
                //includepaths.Add(p => p.Municipality);

                EntityField data = Repository.Get<EntityField>(null, p => p.EntityFieldName == entityfieldname.Trim());
                return data;
            }
            else
            {
                return Repository.Get<EntityField>(null, p => p.EntityFieldName == entityfieldname.Trim());
            }
        }

        public IQueryable<EntityFieldVM> ListEntityField(int entityID)
        {
            return Repository.GetListWithSelect<EntityField, EntityFieldVM>(p => new EntityFieldVM { EntityFieldID = p.EntityFieldID, EntityFieldName = p.EntityFieldName, DisplayName = p.DisplayName, GroupBoxName = p.GroupBoxName, TabName = p.TabName, IsMandatory = p.IsMandatory }, null, o => o.EntityID == entityID);
        }

        public IQueryable<EntityField> ListEntityField(string entity)
        {
            if (entity == string.Empty)
            {
                return Repository.GetList<EntityField, string>(e => e.Entity.Name == "%", e => e.EntityFieldName);
            }
            else
            {
                return Repository.GetList<EntityField, string>(e => e.Entity.Name == entity, e => e.EntityFieldName);
            }
        }

        public IEnumerable<EntityField> GetEntityFieldsAsList(GridParam filters)
        {
            GridResult<EntityField> returnList = new GridResult<EntityField>();
            returnList = GetEntityFieldList(filters);
            return returnList.Items;
        }
        public GridResult<EntityField> GetEntityFieldList(GridParam filters)
        {
            try
            {
                GridResult<EntityField> result = new GridResult<EntityField>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<EntityField>().Count();
                Expression<Func<EntityField, bool>> where = null;
                if (filters.ListFilterBy != null)
                {
                    foreach (FilterField field in filters.ListFilterBy)
                    {
                        if (field.Property.Length == 0 || field.Operator.Length == 0 || field.Value.Length == 0)
                        {
                            throw new Exception("A Filter field has not been specified properly.");
                        }
                        if (where == null)
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<EntityField>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<EntityField>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<EntityField> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<EntityField, object>>> includepaths = new List<Expression<Func<EntityField, object>>>();
                    includepaths.Add(p => p.Activity);
                    includepaths.Add(p => p.Entity);
                    includepaths.Add(p => p.EntityFieldDataType);
                    includepaths.Add(p => p.StcData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = this.GetList<EntityField>(where);
                    }
                    else
                    {
                        list = this.GetList<EntityField, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = this.GetList<EntityField>(where);
                    }
                    else
                    {
                        list = this.GetList<EntityField>();
                    }

                }

                //APPLY ALL SORTING

                if (filters.ListOrderBy != null && filters.ListOrderBy.Count() > 0)
                {
                    foreach (var sort in filters.ListOrderBy)
                    {
                        if (sort.Property.Length == 0 || sort.Value.Length == 0)
                        {
                            throw new Exception("A sort field has not been specified properly.");
                        }
                        list = list.OrderBy(sort.Property, sort.Value);
                    }
                }
                else
                {
                    list = list.OrderBy(o => o.EntityFieldName);
                }
                //APPLY PAGE SIZE
                //Get total filtered rows before paging is applied 
                result.TotalFilteredCount = list.Count();
                list = list.Skip(filters.PageNo).Take(filters.PageSize);

                //if (filters.Includerelations)// Add includepaths into method 
                //{
                //    List<EntityField> returnlist = new List<EntityField>();

                //    foreach (EntityField ent in list)
                //    {
                //        ent.Activity = this.Repository.Get<Activity>(o => o.ActivityID == ent.ActivityID) != null ? this.Repository.Get<Activity>(o => o.ActivityID == ent.ActivityID) : null ;
                //      //  ent.Entity = this.Repository.Get<Entity>(o => o.EntityID == ent.EntityID) != null ? this.Repository.Get<Entity>(o => o.EntityID == ent.EntityID) : null;
                //        ent.EntityFieldDataType = this.Repository.Get<EntityFieldDataType>(o => o.EntityFieldDataTypeID == ent.EntityFieldDataTypeID) != null ? this.Repository.Get<EntityFieldDataType>(o => o.EntityFieldDataTypeID == ent.EntityFieldDataTypeID) : null;
                //        returnlist.Add(ent);
                //    }

                //    result.Items = returnlist.AsQueryable();
                //}
                //else
                //{
                    result.Items = list;
                //}
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEntityField(EntityField newEntity)
        {

            try
            {
                int row = Repository.UoW.Add<EntityField>(newEntity);
                if (row > 0)
                {
                    //If exists update else Create default EntityResource language entry
                    EntityResource res = Get<EntityResource>(r => r.ResourceKey == newEntity.EntityFieldName && r.ResourceCulture == "en-US");
                    EntityResource resource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = newEntity.EntityFieldName, ResourceValue = newEntity.DisplayName };
                    if (res == null)
                    {
                        Repository.UoW.Add<EntityResource>(resource);
                    }
                    else
                    {
                        res.ResourceValue = newEntity.DisplayName;
                        Repository.UoW.Update<EntityResource>(resource);
                    }
                    //Tooltip
                    if (newEntity.TabName != null)
                    {
                        //If exists update else Create default EntityResource language entry
                        EntityResource tabresource = Get<EntityResource>(r => r.ResourceKey == "Tab" + newEntity.TabName && r.ResourceCulture == "en-US");
                        EntityResource newresource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = "Tab" + newEntity.TabName, ResourceValue = newEntity.TabName };
                        if (tabresource == null)
                        {
                            Repository.UoW.Add<EntityResource>(newresource);
                        }
                        else
                        {
                            tabresource.ResourceValue = newEntity.TabName;
                            Repository.UoW.Update<EntityResource>(tabresource);
                        }
                    }

                    //GroupBoxName
                    if (newEntity.GroupBoxName != null)
                    {
                        //If exists update else Create default EntityResource language entry
                        EntityResource grpresource = Get<EntityResource>(r => r.ResourceKey == "GroupBox" + newEntity.GroupBoxName && r.ResourceCulture == "en-US");
                        EntityResource newgrpresource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = "GroupBox" + newEntity.GroupBoxName, ResourceValue = newEntity.GroupBoxName };
                        if (grpresource == null)
                        {
                            Repository.UoW.Add<EntityResource>(newgrpresource);
                        }
                        else
                        {
                            grpresource.ResourceValue = newEntity.GroupBoxName;
                            Repository.UoW.Update<EntityResource>(grpresource);
                        }
                    }
                }
                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public EntityField AddReturnEntityField(EntityField newEntity)
        {
            try
            {
                newEntity.StcStatusID = this.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<EntityField>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateEntityField(EntityField editEntity)
        {
            try
            {
                int row = Repository.UoW.Update<EntityField>(editEntity);
                if (row > 0)
                {
                    //If exists update else Create default EntityResource language entry
                    EntityResource res = Get<EntityResource>(r => r.ResourceKey == editEntity.EntityFieldName && r.ResourceCulture == "en-US");
                    EntityResource resource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = editEntity.EntityFieldName, ResourceValue = editEntity.DisplayName };
                    if (res == null)
                    {
                        Repository.UoW.Add<EntityResource>(resource);
                    }
                    else
                    {
                        res.ResourceValue = editEntity.DisplayName;
                        Repository.UoW.Update<EntityResource>(res);
                    }
                    //Tooltip
                    if (editEntity.TabName != null)
                    {
                        //If exists update else Create default EntityResource language entry
                        EntityResource tabresource = Get<EntityResource>(r => r.ResourceKey == "Tab" + editEntity.TabName && r.ResourceCulture == "en-US");
                        EntityResource newresource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = "Tab" + editEntity.TabName, ResourceValue = editEntity.TabName };
                        if (tabresource == null)
                        {
                            Repository.UoW.Add<EntityResource>(newresource);
                        }
                        else
                        {
                            tabresource.ResourceValue = editEntity.TabName;
                            Repository.UoW.Update<EntityResource>(tabresource);
                        }
                    }
                    //GroupBoxName
                    if (editEntity.GroupBoxName != null)
                    {
                        //If exists update else Create default EntityResource language entry
                        EntityResource grpresource = Get<EntityResource>(r => r.ResourceKey == "GroupBox" + editEntity.GroupBoxName && r.ResourceCulture == "en-US");
                        EntityResource newgrpresource = new EntityResource() { ResourceCulture = "en-US", IsActive = true, ResourceKey = "GroupBox" + editEntity.GroupBoxName, ResourceValue = editEntity.GroupBoxName };
                        if (grpresource == null)
                        {
                            Repository.UoW.Add<EntityResource>(newgrpresource);
                        }
                        else
                        {
                            grpresource.ResourceValue = editEntity.GroupBoxName;
                            Repository.UoW.Update<EntityResource>(grpresource);
                        }
                    }
                }
                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEntityField(EntityField deleteEntity)
        {
            try
            {
                return Repository.UoW.Delete<EntityField>(deleteEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEntityField(int id)
        {
            try
            {
                EntityField entityField = this.GetEntityField(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<EntityField>(entityField);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Entity Menu

        public string RebuildMenu()
        {
            StringBuilder xmlString = new StringBuilder();
            xmlString.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            xmlString.Append("<siteMap>");
            xmlString.Append("<siteMapNode title=\"Home\" controller=\"Home\" action=\"Overview\">");

            List<EntityMenu> parentlistHeaderLevel = this.ListEntityMenu().Where(o => o.EntityMenuParentID == null && o.IsActive == true).OrderBy(o => o.ParentSequence).ToList<EntityMenu>();
            foreach (EntityMenu parentMenu in parentlistHeaderLevel)
            {
                if (parentMenu.Url != null)
                {
                    xmlString.Append("<siteMapNode title=\"" + parentMenu.MenuDisplayName + "-" + parentMenu.Url + "\">");
                }
                else
                {
                    xmlString.Append("<siteMapNode title=\"" + parentMenu.MenuDisplayName + "\">");
                    xmlString.Append(GetMenuChildren((int)parentMenu.EntityMenuID));
                }
                

                xmlString.Append("</siteMapNode>");
            }

            xmlString.Append("</siteMapNode>");
            xmlString.Append("</siteMap>");

            return xmlString.ToString();
        }
        private string GetMenuChildren(int entityMenuParent)
        {
            List<EntityMenu> list = this.ListEntityMenu().Where(o => o.EntityMenuParentID == entityMenuParent && o.IsActive == true).OrderBy(o => o.Sequence).ToList<EntityMenu>();
            StringBuilder xmlString = new StringBuilder();
            foreach (EntityMenu menu in list)
            {
                if (menu.Url == null)
                {
                    xmlString.Append("<siteMapNode title=\"" + menu.MenuDisplayName + "\">");
                    xmlString.Append(GetMenuChildren((int)menu.EntityMenuID));
                    xmlString.Append("</siteMapNode>");
                }
                else
                {
                    xmlString.Append("<siteMapNode title=\"" + menu.MenuDisplayName + "-" + menu.Url + "\"/>");
                }
                
            }
            return xmlString.ToString();
        }
        public EntityMenu GetEntityMenu(int id, bool includeRelations)
        {

            if (includeRelations)
            {
                EntityMenu data = Repository.Get<EntityMenu>(null, p => p.EntityMenuID == id);
                return data;
            }
            else
            {
                return Repository.Get<EntityMenu>(null, p => p.EntityMenuID == id);
            }
        }

        public IQueryable<EntityMenu> ListEntityMenu()
        {
            return Repository.GetList<EntityMenu>();
        }

        public int AddStcData(EntityMenu newEntity)
        {
            return Repository.UoW.Add<EntityMenu>(newEntity);
        }

        public int UpdateStcData(EntityMenu editEntity)
        {
            return Repository.UoW.Update<EntityMenu>(editEntity);
        }

        public int DeleteStcData(EntityMenu deleteEntity)
        {
            return Repository.UoW.Delete<EntityMenu>(deleteEntity);
        }

        #endregion

        #region Generic Get Methods

        public T Get<T>(Expression<Func<T, bool>> whereclause) where T : class
        {
            try
            {
                return Repository.Get<T>(whereclause);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        #endregion

        #region Generic GetList Methods
        public IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths, Expression<Func<T, bool>> whereclause, Expression<Func<T, TKey>> orderBy) where T : class
        {
           return Repository.GetList<T, TKey>(includepaths,whereclause, orderBy);
        }
        public IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths,Expression<Func<T, TKey>> orderBy) where T : class
        {
            return Repository.GetList<T, TKey>(includepaths,orderBy);
        }
        public IQueryable<T> GetList<T, TKey>(List<Expression<Func<T, object>>> includepaths) where T : class
        {
            return Repository.GetList<T, TKey>(includepaths);
        }
        public IQueryable<T> GetList<T, TKey>(Expression<Func<T, bool>> whereclause, Expression<Func<T, TKey>> orderBy) where T : class
        {
            return Repository.GetList<T, TKey>(whereclause, orderBy);
        }
        public IQueryable<T> GetList<T, TKey>(Expression<Func<T, TKey>> orderBy) where T : class
        {
            return Repository.GetList<T, TKey>(orderBy);
        }
        public IQueryable<T> GetList<T>(Expression<Func<T, bool>> whereclause) where T : class
        {
            return Repository.GetList<T>(whereclause);
        }
        public IQueryable<T> GetList<T>() where T : class
        {
            return Repository.GetList<T>();
        }

        public IList GetDataByEntityName(GridParam filter)
        {
            var statuslist = new[] { (int)Enumerations.SupervisionStatus.Approved, (int)Enumerations.SupervisionStatus.PendingUsable };
          //  ShipmentManager dsddbmanager = new ShipmentManager();//Points to DSD db
            AdminManager managerdbmng = new AdminManager();//Points to Manager DB

           
                 
            //TODO change this to use reflection to work out which object type 
            switch (filter.EntityName.Trim())
            {
                case "StpDataType":
                    {
                        return managerdbmng.GetList<StpDataType>(GetWhereClause<StpDataType>(filter)).ToList();
                    }
                case "StpData":
                    {
                        return managerdbmng.GetList<StpData>(GetWhereClause<StpData>(filter)).ToList();
                    }
                case "StcDataType":
                    {
                        return managerdbmng.GetList<StcDataType>(GetWhereClause<StcDataType>(filter)).ToList();
                    }
                case "StcData":
                    {
                        return managerdbmng.GetList<StcData>(GetWhereClause<StcData>(filter)).ToList();
                    }
				case "Entity":
					{
						return managerdbmng.GetList<Entity>(GetWhereClause<Entity>(filter)).ToList();
					}
                case "EntityField":
                    {
                        return managerdbmng.GetList<EntityField>(GetWhereClause<EntityField>(filter)).ToList();
                    }
                case "EntityFieldDataType":
                    {
                        return managerdbmng.GetList<EntityFieldDataType>(GetWhereClause<EntityFieldDataType>(filter)).ToList();
                    }
                case "EntityMenu":
                    {
                        return managerdbmng.GetList<EntityMenu>(GetWhereClause<EntityMenu>(filter)).ToList();
                    }
                case "EntityResource":
                    {
                        return managerdbmng.GetList<EntityResource>(GetWhereClause<EntityResource>(filter)).ToList();
                    }
                case "Organization":
					{
						return managerdbmng.GetList<Organization>(GetWhereClause<Organization>(filter)).ToList();
					}
				case "User":
                    {
                        return managerdbmng.ExecuteLinqQuery<User>("select * from [user]").Where(GetWhereClause<User>(filter)).ToList();
                    }
                case "UserRole":
                    {
                        return managerdbmng.GetList<UserRole>(GetWhereClause<UserRole>(filter)).ToList();
                    }
                case "UserRoleActivity":
                    {
                        return managerdbmng.GetList<UserRoleActivity>(GetWhereClause<UserRoleActivity>(filter)).ToList();
                    }
                case "Activity":
                    {
                        return managerdbmng.GetList<Activity>(GetWhereClause<Activity>(filter)).ToList();
                    }

                case "Application":
                    {
                        return managerdbmng.GetList<Application>(GetWhereClause<Application>(filter)).ToList();
                    }
                case "Contact":
                    {
                        return managerdbmng.GetList<Contact>(GetWhereClause<Contact>(filter)).ToList();
                    }
                case "Asset":
                    {
                        return managerdbmng.GetList<Asset>(GetWhereClause<Asset>(filter)).ToList();
                    }
                case "Person":
                    {
                        return managerdbmng.GetList<Person>(GetWhereClause<Person>(filter)).ToList();
                    }
                case "Supplier":
                    {
                        return managerdbmng.GetList<Supplier>(GetWhereClause<Supplier>(filter)).ToList();
                    }
                case "Consumable":
                    {
                        //ConsumableManager manag = new ConsumableManager();
                        //GridResult<Consumable> data = manag.GetConsumable(filter);
                        //return data.Items.ToList();
                        return managerdbmng.GetList<Consumable>(GetWhereClause<Consumable>(filter)).ToList();
                    }
            }


            return null;
        }
       
        private Expression<Func<T, bool>>  GetWhereClause<T>(GridParam filter) where T : class
        {
            Expression<Func<T, bool>> where = null;

            foreach (FilterField field in filter.ListFilterBy)
            {
                if (field.Property.Length == 0 || field.Operator.Length == 0 || field.Value.Length == 0)
                {
                    throw new Exception("A Filter field has not been specified properly.");
                }
                if (where == null)
                {
                    where = Common.QueryHelpers.BuildFilter.BuildWhereClause<T>(field.Property, field.Operator, field.Value);
                }
                else
                {
                    where = Common.QueryHelpers.BuildFilter.BuildWhereClause<T>(field.Property, field.Operator, field.Value, where);
                }

            }
            
        return where;
        }
        #endregion
        
        //#region Document

        //public int AddDocument(DALEFModel.Document newEntity)
        //{
        //    return Repository.UoW.Add<DALEFModel.Document>(newEntity);
        //}

        //public int UpdateDocument(DALEFModel.Document editEntity)
        //{
        //    return Repository.UoW.Update<DALEFModel.Document>(editEntity);
        //}

        //public int DeleteDocument(DALEFModel.Document deleteEntity)
        //{
        //    return Repository.UoW.Delete<DALEFModel.Document>(deleteEntity);
        //}

        //#endregion

        #region Static Data Methods


        public StcData GetStcData(int id, bool includeRelations)
        {

            if (includeRelations)
            {
                StcData data = Repository.Get<StcData>(null, p => p.StcDataID == id);
                return data;
            }
            else
            {
                return Repository.Get<StcData>(null, p => p.StcDataID == id);
            }
        }

        public IQueryable<StcData> ListStcData(Enumerations.StaticDataType? typeID)
        {
            List<Expression<Func<StcData, object>>> includepaths = new List<Expression<Func<StcData, object>>>();
            includepaths.Add(p => p.StcDataType);
            if (typeID == null)
            {
                return Repository.GetList<StcData, string>(includepaths, e => e.Description);
            }
            else
            {
                return Repository.GetList<StcData, string>(includepaths, p => p.StcDataTypeID == (int)typeID, e => e.StcDataType.StaticDataType);
            }


        }

        public int AddStcData(StcData newEntity)
        {
            return Repository.UoW.Add<StcData>(newEntity);
        }

        public int UpdateStcData(StcData editEntity)
        {
            return Repository.UoW.Update<StcData>(editEntity);
        }

        public int DeleteStcData(StcData deleteEntity)
        {
            return Repository.UoW.Delete<StcData>(deleteEntity);
        }

        #endregion

        #region List Entities In Model

        public IQueryable<T> ExecuteLinqQuery<T>(string sqlQuery, object[] parameters) where T : new()
        {
            return Repository.ExecuteLinqQuery<T>(sqlQuery, parameters);
        }
        public IQueryable<T> ExecuteLinqQuery<T>(string sqlQuery) where T : new()
        {
            object[] paras = { };
            return Repository.ExecuteLinqQuery<T>(sqlQuery, paras);
        }

        public List<object> GetAllObjectsInDataModel()
        {
            List<object> objects = new List<object>();
            XmlDocument loResource = new XmlDocument();
            loResource.Load("C:\\FJSBilling\\DALEFBase\\Model\\PromisModel.edmx");
            //foreach(ject o in DALEFBase.Model)
            //{

            //}
            return objects;
        }

        #endregion

        #region Export and Import Methods 

        #region Export Methods
        //TODO move to common maybe
        public MemoryStream ExportDataToPDF<T>(List<T> list, bool includeDetails, string details) where T : class,new()
        {
            try
            {
                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4_LANDSCAPE);
                // Document doc = new Document(new Rectangle(600, 800), 10, 10, 10, 10);

                MemoryStream output = new MemoryStream();
                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, output);
                //doc.SetPageSize(new Rectangle(600, 800));
                doc.Open();
                //TODO add functionality here for any Headers or custom client PDF formating 

                //Add content to the document
                //Get EntityField details
                Entity ent = this.GetEntity(typeof(T).Name, true);

                List<EntityField> fields = ent.EntityField.Where(f => f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo" && f.IsActive == true).ToList();

                int numOfColumns = fields.Count();
                iTextSharp.text.pdf.PdfPTable dataTable = new iTextSharp.text.pdf.PdfPTable(numOfColumns);

                //ADD Title
                iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new Phrase("Exported list of " + typeof(T).Name + " Date: " + DateTime.Now));
                cell.Colspan = numOfColumns;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                dataTable.AddCell(cell);
                //----------------------
                //Add details if required
                if (includeDetails)
                {
                    iTextSharp.text.pdf.PdfPCell celldetails = new iTextSharp.text.pdf.PdfPCell(new Phrase(details));
                    celldetails.Colspan = numOfColumns;
                    celldetails.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    dataTable.AddCell(celldetails);

                }
                //------------------------
                dataTable.HeaderRows = 1;
                dataTable.DefaultCell.BorderWidth = 1;
                dataTable.DefaultCell.Padding = 3;

                foreach (EntityField field in fields)
                {
                    if (!field.IsPrimaryKey)
                    {
                        dataTable.AddCell(field.DisplayName.Trim());
                    }
                }

                //Add Data Rows
                foreach (var item in list)
                {
                    var itemProperties = item.GetType().GetProperties();
                    string propName = string.Empty;

                    foreach (EntityField field in fields)
                    {
                        var itemProperty = itemProperties.Where(m => m.Name == field.EntityFieldName).FirstOrDefault();

                        if (itemProperty != null)
                        {
                            propName = itemProperty.Name;
                        }

                        if (propName.Trim() == field.EntityFieldName.Trim())
                        {
                            if (!field.IsPrimaryKey)
                            {
                                if (itemProperty.GetValue(item, null) != null)
                                {
                                    if (field.IsForeignKey)
                                    {
                                        if (field.ComboBoxDisplayFieldID != null)
                                        {
                                            EntityField selectEntityField = this.Get<EntityField>(o => o.EntityFieldID == field.ComboBoxDisplayFieldID);
                                            string tableName = this.Get<Entity>(o => o.EntityID == selectEntityField.EntityID).TableName;
                                            string selectField = selectEntityField.EntityFieldName;
                                            string primaryField = this.Get<EntityField>(o => o.EntityID == selectEntityField.EntityID && o.IsPrimaryKey == true).EntityFieldName;

                                            PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                            Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                            string sqlQuery = "Select cast(" + selectField + " as varchar) as description from [" + tableName + "] Where " + primaryField + " = " + ID;
                                            object[] paras = { };
                                            SelectResult result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                            dataTable.AddCell(result.Description);

                                        }
                                        else
                                        {
                                            dataTable.AddCell("");
                                        }
                                    }
                                    else
                                    {
                                        dataTable.AddCell(itemProperty.GetValue(item, null).ToString());
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

        public MemoryStream ExportDataToExcel<T>(List<T> list, bool includeDetails, string details) where T : class,new()
        {
            try
            {
                //Create new Excel workbook
                var workbook = new HSSFWorkbook();

                //Create new Excel sheet
                var sheet = workbook.CreateSheet();

                MemoryStream output = new MemoryStream();

                Entity ent = this.GetEntity(typeof(T).Name, true);

                List<EntityField> fields = ent.EntityField.Where(f => f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo" && f.IsActive == true).ToList();

                int numOfColumns = fields.Count();

                string title = "Exported list of " + ent.MngCtlrName + " Date: " + DateTime.Now;
                //Add details if required
                if (includeDetails)
                {
                    title = title + " " + details;
                }
                //Create a Title row
                var titleRow = sheet.CreateRow(0);
                titleRow.CreateCell(0).SetCellValue(title);
                
                //Create a header row
                var headerRow = sheet.CreateRow(1);
                
                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);
                int rowNumber = 2; //3rd row after header row (titlerow is 0)
                int cellsNo = 0;
                //Add Data Rows
                foreach (var item in list)
                {
                    //Create a new row
                    var row = sheet.CreateRow(rowNumber++);
                    
                    cellsNo = 0;
                    var itemProperties = item.GetType().GetProperties().OrderBy(m => m.Name);
                    string propName = string.Empty;
                    string existsInHeader = string.Empty;

                    foreach (EntityField field in fields)
                    {
                        var itemProperty = itemProperties.Where(m => m.Name == field.EntityFieldName).FirstOrDefault();
                        if (itemProperty != null )
                        {
                           propName = itemProperty.Name;
                           headerRow.CreateCell(cellsNo).SetCellValue(field.DisplayName.Trim());
                        }

                        if (propName.Trim() == field.EntityFieldName.Trim())
                        {
                            if (!field.IsPrimaryKey)
                            {
                                if (itemProperty.GetValue(item, null) != null)
                                {
                                    if (field.IsForeignKey)
                                    {
                                        if (field.ComboBoxDisplayFieldID != null)
                                        {
                                            SelectResult result = new SelectResult();
                                            if (field.EntityFieldName.Substring(0, 3).ToLower() == "stp")
                                            {
                                                PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                                Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                                string sqlQuery = "Select DataDescription as description from StpData Where StpDataID = " + ID;
                                                object[] paras = { };
                                                result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                            }
                                            else if (field.EntityFieldName.Substring(0, 3).ToLower() == "stc")
                                            {
                                                PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                                Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                                string sqlQuery = "Select Description as description from StcData Where StcDataID = " + ID;
                                                object[] paras = { };
                                                result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                            }
                                            else
                                            {
                                                EntityField selectEntityField = this.Get<EntityField>(o => o.EntityFieldID == field.ComboBoxDisplayFieldID);
                                                string tableName = this.Get<Entity>(o => o.EntityID == selectEntityField.EntityID).TableName;
                                                string selectField = selectEntityField.EntityFieldName;
                                                string primaryField = this.Get<EntityField>(o => o.EntityID == selectEntityField.EntityID && o.IsPrimaryKey == true).EntityFieldName;

                                                PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                                Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                                string sqlQuery = "Select cast(" + selectField + " as varchar) as description from [" + tableName + "] Where " + primaryField + " = " + ID;
                                                object[] paras = { };
                                                result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                            }
                                            
                                            row.CreateCell(cellsNo).SetCellValue(result.Description);
                                            //row.Cells[cellsNo].CellStyle.WrapText = true;
                                            //row.Cells[cellsNo].CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
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
                                         row.CreateCell(cellsNo).SetCellValue(itemProperty.GetValue(item, null).ToString());
                                         //row.Cells[cellsNo].CellStyle.WrapText = true;
                                         //row.Cells[cellsNo].CellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Top;
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
                
                workbook.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MemoryStream ExportDataToCSV<T>(List<T> list, bool includeDetails, string details) where T : class,new()
        {
            try
            {
                MemoryStream output = new MemoryStream();
                StreamWriter writer = new StreamWriter(output, Encoding.UTF8);
                //Add content to the document
                //Get EntityField details

                Entity ent = this.GetEntity(typeof(T).Name, true);

                List<EntityField> fields = ent.EntityField.Where(f => f.EntityFieldName != "CreatedByID" && f.EntityFieldName != "StcStatusID" && f.EntityFieldName != "VersionNo" && f.IsActive == true).ToList();

                int numOfColumns = fields.Count();

                //ADD Title
                string title = "Exported list of " + typeof(T).Name + " Date: " + DateTime.Now;
                //Add details if required
                if (includeDetails)
                {
                    title = title + System.Environment.NewLine + details;
                }
                writer.Write(title);
                writer.WriteLine();

                foreach (EntityField field in fields)
                {
                    if (!field.IsPrimaryKey)
                    {
                        writer.Write(field.DisplayName.Trim() + ",");
                    }
                }

                writer.WriteLine();

                //Add Data Rows
                foreach (var item in list)
                {
                    var itemProperties = item.GetType().GetProperties();
                    string propName = string.Empty;
                    foreach (EntityField field in fields)
                    {
                        var itemProperty = itemProperties.Where(m => m.Name == field.EntityFieldName).FirstOrDefault();

                        if (itemProperty != null)
                        {
                            propName = itemProperty.Name;
                        }

                        if (propName.Trim() == field.EntityFieldName.Trim())
                        {
                            if (!field.IsPrimaryKey)
                            {
                                if (itemProperty.GetValue(item, null) != null)
                                {
                                    if (field.IsForeignKey)
                                    {
                                        if (field.ComboBoxDisplayFieldID != null)
                                        {
                                            EntityField selectEntityField = this.Get<EntityField>(o => o.EntityFieldID == field.ComboBoxDisplayFieldID);
                                            string tableName = this.Get<Entity>(o => o.EntityID == selectEntityField.EntityID).TableName;
                                            string selectField = selectEntityField.EntityFieldName;
                                            string primaryField = this.Get<EntityField>(o => o.EntityID == selectEntityField.EntityID && o.IsPrimaryKey == true).EntityFieldName;

                                            PropertyInfo relatedObjectinfo = item.GetType().GetProperty(field.EntityFieldName);
                                            Int32 ID = Convert.ToInt32(relatedObjectinfo.GetValue(item, null));
                                            string sqlQuery = "Select cast(" + selectField + " as varchar) as description from [" + tableName + "] Where " + primaryField + " = " + ID;
                                            object[] paras = { };
                                            SelectResult result = ExecuteLinqQuery<SelectResult>(sqlQuery, paras).FirstOrDefault<SelectResult>();
                                            writer.Write(result.Description + ",");

                                        }
                                        else
                                        {
                                            writer.Write(",");
                                        }
                                    }
                                    else
                                    {
                                        writer.Write(itemProperty.GetValue(item, null).ToString() + ",");
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
        #region Import Methods

        public MemoryStream ExportUserDataHeaderToExcel()
        {
            try
            {
                //Create new Excel workbook
                var workbook = new HSSFWorkbook();

                //Create new Excel sheet
                var sheet = workbook.CreateSheet();

                MemoryStream output = new MemoryStream();

                string title = "Export Template of UI Users - Date:" + DateTime.Now;
                
                var titleRow = sheet.CreateRow(0);
                titleRow.CreateCell(0).SetCellValue(title);
                
                var headerRow = sheet.CreateRow(1);
                
                headerRow.CreateCell(0).SetCellValue("Username");
                headerRow.CreateCell(1).SetCellValue("Shipping Point");
                headerRow.CreateCell(2).SetCellValue("Is Active");
                headerRow.CreateCell(3).SetCellValue("Role");

                sheet.CreateFreezePane(0, 1, 0, 1);

                workbook.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MemoryStream ExportDataHeaderToExcel<T>(bool includeDetails, string details) where T : class,new()
        {
            try
            {
                //Create new Excel workbook
                var workbook = new HSSFWorkbook();

                //Create new Excel sheet
                var sheet = workbook.CreateSheet();

                MemoryStream output = new MemoryStream();

                Entity ent = this.GetEntity(typeof(T).Name, true);

                List<EntityField> fields = ent.EntityField.Where<EntityField>(p => p.IsPrimaryKey == false).ToList();

                int numOfColumns = fields.Count();

                string title = "Exported list of " + typeof(T).Name + "  Date:" + DateTime.Now;
                //Add details if required
                if (includeDetails)
                {
                    title = title + " " + details;
                }
                //Create a Title row
                var titleRow = sheet.CreateRow(0);
                titleRow.CreateCell(0).SetCellValue(title);
                //TODO add formatting to title and headers
                //Create a header row
                var headerRow = sheet.CreateRow(1);
                //'Add headers
                int cellNo = 0;

                foreach (EntityField field in fields)
                {
                    if (field.DisplayName.Equals("CreateDateTime") || 
                        field.DisplayName.Equals("ChangeDateTime") || 
                        field.DisplayName.Equals("OrgID"))
                        continue;

                    headerRow.CreateCell(cellNo).SetCellValue(field.DisplayName.Trim());
                    cellNo++;                 
                }

                sheet.CreateFreezePane(0, 1, 0, 1);
              
                workbook.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void UploadTargetTemplateData<T>(HttpPostedFileBase file, string path) where T : class,new()
        //{
        //    try
        //    {
        //        Entity ent = this.GetEntity(typeof(T).Name, true);
        //        DataTable dt = new DataTable();

        //        foreach (EntityField property in ent.EntityField)
        //        {
        //            if (property.EntityFieldName == null)
        //            {
        //                property.EntityFieldName = property.DisplayName;
        //            }
        //            if (!property.EntityFieldName.Equals("Id"))
        //            {
        //                if (property.EntityFieldName.Equals("CreateDateTime") ||
        //                    property.EntityFieldName.Equals("ChangeDateTime") ||
        //                    property.EntityFieldName.Equals("OrgID"))
        //                    continue;

        //                dt.Columns.Add(property.EntityFieldName);
        //            }
        //        }

        //        HSSFWorkbook hssfwb;
     
        //        /*dt.Columns.Add("Year");
        //        dt.Columns.Add("Month");
        //        dt.Columns.Add("MaterialCode");
        //        dt.Columns.Add("MaterialDescription");
        //        dt.Columns.Add("MaterialGroup");
        //        dt.Columns.Add("Target");
        //        dt.Columns.Add("TemplateName");*/

        //        //var workbook = new Workboo();
        //        //workbook.open

        //        //foreach (var worksheet in Workbook.CreateWorkbook(.Worksheets(path))
        //        //{
        //        //}

        //        using (FileStream tempFile = new FileStream(path, FileMode.Open, FileAccess.Read))
        //        {
        //            hssfwb = new HSSFWorkbook(tempFile);

        //            var sheet = hssfwb.GetSheet("Sheet0");
        //            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

        //            while (rows.MoveNext())
        //            {
        //                var row = (HSSFRow)rows.Current;
        //                if (dt.Columns.Count == 0)
        //                {
        //                    for (int j = 0; j < row.LastCellNum; j++)
        //                    {
        //                        dt.Columns.Add(row.GetCell(j).ToString());
        //                    }

        //                    continue;
        //                }

        //                DataRow dr = dt.NewRow();

        //                for (int i = 0; i < row.LastCellNum; i++)
        //                {
        //                    var cell = row.GetCell(i);

        //                    if (cell == null)
        //                    {
        //                        dr[i] = null;
        //                    }

        //                    else
        //                    {
        //                        dr[i] = cell.ToString();
        //                    }

        //                }
        //                dt.Rows.Add(dr);
        //            }
        //        }

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            if ((dr != dt.Rows[0]) && (dr != dt.Rows[1]))
        //            {
        //                switch (typeof(T).Name)
        //                {
        //                    //case "TargetTemplate": 
        //                    //    SalesTargetManager saleTargetMng = new SalesTargetManager();
        //                    //    TargetTemplate targetTemplate = new TargetTemplate();
        //                    //    targetTemplate.Year = dr["Year"].ToString();
        //                    //    targetTemplate.Month = dr["Month"].ToString();
        //                    //    targetTemplate.MaterialCode = dr["MaterialCode"].ToString();
        //                    //    targetTemplate.MaterialDescription = dr["MaterialDescription"].ToString();
        //                    //    targetTemplate.MaterialGroup = dr["MaterialGroup"].ToString();
        //                    //    targetTemplate.Target = Convert.ToInt32(dr["Target"].ToString());
        //                    //    targetTemplate.TemplateName = dr["TemplateName"].ToString();
        //                    //    saleTargetMng.AddTargetTemplate(targetTemplate);
        //                    //    break;

        //                }                  
        //            }
        //        }
        //        System.IO.File.Delete(path);
        //    }                            //case "sysUser":
        //                    //    DriverManager drvMng = new DriverManager();
        //                    //    if(drvMng.GetSysUserByUsername(dr["UserName"].ToString()) != null)
        //                    //    {
        //                    //        User driver = drvMng.GetSysUserByUsername(dr["UserName"].ToString());
        //                    //        driver.UserName = dr["UserName"].ToString();
        //                    //        driver.UserPWD = dr["Password"].ToString();
        //                    //        //driver.user = dr["Type"].ToString();
        //                    //        //driver. = dr["Status"].ToString();
        //                    //        driver.OrgID = 0;
        //                    //        driver.CreateDateTime = DateTime.Now;
        //                    //        drvMng.UpdatesysUser(driver);
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        User driver = new User();
        //                    //        driver.UserName = dr["UserName"].ToString();
        //                    //        driver.UserPWD = dr["Password"].ToString();
        //                    //        //driver.Type = dr["Type"].ToString();
        //                    //        //driver.Status = dr["Status"].ToString();
        //                    //        driver.CreateDateTime = DateTime.Now;
        //                    //        drvMng.AddsysUser(driver);
        //                    //    }
        //                    //    break;
                            
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void UploadUserData(HttpPostedFileBase file, string path)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();

        //        dt.Columns.Add("UserName");
        //        dt.Columns.Add("ShippingPoint");
        //        dt.Columns.Add("IsActive");
        //        dt.Columns.Add("Role");

        //        HSSFWorkbook hssfwb;

        //        using (FileStream tempFile = new FileStream(path, FileMode.Open, FileAccess.Read))
        //        {
        //            hssfwb = new HSSFWorkbook(tempFile);

        //            var sheet = hssfwb.GetSheet("Sheet0");
        //            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

        //            while (rows.MoveNext())
        //            {
        //                var row = (HSSFRow)rows.Current;
        //                if (dt.Columns.Count == 0)
        //                {
        //                    for (int j = 0; j < row.LastCellNum; j++)
        //                    {
        //                        dt.Columns.Add(row.GetCell(j).ToString());
        //                    }

        //                    continue;
        //                }

        //                DataRow dr = dt.NewRow();

        //                for (int i = 0; i < row.LastCellNum; i++)
        //                {
        //                    var cell = row.GetCell(i);

        //                    if (cell == null)
        //                    {
        //                        dr[i] = null;
        //                    }

        //                    else
        //                    {
        //                        dr[i] = cell.ToString();
        //                    }
        //                }
        //                dt.Rows.Add(dr);
        //            }
        //        }

        //        DriverManager drvMng = new DriverManager();
        //        UserManager usrManager = new UserManager();
        //        ShipmentManager shipmentManager = new ShipmentManager();

        //        var shippingPoints = shipmentManager.GetShippingPoints();

        //        var roles = usrManager.GetUserRoleList().ToList();

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            if ((dr != dt.Rows[0]) && (dr != dt.Rows[1]))
        //            {
        //                var roleName = dr["Role"].ToString();
        //                var userName = dr["UserName"].ToString();
        //                var shippingPointName = dr["ShippingPoint"].ToString();

        //                var role = roles.FirstOrDefault(m => m.RoleName == roleName);
        //                var user = usrManager.GetUserList().FirstOrDefault(m => m.UserName == userName);
        //                var shippingPointRecord = shippingPoints.FirstOrDefault(m => m.CodeValue == shippingPointName);

        //                if(user == null)
        //                {
        //                    user = new User();
        //                    user.UserName = userName;
        //                    user.UserPWD = "1111";
        //                    user.UserPWDHash = "sha1:1000:bQVkP3gl8eCL51rLgJ/BSmHbjb9rsQS9:fUjVSUF6CIYxyblkhyLDdKdO4k69oukA";
        //                    user.UserSupervisoryLevel = 0;
        //                    user.Registered = true;

        //                    if (shippingPointRecord == null)
        //                    {
        //                        user.ShippingPoint = 0;
        //                    }
        //                    else
        //                    {
        //                        user.ShippingPoint = (int)shippingPointRecord.Id;
        //                    }

        //                    var isActive = false;
        //                    bool.TryParse(dr["IsActive"].ToString(), out isActive);
        //                    user.IsActive = isActive;

        //                    if (role != null)
        //                    {
        //                        user.UserRoleUser.Add(new UserRoleUser() { UserRoleID = role.UserRoleID, IsActive = true });
        //                    }

        //                    user.CreateDateTime = DateTime.Now;
        //                    user.ChangeDateTime = DateTime.Now;

        //                    var result = usrManager.AddUser(user);
        //                }
        //                else
        //                {
        //                    var isActive = false;
        //                    bool.TryParse(dr["IsActive"].ToString(), out isActive);
        //                    user.IsActive = isActive;

        //                    if (shippingPointRecord == null)
        //                    {
        //                        user.ShippingPoint = 0;
        //                    }
        //                    else
        //                    {
        //                        user.ShippingPoint = (int)shippingPointRecord.Id;
        //                    }

        //                    var userRoleUser = usrManager.GetUserRoleUserList(false).Where(m => m.UserID == user.UserID).FirstOrDefault();

        //                    if (role != null)
        //                    {
        //                        if(userRoleUser == null)
        //                        {
        //                            userRoleUser = new UserRoleUser();
        //                            userRoleUser.UserRoleID = role.UserRoleID;
        //                            userRoleUser.UserID = user.UserID;
        //                            userRoleUser.CreateDateTime = DateTime.Now;

        //                            usrManager.AddUserRoleUser(userRoleUser);
        //                        }
        //                        else
        //                        {
        //                            userRoleUser.UserRoleID = role.UserRoleID;
        //                            userRoleUser.ChangeDateTime = DateTime.Now;

        //                            usrManager.UpdateUserRoleUser(userRoleUser);
        //                        }
        //                    }

        //                    user.IsActive = isActive;
        //                    user.ChangeDateTime = DateTime.Now;
        //                    usrManager.UpdateUser(user);
        //                }
        //            }
        //        }
        //        System.IO.File.Delete(path);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this.repositoryBase != null)
            {
                this.repositoryBase = null;
            }
        }
                
        #endregion

      
    }
}
