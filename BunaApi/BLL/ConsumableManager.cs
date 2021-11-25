using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class ConsumableManager : BaseManager,IDisposable
	{

		public ConsumableManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Consumable Methods

		public IQueryable<Consumable> GetAllConsumable()
		{
			 try
			 {
				   return Repository.GetList<Consumable>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Consumable GetConsumable(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Consumable, object>>> includepaths = new List<Expression<Func<Consumable, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.Asset);
					includepaths.Add(p => p.Asset1);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<Consumable>(includepaths, p => p.ConsumableID == id);

				}
				else
				{

					return Repository.Get<Consumable>(null,p => p.ConsumableID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Consumable> GetConsumable(GridParam filters)
		{
			try
			{
				GridResult<Consumable> result = new GridResult<Consumable>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Consumable>().Count();

				Expression <Func<Consumable, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Consumable>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Consumable>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Consumable> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Consumable, object>>> includepaths = new List<Expression<Func<Consumable, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.Asset);
					includepaths.Add(p => p.Asset1);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Consumable, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Consumable, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Consumable>(where).OrderBy(m => m.ConsumableID);
				}
				else
				{
				list = Repository.GetList<Consumable>().OrderBy(m => m.ConsumableID);
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
					list = list.OrderBy(o => o.ConsumableID);
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
		public int AddConsumable(Consumable newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Consumable>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Consumable AddReturnConsumable(Consumable newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Consumable>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateConsumable(Consumable editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Consumable>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteConsumable(int id)
		{
			try
			{
				Consumable item = this.GetConsumable(id, false);
				return Repository.UoW.Delete<Consumable>(item);

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
