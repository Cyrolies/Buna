using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Common;
using DALEFModel;


namespace CyroTechPortal
{
    public static class PopulateDropDown
    {
       
        //TODO add abiliy to pass a list of fields to filter by e.g. like where clause on repo Expression<Func<T, bool>> whereclause
        public static IEnumerable<SelectListItem> GetDropDownBindingFields<T>(EntityField field,List<FilterField> filters = null)
        {
            try
            {
                AdminController manager = new AdminController();
                //var statuslist = new[] { (int)Enumerations.SupervisionStatus.Approved, (int)Enumerations.SupervisionStatus.PendingUsable };

                //All setup data drop downs
                if (field.EntityFieldName.Substring(0, 3) == "Stp" || field.EntityFieldName.Substring(0, 3) == "STP")
                {
                    int selectedItem = 0;
                    if (field.EntityFieldName == "StpDataTypeID")
                    {
                        var list = manager.GetDataList<T>("StpDataType", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StpDataTypeID", "AppDataType", selectedItem);
                        }
                        
                    }
                    else
                    {
                        //List<StpData> list = new List<StpData>();
                        //string fieldname = Regex.Replace(field.EntityFieldName, "ID", "", RegexOptions.IgnoreCase);
                        //int stptypeid = Enumerations.GetEnumerationValue(typeof(Enumerations.SetupDataType), fieldname.Substring(3));
                        //AdminController controller = new AdminController();
                        //list = controller.GetStpDataPopulatedList().Where(o => o.StpDataTypeID == stptypeid).ToList();
                        var list = manager.GetDataList<T>("StpData", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StpDataID", "DataDescription", selectedItem);
                        }
                    }
                }
                //All static data drop downs
                else if (field.EntityFieldName.Substring(0, 3) == "Stc" || field.EntityFieldName.Substring(0, 3) == "STC")
                {
                    int selectedItem = 0;
                    if (field.EntityFieldName == "StcDataTypeID")
                    {
                        var list = manager.GetDataList<T>("StcDataType", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StcDataTypeID", "StaticDataType", selectedItem);
                        }

                    }
                    else
                    {
                        var list = manager.GetDataList<T>("StcData", filters);
                        if (list != null)
                        {
                            return new SelectList(list, "StcDataID", "Description", selectedItem);
                        }
                    }
                }
                else
                {
                    //string name = field.EntityFieldName.Replace("OrgID", "Organization").Replace("OrgId", "Organization").Replace("BugCreatedByContactID", "Contact").Replace("AssignedContactID", "Contact").Replace("CreatedByID", "User").Replace("SupervisorID", "User").Replace("ParentID", "").Replace("ID", "");
                    string name = Common.Common.ClearEntityFieldName(field.EntityFieldName);
                    //HELL knoes why this method above doesnt return organization back : Robin
                    name = name.Replace("Org", "Organization");

                    Entity entity = manager.GetEntityName(name);
                    if (entity == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "Entity not found for Name = " + name, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    if (field.ComboBoxDisplayFieldID == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "Entityfield.ComboBoxDisplayFieldID not specified on field name : " + field.EntityFieldName, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    List<EntityField> allentityfields = manager.GetEntityFieldListItems();
                    List<EntityField> entityfields = allentityfields.Where(p => p.Entity.Name == entity.Name).ToList();
                    EntityField primaryField = entityfields.Find(o => o.IsPrimaryKey == true);
                    if(primaryField == null)
					{
                        SelectListItem itemo = new SelectListItem() { Text = "No IsPrimaryKey specified in EntityFields for : " + entity.Name, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    EntityField displayField = entityfields.Find(o => o.EntityFieldID == field.ComboBoxDisplayFieldID);
                    if (displayField == null)
                    {
                        SelectListItem itemo = new SelectListItem() { Text = "ComboBoxDisplayFieldID not found = : " + field.ComboBoxDisplayFieldID + " on entity :"+ entity.Name, Value = "0" };
                        List<SelectListItem> selectListo = new List<SelectListItem>() { itemo };
                        return new SelectList(selectListo, "Value", "Text");

                    }
                    int selected = 0;
                    //get data for drop down list
                    //Build where clause
                    StringBuilder whereCause = new StringBuilder();
                    int count = 0;
                    foreach(FilterField fd in filters)
					{
                        if (count == 0)
                        {
                            whereCause.Append(fd.Property + " " + fd.Operator + " " + fd.Value);
                        }
						else // add and
						{
                            whereCause.Append(" and " + fd.Property + " " + fd.Operator + " " + fd.Value);
                        }
                        count++;
                    }
                   
                    var list = manager.PrePopulateInputList(primaryField.EntityFieldName + "," + displayField.EntityFieldName, entity.TableName, whereCause.ToString(), primaryField.EntityFieldName);
                    //var list = manager.GetDataList<T>(entity.Name,filters);

                    //if (name == "Organization")
                    //{
                    //    HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                    //    if (session["User"] != null)
                    //    {
                    //        User user = (User)session["User"];
                    //        selectedItem = (int)user.OrgID;
                    //    }
                    //}

                    //if (name == "tblCodeDesc")
                    //{
                    //    tblCodeDesc codeAll = new tblCodeDesc() { CodeCategory = "TPP", CodeDesc = Localizer.Current.GetString("All"), CodeValue = "0" };
                    //    list.Add(codeAll);
                    //    HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                    //    if (session["User"] != null)
                    //    {
                    //        User user = (User)session["User"];
                    //        selectedItem = (int)user.ShippingPoint;
                    //    }
                    //}

                    if (list != null)
                    {
                        return new SelectList(list, primaryField.EntityFieldName, displayField.EntityFieldName, selected);
                    }


                }

                //return empty selectlist no records found
                SelectListItem item = new SelectListItem() { Text = Localizer.Current.GetString("No Record"), Value = "0" };
                List<SelectListItem> selectList = new List<SelectListItem>() { item };
                return new SelectList(selectList, "Value", "Text");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
    }
        
    }