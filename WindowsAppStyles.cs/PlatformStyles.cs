using Eto;
using Eto.Forms;
using SWF = System.Windows.Forms;

public static class PlatformStyles
{
	public static void ApplyAll()
	{
		Style.Add<Control>(null, item =>
		{
			SWF.Control control = (SWF.Control)item.ControlObject;
			control.Margin = SWF.Padding.Empty;
			control.Padding = SWF.Padding.Empty;
		});


		Style.Add<Button>(null, item =>
		{
			SWF.Button control = (SWF.Button)item.ControlObject;
			control.Margin = SWF.Padding.Empty;
			control.Padding = SWF.Padding.Empty;

			item.Shown += (sender, e) =>
			{
				//MessageBox.Show(control.Parent.GetType().BaseType.FullName);
			};
		});

		Style.Add<StackLayout>(null, item =>
		{
			item.Shown += (sender, e) =>
			{
				//MessageBox.Show(item.ControlObject.GetType().FullName);
			};

			//return;
			/*SWF.TableLayoutPanel control = (SWF.TableLayoutPanel)item.ControlObject;
			control.Margin = SWF.Padding.Empty;
			control.Padding = SWF.Padding.Empty;
			control.AutoSizeMode = SWF.AutoSizeMode.GrowAndShrink;
			control.AutoSize = true;*/
		});

		Style.Add<Panel>(null, item =>
		{
			//SWF.Panel control = (SWF.Panel)item.ControlObject;

			//control.Padding = SWF.Padding.Empty;
		});

		/*Style.Add<ListBox>(null, item =>
		{
			SWF.ListBox control = item.ControlObject as SWF.ListBox;
			control.BorderStyle = SWF.BorderStyle.None;
		});*/
	}
}