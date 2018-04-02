namespace System.Windows.Forms.More.DatasetBinding
{
  public enum ValidationStatus
  {
    Valid = 0,
    Error = 1,
    Warn = 2,
    Required = 4
  }

  public enum ValidationMode
  {
    Attributes,
    NotifyErrorData,
    Custom
  }

  [Flags]
  public enum UpdateMode
  {
    Never = 0,
    OnValidation = 1,
    OnPropertyChanged = 2,
    OnValidationAndPropertyChanged = 4
  }

}