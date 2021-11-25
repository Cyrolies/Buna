using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using DALEFModel;

namespace DSDBLL
{
	public class ReportManager : BaseManager, IDisposable
	{
		public ReportManager()
		{
			base.Model = Enumerations.ModalContext.EzFloManagerEntities;
		}

		public List<HitecJob> GetHitecJobGanttData()
		{
			try
			{
				return Repository.GetList<HitecJob>().Where(o => o.IsActive == true).OrderBy(m => m.CreateDateTime).ToList();
				// return Repository.GetList<HitecJob>().Where(o => o.IsInvoiced == false).OrderBy(m => m.CreateDateTime).ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<HitecJob> GetHitecJobMultiBarData(DateTime startdt,DateTime enddt)
		{
			try
			{
				return Repository.GetList<HitecJob>().Where(o => o.CreateDateTime >= startdt && o.CreateDateTime <= enddt && o.IsActive == true).OrderBy(m => m.CreateDateTime).ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


	}
}
