using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class DocumentManager : BaseManager,IDisposable
	{

		public DocumentManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Document Methods

		public IQueryable<Document> GetAllDocument()
		{
			 try
			 {
				   return Repository.GetList<Document>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Document GetDocument(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Include child objects
					List<Expression<Func<Document, object>>> includepaths = new List<Expression<Func<Document, object>>>();
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					// Add includepaths into method here if used and not null
					return Repository.Get<Document>(includepaths, p => p.DocumentID == id);

				}
				else
				{

					return Repository.Get<Document>(null,p => p.DocumentID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Document> GetDocument(GridParam filters)
		{
			try
			{
				GridResult<Document> result = new GridResult<Document>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Document>().Count();

				Expression <Func<Document, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Document>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Document>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Document> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//Include child objects
					List<Expression<Func<Document, object>>> includepaths = new List<Expression<Func<Document, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.Organization);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.User1);

					//APPLY FILTERS
					if (where != null)
					{
						list = Repository.GetList<Document, string>(includepaths, where);
					}
					else
					{
						list = Repository.GetList<Document, string>(includepaths);
					}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Document>(where).OrderBy(m => m.DocumentID);
				}
				else
				{
				list = Repository.GetList<Document>().OrderBy(m => m.DocumentID);
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
					list = list.OrderBy(o => o.DocumentID);
				}

				//Get total filtered rows before paging is applied 
				result.TotalFilteredCount = list.Count();

			//APPLY PAGE SIZE
			list = list.Skip(filters.PageNo).Take(filters.PageSize);
				IEnumerable<Document> resultData = null;
				List<Document> docs = new List<Document>();
				resultData = list;
				foreach (Document item in resultData)
				{
					item.FileData = null;
					docs.Add(item);
				}
				result.Items = docs.AsEnumerable();
				return result;
			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AddDocument(Document newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Document>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Document AddReturnDocument(Document newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Document>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdateDocument(Document editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Document>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeleteDocument(int id)
		{
			try
			{
				Document item = this.GetDocument(id, false);
				return Repository.UoW.Delete<Document>(item);

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
