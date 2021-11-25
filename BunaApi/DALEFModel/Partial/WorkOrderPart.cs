using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    /// <summary>
    /// Partial class for EntityField model to add extra field to model
    /// Author :Robin Cyrolies
    /// </summary>
    public partial class WorkOrderPart
    {
        public string PartDesc
        { get; set; }
        public string WorkOrderDesc
        { get; set; }
        public string CreatedByDesc
        { get; set; }
        public string OrganizationName
        { get; set; }

    }
}
