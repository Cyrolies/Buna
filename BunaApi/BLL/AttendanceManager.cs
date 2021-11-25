using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class AttendanceManager : BaseManager,IDisposable
	{

		public AttendanceManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Attendance Methods

		public IQueryable<Attendance> GetAllAttendance()
		{
			 try
			 {
				   return Repository.GetList<Attendance>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Attendance GetAttendance(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Attendance, object>>> includepaths = new List<Expression<Func<Attendance, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<Attendance>(includepaths, p => p.AttendanceID == id);

				}
				else
				{

					return Repository.Get<Attendance>(null,p => p.AttendanceID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Attendance> GetAttendance(GridParam filters)
		{
			try
			{
				GridResult<Attendance> result = new GridResult<Attendance>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Attendance>().Count();

				Expression <Func<Attendance, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Attendance>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Attendance>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Attendance> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Attendance, object>>> includepaths = new List<Expression<Func<Attendance, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Attendance, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Attendance, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Attendance>(where).OrderBy(m => m.AttendanceID);
				}
				else
				{
				list = Repository.GetList<Attendance>().OrderBy(m => m.AttendanceID);
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
					list = list.OrderBy(o => o.AttendanceID);
				}

				//Get total filtered rows before paging is applied 
				result.TotalFilteredCount = list.Count();

			//APPLY PAGE SIZE
			list = list.Skip(filters.PageNo).Take(filters.PageSize);

			result.Items = list;
			return result;

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AddAttendance(Attendance newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Attendance>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Attendance AddReturnAttendance(Attendance newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Attendance>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateAttendance(Attendance editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Attendance>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteAttendance(int id)
		{
			try
			{
				Attendance item = this.GetAttendance(id, false);
				return Repository.UoW.Delete<Attendance>(item);

			}catch(Exception ex)
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
