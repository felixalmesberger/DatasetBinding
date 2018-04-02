using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms.More.DatasetBinding
{
  [ToolboxItem(true)]
  public class PropertyProvider<TExtends> : Component, IExtenderProvider
  {
    private readonly Dictionary<string, Dictionary<object, object>> properties 
      = new Dictionary<string, Dictionary<object, object>>();

    public PropertyProvider()
    {
    }

    public PropertyProvider(IContainer container)
    {
      container?.Add(this);
    }

    public T GetProvidedProperty<T>(object key, [CallerMemberName] string callerMethodName = "")
    {
      var propertyDictionary = this.GetPropertyDictionary(callerMethodName, false);
      if (propertyDictionary.ContainsKey(key))
        return (T)propertyDictionary[key];
      return default(T);
    }

    public void SetProvidedProperty(object key, object value, [CallerMemberName] string callerMethodName = "")
    {
      var propertyDictionary = this.GetPropertyDictionary(callerMethodName, false);
      if (propertyDictionary.ContainsKey(key))
      {
        if (value != null && !(value is string) || !string.IsNullOrEmpty((string)value))
          propertyDictionary[key] = value;
        else
          propertyDictionary.Remove(key);
      }
      else if (value != null && !(value is string) || !string.IsNullOrEmpty((string)value))
        propertyDictionary.Add(key, value);
    }

    public Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(string propertyName)
    {
      var propertyDictionary = this.GetPropertyDictionary(propertyName, true);
      if (propertyDictionary == null)
        return null;

      var dictionary = new Dictionary<TKey, TValue>();
      foreach (var keyValuePair in propertyDictionary)
        dictionary.Add((TKey)keyValuePair.Key, (TValue)keyValuePair.Value);

      return dictionary;
    }

    private Dictionary<object, object> GetPropertyDictionary(string callerMethodName, bool straightName = false)
    {
      if (string.IsNullOrWhiteSpace(callerMethodName) || callerMethodName.Length < 4 && straightName)
        throw new ArgumentException("callerMethodName");
      if (!callerMethodName.StartsWith("Get") && !callerMethodName.StartsWith("Set") && !straightName)
        straightName = true;
      var key = callerMethodName;

      if (!straightName)
        key = callerMethodName.Substring(3);

      if (this.properties.ContainsKey(key))
        return this.properties[key];

      var dictionary = new Dictionary<object, object>();
      this.properties.Add(key, dictionary);
      return dictionary;
    }

    public virtual bool CanExtend(object extendee)
    {
      return extendee is TExtends;
    }
  }
}
