using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class ProductionManager : BaseManager,IDisposable
	{

		public ProductionManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Production Methods

		public IQueryable<Production> GetAllProduction()
		{
			 try
			 {
				   return Repository.GetList<Production>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Production GetProduction(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Production, object>>> includepaths = new List<Expression<Func<Production, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.User2);
					includepaths.Add(p => p.Asset);

					// Add includepaths into method here if used and not null
					return Repository.Get<Production>(includepaths, p => p.ProductionID == id);

				}
				else
				{

					return Repository.Get<Production>(null,p => p.ProductionID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Production> GetProduction(GridParam filters)
		{
			try
			{
				GridResult<Production> result = new GridResult<Production>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Production>().Count();

				Expression <Func<Production, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Production>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Production>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Production> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Production, object>>> includepaths = new List<Expression<Func<Production, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.Organization);	
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.User2);
					includepaths.Add(p => p.Asset);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Production, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Production, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Production>(where).OrderBy(m => m.ProductionID);
				}
				else
				{
				list = Repository.GetList<Production>().OrderBy(m => m.ProductionID);
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
					list = list.OrderBy(o => o.ProductionID);
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
		public int AddProduction(Production newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Production>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Production AddReturnProduction(Production newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Production>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateProduction(Production editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Production>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteProduction(int id)
		{
			try
			{
				Production item = this.GetProduction(id, false);
				return Repository.UoW.Delete<Production>(item);

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
