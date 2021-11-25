using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DALEFModel;
using Common;

namespace DSDBLL
{

	public sealed class StockReportManager : BaseManager,IDisposable
	{

		public StockReportManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}


		#region vwStockReport Methods

				
		public GridResult<vwStockReport> GetStockReport(GridParam filters)
		{
			try
			{
				GridResult<vwStockReport> result = new GridResult<vwStockReport>();
				//Get total rows before filtering is applied
				result.TotalCount = this.GetList<vwStockReport>().Count();

				Expression <Func<vwStockReport, bool>> where = null;
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
								where = Common.QueryHelpers.BuildFilter.BuildWhereClause<vwStockReport>(field.Property, field.Operator, field.Value);
							}
							else
							{
							 where = Common.QueryHelpers.BuildFilter.BuildWhereClause<vwStockReport>(field.Property, field.Operator, field.Value, where);
							}
						}
					}

				IQueryable<vwStockReport> list = null;
				
				if (where != null)
				{
				list = Repository.GetList<vwStockReport>(where);
				}
				else
				{
				list = Repository.GetList<vwStockReport>();
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
					list = list.OrderByDescending(o => o.StockTakeDate);
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
		
		#endregion

		#region Dispose
		public void Dispose()
		{
		}
		#endregion
	}
}
