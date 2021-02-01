using App.AdventureMaker.Core.Controls;
using Distance.AdventureMaker.Common.Models.UI;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Forms
{
	public class NewProjectWindow : Dialog<ProjectCreateData>
	{
		private readonly PropertiesListBase propertiesPanel;
		private readonly TextBox nameBox;
		private readonly TextBox descriptionBox;
		private readonly TextBoxWithButton folderBox;
		private readonly Button confirmButton;
		private readonly Button cancelButton;

		private readonly SelectFolderDialog folderDialog;

		public NewProjectWindow()
		{
			Title = "New project";
			Padding = new Padding(8);
			MinimumSize = new Size(512, 160);

			Content = new StackLayout()
			{
				Orientation = Orientation.Vertical,
				HorizontalContentAlignment = HorizontalAlignment.Stretch,
				Spacing = 8,
				Items =
				{
					new StackLayoutItem(propertiesPanel = new PropertiesListBase())
					{
						Expand = true
					},
					new StackLayoutItem(new StackLayout()
					{
						Orientation = Orientation.Horizontal,
						VerticalContentAlignment = VerticalAlignment.Stretch,
						Spacing = 8,
						Padding = new Padding(0, 8, 0, 0),

						Items =
						{
							null,
							(confirmButton = new Button(OnConfirm)
							{ 
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16)
							}),
							(cancelButton = new Button(OnCancel)
							{ 
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)
							})
						}
					})
					{
						Expand = false
					}
				}
			};

			folderDialog = new SelectFolderDialog()
			{
				Title = "Project folder",
				//Directory = EtoEnvironment.GetFolderPath(EtoSpecialFolder.Documents)
			};

			propertiesPanel.AddRow("Project name", nameBox = new TextBox());
			propertiesPanel.AddRow("Description", descriptionBox = new TextBox());
			propertiesPanel.AddRow("Folder", folderBox = new TextBoxWithButton() { ControlEnabled = false });
			propertiesPanel.CompleteRows();

			AbortButton = cancelButton;
			DefaultButton = confirmButton;

			folderBox.ButtonClick += SelectFolder;
		}

		private void SelectFolder()
		{
			if (folderDialog.ShowDialog(this) == DialogResult.Ok)
			{
				folderBox.Text = folderDialog.Directory;
			}
		}

		private void OnConfirm(object sender, EventArgs e)
		{
			ProjectCreateData data = new ProjectCreateData(nameBox.Text, descriptionBox.Text, folderBox.Text);

			if (!data.Validate(out string error))
			{
				MessageBox.Show(error, "", MessageBoxButtons.OK);
			}
			else
			{
				Close(data);
			}
		}

		private void OnCancel(object sender, EventArgs e)
		{
			Close(null);
		}
	}
}