using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Linq;

namespace App.AdventureMaker.Core.Forms
{
	public class AddResourceWindow : Dialog<CampaignResource>
	{
		private readonly IEditor<CampaignFile> editor;

		private readonly EnumRadioButtonList<ResourceType> dropDown;

		public AddResourceWindow(IEditor<CampaignFile> editor)
		{
			this.editor = editor;

			Title = "Add new resource";

			Size = MinimumSize = new Size(400, 250);
			Resizable = false;

			Content = new StackLayout()
			{
				Style = "vertical",
				Spacing = 8,

				Items =
				{
					new StackLayoutItem(new GroupBox()
					{
						Text = "Select a resource type to add",
						Content = (dropDown = new EnumRadioButtonList<ResourceType>()
						{
							Orientation = Orientation.Vertical,
							Spacing = new Size(8, 8)
						})
					}, true),
					new StackLayoutItem(new StackLayout()
					{
						Style = "horizontal",
						Spacing = 8,

						Items =
						{
							null,
							(DefaultButton = new Button(OnConfirm)
							{
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16),
								Enabled = false
							}),
							(AbortButton = new Button(OnCancel)
							{
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)
							})
						}
					}, false),
				}
			};

			const ResourceType typeToRemove = ResourceType.None;

			dropDown.Items.Remove(dropDown.Items.First(item =>
			{
				return Equals(item.Key, ((int)typeToRemove).ToString())
					&& Equals(item.Text, typeToRemove.ToString());
			}));

			dropDown.SelectedValueChanged += (sender, e) => DefaultButton.Enabled = true;
		}

		protected void OnConfirm(object sender, EventArgs e)
		{
			if (dropDown.SelectedIndex > -1)
			{
				Visible = false;
				ParentWindow.BringToFront();
				Close(Constants.RESOURCE_DIALOGS[dropDown.SelectedValue](null, editor));
			}
		}

		protected void OnCancel(object sender, EventArgs e)
		{
			Close(null);
		}
	}
}
