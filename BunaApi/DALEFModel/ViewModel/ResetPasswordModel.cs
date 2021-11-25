using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DALEFModel
{
    public class ResetPasswordModel
    {
        //public ResetPasswordModel(IEnumerable<DALEFModel.MobileUser> Model)
        //{
        //    UserList = new List<UserResetModel>();

        //    if (Model == null)
        //        return;

        //    foreach (var item in Model)
        //    {
        //        UserList.Add(new UserResetModel { IsChecked = false, Name = item.Name, UserID = item.Id });
        //    }
        //}

        public ResetPasswordModel(IEnumerable<DALEFModel.User> Model)
        {
            UserList = new List<UserResetModel>();

            if (Model == null)
                return;

            foreach (var item in Model)
            {
                UserList.Add(new UserResetModel { IsChecked = false, Name = item.UserName, UserID = item.UserID });
            }
        }
        public List<UserResetModel> UserList { get; set; }
    }

    public class UserResetModel
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public long UserID { get; set; }
    }
}