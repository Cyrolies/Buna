using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class WorkOrderPartManager : BaseManager,IDisposable
	{

		public WorkOrderPartManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region WorkOrderPart Methods

		public IQueryable<WorkOrderPart> GetWorkOrderPart()
		{
			 try
			 {
				   return Repository.GetList<WorkOrderPart>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public WorkOrderPart GetWorkOrderPart(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					List<Expression<Func<WorkOrderPart, object>>> includepaths = new List<Expression<Func<WorkOrderPart, object>>>();
					includepaths.Add(p => p.Part);
					includepaths.Add(p => p.WorkOrder);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.CreatedByID);

					// Add includepaths into method here if used and not null
					return Repository.Get<WorkOrderPart>(null, p => p.WorkOrderPartID == id);

				}
				else
				{

					return Repository.Get<WorkOrderPart>(null,p => p.WorkOrderPartID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<WorkOrderPart> GetWorkOrderPart(GridParam filters)
		{
			try
			{
				GridResult<WorkOrderPart> result = new GridResult<WorkOrderPart>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<WorkOrderPart>().Count();

				Expression <Func<WorkOrderPart, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<WorkOrderPart>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<WorkOrderPart>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<WorkOrderPart> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					List<Expression<Func<WorkOrderPart, object>>> includepaths = new List<Expression<Func<WorkOrderPart, object>>>();
					includepaths.Add(p => p.Part);
					includepaths.Add(p => p.WorkOrder);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<WorkOrderPart, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<WorkOrderPart, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<WorkOrderPart>(where).OrderBy(m => m.WorkOrderPartID);
				}
				else
				{
				list = Repository.GetList<WorkOrderPart>().OrderBy(m => m.WorkOrderPartID);
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
					list = list.OrderBy(o => o.WorkOrderPartID);
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
		public IQueryable<WorkOrderPart> GetWorkOrderPart(bool includeRelations, Expression<Func<WorkOrderPart, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
		{
			try
			{

				IQueryable<WorkOrderPart> list = null;
				if (includeRelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					//APPLY FILTERS
					//if (where != null)
					//{
					//list = Repository.GetList<WorkOrderPart, string>(includepaths, where);
					//}
					//else
					//{
					//list = Repository.GetList<WorkOrderPart, string>(includepaths);
				//}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<WorkOrderPart>(where).OrderBy(m => m.WorkOrderPartID);
				}
				else
				{
				list = Repository.GetList<WorkOrderPart>().OrderBy(m => m.WorkOrderPartID);
				}
			}

			//APPLY ALL SORTING
			if (listOrderBy != null && listOrderBy.Count() > 0)
			{
			foreach (var sort in listOrderBy)
			{
			list = QueryFilters.Sort<WorkOrderPart>(list.ToList(), sort.Key, sort.Value).AsQueryable();
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
		public IQueryable<WorkOrderPart> GetWorkOrderPart(bool includeRelations)
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
					return Repository.GetList<WorkOrderPart>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<WorkOrderPart>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddWorkOrderPart(WorkOrderPart newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<WorkOrderPart>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public WorkOrderPart AddReturnWorkOrderPart(WorkOrderPart newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<WorkOrderPart>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateWorkOrderPart(WorkOrderPart editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<WorkOrderPart>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteWorkOrderPart(int id)
		{
			try
			{
				WorkOrderPart item = this.GetWorkOrderPart(id, false);
				return Repository.UoW.Delete<WorkOrderPart>(item);

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
