using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEFModel.GraphModel
{
	public class GanttItem
	{
		public string id { get; set; }
		public string title { get; set; }
		public string status { get; set; }
		public string startdate { get; set; }
		public string enddate { get; set; }
		public string url { get; set; }
		public string desc { get; set; }
		public string dateorder { get; set; }

	}
}
