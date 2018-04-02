using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.More.DatasetBinding;

namespace TestApp
{
  public partial class Form1 : Form
  {

    private User User
    {
      get => this.datasetBinding.DataSource as User;
      set => this.datasetBinding.DataSource = value;
    }

    public Form1()
    {
      this.InitializeComponent();
      this.datasetBinding.SetVerbose();
    }

    protected override void OnLoad(EventArgs e)
    {
      this.User = new User();
      base.OnLoad(e);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      if (!this.datasetBinding.HasChanges)
        return;

      //Unsaved changes, ask if really close
      var reallyClose = MessageBox.Show(owner: this, 
                                        text: "Unsaved changes, really close?", 
                                        caption: Application.ProductName,
                                        buttons: MessageBoxButtons.YesNo,
                                        icon: MessageBoxIcon.Asterisk);
      if (reallyClose == DialogResult.No)
        e.Cancel = true;

      base.OnClosing(e);
    }

    private void datasetBinding_ValidityChanged(object sender, EventArgs e)
    {
      this.btnOk.Enabled = ((DatasetBinding)sender).IsValid;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.datasetBinding.WriteBindings();
      MessageBox.Show(owner: this,
                      text: "Created user.",
                      caption: Application.ProductName,
                      buttons: MessageBoxButtons.OK,
                      icon: MessageBoxIcon.Information);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }

  public class User
  {
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [Mail]
    public string Mail { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [WarnForWeakPassword]
    public string Password { get; set; }

  }

  public class MailAttribute : ValidationAttribute
  {
    private static readonly Regex MailRegex = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

    protected override ValidationResult IsValid(object value, ValidationContext ctx)
    {
      var mail = value as string;

      //only validate when content is there
      if (string.IsNullOrWhiteSpace(mail))
        return ValidationResult.Success;

      return MailRegex.IsMatch(mail)
        ? ValidationResult.Success
        : new ValidationResult("Not a valid mail", new[] { ctx.MemberName });

    }
  }

  public class WarnForWeakPasswordAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var password = value as string;

      //only validate when content is there
      if (string.IsNullOrWhiteSpace(password))
        return ValidationResult.Success;

      if (password.Length < 8)
        return new ValidationResult("Password is weak.");
      else
        return ValidationResult.Success;
    }
  }
}
