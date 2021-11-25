using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace DSDBLL
{

	public sealed class StudentManager : BaseManager,IDisposable
	{

		public StudentManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Student Methods

		public IQueryable<Student> GetAllStudent()
		{
			 try
			 {
				   return Repository.GetList<Student>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public bool UpdateStudentsFromEdAdmin(string school)
		{
			Root edAdminStudents = new Root();
			string schoolCampusName = "";
			int orgID = 5;//DSG
			switch (school.ToUpper())
			{
				case "DSG":
					schoolCampusName = "DSG Senior";
					orgID = 5;
					break;
				case "DSGJ":
					schoolCampusName = "DSG Junior";
					break;
				case "SAP":
					schoolCampusName = "SA Prop";
					break;
				case "STA":
					schoolCampusName = "St Andrews";
					break;
				default:
					schoolCampusName = "";
					break;
			}

			using (HttpClient httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				string EdAdminAPI = ConfigurationManager.AppSettings["EdAdminAPI"];
				string apiFilter = "?query=students&campus=" + schoolCampusName + "&statusid=1";
				string EdAdminKey = "&key=" + ConfigurationManager.AppSettings["EdAdminAPIKey"];

				HttpResponseMessage response = httpClient.GetAsync(EdAdminAPI + apiFilter + EdAdminKey).Result;

				if (response.IsSuccessStatusCode)
				{
					var jsonString = response.Content.ReadAsStringAsync();
					jsonString.Wait();
					var serializer = new XmlSerializer(typeof(Root));

					using (TextReader reader = new StringReader(jsonString.Result))
					{
						edAdminStudents = (Root)serializer.Deserialize(reader);
					}

					//Get student list from Database
					List<Student> dbStudents = new List<Student>();
					dbStudents = this.GetAllStudent().Where(o =>o.OrgID == orgID).ToList();
					foreach (Students student in edAdminStudents.Students)
					{
						int studentID = Convert.ToInt32(student.ID);
						Student dbstudent = dbStudents.Where(o => o.StudentNo == studentID).FirstOrDefault();
						if(dbstudent == null)//Add student to DB
						{
							Student newStudent = new Student()
							{
								StudentNo = studentID,
								AdmissionNo = student.AdmNo,
								Firstname = student.FirstName + " " + student.MiddleName,
								Surname = student.LastName,
								Gender = student.Gender,
								Grade = student.InGrade,
								OrgID = orgID,
								IsActive = true,
							};
							this.AddStudent(newStudent);
						}
					}
					return true;
				}
				else
				{
					var readAsStringAsync = response.Content.ReadAsStringAsync();
					throw new Exception(readAsStringAsync.Result);
				}
			}
		}


		public Student GetStudent(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Student, object>>> includepaths = new List<Expression<Func<Student, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<Student>(includepaths, p => p.StudentID == id);

				}
				else
				{

					return Repository.Get<Student>(null,p => p.StudentID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Student> GetStudent(GridParam filters)
		{
			try
			{
				GridResult<Student> result = new GridResult<Student>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Student>().Count();

				Expression <Func<Student, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Student>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Student>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Student> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Student, object>>> includepaths = new List<Expression<Func<Student, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Student, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Student, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Student>(where).OrderBy(m => m.StudentID);
				}
				else
				{
				list = Repository.GetList<Student>().OrderBy(m => m.StudentID);
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
					list = list.OrderBy(o => o.StudentID);
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
		public int AddStudent(Student newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Student>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Student AddReturnStudent(Student newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Student>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateStudent(Student editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Student>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteStudent(int id)
		{
			try
			{
				Student item = this.GetStudent(id, false);
				return Repository.UoW.Delete<Student>(item);

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
