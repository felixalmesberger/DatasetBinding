using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

using System.Windows.Forms.More.DatasetBinding.Designer;
using System.Windows.Forms.More.DatasetBinding.Validators;

namespace System.Windows.Forms.More.DatasetBinding
{
  [ProvideProperty("DataSourceProperty", typeof(Control))]
  [ProvideProperty("ControlProperty", typeof(Control))]
  [ToolboxBitmap(typeof(BindingSource))]
  [ToolboxItem(true)]
  public class DatasetBinding : PropertyProvider<Control>, ISupportInitialize
  {

    #region events

    /// <summary>
    /// Fires when overall validty of dataset changed
    /// </summary>
    public event EventHandler ValidityChanged;

    private void OnValidityChanged()
    {
      this.ValidityChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///  Fires when dataset has unsaved changes
    /// </summary>
    public event EventHandler HasChangesChanged;

    private void OnHasChangesChanged()
    {
      this.HasChangesChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region members

    // list of all bindings
    private readonly List<ControlPropertyBinding> bindings = new List<ControlPropertyBinding>();

    // default back colors of all properties
    private readonly Dictionary<Control, Color> defaultBackColor = new Dictionary<Control, Color>();

    private object dataSource;
    private IValidator validator;
    private bool hasChanges;
    private bool isValid;

    #endregion

    #region constructor

    public DatasetBinding(IContainer container)
      : this()
    {
      container?.Add(this);
    }

    public DatasetBinding()
    {
      //set defaults
      this.StatusProvider = new StatusProvider();
      this.DifferentValueColor = Color.Khaki;
      this.ErrorColor = Color.Tomato;
      this.UpdateMode = UpdateMode.Never;
      this.ValidationMode = ValidationMode.Attributes;
      this.ShowDifferences = true;
    }

    #endregion

    #region properties

    [RefreshProperties(RefreshProperties.Repaint)]
    [AttributeProvider(typeof(IListSource))]
    [Description("Object to bind controls to")]
    public object DataSource
    {
      get => this.dataSource;
      set
      {
        //check for multiple possibilities

        //if datasource is binding source then set datasource to datasource of binding source
        if (value is BindingSource bindingSource)
          this.dataSource = bindingSource.DataSource;
        else
          this.dataSource = value;

        if (this.IsDesignerHosted())
          this.InitializeAvailableDesignTimeProperties();

        if (this.dataSource is Type)
          return;

        this.RefreshDataSource();
        this.ReadBindings();
      }
    }

    [Browsable(true)]
    [Editable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Description("Provides icons to show status")]
    public StatusProvider StatusProvider { get; set; }

    [Description("Make differences between control value and value of property visible")]
    public bool ShowDifferences { get; set; }

    [Description("Background color of control value, when value is different.")]
    public Color DifferentValueColor { get; set; }

    [Description("Background color of control value, when value is not valid.")]
    public Color ErrorColor { get; set; }

    [Description("- Never: Manualy update by calling read or write method \n" +
                 "- OnValidation: Sync control value to DataSource value on control validation \n" +
                 "- OnPropertyChanged: Sync DataSource value to control value when PropertyChanged event fired\n" +
                 "- OnValidationAndPropertyChanged: Two Way DataBinding combining both")]
    public UpdateMode UpdateMode { get; set; }

    [Description("- Attributes: Validate using data annotations\n" +
                 "- NotifyErrorData: Object implements INotifyErrorData \n" + 
                 "- Custom: Provide own validator")]
    public ValidationMode ValidationMode { get; set; }

    [Description("Validate all values alway. Needed if validity of prop a dependends on prop b")]
    public bool CrossValidation { get; set; }


    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IValidator Validator
    {
      get => this.validator;
      set
      {
        if (this.ValidationMode != ValidationMode.Custom
          && !(value is AttributeValidator)
          && !(value is NotifyDataErrorValidator))
          throw new Exception("Set ValidationMode to Custom to use own Validator.");

        this.validator = value;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsValid
    {
      get => this.isValid;
      private set
      {
        if (this.isValid != value)
        {
          this.isValid = value;
          this.OnValidityChanged();
        }
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasChanges
    {
      get => this.hasChanges;
      private set
      {
        if (this.hasChanges != value)
        {
          this.hasChanges = value;
          this.OnHasChangesChanged();
        }
      }
    }

    /// <summary>
    /// Needed for designer to show all propertieso
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal List<string> AvailableDataSourceProperties { get; private set; }

    #endregion

    #region provided properties

    [Category("DatasetBinding")]
    [Description("Property of DataSource")]
    [Editor(typeof(DataSourcePropertyChooser), typeof(UITypeEditor))]
    public string GetDataSourceProperty(object control)
    {
      return this.GetProvidedProperty<string>(control);
    }

    public void SetDataSourceProperty(object control, string dataSourceProperty)
    {
      this.SetProvidedProperty(control, dataSourceProperty);
    }

    [Category("DatasetBinding")]
    [Description("Property of Control")]
    public string GetControlProperty(object control)
    {
      var result = this.GetProvidedProperty<string>(control);
      return string.IsNullOrEmpty(result) 
        ? TypeDescriptor.GetDefaultProperty(control).Name
        : result;
    }

    public void SetControlProperty(object control, string controlProperty)
    {this.SetProvidedProperty(control, controlProperty);
    }

    #endregion

    /// <summary>
    /// Read all bindings from datasource
    /// </summary>
    public void ReadBindings()
    {
      foreach (var binding in this.bindings)
        binding.ReadValue();
    }

    /// <summary>
    /// Save all bindings to datasource
    /// </summary>
    public void WriteBindings()
    {
      foreach (var binding in this.bindings)
        binding.WriteValue();
    }

    /// <summary>
    /// Refresh datasource of all bindings
    /// </summary>
    private void RefreshDataSource()
    {
      if (this.DataSource is Type)
        return;

      foreach (var binding in this.bindings)
      {
        binding.DataSource = this.DataSource;
        binding.ReadValue();
      }
    }

    /// <summary>
    /// Not needed
    /// </summary>
    public void BeginInit()
    {
    }

    /// <summary>
    /// Initialize after initialization of winform controls
    /// </summary>
    public void EndInit()
    {
      this.InitializeValidator();
      this.InitializeBindings();
      this.RefreshDataSource();
    }

    private void InitializeBindings()
    {
      var dataSourcePropertyNames = this.GetDictionary<Control, string>("DataSourceProperty");
      var controlPropertyNames = this.GetDictionary<Control, string>("ControlProperty");
      
      // all bound controls are controls that have a datasource property and contrlol property
      var controls = dataSourcePropertyNames.Keys.Where(x => controlPropertyNames.ContainsKey(x));

      foreach (var control in controls)
      {
        var controlPropertyName = controlPropertyNames[control];
        var objectPropertyName = dataSourcePropertyNames[control];

        if (string.IsNullOrWhiteSpace(objectPropertyName))
          continue;

        try
        {
          var controlPropertyBinding
            = new ControlPropertyBinding(control, controlPropertyName, objectPropertyName, this.UpdateMode,
              this.Validator);

          controlPropertyBinding.PropertyChanged += this.BindingStateChanged;
          this.bindings.Add(controlPropertyBinding);
        }
        catch (Exception ex)
        {
          Logger.Log($"Could not bind control {control?.Name ?? "Unknown"} to property {objectPropertyName ?? "null"}", ex);
        }

        if(!this.defaultBackColor.ContainsKey(control))
          this.defaultBackColor.Add(control, control.BackColor);
      }
    }
    
    /// <summary>
    /// Handle changes of binding
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BindingStateChanged(object sender, PropertyChangedEventArgs e)
    {
      var binding = sender as ControlPropertyBinding;
      if (e.PropertyName == nameof(binding.IsDifferent))
        this.HandleChangedDifference(binding);
      else if (e.PropertyName == nameof(binding.ValidationResult))
        this.HandleChangedValidationResult(binding);
      else if(e.PropertyName == nameof(binding.ControlValue))
        this.HandleChangedControlValue(binding);
    }


    private void HandleChangedValidationResult(ControlPropertyBinding binding)
    {
      var vr = binding.ValidationResult;

      if (vr.ValidationStatus == ValidationStatus.Error)
      {
        binding.Control.BackColor = this.ErrorColor;
      }
      else
      {
        binding.Control.BackColor = binding.IsDifferent 
          ? this.DifferentValueColor 
          : defaultBackColor[binding.Control];
      }

      this.StatusProvider.SetStatus(vr.ValidationStatus, binding.Control, vr.ErrorMessage);

      this.IsValid = this.bindings.All(this.IsBindingValid);

    }

    private bool IsBindingValid(ControlPropertyBinding binding)
    {
      if (binding?.ValidationResult == null)
        return true;

      var vs = binding.ValidationResult.ValidationStatus;

      return vs == ValidationStatus.Valid || vs == ValidationStatus.Warn;
    }

    private void HandleChangedControlValue(ControlPropertyBinding senderBinding)
    {
      if (this.DataSource == null)
        return;

      // ReSharper disable once InvertIf
      if (this.CrossValidation)
        foreach(var binding in this.bindings.Except(new [] {senderBinding}))
          binding.Validate();
    }

    private void HandleChangedDifference(ControlPropertyBinding binding)
    {
      if (!this.ShowDifferences)
        return;

      if (binding.ValidationResult == null || binding.ValidationResult.ValidationStatus == ValidationStatus.Valid)
      {
        if (binding.IsDifferent)
          binding.Control.BackColor = this.DifferentValueColor;
        else
          binding.Control.BackColor = this.defaultBackColor[binding.Control];
      }

      this.HasChanges = this.bindings.All(x => !x.IsDifferent);

    }

    private void InitializeValidator()
    {
      try
      {
        switch (this.ValidationMode)
        {
          case ValidationMode.Attributes:
            this.Validator = new AttributeValidator(this.GetCurrentControlValues);
            break;
          case ValidationMode.NotifyErrorData:
            this.Validator = new NotifyDataErrorValidator();
            break;
          case ValidationMode.Custom:
            break;
        }
      }
      catch (Exception ex)
      {
        Logger.Log("Error while creating validator", ex); 
      }
    }

    private Dictionary<string, object> GetCurrentControlValues()
    {
      return this.bindings.ToDictionary(x => x.ObjectPropertyName, x => x.ControlValue);
    }

    private void InitializeAvailableDesignTimeProperties()
    {
      if (!this.IsDesignerHosted())
        return;

      if (this.DataSource == null)
        return;

      Type type;
      if (this.DataSource is BindingSource bindingSource)
      {
        var bindingSourceSource = bindingSource.DataSource;
        if (bindingSourceSource is Type)
          type = bindingSourceSource as Type;
        else
          type = bindingSourceSource.GetType();
      }
      else if (this.DataSource is Type)
        type = this.DataSource as Type;
      else
        type = this.DataSource.GetType();

      this.AvailableDataSourceProperties = type.GetProperties()
        .Select(x => x.Name)
        .ToList();

    }

  }

}