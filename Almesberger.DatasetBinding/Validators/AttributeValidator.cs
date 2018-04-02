using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms.More.DatasetBinding.Validators
{
  public class AttributeValidator : IValidator
  {

    public static IServiceProvider ServiceProvider { get; set; }

    private readonly Func<Dictionary<string, object>> getControlValues;

    public AttributeValidator(Func<Dictionary<string, object>> ValueFactory)
    {
      this.getControlValues = ValueFactory;
      this.PropertyValues = new Dictionary<PropertyInfo, object>();
    }

    public Dictionary<PropertyInfo, object> PropertyValues { get; set; }

    public void SetAdditionalPropertyValue(PropertyInfo prop, object obj)
    {
      if (this.PropertyValues.ContainsKey(prop))
        this.PropertyValues[prop] = obj;
      else
        this.PropertyValues.Add(prop, obj);
    }

    public ValidatorResult Validate(PropertyInfo property, object instance, object value)
    {

      Throw.IfNull(property, nameof(property));
      Throw.IfNull(instance, nameof(instance));
      
      var validationAttributes = property.GetCustomAttributes<ValidationAttribute>(true);

      var status = ValidationStatus.Valid;

      var msgBuilder = new StringBuilder();

      var allControlValues = this.getControlValues();
      var ctx = this.CreateValidationContext(instance, allControlValues);
      foreach (var attribute in validationAttributes)
      {
        var result = attribute.GetValidationResult(value, ctx);

        if (result == ValidationResult.Success)
          continue;

        if (status == ValidationStatus.Valid)
        {
          if (attribute is RequiredAttribute)
            status = ValidationStatus.Required;
          else if (this.IsWarning(attribute, result))
            status = ValidationStatus.Warn;
          else 
            status = ValidationStatus.Error;
        }
        else
        {
          status = ValidationStatus.Error;
        }


        msgBuilder.AppendLine(result?.ErrorMessage);

      }

      return new ValidatorResult()
      {
        ValidationStatus = status,
        ErrorMessage = msgBuilder.ToString()
      };

    }

    private bool IsWarning(ValidationAttribute attribute, ValidationResult result)
    {
      return result is WarningValidationResult
             || attribute.GetType().Name.ToUpper().Contains("WARN");
    }

    private ValidationContext CreateValidationContext(object instance, Dictionary<string, object> allControlValues)
    {

      foreach (var additionalPropVal in this.PropertyValues)
        allControlValues.Add(additionalPropVal.Key.Name, additionalPropVal.Key.GetValue(additionalPropVal.Value));
      
      return new ValidationContext(instance, ServiceProvider, allControlValues.ToDictionary(x => (object)x.Key, x => x.Value));
    }
  }
}