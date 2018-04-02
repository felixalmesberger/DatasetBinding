# DatasetBinding
Bind a set of controls to object, including validation and change tracking. For Windows Forms

<img src="https://github.com/felixalmesberger/felixalmesberger.github.io/blob/master/datasetbinding.gif?raw=true" alt="Demo" width="500px" />

## How it works
DatasetBinding is a property provider. So you can create your data bindings in design mode.
This is how it looks like:

<img src="https://github.com/felixalmesberger/felixalmesberger.github.io/blob/master/chooseprops.gif?raw=true" alt="Demo" width="300px" />

## Show me code
This is the whole code of the demo application...

  ```csharp

  public partial class Form1 : Form
  {
    public Form1()
    {
      this.InitializeComponent();
      this.datasetBinding.SetVerbose();
    }

    // wrap datasource of datasetbinding in property with cast
    private User User
    {
      get => this.datasetBinding.DataSource as User;
      set => this.datasetBinding.DataSource = value;
    }

    protected override void OnLoad(EventArgs e)
    {
      //set dummy user
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

    //only allow ok to be clicked if valid
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
  ```
  
  ### Create a data structure
  Notice the validation attributes
  ```csharp
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
```

  ### Create custom validation attributes

  ```csharp

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
    ```


