using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class PartManager : BaseManager,IDisposable
	{

		public PartManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region Part Methods

		public IQueryable<Part> GetPart()
		{
			 try
			 {
				   return Repository.GetList<Part>();
			 }catch(Exception ex)
			 {
				   throw ex;
			 }
		}

		public Part GetPart(int id,bool includeRelations)
		{
			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					List<Expression<Func<Part, object>>> includepaths = new List<Expression<Func<Part, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.Organization);


					// Add includepaths into method here if used and not null
					return Repository.Get<Part>(null, p => p.PartID == id);

				}
				else
				{

					return Repository.Get<Part>(null,p => p.PartID == id);

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public GridResult<Part> GetPart(GridParam filters)
		{
			try
			{
				GridResult<Part> result = new GridResult<Part>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<Part>().Count();

				Expression <Func<Part, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Part>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<Part>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<Part> list = null;
				if (filters.Includerelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					List<Expression<Func<Part, object>>> includepaths = new List<Expression<Func<Part, object>>>();
					includepaths.Add(p => p.StpData);
					includepaths.Add(p => p.StpData1);
					includepaths.Add(p => p.User);
					includepaths.Add(p => p.Organization);
				
					//APPLY FILTERS
					if (where != null)
					{
					list = Repository.GetList<Part, string>(includepaths, where);
					}
					else
					{
					list = Repository.GetList<Part, string>(includepaths);
				}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Part>(where).OrderBy(m => m.PartID);
				}
				else
				{
				list = Repository.GetList<Part>().OrderBy(m => m.PartID);
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
					list = list.OrderBy(o => o.PartID);
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
		public IQueryable<Part> GetPart(bool includeRelations, Expression<Func<Part, bool>> where = null, Dictionary<string, string> listOrderBy = null, int page = 0, int size = 0)
		{
			try
			{

				IQueryable<Part> list = null;
				if (includeRelations)// Add includepaths into method 
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					//APPLY FILTERS
					//if (where != null)
					//{
					//list = Repository.GetList<Part, string>(includepaths, where);
					//}
					//else
					//{
					//list = Repository.GetList<Part, string>(includepaths);
				//}
			}
			else
			{
				if (where != null)
				{
				list = Repository.GetList<Part>(where).OrderBy(m => m.PartID);
				}
				else
				{
				list = Repository.GetList<Part>().OrderBy(m => m.PartID);
				}
			}

			//APPLY ALL SORTING
			if (listOrderBy != null && listOrderBy.Count() > 0)
			{
			foreach (var sort in listOrderBy)
			{
			list = QueryFilters.Sort<Part>(list.ToList(), sort.Key, sort.Value).AsQueryable();
			}
			}

			//APPLY PAGE SIZE
			if (page > 0 && size > 0)
			{
				list = list.Skip((page - 1) * size).Take(size);
			}
			return list;

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public IQueryable<Part> GetPart(bool includeRelations)
		{

			try
			{
				if (includeRelations)
				{

					//Add Includes here example below
					//STARTCUSTOMCODE
					//List<Expression<Func<StpData, object>>> includepaths = new List<Expression<Func<StpData, object>>>();
					//includepaths.Add(p => p.StpDataType);

					// Add includepaths into method here if used and not null
					return Repository.GetList<Part>();
					//ENDCUSTOMCODE

				}
				else
				{

					return Repository.GetList<Part>();

				}

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int AddPart(Part newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Add<Part>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}
		public Part AddReturnPart(Part newEntity)
		{
			try
			{
				newEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Add, newEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.AddEntityReturnEntity<Part>(newEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int UpdatePart(Part editEntity)
		{
			try
			{
				editEntity.StcStatusID = base.SetSupervisionStatus(Enumerations.RepositoryAction.Update, editEntity.setToPendingUsable);//Sets to Pending if supervision On else Approved if Off
				return Repository.UoW.Update<Part>(editEntity);

			}catch(Exception ex)
			{
				throw ex;
			}
		}

		public int DeletePart(int id)
		{
			try
			{
				Part item = this.GetPart(id, false);
				return Repository.UoW.Delete<Part>(item);

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
