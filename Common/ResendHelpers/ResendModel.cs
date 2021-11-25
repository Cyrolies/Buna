using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ResendHelpers
{
    public class ResendModel
    {
        public string ModelName { get; set; }
        public string ControllerName { get; set; }
        public string PageTitleName { get; set; }
        public List<string> EventList { get; set; }
    }
}
