using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class HitecJobManager : BaseManager,IDisposable
	{

		public HitecJobManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region HitecJob Methods

		public IQueryable<HitecJob> GetHitecJob()
		{
			 try
			 {
				   return Repository.GetList<HitecJob>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public HitecJob GetHitecJob(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					List<Expression<Func<HitecJob, object>>> includepaths = new List<Expression<Func<HitecJob, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);

					// Add includepaths into method here if used and not null
					return Repository.Get<HitecJob>(includepaths, p => p.HitecJobID == id);

				}
				else
				{

					return Repository.Get<HitecJob>(null,p => p.HitecJobID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<HitecJob> GetHitecJob(GridParam filters)
		{
			try
			{
				GridResult<HitecJob> result = new GridResult<HitecJob>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<HitecJob>().Count();

				Expression <Func<HitecJob, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<HitecJob>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<HitecJob>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<HitecJob> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{
					List<Expression<Func<HitecJob, object>>> includepaths = new List<Expression<Func<HitecJob, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<HitecJob, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<HitecJob, string>(includepaths);
					}
				}
				else
				{
					if (where != null)
					{
					list = Repository.GetList<HitecJob>(where).OrderBy(m => m.HitecJobID);
					}
					else
					{
					list = Repository.GetList<HitecJob>().OrderBy(m => m.HitecJobID);
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
					list = list.OrderBy(o => o.HitecJobID);
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
		public IQueryable<HitecJob> GetHitecJob(bool includeRelations, Expression<Func<HitecJob, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
		{
			try
			{

				IQueryable<HitecJob> list = null;
				if (includeRelations)// Add includepaths into method 
				{

					List<Expression<Func<HitecJob, object>>> includepaths = new List<Expression<Func<HitecJob, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
						list = Repository.GetList<HitecJob, string>(includepaths, where);
					}
					else
					{
						list = Repository.GetList<HitecJob, string>(includepaths);
					}
				}
				else
				{
					if (where != null)
					{
					list = Repository.GetList<HitecJob>(where).OrderBy(m => m.HitecJobID);
					}
					else
					{
					list = Repository.GetList<HitecJob>().OrderBy(m => m.HitecJobID);
					}
				}

			//APPLY ALL SORTING
			if (listOrderBy != null && listOrderBy.Count() > 0)
			{
			foreach (var sort in listOrderBy)
			{
			list = QueryFilters.Sort<HitecJob>(list.ToList(), sort.Key, sort.Value).AsQueryable();
			}
			}

			//APPLY PAGE SIZE
			if (page > 0 && size > 0)
			{
				list = list.Skip((page - 1) * size).Take(size);
			}
			return list;

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		
		public int AddHitecJob(HitecJob newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<HitecJob>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public HitecJob AddReturnHitecJob(HitecJob newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<HitecJob>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateHitecJob(HitecJob editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<HitecJob>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteHitecJob(int id)
		{
			try
			{
				HitecJob item = this.GetHitecJob(id, false);
				return Repository.UoW.Delete<HitecJob>(item);

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
