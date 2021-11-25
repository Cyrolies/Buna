using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALBase;
using DALEFModel;
using Common;
using DALEFBase;

namespace DSDBLL
{

	public sealed class AdminManager : BaseManager,IDisposable
	{
       
        public AdminManager()
		{
            base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}

        #region Organization Methods

        public Organization GetOrganization(int id, bool includeRelations)
        {
            try
            {
                if (includeRelations)
                {

                    //Add Includes here example below
                    //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                    //includepaths.Add(p => p.StcData);

                    // Add includepaths into method here if used and not null
                    return Repository.Get<Organization>(null, p => p.OrgID == id);

                }
                else
                {

                    return Repository.Get<Organization>(null, p => p.OrgID == id);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Organization> GetOrganization(bool includeRelations)
        {

            try
            {
                if (includeRelations)
                {

                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                    //includepaths.Add(p => p.StpDataType);

                    // Add includepaths into method here if used and not null
                    return Repository.GetList<Organization>();
                    //ENDCUSTOMCODE

                }
                else
                {

                    return Repository.GetList<Organization>();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddOrganization(Organization newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.Add<Organization>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateOrganization(Organization editEntity)
        {
            try
            {
                editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.Update<Organization>(editEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteOrganization(Organization deleteEntity)
        {
            try
            {
                deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Organization>(deleteEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteOrganization(int id)
        {
            try
            {
                Organization organization = this.GetOrganization(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Organization>(organization);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Application Methods

        public Application GetApplication(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StcData);

					// Add includepaths into method here if used and not null
					return Repository.Get<Application>(null, p => p.ApplicationID == id);

				}
				else
				{

					return Repository.Get<Application>(null,p => p.ApplicationID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public IQueryable<Application> GetApplication(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					// Add includepaths into method here if used and not null
					return Repository.GetList<Application>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<Application>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddApplication(Application newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Application>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateApplication(Application editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Application>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteApplication(Application deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<Application>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public int DeleteApplication(int id)
        {
            try
            {
                Application application = this.GetApplication(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Application>(application);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Page Methods

        public Page GetPage(int id, bool includeRelations)
        {
            try
            {
                if (includeRelations)
                {

                    //Add Includes here example below
                    //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                    //includepaths.Add(p => p.StcData);

                    // Add includepaths into method here if used and not null
                    return Repository.Get<Page>(null, p => p.PageID == id);

                }
                else
                {

                    return Repository.Get<Page>(null, p => p.PageID == id);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Page> GetPage(bool includeRelations)
        {

            try
            {
                if (includeRelations)
                {

                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    //List<Expression<Func<Page, object>>> includepaths = new List<Expression<Func<Page, object>>>();
                    //includepaths.Add(p => p.StpDataType);

                    // Add includepaths into method here if used and not null
                    return Repository.GetList<Page>();
                    //ENDCUSTOMCODE

                }
                else
                {

                    return Repository.GetList<Page>();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddPage(Page newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.Add<Page>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdatePage(Page editEntity)
        {
            try
            {
                editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.Update<Page>(editEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeletePage(Page deleteEntity)
        {
            try
            {
                deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Page>(deleteEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeletePage(int id)
        {
            try
            {
                Page page = this.GetPage(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Page>(page);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region EntityMenu Methods

        public EntityMenu GetEntityMenu(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StcData);

					// Add includepaths into method here if used and not null
					return Repository.Get<EntityMenu>(null, p => p.EntityMenuID == id);

				}
				else
				{

					return Repository.Get<EntityMenu>(null,p => p.EntityMenuID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public GridResult<EntityMenu> GetEntityMenuList(GridParam filters)
        {
            try
            {
                GridResult<EntityMenu> result = new GridResult<EntityMenu>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<EntityMenu>().Count();
                Expression<Func<EntityMenu, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<EntityMenu>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<EntityMenu>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<EntityMenu> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<EntityMenu, object>>> includepaths = new List<Expression<Func<EntityMenu, object>>>();
                   // includepaths.Add(p => p..StpData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<EntityMenu, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<EntityMenu, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<EntityMenu>(where);
                    }
                    else
                    {
                        list = Repository.GetList<EntityMenu>();
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
                    list = list.OrderBy(o => o.EntityMenuID);
                }
                //Get total filtered rows before paging is applied 
                result.TotalFilteredCount = list.Count();
                list = list.Skip(filters.PageNo).Take(filters.PageSize);

                result.Items = list;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<EntityMenu> GetEntityMenuList(bool includeRelations, Expression<Func<EntityMenu, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
        {

            try
            {
                IQueryable<EntityMenu> list;

                if (includeRelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<EntityMenu, object>>> includepaths = new List<Expression<Func<EntityMenu, object>>>();
                    //includepaths.Add(p => p.Activity);
                    //includepaths.Add(p => p.StcData);
                    //includepaths.Add(p => p.StcData1);
                    //includepaths.Add(p => p.UserRole);
                    //ENDCUSTOMCODE
                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<EntityMenu, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<EntityMenu, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<EntityMenu>(where);
                    }
                    else
                    {
                        list = Repository.GetList<EntityMenu>();
                    }

                }
                //resultList = list;
                ////APPLY ALL SORTING
                //List<User> newList = resultList.ToList();
                if (listOrderBy != null && listOrderBy.Count() > 0)
                {
                    foreach (var sort in listOrderBy)
                    {
                        list = list.OrderBy(sort.Key, sort.Value);
                    }
                }
                else
                {
                    list = list.OrderBy(o => o.MenuDisplayName);
                }
                //APPLY PAGE SIZE
                if (page > 0 && size > 0)
                {
                    list = list.Skip((page - 1) * size).Take(size);
                }

                
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EntityMenu> GetMenuList(User user)
        {
            var menuItems = new List<EntityMenu>();

            var items = GetEntityMenu(false).Where(o => o.IsActive == true).ToList();

            var isAdmin = true;

            if (isAdmin)
            {
                //Get permissions
                foreach (EntityMenu mnu in items)
                {
                    mnu.Param1 = "true";
                    menuItems.Add(mnu);
                }
            }
            else
            {
                foreach (EntityMenu mnu in items)
                {
                    if (mnu.Url != null)
                    {
                        mnu.Param1 = "false";

                        Entity ent = null;

                        if (mnu.EntityID.HasValue)
                        {
                            ent = GetEntity(mnu.EntityID.Value, false);
                        }
                        else
                        {
                            ent = GetEntity(mnu.Url.Substring(mnu.Url.LastIndexOf('/') + 1).Replace("Setup", ""), false);
                        }

                        if (ent != null && user != null)
                        {
                            bool noPermission = false;
                            if (user.UserRoleID > 0)
                            {
                                if (getFieldPermission(ent.ActivityID, user, ent.Name, out noPermission))
                                {
                                    if (!noPermission)
                                    {
                                        mnu.Param1 = "true";
                                    }

                                }
                            }
                        }
                    }
                    menuItems.Add(mnu);
                }
            }

            return menuItems;
        }

		public IQueryable<EntityMenu> GetEntityMenu(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					// Add includepaths into method here if used and not null
					return Repository.GetList<EntityMenu>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<EntityMenu>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddEntityMenu(EntityMenu newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<EntityMenu>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public EntityMenu AddReturnEntityMenu(EntityMenu newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<EntityMenu>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateEntityMenu(EntityMenu editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<EntityMenu>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteEntityMenu(EntityMenu deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<EntityMenu>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public int DeleteEntityMenu(int id)
        {
            try
            {
                EntityMenu entityMenu = this.GetEntityMenu(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<EntityMenu>(entityMenu);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region EntityResource Methods

        public EntityResource GetEntityResource(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

                    //Add Includes here example below
                    List<Expression<Func<EntityResource, object>>> includepaths = new List<Expression<Func<EntityResource, object>>>();
                    includepaths.Add(p => p.StcData);

                    // Add includepaths into method here if used and not null
                    return Repository.Get<EntityResource>(includepaths, p => p.ResourceID == id);

				}
				else
				{

					return Repository.Get<EntityResource>(null,p => p.ResourceID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public EntityResource GetEntityResource(string key,string culture, bool includeRelations)
        {
            try
            {
                if (includeRelations)
                {

                    //Add Includes here example below
                    List<Expression<Func<EntityResource, object>>> includepaths = new List<Expression<Func<EntityResource, object>>>();
                    includepaths.Add(p => p.StcData);

                    // Add includepaths into method here if used and not null
                    return Repository.Get<EntityResource>(includepaths, p => p.ResourceKey == key && p.ResourceCulture == culture);

                }
                else
                {

                    return Repository.Get<EntityResource>(null, p => p.ResourceKey == key && p.ResourceCulture == culture);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EntityResource> GetEntityResourceList(int OrgID)
        {
            try
            {
                List<EntityResource> list = Repository.GetList<EntityResource>(o => o.OrgID == OrgID).OrderBy(o=>o.ResourceID).ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public GridResult<EntityResource> GetEntityResourceList(GridParam filters)
        {
            try
            {
                GridResult<EntityResource> result = new GridResult<EntityResource>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<EntityResource>().Count();

                Expression<Func<EntityResource, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<EntityResource>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<EntityResource>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<EntityResource> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<EntityResource, object>>> includepaths = new List<Expression<Func<EntityResource, object>>>();
                    includepaths.Add(p => p.Organization);
                    includepaths.Add(p => p.StcData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<EntityResource, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<EntityResource, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<EntityResource>(where);
                    }
                    else
                    {
                        list = Repository.GetList<EntityResource>();
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
                    list = list.OrderBy(o => o.ResourceID);
                }
                //Get total filtered rows before paging is applied 
                result.TotalFilteredCount = list.Count();
                list = list.Skip(filters.PageNo).Take(filters.PageSize);


                result.Items = list;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<EntityResource> GetEntityResource(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					// Add includepaths into method here if used and not null
					return Repository.GetList<EntityResource>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<EntityResource>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddEntityResource(EntityResource newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<EntityResource>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public EntityResource AddReturnEntityResource(EntityResource newEntity)
        {
            try
            {
                newEntity.StcStatusID = this.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<EntityResource>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateEntityResource(EntityResource editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<EntityResource>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteEntityResource(EntityResource deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<EntityResource>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public int DeleteEntityResource(int id)
        {
            try
            {
                EntityResource entityResource = this.GetEntityResource(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<EntityResource>(entityResource);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Stp Data Methods
        //public int GetSTPTypeFromEnum(string stptype)
        //{
        //    int stptypeid = 0;
        //    foreach (string str in Enumerations.getv.SetupDataType)

        //        return stptypeid;
        //}
        //put in custom code section
        public StpData GetStpData(int id, bool includeRelations)
        {

            if (includeRelations)
            {
                //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                //includepaths.Add(p => p.StcData);
                //includepaths.Add(p => p.Municipality);

                StpData data = Repository.Get<StpData>(null, p => p.StpDataID == id);

                return data;
            }
            else
            {
                return Repository.Get<StpData>(null, p => p.StpDataID == id);
            }
        }
        public StpData GetStpData(string abbre, bool includeRelations)
        {

            if (includeRelations)
            {
                //List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                //includepaths.Add(p => p.StcData);
                //includepaths.Add(p => p.Municipality);

                StpData data = Repository.Get<StpData>(null, p => p.DataCode.ToLower() == abbre.ToLower());

                return data;
            }
            else
            {
                return Repository.Get<StpData>(null, p => p.DataCode.ToLower() == abbre.ToLower());
            }
        }
        public GridResult<StpData> GetSTPDataList(GridParam filters)
        {
            try
            {
                GridResult<StpData> result = new GridResult<StpData>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<StpData>().Count();
                Expression<Func<StpData, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StpData>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StpData>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<StpData> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
                    includepaths.Add(p => p.StpDataType);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<StpData, string>(includepaths, where,true);
                    }
                    else
                    {
                        list = Repository.GetList<StpData, string>(includepaths, true);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<StpData>(where);
                    }
                    else
                    {
                        list = Repository.GetList<StpData>();
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
                    list = list.OrderBy(o => o.StpDataID);
                }
                //APPLY PAGE SIZE
                //Get total filtered rows before paging is applied 
                result.TotalFilteredCount = list.Count();
                list = list.Skip(filters.PageNo).Take(filters.PageSize);
                if (filters.Includerelations)// Add includepaths into method 
                {
                    List<StpData> returnlist = new List<StpData>();
                    foreach (StpData stdata in list)
                    {
                        stdata.StpDataType = this.Repository.Get<StpDataType>(o => o.StpDataTypeID == stdata.StpDataTypeID);
                        returnlist.Add(stdata);
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


        //put in custom code section
        /// <summary>
        /// Lists the STP data.
        /// </summary>
        /// <param name="typeID">The type identifier.</param>
        /// <returns></returns>
        public IQueryable<StpData> ListStpData(Enumerations.SetupDataType? typeID)
        {
            List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
            includepaths.Add(p => p.StpDataType);
            if (typeID == null)
            {
                return Repository.GetList<StpData, string>(includepaths, e => e.DataCode);
            }
            else
            {
                return Repository.GetList<StpData, string>(includepaths, p => p.StpDataTypeID == (int)typeID, e => e.DataCode);
            }
        }

        public int AddStpData(StpData newEntity)
        {
            return Repository.UoW.Add<StpData>(newEntity);
        }

        public StpData AddReturnStpData(StpData newEntity)
        {
            try
            {
                newEntity.StcStatusID = this.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<StpData>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateStpData(StpData editEntity)
        {
            return Repository.UoW.Update<StpData>(editEntity);
        }

        public int DeleteStpData(StpData deleteEntity)
        {
            return Repository.UoW.Delete<StpData>(deleteEntity);
        }

        public int DeleteStpData(int id)
        {
            try
            {
                StpData stpData = this.GetStpData(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<StpData>(stpData);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
      
        #region Dispose
        public void Dispose()
		{
            
        }

        
        #endregion
    }
}
