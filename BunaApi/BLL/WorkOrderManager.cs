using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class WorkOrderManager : BaseManager,IDisposable
	{

		public WorkOrderManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region WorkOrder Methods

		public IQueryable<WorkOrder> GetWorkOrder()
		{
			 try
			 {
				   return Repository.GetList<WorkOrder>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public WorkOrder GetWorkOrder(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					List<Expression<Func<WorkOrder, object>>> includepaths = new List<Expression<Func<WorkOrder, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.Organization);

					// Add includepaths into method here if used and not null
					return Repository.Get<WorkOrder>(null, p => p.WorkOrderID == id);

				}
				else
				{

					return Repository.Get<WorkOrder>(null,p => p.WorkOrderID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<WorkOrder> GetWorkOrder(GridParam filters)
		{
			try
			{
				GridResult<WorkOrder> result = new GridResult<WorkOrder>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<WorkOrder>().Count();

				Expression <Func<WorkOrder, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<WorkOrder>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<WorkOrder>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<WorkOrder> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					List<Expression<Func<WorkOrder, object>>> includepaths = new List<Expression<Func<WorkOrder, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.Organization);


					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<WorkOrder, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<WorkOrder, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<WorkOrder>(where).OrderBy(m => m.WorkOrderID);
				}
				else
				{
				list = Repository.GetList<WorkOrder>().OrderBy(m => m.WorkOrderID);
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
					list = list.OrderBy(o => o.WorkOrderID);
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
		public IQueryable<WorkOrder> GetWorkOrder(bool includeRelations, Expression<Func<WorkOrder, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
		{
			try
			{

				IQueryable<WorkOrder> list = null;
				if (includeRelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					//APPLY FILTERS
					//if (where != null)
					//{
					//list = Repository.GetList<WorkOrder, string>(includepaths, where);
					//}
					//else
					//{
					//list = Repository.GetList<WorkOrder, string>(includepaths);
				//}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<WorkOrder>(where).OrderBy(m => m.WorkOrderID);
				}
				else
				{
				list = Repository.GetList<WorkOrder>().OrderBy(m => m.WorkOrderID);
				}
			}

			//APPLY ALL SORTING
			if (listOrderBy != null && listOrderBy.Count() > 0)
			{
			foreach (var sort in listOrderBy)
			{
			list = QueryFilters.Sort<WorkOrder>(list.ToList(), sort.Key, sort.Value).AsQueryable();
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
		public IQueryable<WorkOrder> GetWorkOrder(bool includeRelations)
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
					return Repository.GetList<WorkOrder>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<WorkOrder>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddWorkOrder(WorkOrder newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<WorkOrder>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public WorkOrder AddReturnWorkOrder(WorkOrder newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<WorkOrder>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateWorkOrder(WorkOrder editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<WorkOrder>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteWorkOrder(int id)
		{
			try
			{
				WorkOrder item = this.GetWorkOrder(id, false);
				return Repository.UoW.Delete<WorkOrder>(item);

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
