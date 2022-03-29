using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class UserManager : BaseManager,IDisposable
	{

		public UserManager()
		{
            base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}
        
        #region User Methods
              
        public IQueryable<User> GetUserList()
        {
            try
            {
                return Repository.GetList<User>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetUser(int id, bool includeRelations)
        {
            try
            {
                User returnUser = new User();
                if (includeRelations)
                {

                    //Add Includes here example below
                    List<Expression<Func<User, object>>> includepaths = new List<Expression<Func<User, object>>>();
                    includepaths.Add(p => p.UserRole);
                    includepaths.Add(p => p.StpData);
                    includepaths.Add(p => p.StpData1);
                    includepaths.Add(p => p.StpData2);
                    includepaths.Add(p => p.Organization1);
                    includepaths.Add(p => p.User2);
                    includepaths.Add(p => p.User3);
                    includepaths.Add(p => p.StcData);

                    // Add includepaths into method here if used and not null
                    returnUser = Repository.Get<User>(includepaths, p => p.UserID == id);
                     
                }
                else
                {

                    returnUser = Repository.Get<User>(null, p => p.UserID == id);

                }
                
                return returnUser;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetUser(string username,bool active, bool includeRelations)
        {
            try
            {
                User returnUser = new User();
                if (includeRelations)
                {

                    //Add Includes here example below
                    List<Expression<Func<User, object>>> includepaths = new List<Expression<Func<User, object>>>();
                    includepaths.Add(p => p.UserRole);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.Organization1);
					includepaths.Add(p => p.User2);
					includepaths.Add(p => p.User3);
                    includepaths.Add(p => p.Person2);
                    includepaths.Add(p => p.Supplier2);


                    // Add includepaths into method here if used and not null
                    returnUser = Repository.Get<User>(includepaths, p => p.UserName == username && p.IsActive == active);
                    //Get Permissions for this userrole
                    returnUser.UserRoleActivity = Repository.GetList<UserRoleActivity>(p => p.UserRoleID == returnUser.UserRoleID).ToList();

                }
                else
                {

                    returnUser = Repository.Get<User>(null, p => p.UserName == username && p.IsActive == active);
                }
                
                return returnUser;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public IQueryable<User> GetUser(bool includeRelations, Expression<Func<User, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
        //{

        //    try
        //    {
        //        IQueryable<User> list;
        //        IQueryable<User> resultList = null;
        //        if (includeRelations)// Add includepaths into method 
        //        {
        //            //Add Includes here example below
        //            //STARTCUSTOMCODE
        //            List<Expression<Func<User, object>>> includepaths = new List<Expression<Func<User, object>>>();
        //            includepaths.Add(p => p.UserRole);
        //            includepaths.Add(p => p.StpData);
        //            includepaths.Add(p => p.StpData1);
        //            includepaths.Add(p => p.StpData2);
        //            includepaths.Add(p => p.Organization1);
        //            includepaths.Add(p => p.User2);
        //            includepaths.Add(p => p.User3);
        //            includepaths.Add(p => p.StcData);


        //            //ENDCUSTOMCODE
        //            //APPLY FILTERS
        //            if (where != null)
        //            {
        //                list = Repository.GetList<User, string>(includepaths, where);
        //            }
        //            else
        //            {
        //                list = Repository.GetList<User, string>(includepaths);
        //            }
                    
        //        }
        //        else
        //        {
        //            if (where != null)
        //            {
        //                list = Repository.GetList<User>(where);
        //            }
        //            else
        //            {
        //                list = Repository.GetList<User>();
        //            }
                    
        //        }
        //        //if (shippingPointID > 0)
        //        //{
        //        //    list = list.Where(o => o.ShippingPoint == shippingPointID);
        //        //}
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
        //            list = list.OrderBy(o => o.UserName);
        //        }
        //        //APPLY PAGE SIZE
        //        if (page > 0 && size > 0)
        //        {
        //            list = list.Skip((page - 1) * size).Take(size);
        //        }
        //        if (includeRelations)// Add includepaths into method 
        //        {
        //            //foreach (User user in list)
        //            //{
        //            //    if (user.ShippingPoint == 0)
        //            //    {
        //            //        user.Email = "All";
        //            //    }
        //            //    else
        //            //    {
        //            //        CodeDesc shippingpoint = soman.Get<CodeDesc>(o => o.Id == user.ShippingPoint);
        //            //        if (shippingpoint != null)
        //            //        {
        //            //            user.Email = shippingpoint.CodeValue + "-" + shippingpoint.CodeDescription;
        //            //        }
        //            //    }
        //            //}
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public GridResult<User> GetUserList(GridParam filters)
        {
            try
            {
                GridResult<User> result = new GridResult<User>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<User>().Count();
                Expression<Func<User, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<User>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<User>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<User> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<User, object>>> includepaths = new List<Expression<Func<User, object>>>();
                    includepaths.Add(p => p.UserRole);
                    includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					//includepaths.Add(p => p.Organization1);
					includepaths.Add(p => p.User2);
					includepaths.Add(p => p.User3);
					includepaths.Add(p => p.StcData);
					//ENDCUSTOMCODE

					//APPLY FILTERS
					if (where != null)
                    {
                        list = Repository.GetList<User, string>(includepaths,where);
                    }
                    else
                    {
                        list = Repository.GetList<User, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<User>(where);
                    }
                    else
                    {
                        list = Repository.GetList<User>();
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
                    list = list.OrderBy(o => o.UserID);
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

        public int AddUser(User newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                newEntity.UserPWDHash = PasswordHash.CreateHash("1111");
                return Repository.UoW.Add<User>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public User AddReturnUser(User newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<User>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int UpdateUser(User editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                
              //  editEntity.UserPWDHash = Common.PasswordHash.CreateHash(editEntity.UserPWD);
                return Repository.UoW.Update<User>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteUser(User deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<User>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public int DeleteUser(int userID)
        {
            try
            {
                User user = this.GetUser(userID, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<User>(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveUserSettings(int userID, int? languageID, int? themeID, int? OrgID, int? ShippingPointID)
        {
            try
            {
                User user = Repository.Get<User>(o => o.UserID == userID);
                user.StpLanguageID = languageID;
                user.StpThemeID = themeID;
                user.OrgID = OrgID;

                return Repository.UoW.Update<User>(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public int AddUserForRegistering(User newEntity)
        {
            try
            {
                return Repository.UoW.Add<User>(newEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UserRole Methods

        public IQueryable<UserRole> GetUserRoleList(bool includeRelations, Expression<Func<UserRole, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
        {

            try
            {
                IQueryable<UserRole> list;

                if (includeRelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<UserRole, object>>> includepaths = new List<Expression<Func<UserRole, object>>>();
                    includepaths.Add(p => p.StcData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<UserRole, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRole, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<UserRole>(where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRole>();
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
                    list = list.OrderBy(o => o.RoleName);
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

        public GridResult<UserRole> GetUserRoleList(GridParam filters)
        {
            try
            {
                GridResult<UserRole> result = new GridResult<UserRole>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<UserRole>().Count();
                Expression<Func<UserRole, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<UserRole>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<UserRole>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<UserRole> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<UserRole, object>>> includepaths = new List<Expression<Func<UserRole, object>>>();
                    includepaths.Add(p => p.UserRoleActivity);
                   
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<UserRole, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRole, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<UserRole>(where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRole>();
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
                    list = list.OrderBy(o => o.UserRoleID);
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

        public UserRole GetUserRole(int id,bool includeRelations, bool includeDropDownDData = true)
		{
			try
			{
                UserRole returnUserRole = new UserRole();
				if (includeRelations)
				{

                    //Add Includes here example below
                    List<Expression<Func<UserRole, object>>> includepaths = new List<Expression<Func<UserRole, object>>>();
                    includepaths.Add(p => p.StcData);
                    includepaths.Add(p => p.UserRoleActivity);

                    // Add includepaths into method here if used and not null
                    returnUserRole = Repository.Get<UserRole>(includepaths, p => p.UserRoleID == id);

				}
				else
				{

                    returnUserRole = Repository.Get<UserRole>(null,p => p.UserRoleID == id);

				}
                if (includeDropDownDData)
                {
                    returnUserRole.Users = Repository.GetList<User>(o => o.IsActive == true).ToList();
                }
                return returnUserRole;


            }
            catch(Exception ex)
			{
				throw ex;
			}
		}

      
        public int AddUserRole(UserRole newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<UserRole>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public UserRole AddReturnUserRole(UserRole newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<UserRole>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateUserRole(UserRole editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<UserRole>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteUserRole(UserRole deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<UserRole>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public int DeleteUserRole(int id)
        {
            try
            {
                UserRole item = this.GetUserRole(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<UserRole>(item);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UserRoleActivity Methods

        public UserRoleActivity GetUserRoleActivity(int id,bool includeRelations, bool includeDropDownDData = true)
		{
			try
			{
                UserRoleActivity returnURActivity = new UserRoleActivity();
                if (includeRelations)
                {

                    //Add Includes here example below
                    List<Expression<Func<UserRoleActivity, object>>> includepaths = new List<Expression<Func<UserRoleActivity, object>>>();
                    includepaths.Add(p => p.Activity);
                    includepaths.Add(p => p.StcData);
                    includepaths.Add(p => p.StcData1);
                    includepaths.Add(p => p.UserRole);

                    returnURActivity = Repository.Get<UserRoleActivity>(includepaths, p => p.UserRoleActivityID == id);

                }
                else
                {

                    returnURActivity = Repository.Get<UserRoleActivity>(null, p => p.UserRoleActivityID == id);

                }
                if (includeDropDownDData && returnURActivity != null)
                {
                    returnURActivity.Activities = Repository.GetList<Activity>(o => o.IsActive == true).ToList();
                    returnURActivity.UserRoles = Repository.GetList<UserRole>(o => o.IsActive == true).ToList();
                    returnURActivity.Permissions = Repository.GetList<StcData>(o => o.StcDataTypeID == (int)Enumerations.StaticDataType.Permission).ToList();
                }
                return returnURActivity;

            }
            catch(Exception ex)
			{
				throw ex;
			}
		}

        public IQueryable<UserRoleActivity> GetUserRoleActivityList(bool includeRelations, Expression<Func<UserRoleActivity, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
        {

            try
            {
                IQueryable<UserRoleActivity> list;
                
                if (includeRelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<UserRoleActivity, object>>> includepaths = new List<Expression<Func<UserRoleActivity, object>>>();
                    includepaths.Add(p => p.Activity);
                    includepaths.Add(p => p.StcData);
                    includepaths.Add(p => p.StcData1);
                    includepaths.Add(p => p.UserRole);
                    //ENDCUSTOMCODE
                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<UserRoleActivity, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRoleActivity, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<UserRoleActivity>(where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRoleActivity>();
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
                    list = list.OrderBy(o => o.ActivityID);
                }
                //APPLY PAGE SIZE
                if (page > 0 && size > 0)
                {
                    list = list.Skip((page - 1) * size).Take(size);
                }

                //foreach (UserRoleActivity usa in list)
                //{
                //    if (usa.Activity != null && usa.UserRole != null)
                //    {
                //        usa.UserRoleActivityName = usa.Activity.ActivityName + "|" + usa.UserRole.RoleName;
                //    }
                //}

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GridResult<UserRoleActivity> GetUserRoleActivityList(GridParam filters)
        {
            try
            {
                GridResult<UserRoleActivity> result = new GridResult<UserRoleActivity>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<UserRoleActivity>().Count();
                Expression<Func<UserRoleActivity, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<UserRoleActivity>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<UserRoleActivity>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<UserRoleActivity> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<UserRoleActivity, object>>> includepaths = new List<Expression<Func<UserRoleActivity, object>>>();
                    includepaths.Add(p => p.UserRole);
                    includepaths.Add(p => p.Activity);
                    includepaths.Add(p => p.StcData1);
                    includepaths.Add(p => p.StcData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<UserRoleActivity, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRoleActivity, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<UserRoleActivity>(where);
                    }
                    else
                    {
                        list = Repository.GetList<UserRoleActivity>();
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
                    list = list.OrderBy(o => o.ActivityID);
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

        public IQueryable<UserRoleActivity> GetUserRoleActivity(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{
                    List<UserRoleActivity> list = new List<UserRoleActivity>();
					//Add Includes here example below
					//STARTCUSTOMCODE
                    List<Expression<Func<UserRoleActivity, object>>> includepaths = new List<Expression<Func<UserRoleActivity, object>>>();
                    includepaths.Add(p => p.Activity);
                    includepaths.Add(p => p.StcData);
                    includepaths.Add(p => p.StcData1);
                    includepaths.Add(p => p.UserRole);

                    list = Repository.GetList<UserRoleActivity, int>(includepaths, o => o.ActivityID).ToList();

                    //foreach (UserRoleActivity usa in list)
                    //{
                    //    if (usa.Activity != null && usa.UserRole != null)
                    //    {
                    //        usa.UserRoleActivityName = usa.Activity.ActivityName + "|" + usa.UserRole.RoleName;
                    //    }
                    //}
                    // Add includepaths into method here if used and not null
                    //return Repository.GetList<UserRoleActivity, int>(includepaths, o => o.ActivityID);
					//ENDCUSTOMCODE

                    return list.AsQueryable();

				}
				else
				{

					return Repository.GetList<UserRoleActivity>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddUserRoleActivity(UserRoleActivity newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<UserRoleActivity>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public UserRoleActivity AddReturnUserRoleActivity(UserRoleActivity newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<UserRoleActivity>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateUserRoleActivity(UserRoleActivity editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<UserRoleActivity>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteUserRoleActivity(UserRoleActivity deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<UserRoleActivity>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

        public int DeleteUserRoleActivity(int id)
        {
            try
            {
                UserRoleActivity item = this.GetUserRoleActivity(id, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<UserRoleActivity>(item);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UserRoleNotificationLevel Methods

        public UserRoleNotificationLevel GetUserRoleNotificationLevel(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
                    List<Expression<Func<UserRoleNotificationLevel, object>>> includepaths = new List<Expression<Func<UserRoleNotificationLevel, object>>>();
                    includepaths.Add(p => p.UserRoleActivity);
                    includepaths.Add(p => p.UserRoleActivity.Activity);
                    includepaths.Add(p => p.UserRoleActivity.UserRole);

					// Add includepaths into method here if used and not null
                    return Repository.Get<UserRoleNotificationLevel>(includepaths, p => p.UserRoleNotificationLevelID == id);

				}
				else
				{

					return Repository.Get<UserRoleNotificationLevel>(null,p => p.UserRoleNotificationLevelID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public IQueryable<UserRoleNotificationLevel> GetUserRoleNotificationLevel(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
                    List<Expression<Func<UserRoleNotificationLevel, object>>> includepaths = new List<Expression<Func<UserRoleNotificationLevel, object>>>();
                    includepaths.Add(p => p.UserRoleActivity);
                                        
					// Add includepaths into method here if used and not null
                    return Repository.GetList<UserRoleNotificationLevel, int>(includepaths, o => o.UserRoleNotificationLevelID);
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<UserRoleNotificationLevel>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddUserRoleNotificationLevel(UserRoleNotificationLevel newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<UserRoleNotificationLevel>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateUserRoleNotificationLevel(UserRoleNotificationLevel editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<UserRoleNotificationLevel>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteUserRoleNotificationLevel(UserRoleNotificationLevel deleteEntity)
		{
			try
			{
				 deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
				return Repository.UoW.Delete<UserRoleNotificationLevel>(deleteEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
        #endregion
     
        #region Activity Methods

        public GridResult<Activity> GetActivityList(GridParam filters)
        {
            try
            {
                GridResult<Activity> result = new GridResult<Activity>();
                //Get total rows before filtering is applied
                result.TotalCount = this.GetList<Activity>().Count();
                Expression<Func<Activity, bool>> where = null;
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
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Activity>(field.Property, field.Operator, field.Value);
                        }
                        else
                        {
                            where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Activity>(field.Property, field.Operator, field.Value, where);
                        }

                    }
                }

                IQueryable<Activity> list;

                if (filters.Includerelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<Activity, object>>> includepaths = new List<Expression<Func<Activity, object>>>();
                    includepaths.Add(p => p.StpData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<Activity,string>(includepaths,where);
                    }
                    else
                    {
                        list = Repository.GetList<Activity, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<Activity>(where);
                    }
                    else
                    {
                        list = Repository.GetList<Activity>();
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
                    list = list.OrderBy(o => o.ActivityID);
                }
                //Get total filtered rows before paging is applied 
                result.TotalFilteredCount = list.Count();
                list = list.Skip(filters.PageNo).Take(filters.PageSize);

                List<Activity> activities = new List<Activity>();
                foreach (Activity act in list)
                {
                    
                        if (act.StpActivityGroupID > 0)
                        {
                            IQueryable<StpData> datas = Repository.GetList<StpData>(o => o.StpDataTypeID == (int)Enumerations.SetupDataType.ActivityGroup && o.StpDataID == act.StpActivityGroupID);
                            if (datas.Count() > 0)
                            {
                                act.StpActivityGroup = datas.SingleOrDefault() != null ? datas.SingleOrDefault().DataDescription : "";
                            }
                        }
                    
                    activities.Add(act);
                }

                result.Items = list;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Activity GetActivity(int id, bool includeRelations)
        {
            try
            {
                Activity returnActivity = new Activity();
                if (includeRelations)
                {
                    //Add Includes here example below
                    List<Expression<Func<Activity, object>>> includepaths = new List<Expression<Func<Activity, object>>>();
                    includepaths.Add(p => p.StcData);
                    includepaths.Add(p => p.StpData);

                    // Add includepaths into method here if used and not null
                    returnActivity = Repository.Get<Activity>(includepaths, p => p.ActivityID == id);

                }
                else
                {
                    returnActivity = Repository.Get<Activity>(null, p => p.ActivityID == id);
                }
                return returnActivity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<Activity> GetActivityList(bool includeRelations, Expression<Func<Activity, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
        {

            try
            {
                IQueryable<Activity> list;

                if (includeRelations)// Add includepaths into method 
                {
                    //Add Includes here example below
                    //STARTCUSTOMCODE
                    List<Expression<Func<Activity, object>>> includepaths = new List<Expression<Func<Activity, object>>>();
                    includepaths.Add(p => p.StcData);
                    //ENDCUSTOMCODE

                    //APPLY FILTERS
                    if (where != null)
                    {
                        list = Repository.GetList<Activity, string>(includepaths, where);
                    }
                    else
                    {
                        list = Repository.GetList<Activity, string>(includepaths);
                    }

                }
                else
                {
                    if (where != null)
                    {
                        list = Repository.GetList<Activity>(where);
                    }
                    else
                    {
                        list = Repository.GetList<Activity>();
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
                    list = list.OrderBy(o => o.ActivityName);
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

        public IQueryable<Activity> GetActivity(bool includeRelations)
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
                    return Repository.GetList<Activity>();
                    //ENDCUSTOMCODE

                }
                else
                {

                    return Repository.GetList<Activity>();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddActivity(Activity newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.Add<Activity>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Activity AddReturnActivity(Activity newEntity)
        {
            try
            {
                newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.AddEntityReturnEntity<Activity>(newEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateActivity(Activity editEntity)
        {
            try
            {
                editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
                return Repository.UoW.Update<Activity>(editEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteActivity(Activity deleteEntity)
        {
            try
            {
                deleteEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, deleteEntity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Activity>(deleteEntity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteActivity(int activityID)
        {
            try
            {
                Activity activity = this.GetActivity(activityID, false);
                // activity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Delete, activity.setToPendingUsable);//Sets to Pending Deletion if supervision On else Approved if Off
                return Repository.UoW.Delete<Activity>(activity);

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
