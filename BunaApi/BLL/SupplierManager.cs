using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class SupplierManager : BaseManager,IDisposable
	{

		public SupplierManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Supplier Methods

		public IQueryable<Supplier> GetAllSupplier()
		{
			 try
			 {
				   return Repository.GetList<Supplier>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Supplier GetSupplier(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Supplier, object>>> includepaths = new List<Expression<Func<Supplier, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.User2);

					// Add includepaths into method here if used and not null
					return Repository.Get<Supplier>(includepaths, p => p.SupplierID == id);

				}
				else
				{

					return Repository.Get<Supplier>(null,p => p.SupplierID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Supplier> GetSupplier(GridParam filters)
		{
			try
			{
				GridResult<Supplier> result = new GridResult<Supplier>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Supplier>().Count();

				Expression <Func<Supplier, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Supplier>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Supplier>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Supplier> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Supplier, object>>> includepaths = new List<Expression<Func<Supplier, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.User2);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Supplier, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Supplier, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Supplier>(where).OrderBy(m => m.SupplierID);
				}
				else
				{
				list = Repository.GetList<Supplier>().OrderBy(m => m.SupplierID);
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
					list = list.OrderBy(o => o.SupplierID);
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
		public int AddSupplier(Supplier newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Supplier>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Supplier AddReturnSupplier(Supplier newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Supplier>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateSupplier(Supplier editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Supplier>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteSupplier(int id)
		{
			try
			{
				Supplier item = this.GetSupplier(id, false);
				return Repository.UoW.Delete<Supplier>(item);

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
