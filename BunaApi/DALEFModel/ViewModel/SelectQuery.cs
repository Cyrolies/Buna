using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEFModel
{
	public class SelectQuery
	{
		public string fields { get; set; }
		public string table { get; set; }
		public string where { get; set; }
		public string Operator { get; set; }
		public string filter { get; set; }
		public string orderby { get; set; }
		public string direction { get; set; } = "ASC";

		public string GetQuery()
		{
			StringBuilder strQuery = new StringBuilder();
			strQuery.Append("Select distinct " + fields + " from [" + table + "]");
			if (where.Length > 0)
			{
				strQuery.Append(" Where " + where);
			}
			if (orderby.Length > 0)
			{
				strQuery.Append(" order by " + orderby + " " + direction);
			}
			//else
			//{
			//	strQuery.Append(" order by " + fields + " " + direction);
			//}
			return strQuery.ToString();
		}
	}
}
