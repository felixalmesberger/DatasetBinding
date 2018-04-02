using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms.More.DatasetBinding.Validators
{
  public class NotifyDataErrorValidator : IValidator
  {
    public ValidatorResult Validate(PropertyInfo property, object instance, object value)
    {
      var notifier = instance as IDataErrorInfo;
      if (notifier == null)
        throw new Exception($"{instance.GetType().Name} does not implement IDataErrorInfo.");
      var error = notifier[property.Name];

      var status = ValidationStatus.Valid;
      if (!string.IsNullOrWhiteSpace(error))
      {
        if(error.ToUpper().StartsWith("WARN"))
          status = ValidationStatus.Warn;
        else if (error.ToUpper().StartsWith("REQUIRED"))
          status = ValidationStatus.Required;
        else
          status = ValidationStatus.Error;
      }

      return new ValidatorResult()
      {
        ErrorMessage = error,
        ValidationStatus = status
      };
      
    }
  }
}