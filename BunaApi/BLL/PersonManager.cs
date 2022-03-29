using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class PersonManager : BaseManager,IDisposable
	{

		public PersonManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Person Methods

		public IQueryable<Person> GetAllPerson()
		{
			 try
			 {
				   return Repository.GetList<Person>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Person GetPerson(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Person, object>>> includepaths = new List<Expression<Func<Person, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.User2);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.StpData3);
					includepaths.Add(p => p.StpData4);
					includepaths.Add(p => p.StpData5);


					// Add includepaths into method here if used and not null
					return Repository.Get<Person>(includepaths, p => p.PersonID == id);

				}
				else
				{

					return Repository.Get<Person>(null,p => p.PersonID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Person> GetPerson(GridParam filters)
		{
			try
			{
				GridResult<Person> result = new GridResult<Person>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Person>().Count();

				Expression <Func<Person, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Person>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Person>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Person> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Person, object>>> includepaths = new List<Expression<Func<Person, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.StpData2);
					includepaths.Add(p => p.StpData3);
					includepaths.Add(p => p.StpData4);
					includepaths.Add(p => p.StpData5);
					//includepaths.Add(p => p.User2);


					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Person, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Person, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Person>(where).OrderBy(m => m.PersonID);
				}
				else
				{
				list = Repository.GetList<Person>().OrderBy(m => m.PersonID);
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
					list = list.OrderBy(o => o.PersonID);
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
		public int AddPerson(Person newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Person>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Person AddReturnPerson(Person newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Person>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdatePerson(Person editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Person>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeletePerson(int id)
		{
			try
			{
				Person item = this.GetPerson(id, false);
				return Repository.UoW.Delete<Person>(item);

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
