using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms.More.DatasetBinding;

namespace System.Windows.Forms.More.DatasetBinding.Validators
{
  public interface IValidator
  {

    ValidatorResult Validate(PropertyInfo property, object instance, object value);
  }
}