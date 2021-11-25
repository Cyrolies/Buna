using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class PersonBiometricManager : BaseManager,IDisposable
	{

		public PersonBiometricManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region PersonBiometric Methods

		public IQueryable<PersonBiometric> GetAllPersonBiometric()
		{
			 try
			 {
				   return Repository.GetList<PersonBiometric>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public PersonBiometric GetPersonBiometric(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<PersonBiometric, object>>> includepaths = new List<Expression<Func<PersonBiometric, object>>>();
					//includepaths.Add(p => p.Organization);
					//includepaths.Add(p => p.User);
					//includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<PersonBiometric>(includepaths, p => p.PersonBiometricID == id);

				}
				else
				{

					return Repository.Get<PersonBiometric>(null,p => p.PersonBiometricID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public List<PersonBiometric> GetPersonBiometricByPersonID(int id, bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<PersonBiometric, object>>> includepaths = new List<Expression<Func<PersonBiometric, object>>>();
					includepaths.Add(p => p.Biometric);
					includepaths.Add(p => p.Person);
					//includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					
					return Repository.GetList<PersonBiometric,string>(includepaths, p => p.PersonID == id).ToList();

				}
				else
				{

					return Repository.GetList<PersonBiometric,string>(null, p => p.PersonID == id).ToList();

				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<PersonBiometric> GetBiometricList(int orgID,int biometricType, bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<PersonBiometric, object>>> includepaths = new List<Expression<Func<PersonBiometric, object>>>();
					includepaths.Add(p => p.Biometric);
					includepaths.Add(p => p.Person);
					//includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null

					return Repository.GetList<PersonBiometric, string>(includepaths, p => p.Person.OrgID == orgID && p.Biometric.StpBiometricTypeID == biometricType).ToList();

				}
				else
				{

					return Repository.GetList<PersonBiometric, string>(null, p => p.Person.OrgID == orgID && p.Biometric.StpBiometricTypeID == biometricType).ToList();

				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<PersonBiometric> GetPersonBiometric(GridParam filters)
		{
			try
			{
				GridResult<PersonBiometric> result = new GridResult<PersonBiometric>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<PersonBiometric>().Count();

				Expression <Func<PersonBiometric, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<PersonBiometric>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<PersonBiometric>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<PersonBiometric> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<PersonBiometric, object>>> includepaths = new List<Expression<Func<PersonBiometric, object>>>();
					//includepaths.Add(p => p.Organization);
					//includepaths.Add(p => p.User);
					//includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<PersonBiometric, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<PersonBiometric, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<PersonBiometric>(where).OrderBy(m => m.PersonBiometricID);
				}
				else
				{
				list = Repository.GetList<PersonBiometric>().OrderBy(m => m.PersonBiometricID);
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
					list = list.OrderBy(o => o.PersonBiometricID);
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
		public int AddPersonBiometric(PersonBiometric newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<PersonBiometric>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public PersonBiometric AddReturnPersonBiometric(PersonBiometric newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<PersonBiometric>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdatePersonBiometric(PersonBiometric editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<PersonBiometric>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeletePersonBiometric(int id)
		{
			try
			{
				PersonBiometric item = this.GetPersonBiometric(id, false);
				return Repository.UoW.Delete<PersonBiometric>(item);

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
