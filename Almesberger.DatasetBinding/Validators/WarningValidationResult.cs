using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace System.Windows.Forms.More.DatasetBinding.Validators
{
  public class WarningValidationResult : ValidationResult
  {
    public WarningValidationResult(string errorMessage)
      : base(errorMessage)
    {
    }

    public WarningValidationResult(string errorMessage, IEnumerable<string> memberNames)
      : base(errorMessage, memberNames)
    {
    }
  }
}