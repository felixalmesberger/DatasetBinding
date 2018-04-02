using System.ComponentModel.DataAnnotations;

namespace System.Windows.Forms.More.DatasetBinding.Validators
{
  public static class ValidationContextExtensions
  {
    public static T GetValue<T>(this ValidationContext ctx, string name)
    {
      if (ctx.Items != null && ctx.Items.ContainsKey(name))
        return (T) ctx.Items[name];
      else
      {
        var prop = ctx.ObjectInstance.GetType().GetProperty(name);
        if (prop == null)
          return default(T);
        else
          return (T) prop.GetValue(ctx.ObjectInstance);
      }
    }
  }
}