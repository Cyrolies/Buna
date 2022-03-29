using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ComponentModel;
using System.Web.Routing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using DALEFModel;
using Common;
using Models;
using BunaPortal.HTMLHelpers;
using BunaPortal.Repository;

namespace BunaPortal
{

	public class PersonController : BaseController
	{


		#region Person Methods

		/// <summary>
		/// Person this instance.
		/// <summary>
		/// <returns></returns>
		[HttpGet]
		public ViewResult Person()
		{
			return View();
		}

		/// <summary>
		/// Gets the Person list.
		/// <summary>
		/// <param name="jQueryDataTablesModel">The jquery data tables model.</param>
		/// <returns></returns>
		public ActionResult GetPersonList(JQueryDataTablesModel jQueryDataTablesModel)
		{
			User user = null;
			try
			{
				GridParam gridParams = new GridParam();
				gridParams.PageNo = jQueryDataTablesModel.iDisplayStart;
				gridParams.PageSize = jQueryDataTablesModel.iDisplayLength == 0 ? 10 : jQueryDataTablesModel.iDisplayLength;
				gridParams.ListOrderBy = jQueryDataTablesModel.GetSortedColumns().ToList().Count > 0 ? jQueryDataTablesModel.GetSortedColumns().ToList() : null;
				gridParams.ListFilterBy = jQueryDataTablesModel.GetColumnsFilters().ToList().Count > 0 ? jQueryDataTablesModel.GetColumnsFilters().ToList() : null;
				if (Session != null && Session["User"] != null)
				{
					if (gridParams.ListFilterBy == null)
					{
						gridParams.ListFilterBy = new List<FilterField>();
					}
					user = (User)Session["User"];

					if (user.UserRoleID == 5)//Its a Farmer thats logged in then only show his farms
					{
						gridParams.ListFilterBy.Add(new FilterField() { Property = "UserID", Operator = "=", Value = user.UserID.ToString() });
					}
					//Always filter by orgid
					gridParams.ListFilterBy.Add(new FilterField() { Property = "OrgID", Operator = "=", Value = user.OrgID.ToString() });
				}
				else
				{
					RedirectToAction("Login", "Account");
				}

				List<Person> retList = new List<Person>();
				PersonRepository repo = new PersonRepository();
				GridResult<Person> result = repo.GetPersonList(gridParams);
							
				foreach (Person item in result.Items)
				{
											
					item.EditButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnEdit'  data-id='" + item.PersonID + "' ><i class='fa fa-pencil' aria-hidden='true'></i></a></div>";

					if (user.UserRoleID != 5 && user.UserRoleID != 6)
					{
						item.IsActiveCheckBox = item.IsActive == true ? " <span class='label label-success'>" + Localizer.Current.GetString("True") + "</span></td>" : "<span class='label label-danger'>" + Localizer.Current.GetString("False") + "</span></td>";
						item.DeleteButton = "<div class='btn-group btn-group-xs'><a href='#' class='btn btnDelete'  data-id='" + item.PersonID + "' ><i class='fa fa-times - square - o' aria-hidden='true'></i></a></div>";
					}
						
				retList.Add(item);
				}

				return Json(new
				{
					sEcho = jQueryDataTablesModel.sEcho,
					iTotalRecords = result.TotalCount,
					iTotalDisplayRecords = result.TotalFilteredCount,
					aaData = retList
				},
				JsonRequestBehavior.AllowGet);
					

			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// <param name="PersonID">The Person identifier.</param>
		/// <summary>
		/// <returns></returns>
		public ActionResult GetPerson(int? id)
		{

			try
				{
					User user = null;
					string viewToUse = "EditPersonMalawi";
					//int stpPersonType = 122; //Malawi
					if (Session != null && Session["User"] != null)
					{
						user = (User)Session["User"];
						if(user.OrgID == 4)
						{
							viewToUse = "EditPersonZambia";
						}
					}
					else
					{
						return RedirectToAction("Home", "Home");
					}
					
					DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
					TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
					
					
					if (id == null)
					{
					if (user.UserRoleID == 5)//Is a farmer
					{
						return Content(CommonHelper.ShowNotification(false, "You do not have permission to add farmers"));
					}

					return PartialView(viewToUse, new Person()
						{
							CreateDateTime = SATime,
							CreatedByID = ((User)Session["User"]).UserID,
							OrgID = ((User)Session["User"]).OrgID,
							StpPersonTypeID = 122,
							IsActive = false,
						});
					}
				
					PersonRepository repo = new PersonRepository();
					Person item = repo.GetPerson(id.ToString());
					item.ChangeDateTime = SATime;
					item.CreatedByID = ((User)Session["User"]).UserID;
					
							
					return PartialView(viewToUse, item);
					
				}
				catch (System.Exception ex)
				{
					return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
				}

			}

		/// <summary>
		/// <param name="Person">The activity.</param>
		/// <summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult EditPerson(Person newItem)
		{

			try
				{
				//if (Session == null && Session["User"] == null)
				//{
				//	RedirectToAction("Home", "Home");
				//}
				//User user = (User)Session["User"];
				PersonRepository repo = new PersonRepository();
				if(newItem.PersonID == 0)
				{
					repo.Add(newItem);
					return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Added")));
				}
				else

				{
					repo.Update(newItem);
					return Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Updated")));
				}
				
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}

		/// <summary>
		/// Deletes the Person.
		/// <summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public ActionResult DeletePerson(int id)
		{
			try
			{

				PersonRepository repo = new PersonRepository();
				repo.Delete(id);
				return  Content(CommonHelper.ShowNotification(true, Localizer.Current.GetString("Successfully Deleted")));
				
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}

		}

		/// <summary>
		/// Exports the form.
		/// <summary>
		/// <param name="dtParams">The data list.</param>
		/// <returns></returns>
		public ActionResult ExportForm(string dataList)
		{
			try
			{
			Export exportdata = new Export();
			exportdata.Controller ="Person";
			exportdata.Entity ="Person";
			exportdata.DatatableParams = dataList;
			return PartialView("ExportControl", exportdata);
			}
			catch (System.Exception ex)
			{
			return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}
		}
		/// <summary>
		/// Exports the data.
		/// <summary>
		/// <param name="exportData">The export data.</param>
		/// <returns></returns>
		/// <exception cref="System.Exception">
		/// </exception>
		[HttpPost]
		public FileResult ExportData(Export exportDetails)
		{
			try
			{
				FileContentResult file = base.ExportBase<Person>(exportDetails);
				if (file != null)
				{
				return file;
				}
				else
				{
				throw new System.Exception(Localizer.Current.GetString("ExportError"));
				}
		}
		catch (System.Exception ex)
		{
			throw ex;
		}
		}
		#endregion

		#region Asset- Farm
		public ActionResult GetFarmAsset(string Farmname,int farmtype)
		{

			try
			{
				Asset newFarm = new Asset()
				{
					Name = Farmname,
					StpAssetCategoryID = farmtype,
					IsActive = false,

				};
				DateTime SATime = TimeZoneInfo.ConvertTime(DateTime.Now,
				TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
				return PartialView("EditAsset", new Asset()
					{
						CreateDateTime = SATime,
						CreatedByID = ((User)Session["User"]).UserID,
						OrgID = ((User)Session["User"]).OrgID,
						IsActive = true,
					});
				
			}
			catch (System.Exception ex)
			{
				return Content(CommonHelper.ShowNotification(false, ExceptionHandler.Handle(ex).CreateDetailNoHtml()));
			}

		}
		#endregion
	}
}
