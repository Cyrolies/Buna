using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Collections;
using System.Globalization;
using System.Text;
using DALEFModel;
using CyroTechPortal;
using System.Web.Mvc.Html;

namespace CyroTechPortal
{
    public static class MvcHtmlExtensions 
    {
        public static MvcHtmlString CustomLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression) where TModel : new()
        {
            try
            {
                AdminController manager = new AdminController();
                TModel myObj = new TModel();
                string entityname = myObj.GetType().Name;
                string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
                //   List<EntityField> entityfields = manager.GetEntityFieldList().Where(p => p.EntityFieldName == fieldname).ToList<EntityField>();
                List<EntityField> entityfields = manager.GetEntityFieldListItems();
                EntityField field = entityfields.Where(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname).FirstOrDefault();
                int permission = getFieldPermission(field);
                if (permission == (Int32)Common.Enumerations.Permissions.None)//Permission is set to None
                {
                    return new MvcHtmlString("");
                }
                //if (!field.IsActive || field.IsDisabled)//check if field is disabled this overrides any permissions
                //{
                //    permission = 11;
                //}

                //if (permission < (Int32)Common.Enumerations.Permissions.View)//Permission is set to View
                //{
                    return System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression, Localizer.Current.GetString(field.EntityFieldName), new { @class = "control-label" });
                //}
                //return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MvcHtmlString CustomTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) where TModel : new()
        {
            try
            { 
            AdminController manager = new AdminController();
            TModel myObj = new TModel();
            string entityname = myObj.GetType().Name;
            string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
            List<EntityField> entityfields = manager.GetEntityFieldListItems();
            entityfields = entityfields.Where(p => p.EntityFieldName == fieldname).ToList();
            if(entityfields.Count() == 0)
			{
                throw new Exception("This EntityField is not specified :" + fieldname +" on Entity : " + entityname);
            }
            foreach (EntityField fd  in entityfields)
            {
                if (fd.Entity == null)
                {
                    throw new Exception("Entity child not specified on EntityField add relation in db :" + fieldname + " on Entity : " + entityname);
                }
            }
            EntityField field = entityfields.Where(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname).FirstOrDefault();

            if (field == null)
            {
               throw new Exception("This EntityField is not specified : " + fieldname + " on Entity : " + entityname );
            }
                int permission = getFieldPermission(field);
                if (permission == (Int32)Common.Enumerations.Permissions.None)//Permission is set to None
                {
                    return new MvcHtmlString(System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString());
                }
                if (!field.IsActive || field.IsDisabled)//check if field is disabled this overrides any permissions
                {
                    permission = 11;
                }

                if (permission < (Int32)Common.Enumerations.Permissions.View)//Permission is set to View
                {
                    if (field.IsToolTip) 
                    {
                        if (field.StcControlTypeID == 18)//date
                        {
                            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, "{0:yyyy/MM/dd}", new { @class = "form-control", @type = "datetime", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()) });
                        }
                        else if (field.StcControlTypeID == 25)//Time picker
                        {
                            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", @type = "time", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()) });
                        }
                        else if (field.StcControlTypeID == 29)//Decimal inputplaceholder="0.00" required name="price" min="0" value="0" step="0.01"
                        {
                            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", @type = "number", @step = "0.01", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()) });
                        }
                        else
                        {
                            if (field.StcControlTypeID == 27) //autocompletetextbox
                            {
                                return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", @autocomplete = "on", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()) });
                            }
							else
							{
                                return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()) });                        
                            }
                        }

                    }
                    else
                    {
                        if (field.StcControlTypeID == 18)//date
                        {
                            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, "{0:yyyy/MM/dd}", new { @class = "form-control" , @type = "datetime", style = "height:" + field.ControlHeight + ";\"" });
                        }
                        else if (field.StcControlTypeID == 25)//Time picker
                        {
                            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", @type = "time", style = "height:" + field.ControlHeight + ";\"" });
                        }
                        else if (field.StcControlTypeID == 29)//Decimal inputplaceholder="0.00" required name="price" min="0" value="0" step="0.01"
                        {
                            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", @type = "number", @step = "0.01", style = "height:" + field.ControlHeight + ";\"" });
                        }
                        else
                        {
                            if (field.StcControlTypeID == 27) //autocompletetextbox
                            {
                                return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", @autocomplete = "on", style = "height:" + field.ControlHeight + ";\""});
                            }
                            else
                            {
                                return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "form-control", style = "height:" + field.ControlHeight + ";\"" });
                            }
                        }
                    }
                }//read only viewable
                else
                {
                   // return System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "readOnly", @readonly = "read-only", @disabled = "disabled", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("Disabled") });
                    return MvcHtmlString.Create(System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, new { @class = "readOnly", @readonly = "read-only", @disabled = "disabled", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("Disabled") }).ToString() +
                      System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString()); // add hidden field if its disabled
                }
                
                
            
           
        }
            catch (Exception ex)
            {
                throw ex;
            }
}

        public static MvcHtmlString CustomTextBoxAreaFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) where TModel : new()
        {
            try
            { 
            AdminController manager = new AdminController();
            TModel myObj = new TModel();
            string entityname = myObj.GetType().Name;
            string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
            List<EntityField> entityfields = manager.GetEntityFieldListItems();
            EntityField field = entityfields.Where(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname).FirstOrDefault();

            if (field != null)
            {
                //Checks users permissions based on there userroles for this field 
                int permission = getFieldPermission(field);
                if (permission == (Int32)Common.Enumerations.Permissions.None)//Permission is set to None
                {
                    return new MvcHtmlString("");
                }
                if (!field.IsActive || field.IsDisabled)//check if field is disabled this overrides any permissions
                {
                    permission = 11;
                }

                if (permission < (Int32)Common.Enumerations.Permissions.View)//Permission is set to View
                {
                    if (field.IsToolTip)
                    {
                        return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, new { @class = "form-control", style ="height:"+field.ControlHeight+";\"" , title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()) });

                    }
                    else
                    {
                        return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, new { @class = "form-control", style = "height:" + field.ControlHeight + ";\"" });
                        
                    }
                }
                else
                {
                  //  return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, new { @class = "readOnly", @readonly = "read-only", @disabled = "disabled", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("Disabled") });
                       return MvcHtmlString.Create(System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, new { @class = "readOnly", @readonly = "read-only", @disabled = "disabled", style = "height:" + field.ControlHeight + ";\"", title = Localizer.Current.GetString("Disabled") }).ToString() +
                       System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString()); // add hidden field if its disabled
                    }
           
            }
            else
            {
                return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, new { @class = "form-control", style = "height:" + field.ControlHeight + ";\"" });
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MvcHtmlString CustomCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression) where TModel : new()
        {
            try
            { 
            //Get field's details 
            AdminController manager = new AdminController();
            TModel myObj = new TModel();
            string entityname = myObj.GetType().Name;
            string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
            List<EntityField> entityfields = manager.GetEntityFieldListItems();
            EntityField field = entityfields.Where(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname).FirstOrDefault();

            if (field != null)
            {
                //Checks users permissions based on there userroles for this field 
                int permission = getFieldPermission(field);
                if (permission == (Int32)Common.Enumerations.Permissions.None)//Permission is set to None
                {
                    return new MvcHtmlString("");
                }
                if (!field.IsActive || field.IsDisabled)//check if field is disabled this overrides any permissions
                {
                    permission = 11;
                }

                    if (permission < (Int32)Common.Enumerations.Permissions.View)//Permission is set to View
                    {
                        if (!field.IsToolTip)
                        {
                            return System.Web.Mvc.Html.InputExtensions.CheckBoxFor(htmlHelper, expression, new { @class = "myCheckbox" });
                        }
                        else
                        {
                            return System.Web.Mvc.Html.InputExtensions.CheckBoxFor(htmlHelper, expression, new { @title = Localizer.Current.GetString("ToolTip" + field.EntityFieldName.Trim()), @class = "form-control" });

                        }
                    }
                    else
                    {
                        return MvcHtmlString.Create(System.Web.Mvc.Html.InputExtensions.CheckBoxFor(htmlHelper, expression, new { @class = "readOnly", @readonly = "read-only", @disabled = "disabled", @title = Localizer.Current.GetString("Disabled") }).ToString() +
                        System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString()); // add hidden field if its disabled
                        
                    }
            }
            else
            {
                return System.Web.Mvc.Html.InputExtensions.CheckBoxFor(htmlHelper, expression);
            }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MvcHtmlString CustomDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) where TModel : new()
        {
            try
            { 
                AdminController manager = new AdminController();
                TModel myObj = new TModel();
                string entityname = myObj.GetType().Name;
                string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
                List<EntityField> entityfields = manager.GetEntityFieldListItems();
                EntityField field = entityfields.Where(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname).FirstOrDefault();
                if(field == null)
				{
                    throw new Exception("No entityfield found for : " + fieldname + " for Entity : " + entityname);
				}
                //Checks users permissions based on there userroles for this field 
                int permission = getFieldPermission(field);
                if (permission == (Int32)Common.Enumerations.Permissions.None)//Permission is set to None
                {
                    return MvcHtmlString.Create(System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString()); // add hidden field if none
                }
                
                IEnumerable<SelectListItem> selectList = manager.GetDataSelectList<TModel>("", field.EntityFieldName, entityname);

                if (!field.IsActive || field.IsDisabled)//check if field is disabled this overrides any permissions
                {
                    permission = 11;
                }
                if (permission < (Int32)Common.Enumerations.Permissions.View)//Permission is set to View
                {
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    //  routeValues.Add("class", "t-widget t-input");
                  //  routeValues.Add("style", "width: 65px;height:45px");
                    routeValues.Add("style", "width:" + field.ControlLength + ";height:" + field.ControlHeight + "");
                    routeValues.Add("title", Localizer.Current.GetString("ToolTipStatus"));
                    routeValues.Add("class", "form-control");


                    return System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, Localizer.Current.GetString("Select"), routeValues);

                }
                else
                {
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                //    routeValues.Add("class", "readOnly");
                    routeValues.Add("readonly", "read-only");
                    routeValues.Add("disabled", "disabled");
                    routeValues.Add("class", "form-control");
                    routeValues.Add("style", "width:" + field.ControlLength + ";height:" + field.ControlHeight + "");
                    routeValues.Add("title", Localizer.Current.GetString("Disabled"));



                    //return System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, Localizer.Current.GetString("Select"), routeValues);
                    MvcHtmlString htm = MvcHtmlString.Create(System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, Localizer.Current.GetString("Select"), routeValues).ToString() +
                        System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString()); // add hidden field if its disabled

                    return MvcHtmlString.Create(System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, Localizer.Current.GetString("Select"), routeValues).ToString() +
						System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, expression).ToString()); // add hidden field if its disabled
				}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MvcHtmlString CustomValidationMessageFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) where TModel : new()
        {
            try
            {
                AdminController manager = new AdminController();
                TModel myObj = new TModel();
                string entityname = myObj.GetType().Name;
                string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
                List<EntityField> entityfields = manager.GetEntityFieldListItems();
                EntityField field = entityfields.Where(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname).FirstOrDefault();
                int permission = getFieldPermission(field);
                if (permission == (Int32)Common.Enumerations.Permissions.None)//Permission is set to None
                {
                    return new MvcHtmlString("");
                }

                if (permission < (Int32)Common.Enumerations.Permissions.View)//Permission is set to View
                {
                    var propertyName = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).PropertyName;
                    var modelState = htmlHelper.ViewData.ModelState;
                    RouteValueDictionary routeValues = new RouteValueDictionary();
                    routeValues.Add("class", "readOnly");

                    // If we have multiple (server-side) validation errors, collect and present them.
                    if (modelState.ContainsKey(propertyName))
                    {
                        var msgs = new StringBuilder();
                        foreach (ModelError error in modelState[propertyName].Errors)
                        {
                            msgs.AppendLine(Localizer.Current.GetString(error.ErrorMessage));
                        }

                        // Return standard ValidationMessageFor, overriding the message with our concatenated list of messages.
                        return htmlHelper.ValidationMessageFor(expression, msgs.ToString(), routeValues as IDictionary<string, object> ?? routeValues);
                    }

                    // Revert to default behaviour.
                    return htmlHelper.ValidationMessageFor(expression, null, routeValues as IDictionary<string, object> ?? routeValues);
                }
                return new MvcHtmlString("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public static int getFieldPermission(EntityField field)
		{
			int permission = 8;
			
			HttpSessionStateBase session = new HttpSessionStateWrapper(HttpContext.Current.Session);
			if (session["User"] != null)
			{
				User user = (User)session["User"];
				if (user.UserRoleActivity.Count > 0)
				{
					foreach(UserRoleActivity ura in user.UserRoleActivity)
					{
                        if(ura.ActivityID == field.ActivityID)
						{
                            permission = ura.StcPermissionID ?? 24;
						}
					}
				}
				
			}

			return permission;
		}

		//public static bool enableControl<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression) where TModel : new()
		//{
		//    BaseManager manager = new BaseManager();
		//    TModel myObj = new TModel();
		//    string entityname = myObj.GetType().Name;
		//    string fieldname = expression.Body.ToString().Substring(expression.Body.ToString().IndexOf(".") + 1);
		//    EntityField field = manager.Get<EntityField>(p => p.EntityFieldName == fieldname && p.Entity.Name == entityname);
		//    if (!field.IsActive)//check if field is disabled this overrides any permissions
		//    {
		//        return false;
		//    }
		//    return getFieldPermission(manager, field.ActivityID, entityname);

		//}



		/// <summary>
		/// Returns a checkbox for each of the provided <paramref name="items"/>.
		/// </summary>
		public static MvcHtmlString CheckBoxListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> items, object htmlAttributes = null)
        {
            try
            { 
                var listName = ExpressionHelper.GetExpressionText(expression);
                var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

                items = GetCheckboxListWithDefaultValues(metaData.Model, items);
                return htmlHelper.CheckBoxList(listName, items, htmlAttributes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns a checkbox for each of the provided <paramref name="items"/>.
        /// </summary>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string listName, IEnumerable<SelectListItem> items, object htmlAttributes = null)
        {
            try
            { 
                var container = new TagBuilder("div");
                foreach (var item in items)
                {
                    var label = new TagBuilder("label");
                    label.MergeAttribute("class", "checkbox"); // default class
                    label.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

                    var cb = new TagBuilder("input");
                    cb.MergeAttribute("type", "checkbox");
                    cb.MergeAttribute("name", listName);
                    cb.MergeAttribute("value", item.Value ?? item.Text);
                    if (item.Selected)
                        cb.MergeAttribute("checked", "checked");

                    label.InnerHtml = cb.ToString(TagRenderMode.SelfClosing) + item.Text;

                    container.InnerHtml += label.ToString();
                }

                return new MvcHtmlString(container.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static IEnumerable<SelectListItem> GetCheckboxListWithDefaultValues(object defaultValues, IEnumerable<SelectListItem> selectList)
        {
            try
            { 
                var defaultValuesList = defaultValues as IEnumerable;

                if (defaultValuesList == null)
                    return selectList;

                IEnumerable<string> values = from object value in defaultValuesList
                                             select Convert.ToString(value, CultureInfo.CurrentCulture);

                var selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                var newSelectList = new List<SelectListItem>();

                foreach (var item in selectList)
                {
                    item.Selected = (item.Value != null) ? selectedValues.Contains(item.Value) : selectedValues.Contains(item.Text);
                    newSelectList.Add(item);   
                }

                return newSelectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}