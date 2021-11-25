using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class BiometricManager : BaseManager,IDisposable
	{

		public BiometricManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Biometric Methods

		public IQueryable<Biometric> GetAllBiometric()
		{
			 try
			 {
				   return Repository.GetList<Biometric>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Biometric GetBiometric(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Biometric, object>>> includepaths = new List<Expression<Func<Biometric, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<Biometric>(includepaths, p => p.BiometricID == id);

				}
				else
				{

					return Repository.Get<Biometric>(null,p => p.BiometricID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Biometric> GetBiometric(GridParam filters)
		{
			try
			{
				GridResult<Biometric> result = new GridResult<Biometric>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Biometric>().Count();

				Expression <Func<Biometric, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Biometric>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Biometric>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Biometric> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Biometric, object>>> includepaths = new List<Expression<Func<Biometric, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Biometric, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Biometric, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Biometric>(where).OrderBy(m => m.BiometricID);
				}
				else
				{
				list = Repository.GetList<Biometric>().OrderBy(m => m.BiometricID);
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
					list = list.OrderBy(o => o.BiometricID);
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
		public int AddBiometric(Biometric newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Biometric>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Biometric AddReturnBiometric(Biometric newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Biometric>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateBiometric(Biometric editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Biometric>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteBiometric(int id)
		{
			try
			{
				Biometric item = this.GetBiometric(id, false);
				return Repository.UoW.Delete<Biometric>(item);

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
