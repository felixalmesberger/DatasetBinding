using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms.More.DatasetBinding.Validators;

namespace System.Windows.Forms.More.DatasetBinding
{
  /// <summary>
  /// Manages binding between property of an object 
  /// to property of an control.
  /// implements INotifyPropertyChanged to notify DataSetBinding about
  /// changed value, validity and difference
  /// </summary>
  internal class ControlPropertyBinding : INotifyPropertyChanged
  {

    #region property changed 

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region members

    private PropertyInfo controlProperty;
    private PropertyInfo objectProperty;
    private TypeConverter typeConverter;

    private object objectValue;
    private bool isDifferent;
    private ValidatorResult validationResult;

    private object defaultValue;

    private object dataSource;
    private object controlValue;

    //guards
    private bool updateControlGuard;
    private bool updateObjectGuard;

    #endregion

    #region constructor

    internal ControlPropertyBinding(Control control,
                                  string controlPropertyName,
                                  string objectPropertyName,
                                  UpdateMode updateMode,
                                  IValidator validator)
    {
      Throw.IfNull(control, nameof(control));
      Throw.IfNull(controlPropertyName, nameof(controlPropertyName));
      Throw.IfNull(objectPropertyName, nameof(objectPropertyName));

      this.Validator = validator;
      this.UpdateMode = updateMode;

      this.SynchronizationContext = SynchronizationContext.Current;

      this.Control = control;
      this.ControlPropertyName = controlPropertyName;
      this.ObjectPropertyName = objectPropertyName;

      this.InitializeControlProperty();
      this.AttachToControlChangedEvent();

      this.InitializeDefaultValue();
    }

    #endregion

    #region properties

    /// <summary>
    /// Bound control
    /// </summary>
    public Control Control { get; private set; }

    /// <summary>
    /// Name of property of control
    /// </summary>
    public string ControlPropertyName { get; private set; }

    /// <summary>
    /// Current value of control
    /// </summary>
    public object ControlValue
    {
      get => controlValue;
      set
      {
        this.controlValue = value;
        this.OnPropertyChanged();
      }
    }

    public object DataSource
    {
      get => this.dataSource;
      set
      {
        this.DetachFromObjectChangedEvent();
        this.dataSource = value;
        if (this.dataSource != null)
        {
          try
          {
            this.AttachToDataSourceChangedEvent();
            this.InitializeDataSourceProperty();
            this.InitializeTypeConverter();
          }
          catch (Exception ex)
          {
            Logger.Log("Error while initializing datasource", ex);
          }
        }
      }
    }

    /// <summary>
    /// Name of property of bound datasource
    /// </summary>
    public string ObjectPropertyName { get; private set; }

    /// <summary>
    /// UpdateMode
    ///   - Never: Manualy update by calling read or write method
    ///   - OnValidation: Sync control value to DataSource value on control validation
    ///   - OnPropertyChanged: Sync DataSource value to control value when PropertyChanged event fired
    ///   - OnValidationAndPropertyChanged: Two Way DataBinding combining both
    /// </summary>
    public UpdateMode UpdateMode { get; private set; }

    /// <summary>
    /// Indicates wheather current control value is different from data source value
    /// only makes sense if UpdateMode is Manual or OnPropertyChanged
    /// </summary>
    public bool IsDifferent
    {
      get => this.isDifferent;
      set
      {
        this.isDifferent = value;
        this.OnPropertyChanged();
      }
    }

    /// <summary>
    /// ValidationStatus of current value
    /// </summary>
    public ValidatorResult ValidationResult
    {
      get => validationResult;
      set
      {
        this.validationResult = value;
        this.OnPropertyChanged();
      }
    }

    /// <summary>
    /// Validator implementation used to validate value
    /// </summary>
    public IValidator Validator { get; set; }

    /// <summary>
    /// Used to synchronize calls to control to ui thread
    /// </summary>
    public SynchronizationContext SynchronizationContext { get; set; }

    #endregion

    /// <summary>
    /// Read value from datasource
    /// </summary>
    public void ReadValue()
    {
      // avoid circle updates by setting guards
      if (this.updateObjectGuard)
        return;

      try
      {
        this.updateControlGuard = true;

        if (this.DataSource == null)
        {
          this.ResetDefaults();
          return;
        }

        object convertedValue = null;

        try
        {
          this.objectValue = this.objectProperty.GetValue(this.DataSource);

          //convert value type
          if (typeConverter != null)
            convertedValue = typeConverter.ConvertTo(this.objectValue, this.controlProperty.PropertyType);
          else
            convertedValue = this.objectValue;

          convertedValue = Convert.ChangeType(convertedValue, this.controlProperty.PropertyType);

          this.SetControlValue(convertedValue);
          this.ControlValue = convertedValue;

          //important to keep order of checking
          this.CheckDifference();
          this.Validate();
        }
        catch (Exception ex)
        {
          this.SetConversionError(convertedValue, this.controlProperty.PropertyType, this.ObjectPropertyName);
          Logger.Log($"Could not read value from property {this.ObjectPropertyName}.", ex);
        }

      }
      finally
      {
        this.updateControlGuard = false;
      }
    }

    /// <summary>
    /// Save current control value to datasource
    /// </summary>
    public void WriteValue()
    {
      // avoid circle updates by setting guards

      if (this.updateControlGuard)
        return;

      try
      {
        this.updateObjectGuard = true;

        try
        {
          var convertedValue = Convert.ChangeType(this.ControlValue, this.objectProperty.PropertyType);
          this.objectProperty.SetValue(this.DataSource, convertedValue);
          this.objectValue = this.ControlValue;

          //important to keep order of checking
          this.CheckDifference();
          this.Validate();
        }
        catch (Exception ex)
        {
          this.SetConversionError(this.ControlValue, this.objectProperty?.PropertyType, this.ObjectPropertyName);
          Logger.Log($"Could not read value from control {this.Control?.Name ?? "null"}", ex);
        }

      }
      finally
      {
        this.updateControlGuard = false;
      }
    }

    /// <summary>
    /// Reads value from control to memory
    /// </summary>
    private void ReadControlValue()
    {
      var value = this.controlProperty.GetValue(this.Control);

      if (typeConverter != null)
        this.ControlValue = typeConverter.ConvertFrom(value);
      else
        this.ControlValue = value;
    }

    /// <summary>
    /// Reset value to default value -> value when binding was initialized
    /// </summary>
    private void ResetDefaults()
    {
      this.SetControlValue(this.defaultValue);
    }

    /// <summary>
    /// Sets value to control
    /// </summary>
    private void SetControlValue(object value)
    {
      if (value == null)
        this.controlProperty.SetValue(this.Control, null);
      else
      {
        var convertedType = Convert.ChangeType(value, this.controlProperty.PropertyType);
        this.controlProperty.SetValue(this.Control, convertedType);
      }
    }

    /// <summary>
    /// Initialize PropertyInfo for control
    /// </summary>
    private void InitializeControlProperty()
    {
      this.controlProperty = this.Control.GetType().GetProperty(this.ControlPropertyName);
      Throw.IfNull(this.controlProperty,
        $"Could not find property {this.ControlPropertyName} in object of type {this.Control.GetType().Name}");
    }

    /// <summary>
    /// Initialize PropertyInfo for DataSource
    /// </summary>
    private void InitializeDataSourceProperty()
    {
      //if in designmode datasource is a type
      Type objType;
      if (this.DataSource is Type type)
        objType = type;
      else
        objType = this.DataSource.GetType();

      this.objectProperty = objType.GetProperty(this.ObjectPropertyName);
      Throw.IfNull(this.objectProperty,
        $"Could not find property {this.ObjectPropertyName} in object of type {this.DataSource.GetType().Name}");
    }

    /// <summary>
    /// Cache current value (when initialized) as default value
    /// </summary>
    private void InitializeDefaultValue()
    {
      this.ReadControlValue();
      this.defaultValue = this.ControlValue;
    }

    /// <summary>
    /// Attaches to validate event of control
    /// </summary>
    private void AttachToControlChangedEvent()
    {
      this.Control.Validated -= this.ControlOnValidated;
      this.Control.Validated += this.ControlOnValidated;
    }

    /// <summary>
    /// Handles value of control, when validated event of control fired
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="eventArgs"></param>
    private void ControlOnValidated(object sender, EventArgs eventArgs)
    {
      this.ReadControlValue();

      if (this.UpdateMode.HasFlag(UpdateMode.OnValidation))
      {
        this.ReadValue();
      }
      else
      {
        //Important to check difference first
        this.CheckDifference();
        this.Validate();
      }
    }

    /// <summary>
    /// Attach to property changed event of datasource
    /// </summary>
    private void AttachToDataSourceChangedEvent()
    {
      if (!this.UpdateMode.HasFlag(UpdateMode.OnPropertyChanged))
        return;

      if (!(this.DataSource is INotifyPropertyChanged changingObject))
        throw new Exception($"{this.DataSource.GetType().Name} does not implement INotifyPropertyChanged");

      changingObject.PropertyChanged += this.DataSourcePropertyChanged;
    }

    /// <summary>
    /// Detach from property changed event of datasource
    /// </summary>
    private void DetachFromObjectChangedEvent()
    {
      if (this.DataSource is INotifyPropertyChanged changingObject)
        changingObject.PropertyChanged -= this.DataSourcePropertyChanged;
    }

    /// <summary>
    /// Handle changed value from datasource property
    /// </summary>
    private void DataSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      // because only attached to event if updatemode contains OnPropertyChanged
      // check is not needed here
      this.InvokeOnUiThread(this.ReadValue);
    }

    /// <summary>
    /// Checks if current control value is different from object property value
    /// </summary>
    private void CheckDifference()
    {
      //if both types are strings... dont make a difference between null and ""
      if (this.controlProperty?.PropertyType == typeof(string)
        && this.objectProperty?.PropertyType == typeof(string)
        && string.IsNullOrWhiteSpace(this.controlValue as string)
        && string.IsNullOrWhiteSpace(this.objectValue as string)
        )
      {
        this.IsDifferent = false;
        return;
      }

      this.IsDifferent = !Equals(this.ControlValue, this.objectValue);
    }

    /// <summary>
    /// Validate current control value
    /// </summary>
    public void Validate()
    {
      try
      {
        this.ValidationResult = this.Validator?.Validate(this.objectProperty, this.DataSource, this.ControlValue);
      }
      catch (Exception ex)
      {
        Logger.Log(
          $"Error while validation value {this.controlValue}, for property {this.objectProperty?.Name ?? "null"}");
      }
    }

    /// <summary>
    /// Gets type converter from bound properties if exists
    /// </summary>
    private void InitializeTypeConverter()
    {
      try
      {
        var attr = this.objectProperty.GetCustomAttribute<TypeConverterAttribute>(true);
        if (attr == null)
          return;

        var name = attr.ConverterTypeName;
        var type = Type.GetType(name);
        var converter = Activator.CreateInstance(type) as TypeConverter;

        //check validity
        if (converter.CanConvertFrom(this.controlProperty.PropertyType)
            && converter.CanConvertTo(this.objectProperty.PropertyType))
        {
          this.typeConverter = converter;
        }
      }
      catch (Exception ex)
      {
        Logger.Log($"Could not InitializeTypeConverter for property {this.objectProperty.Name}.", ex);
      }
    }

    /// <summary>
    /// If conversion failed from source to dest
    /// set validation result to conversion error
    /// </summary>
    private void SetConversionError(object value, Type type, string propertyName)
    {
      this.ValidationResult = new ConversionFailure(value?.ToString(), type, propertyName);
    }

    /// <summary>
    /// If synchronization context is set, invoke on sync context
    /// </summary>
    /// <param name="action"></param>
    private void InvokeOnUiThread(Action action)
    {
      if (this.SynchronizationContext != null)
        this.SynchronizationContext.Post(o => action(), null);
      else
        action();
    }

  }

  public class ConversionFailure : ValidatorResult
  {
    public ConversionFailure(string value, Type dest, string propertyName)
    {
      this.ErrorMessage = $"Could not convert {value} to type {dest?.Name} in property {propertyName}";
      this.ValidationStatus = ValidationStatus.Error;
    }
  }
}
