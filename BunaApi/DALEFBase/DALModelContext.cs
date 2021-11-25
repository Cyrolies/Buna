using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
//using System.Data.Objects;
using System.Data;
using System.Data.Entity.ModelConfiguration;
using System.Configuration;
//using System.Data.EntityClient;
using Common;
using DALEFBase;

//Robin 2011-08-17 not implementing this class at present as its only needed if we use proper POCO objects 
//this would implement the DBContext class which is part of the Microsoft.Data.Entity.CTP
namespace DALEFBase
{
    public partial class DALModelContext : DbContext
    {
        public DALModelContext(string connString)
            : base(connString)
        {
        }
       
        
    }
}