using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms.More.DatasetBinding
{
  [TypeConverter(typeof(ExpandableObjectConverter))]
  public class StatusProvider : ExpandableObjectConverter
  {

    private readonly ErrorProvider errorProvider;
    private readonly ErrorProvider warnProvider;
    private readonly ErrorProvider requiredProvider;
    private readonly ErrorProvider passProvider;

    public StatusProvider()
    {
      //Initialize providers with default values
      this.errorProvider = new ErrorProvider()
      {
        BlinkStyle = ErrorBlinkStyle.NeverBlink,
        Icon = Properties.Resources.DefaultErrorIcon
      };

      this.warnProvider = new ErrorProvider()
      {
        BlinkStyle = ErrorBlinkStyle.NeverBlink,
        Icon = Properties.Resources.DefaultWarnIcon
      };

      this.requiredProvider = new ErrorProvider()
      {
        BlinkStyle = ErrorBlinkStyle.NeverBlink,
        Icon = Properties.Resources.DefaultRequiredIcon
      };

      this.passProvider = new ErrorProvider()
      {
        BlinkStyle = ErrorBlinkStyle.NeverBlink,
        Icon = Properties.Resources.DefaultPassIcon
      };

    }

    public Icon ErrorIcon
    {
      get => this.errorProvider.Icon;
      set => this.errorProvider.Icon = value ?? Properties.Resources.DefaultErrorIcon;
    }

    public Icon WarnIcon
    {
      get => this.warnProvider.Icon;
      set => this.warnProvider.Icon = value ?? Properties.Resources.DefaultWarnIcon;
    }

    public Icon RequiredIcon
    {
      get => this.requiredProvider.Icon;
      set => this.requiredProvider.Icon = value ?? Properties.Resources.DefaultRequiredIcon;
    }

    public Icon PassIcon
    {
      get => this.passProvider.Icon;
      set => this.passProvider.Icon = value ?? Properties.Resources.DefaultPassIcon;
    }

    public bool ShowPassIcon { get; set; }

    public void SetStatus(ValidationStatus status, Control control, string message)
    {

      //empty errors
      this.errorProvider.SetError(control, null);
      this.requiredProvider.SetError(control, null);
      this.passProvider.SetError(control, null);
      this.warnProvider.SetError(control, null);

      if (status == ValidationStatus.Valid)
      {
        if (!this.ShowPassIcon)
          return;
        else
          message = SR.Valid;
      }

      //set error
      ErrorProvider provider = null;
      switch (status)
      {
        case ValidationStatus.Error:
          provider = this.errorProvider;
          break;
        case ValidationStatus.Required:
          provider = this.requiredProvider;
          break;
        case ValidationStatus.Valid:
          provider = this.passProvider;
          break;
        case ValidationStatus.Warn:
          provider = this.warnProvider;
          break;
      }

      provider?.SetIconPadding(control, 4);
      provider?.SetError(control, message);
    }
  }
}