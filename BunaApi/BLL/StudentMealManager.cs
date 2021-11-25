using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class StudentMealManager : BaseManager,IDisposable
	{

		public StudentMealManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region StudentMeal Methods

		public IQueryable<StudentMeal> GetAllStudentMeal()
		{
			 try
			 {
				   return Repository.GetList<StudentMeal>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public StudentMeal GetStudentMeal(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<StudentMeal, object>>> includepaths = new List<Expression<Func<StudentMeal, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<StudentMeal>(includepaths, p => p.StudentMealID == id);

				}
				else
				{

					return Repository.Get<StudentMeal>(null,p => p.StudentMealID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<StudentMeal> GetStudentMeal(GridParam filters)
		{
			try
			{
				GridResult<StudentMeal> result = new GridResult<StudentMeal>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<StudentMeal>().Count();

				Expression <Func<StudentMeal, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StudentMeal>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StudentMeal>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<StudentMeal> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<StudentMeal, object>>> includepaths = new List<Expression<Func<StudentMeal, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<StudentMeal, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<StudentMeal, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<StudentMeal>(where).OrderBy(m => m.StudentMealID);
				}
				else
				{
				list = Repository.GetList<StudentMeal>().OrderBy(m => m.StudentMealID);
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
					list = list.OrderBy(o => o.StudentMealID);
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
		public int AddStudentMeal(StudentMeal newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<StudentMeal>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public StudentMeal AddReturnStudentMeal(StudentMeal newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<StudentMeal>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateStudentMeal(StudentMeal editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<StudentMeal>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteStudentMeal(int id)
		{
			try
			{
				StudentMeal item = this.GetStudentMeal(id, false);
				return Repository.UoW.Delete<StudentMeal>(item);

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
