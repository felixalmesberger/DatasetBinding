using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.More.DatasetBinding;

namespace TestApp
{
  partial class Form1
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.datasetBinding = new System.Windows.Forms.More.DatasetBinding.DatasetBinding(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.txtFirstName = new System.Windows.Forms.TextBox();
      this.txtLastName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtUsername = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtMail = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.btnOk = new System.Windows.Forms.Button();
      this.userBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.datasetBinding)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // datasetBinding
      // 
      this.datasetBinding.CrossValidation = false;
      this.datasetBinding.DataSource = typeof(TestApp.User);
      this.datasetBinding.DifferentValueColor = System.Drawing.Color.Khaki;
      this.datasetBinding.ErrorColor = System.Drawing.Color.Tomato;
      this.datasetBinding.ShowDifferences = true;
      this.datasetBinding.StatusProvider.ErrorIcon = ((System.Drawing.Icon)(resources.GetObject("resource.ErrorIcon")));
      this.datasetBinding.StatusProvider.PassIcon = ((System.Drawing.Icon)(resources.GetObject("resource.PassIcon")));
      this.datasetBinding.StatusProvider.RequiredIcon = ((System.Drawing.Icon)(resources.GetObject("resource.RequiredIcon")));
      this.datasetBinding.StatusProvider.ShowPassIcon = true;
      this.datasetBinding.StatusProvider.WarnIcon = ((System.Drawing.Icon)(resources.GetObject("resource.WarnIcon")));
      this.datasetBinding.UpdateMode = System.Windows.Forms.More.DatasetBinding.UpdateMode.Never;
      this.datasetBinding.ValidationMode = System.Windows.Forms.More.DatasetBinding.ValidationMode.Attributes;
      this.datasetBinding.ValidityChanged += new System.EventHandler(this.datasetBinding_ValidityChanged);
      // 
      // label1
      // 
      this.datasetBinding.SetControlProperty(this.label1, "Text");
      this.datasetBinding.SetDataSourceProperty(this.label1, null);
      this.label1.Location = new System.Drawing.Point(12, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(110, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "First Name:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtFirstName
      // 
      this.datasetBinding.SetControlProperty(this.txtFirstName, "Text");
      this.datasetBinding.SetDataSourceProperty(this.txtFirstName, "s");
      this.txtFirstName.Location = new System.Drawing.Point(128, 8);
      this.txtFirstName.Name = "txtFirstName";
      this.txtFirstName.Size = new System.Drawing.Size(211, 20);
      this.txtFirstName.TabIndex = 1;
      // 
      // txtLastName
      // 
      this.datasetBinding.SetControlProperty(this.txtLastName, "Text");
      this.datasetBinding.SetDataSourceProperty(this.txtLastName, "LastName");
      this.txtLastName.Location = new System.Drawing.Point(128, 34);
      this.txtLastName.Name = "txtLastName";
      this.txtLastName.Size = new System.Drawing.Size(211, 20);
      this.txtLastName.TabIndex = 3;
      // 
      // label2
      // 
      this.datasetBinding.SetControlProperty(this.label2, "Text");
      this.datasetBinding.SetDataSourceProperty(this.label2, null);
      this.label2.Location = new System.Drawing.Point(12, 37);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(110, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Last Name:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtUsername
      // 
      this.datasetBinding.SetControlProperty(this.txtUsername, "Text");
      this.datasetBinding.SetDataSourceProperty(this.txtUsername, "Username");
      this.txtUsername.Location = new System.Drawing.Point(128, 60);
      this.txtUsername.Name = "txtUsername";
      this.txtUsername.Size = new System.Drawing.Size(211, 20);
      this.txtUsername.TabIndex = 5;
      // 
      // label3
      // 
      this.datasetBinding.SetControlProperty(this.label3, "Text");
      this.datasetBinding.SetDataSourceProperty(this.label3, null);
      this.label3.Location = new System.Drawing.Point(12, 63);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(110, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Username:";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtMail
      // 
      this.datasetBinding.SetControlProperty(this.txtMail, "Text");
      this.datasetBinding.SetDataSourceProperty(this.txtMail, "Mail");
      this.txtMail.Location = new System.Drawing.Point(128, 86);
      this.txtMail.Name = "txtMail";
      this.txtMail.Size = new System.Drawing.Size(211, 20);
      this.txtMail.TabIndex = 9;
      // 
      // label5
      // 
      this.datasetBinding.SetControlProperty(this.label5, "Text");
      this.datasetBinding.SetDataSourceProperty(this.label5, null);
      this.label5.Location = new System.Drawing.Point(12, 89);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(110, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Mail:";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label4
      // 
      this.datasetBinding.SetControlProperty(this.label4, "Text");
      this.datasetBinding.SetDataSourceProperty(this.label4, null);
      this.label4.Location = new System.Drawing.Point(13, 115);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(110, 13);
      this.label4.TabIndex = 13;
      this.label4.Text = "Password:";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtPassword
      // 
      this.datasetBinding.SetControlProperty(this.txtPassword, "Text");
      this.datasetBinding.SetDataSourceProperty(this.txtPassword, "Password");
      this.txtPassword.Location = new System.Drawing.Point(128, 112);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '●';
      this.txtPassword.Size = new System.Drawing.Size(211, 20);
      this.txtPassword.TabIndex = 12;
      // 
      // btnCancel
      // 
      this.datasetBinding.SetControlProperty(this.btnCancel, "Text");
      this.datasetBinding.SetDataSourceProperty(this.btnCancel, null);
      this.btnCancel.Location = new System.Drawing.Point(264, 145);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
      this.datasetBinding.SetControlProperty(this.pictureBox1, "Image");
      this.datasetBinding.SetDataSourceProperty(this.pictureBox1, null);
      this.pictureBox1.Location = new System.Drawing.Point(15, 138);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(324, 1);
      this.pictureBox1.TabIndex = 15;
      this.pictureBox1.TabStop = false;
      // 
      // btnOk
      // 
      this.datasetBinding.SetControlProperty(this.btnOk, "Text");
      this.datasetBinding.SetDataSourceProperty(this.btnOk, null);
      this.btnOk.Enabled = false;
      this.btnOk.Location = new System.Drawing.Point(183, 145);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(75, 23);
      this.btnOk.TabIndex = 16;
      this.btnOk.Text = "&Ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
      // 
      // userBindingSource
      // 
      this.userBindingSource.DataSource = typeof(TestApp.User);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(371, 184);
      this.datasetBinding.SetControlProperty(this, "Text");
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.txtMail);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.txtUsername);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.txtLastName);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.txtFirstName);
      this.Controls.Add(this.label1);
      this.datasetBinding.SetDataSourceProperty(this, null);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Create User";
      ((System.ComponentModel.ISupportInitialize)(this.datasetBinding)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.userBindingSource)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private DatasetBinding datasetBinding;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtFirstName;
    private System.Windows.Forms.TextBox txtLastName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtMail;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.BindingSource userBindingSource;
  }
}

