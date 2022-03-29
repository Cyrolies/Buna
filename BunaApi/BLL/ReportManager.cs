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

		

	}
}
