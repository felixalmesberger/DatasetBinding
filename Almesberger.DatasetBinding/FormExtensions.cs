using System.ComponentModel;

namespace System.Windows.Forms.More.DatasetBinding
{
  internal static class FormExtensions
  {
    internal static bool IsDesignerHosted(this IComponent control)
    {
      //when asked in constructor of control license manager works
      if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        return true;
      //else this works
      while (control != null)
      {
        if (control.Site != null && control.Site.DesignMode)
          return true;

        if (control is Control)
          control = ((Control)control).Parent;
        else
          break;
      }
      return false;
    }
  }
}