using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    protected override void OnLoad(EventArgs e)
    {
      this.User = new User();
      base.OnLoad(e);
    }

    private void datasetBinding_ValidityChanged(object sender, EventArgs e)
    {
      this.btnOk.Enabled = (sender as DatasetBinding).IsValid;
    }
  }
}
