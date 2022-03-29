using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class ProductManager : BaseManager,IDisposable
	{

		public ProductManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Product Methods

		public IQueryable<Product> GetAllProduct()
		{
			 try
			 {
				   return Repository.GetList<Product>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Product GetProduct(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Product, object>>> includepaths = new List<Expression<Func<Product, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Supplier);
					// Add includepaths into method here if used and not null
					return Repository.Get<Product>(includepaths, p => p.ProductID == id);

				}
				else
				{

					return Repository.Get<Product>(null,p => p.ProductID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Product> GetProduct(GridParam filters)
		{
			try
			{
				GridResult<Product> result = new GridResult<Product>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Product>().Count();

				Expression <Func<Product, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Product>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Product>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Product> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Product, object>>> includepaths = new List<Expression<Func<Product, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Supplier);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Product, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Product, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Product>(where).OrderBy(m => m.ProductID);
				}
				else
				{
				list = Repository.GetList<Product>().OrderBy(m => m.ProductID);
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
					list = list.OrderBy(o => o.ProductID);
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
		public int AddProduct(Product newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Product>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Product AddReturnProduct(Product newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Product>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateProduct(Product editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Product>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteProduct(int id)
		{
			try
			{
				Product item = this.GetProduct(id, false);
				return Repository.UoW.Delete<Product>(item);

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
