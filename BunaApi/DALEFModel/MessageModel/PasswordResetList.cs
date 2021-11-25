using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    public class PasswordResetList
    {
        public string defaultPwd { get; set; }
        public List<User> usersReset { get; set; }
      
    }
}
