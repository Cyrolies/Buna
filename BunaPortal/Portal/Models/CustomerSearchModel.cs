using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace BunaPortal
{
    public class CustomerSearchModel
    {
        [DisplayName("Name:")]
        public string Name { get; set; }

        [DisplayName("Birthday Range:")]
        public DateTime? BeginBirthday { get; set; }

        public DateTime? EndBirthday { get; set; }

        [DisplayName("Age Range:")]
        public int? BeginAge { get; set; }

        public int? EndAge { get; set; }

        [DisplayName("Phone Number:")]
        public string PhoneNumber { get; set; }
    }
}