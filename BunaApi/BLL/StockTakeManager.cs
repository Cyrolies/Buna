using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class StockTakeManager : BaseManager,IDisposable
	{

		public StockTakeManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region StockTake Methods

		public IQueryable<StockTake> GetStockTake()
		{
			 try
			 {
				   return Repository.GetList<StockTake>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public StockTake GetStockTake(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					List<Expression<Func<StockTake, object>>> includepaths = new List<Expression<Func<StockTake, object>>>();
					includepaths.Add(p => p.Part);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);

					// Add includepaths into method here if used and not null
					return Repository.Get<StockTake>(includepaths, p => p.StockTakeID == id);

				}
				else
				{

					return Repository.Get<StockTake>(null,p => p.StockTakeID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<StockTake> GetStockTake(GridParam filters)
		{
			try
			{
				GridResult<StockTake> result = new GridResult<StockTake>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<StockTake>().Count();

				Expression <Func<StockTake, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StockTake>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StockTake>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<StockTake> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					List<Expression<Func<StockTake, object>>> includepaths = new List<Expression<Func<StockTake, object>>>();
					includepaths.Add(p => p.Part);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<StockTake, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<StockTake, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<StockTake>(where).OrderBy(m => m.StockTakeID);
				}
				else
				{
				list = Repository.GetList<StockTake>().OrderBy(m => m.StockTakeID);
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
					list = list.OrderBy(o => o.StockTakeID);
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
		public IQueryable<StockTake> GetStockTake(bool includeRelations, Expression<Func<StockTake, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
		{
			try
			{

				IQueryable<StockTake> list = null;
				if (includeRelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					//APPLY FILTERS
					//if (where != null)
					//{
					//list = Repository.GetList<StockTake, string>(includepaths, where);
					//}
					//else
					//{
					//list = Repository.GetList<StockTake, string>(includepaths);
				//}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<StockTake>(where).OrderBy(m => m.StockTakeID);
				}
				else
				{
				list = Repository.GetList<StockTake>().OrderBy(m => m.StockTakeID);
				}
			}

			//APPLY ALL SORTING
			if (listOrderBy != null && listOrderBy.Count() > 0)
			{
			foreach (var sort in listOrderBy)
			{
			list = QueryFilters.Sort<StockTake>(list.ToList(), sort.Key, sort.Value).AsQueryable();
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
		public IQueryable<StockTake> GetStockTake(bool includeRelations)
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
					return Repository.GetList<StockTake>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<StockTake>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddStockTake(StockTake newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<StockTake>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public StockTake AddReturnStockTake(StockTake newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<StockTake>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateStockTake(StockTake editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<StockTake>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteStockTake(int id)
		{
			try
			{
				StockTake item = this.GetStockTake(id, false);
				return Repository.UoW.Delete<StockTake>(item);

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
