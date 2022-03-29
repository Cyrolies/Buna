using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace BunaPortal.HTMLHelpers
{

    public class ModelBinder
    {
        public class DecimalModelBinder : System.Web.Mvc.DefaultModelBinder
		{
            public override object BindModel(ControllerContext controllerContext,
											 System.Web.Mvc.ModelBindingContext bindingContext)
            {
                object result = null;

                // Don't do this here!
                // It might do bindingContext.ModelState.AddModelError
                // and there is no RemoveModelError!
                // 
                // result = base.BindModel(controllerContext, bindingContext);

                string modelName = bindingContext.ModelName;
                string attemptedValue =
                    bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

                // Depending on CultureInfo, the NumberDecimalSeparator can be "," or "."
                // Both "." and "," should be accepted, but aren't.
                string wantedSeperator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                string alternateSeperator = (wantedSeperator == "," ? "." : ",");

                if (attemptedValue.IndexOf(wantedSeperator) == -1
                    && attemptedValue.IndexOf(alternateSeperator) != -1)
                {
                    attemptedValue =
                        attemptedValue.Replace(alternateSeperator, wantedSeperator);
                }

                try
                {
                    if (bindingContext.ModelMetadata.IsNullableValueType
                        && string.IsNullOrWhiteSpace(attemptedValue))
                    {
                        return null;
                    }

                    result = decimal.Parse(attemptedValue, NumberStyles.Any);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(modelName, e);
                }

                return result;
            }
        }
    }
}