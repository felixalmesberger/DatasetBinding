using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.More.DatasetBinding.Designer
{
  public class DataSourcePropertyChooser : UITypeEditor
  {

    private IWindowsFormsEditorService editorService;


    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {

      this.editorService = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
      var listBox = new ListBox()
      {
        SelectionMode = SelectionMode.One
      };

       listBox.SelectedIndexChanged += this.SelectedIndexChanged;

      var binding = this.GetDatasetBinding(context);


      if (binding != null)
      {
        var properties = this.GetPropertiesFromBinding(binding);
        listBox.Items.AddRange(properties.ToArray());
      }

      editorService?.DropDownControl(listBox);

      return listBox.SelectedItem as string;
    }

    private void SelectedIndexChanged(object sender, EventArgs e)
    {
      editorService.CloseDropDown();
    }

    protected List<string> GetPropertiesFromBinding(DatasetBinding binding)
    {
      return binding.AvailableDataSourceProperties;
    }

    private DatasetBinding GetDatasetBinding(ITypeDescriptorContext context)
    {

      var name = this.GetDatasetBindingName(context);

      var components = context.Container.Components;

      return components
        .OfType<DatasetBinding>()
        .FirstOrDefault(x => this.GetComponentName(x) == name);

    }

    private string GetDatasetBindingName(ITypeDescriptorContext context)
    {
      var fullname = context
        ?.GetType()
        .GetProperties()
        .FirstOrDefault(x => x.Name == "FullLabel")
        ?.GetValue(context) as string;

      return fullname?.Split(' ').LastOrDefault();
    }

    private string GetComponentName(Component component)
    {
      return component.ToString().Split(' ').FirstOrDefault();
    }
  }
}