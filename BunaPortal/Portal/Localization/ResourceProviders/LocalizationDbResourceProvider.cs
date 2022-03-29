using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALEFModel;
using Common;

namespace BunaPortal
{
    public class LocalizationDbResourceProvider : LocalizationResourceProviderBase
    {
        private string _connectionString;

        public LocalizationDbResourceProvider()
            : this(Constants.CONNSTRING_DEFAULT_NAME)
        {
            
        }

        public LocalizationDbResourceProvider(string connectionStringName)
            : base()
        {
            //  _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            

        }

        protected override string OnGetString(string cultureName, string key)
        {
			//return key;
			AdminController manager = new AdminController();
			EntityResource resource = manager.GetEntityResourceByKey(key, cultureName);
			if (resource != null)
			{
				return resource.ResourceValue;
			}
			else
			{
				return key;// + "(NoR)";//Use this to show all display names that need language keys added
						   // return key;
			}

		}
    }
}
