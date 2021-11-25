using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CyroTechPortal
{
    public class LocalizableDataAnnotationsModelValidatorProvider : DataAnnotationsModelValidatorProvider
    {
        public LocalizableDataAnnotationsModelValidatorProvider()
            : base()
        {
        }

        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            var validators = base.GetValidators(metadata, context, attributes);
            //return validators;
            //var result = new List<LocalizableDataAnnotationsModelValidator>();
            var result = new List<ModelValidator>(); 
            foreach (var validator in validators)
            {
               // LocalizableDataAnnotationsModelValidator newmodelvalidator = new LocalizableDataAnnotationsModelValidator(validator, metadata, context);
               // newmodelvalidator = (LocalizableDataAnnotationsModelValidator)validator;

                foreach (ModelValidationResult res in validator.Validate(this))
                {
                    string mess = Localizer.Current.GetString(res.Message);
                   // ModelValidationResult validater = res;
                    res.Message = string.Format(mess, metadata.DisplayName);
                    //newLocalizedresults.Add(validater);
                   
                }
                //result.Add(validator);

            }
            return validators;
        }
    }
}
