using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class StudentBiometricManager : BaseManager,IDisposable
	{

		public StudentBiometricManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region StudentBiometric Methods

		public IQueryable<StudentBiometric> GetAllStudentBiometric()
		{
			 try
			 {
				   return Repository.GetList<StudentBiometric>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public StudentBiometric GetStudentBiometric(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<StudentBiometric, object>>> includepaths = new List<Expression<Func<StudentBiometric, object>>>();
					includepaths.Add(p => p.Student);
					includepaths.Add(p => p.Biometric);
					// Add includepaths into method here if used and not null
					return Repository.Get<StudentBiometric>(includepaths, p => p.StudentBiometricID == id);

				}
				else
				{

					return Repository.Get<StudentBiometric>(null,p => p.StudentBiometricID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public List<StudentBiometric> GetStudentBiometricByStudentID(int id, bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<StudentBiometric, object>>> includepaths = new List<Expression<Func<StudentBiometric, object>>>();
					includepaths.Add(p => p.Biometric);
					includepaths.Add(p => p.Student);
					//includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null

					return Repository.GetList<StudentBiometric, string>(includepaths, p => p.StudentID == id).ToList();

				}
				else
				{

					return Repository.GetList<StudentBiometric, string>(null, p => p.StudentID == id).ToList();

				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<StudentBiometric> GetStudentBiometricList(int orgID, int biometricType, bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<StudentBiometric, object>>> includepaths = new List<Expression<Func<StudentBiometric, object>>>();
					includepaths.Add(p => p.Biometric);
					includepaths.Add(p => p.Student);
					//includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null

					return Repository.GetList<StudentBiometric, string>(includepaths, p => p.Student.OrgID == orgID && p.Biometric.StpBiometricTypeID == biometricType).ToList();

				}
				else
				{

					return Repository.GetList<StudentBiometric, string>(null, p => p.Student.OrgID == orgID && p.Biometric.StpBiometricTypeID == biometricType).ToList();

				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public GridResult<StudentBiometric> GetStudentBiometric(GridParam filters)
		{
			try
			{
				GridResult<StudentBiometric> result = new GridResult<StudentBiometric>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<StudentBiometric>().Count();

				Expression <Func<StudentBiometric, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StudentBiometric>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<StudentBiometric>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<StudentBiometric> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<StudentBiometric, object>>> includepaths = new List<Expression<Func<StudentBiometric, object>>>();
					includepaths.Add(p => p.Student);
					includepaths.Add(p => p.Biometric);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<StudentBiometric, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<StudentBiometric, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<StudentBiometric>(where).OrderBy(m => m.StudentBiometricID);
				}
				else
				{
				list = Repository.GetList<StudentBiometric>().OrderBy(m => m.StudentBiometricID);
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
					list = list.OrderBy(o => o.StudentBiometricID);
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
		public int AddStudentBiometric(StudentBiometric newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<StudentBiometric>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public StudentBiometric AddReturnStudentBiometric(StudentBiometric newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<StudentBiometric>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateStudentBiometric(StudentBiometric editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<StudentBiometric>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteStudentBiometric(int id)
		{
			try
			{
				StudentBiometric item = this.GetStudentBiometric(id, false);
				return Repository.UoW.Delete<StudentBiometric>(item);

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
