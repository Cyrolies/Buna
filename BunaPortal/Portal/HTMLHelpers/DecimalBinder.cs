using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace CyroTechPortal.HTMLHelpers
{

    public class DecimalBinder : System.Web.Mvc.DefaultModelBinder
	{
        protected override void BindProperty(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var request = controllerContext.HttpContext.Request;

            decimal i;
            var value = request.Form[propertyDescriptor.Name];
            if (propertyDescriptor.PropertyType == typeof(decimal) && decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out i))
            {
                propertyDescriptor.SetValue(bindingContext.Model, i);
                return;
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}