using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using DSDWebApi;
using Common;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DSDBLL;
using DALEFModel;

namespace DSDWebApi.Tests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void GetSurveyQuestions()
        {
            SurveyQuestionManager cont = new SurveyQuestionManager();
            GridParam filter = new GridParam();
            filter.ListFilterBy.Add(new FilterField() { Property = "SurveyID", Operator = "=", Value = "1" });
            //   filter.ListFilterBy.Add(new FilterField() { Property = "ActivityName", Operator = "EndsWith", Value = "User" });
            filter.ListFilterBy.Add(new FilterField() { Property = "IsActive", Operator = "=", Value = "True" });
            filter.ListFilterBy.Add(new FilterField() { Property = "QuestionItem", Operator = "like", Value = "the" });
            filter.ListFilterBy.Add(new FilterField() { Property = "CreateDateTime", Operator = ">=", Value = "2016-10-07" });

            // filter.ListOrderBy.Add("QuestionItem", "Descending");

            filter.PageSize = 3;
            filter.PageNo = 1;
            var result = cont.GetSurveyQuestion(filter);
            Assert.IsTrue(result.TotalFilteredCount > 0, "RESULT COUNT = " + result.TotalFilteredCount);
        }
        //[TestMethod]
        //public void GetStpData()
        //{
        //    Controllers.AdminController adminController = new Controllers.AdminController();
        //    GridParam filter = new GridParam();
        //    //filter.ListFilterBy.Add(new FilterField() { Property = "StpActivityGroupID", Operator = "=", Value = "23" });
        //    //   filter.ListFilterBy.Add(new FilterField() { Property = "ActivityName", Operator = "EndsWith", Value = "User" });
        //    //filter.ListFilterBy.Add(new FilterField() { Property = "IsActive", Operator = "=", Value = "True" });

        //    //filter.ListOrderBy.Add(new FilterField() { Property = "ActivityName", Operator = "=", Value = "Ascending" });

        //    filter.PageSize = 300000;
        //    filter.PageNo = 1;
        //    filter.Includerelations = false;
        //    AdminManager adminmng = new AdminManager();
        //    List<StpData> list = adminmng.GetSTPDataList(filter).ToList();
        //    Assert.IsNotNull(list);
        //    Assert.AreNotEqual(list.Count(),0);

        //}
        //[TestMethod]
        //public void GetActivities()
        //{
        //    Controllers.AdminController adminController = new Controllers.AdminController();
        //    GridParam filter = new GridParam();
        //    filter.ListFilterBy.Add(new FilterField() { Property = "StpActivityGroupID", Operator = "=", Value = "23" });
        //    //   filter.ListFilterBy.Add(new FilterField() { Property = "ActivityName", Operator = "EndsWith", Value = "User" });
        //    filter.ListFilterBy.Add(new FilterField() { Property = "IsActive", Operator = "=", Value = "True" });

        //    filter.ListOrderBy.Add(new FilterField() { Property = "ActivityName", Operator = "=", Value = "Ascending" });

        //    filter.PageSize = 3;
        //    filter.PageNo = 1;
        //    filter.Includerelations = false;
        //    HttpResponseMessage contentResult = adminController.GetActivityList(filter);
        //    Assert.IsNotNull(contentResult);
        //    Assert.IsNotNull(contentResult.Content);

        //}

        //[TestMethod]
        //public void GetEntityMenus()
        //{
        //    Controllers.AdminController adminController = new Controllers.AdminController();
        //    GridParam filter = new GridParam();
        //    filter.ListFilterBy.Add(new FilterField() { Property = "MenuDisplayName", Operator = "like", Value = "User" });
        //    //   filter.ListFilterBy.Add(new FilterField() { Property = "ActivityName", Operator = "EndsWith", Value = "User" });
        //    filter.ListFilterBy.Add(new FilterField() { Property = "IsActive", Operator = "=", Value = "True" });

        //    filter.ListOrderBy.Add("MenuDisplayName", "Descending");

        //    filter.PageSize = 3;
        //    filter.PageNo = 1;
        //    var result = adminController.GetAllEntityMenu(filter);
        //    Assert.IsTrue(result != null, "RESULT COUNT = " + result.RequestMessage);
        //}
    }
}
