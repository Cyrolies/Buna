using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class AssetManager : BaseManager,IDisposable
	{

		public AssetManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Asset Methods

		public IQueryable<Asset> GetAllAsset()
		{
			 try
			 {
				   return Repository.GetList<Asset>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Asset GetAsset(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Asset, object>>> includepaths = new List<Expression<Func<Asset, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Asset2);//ParentAsset
					includepaths.Add(p => p.Person);//AssignedTo

					// Add includepaths into method here if used and not null
					return Repository.Get<Asset>(includepaths, p => p.AssetID == id);

				}
				else
				{

					return Repository.Get<Asset>(null,p => p.AssetID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Asset> GetAsset(GridParam filters)
		{
			try
			{
				GridResult<Asset> result = new GridResult<Asset>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Asset>().Count();

				Expression <Func<Asset, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Asset>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Asset>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Asset> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Asset, object>>> includepaths = new List<Expression<Func<Asset, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Asset2);//ParentAsset
					includepaths.Add(p => p.Person);//AssignedTo

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Asset, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Asset, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Asset>(where).OrderBy(m => m.AssetID);
				}
				else
				{
				list = Repository.GetList<Asset>().OrderBy(m => m.AssetID);
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
					list = list.OrderBy(o => o.AssetID);
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
		public int AddAsset(Asset newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Asset>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Asset AddReturnAsset(Asset newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Asset>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateAsset(Asset editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Asset>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteAsset(int id)
		{
			try
			{
				Asset item = this.GetAsset(id, false);
				return Repository.UoW.Delete<Asset>(item);

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
