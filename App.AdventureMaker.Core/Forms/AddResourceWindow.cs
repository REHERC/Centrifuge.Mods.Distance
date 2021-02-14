using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Forms
{
	public class AddResourceWindow : Dialog<CampaignResource>
	{
		private readonly EnumDropDown<ResourceType> dropDown;
		public AddResourceWindow()
		{
			Size = MinimumSize = new Size(400, 250);

			Content = new StackLayout()
			{
				Orientation = Orientation.Vertical,
				HorizontalContentAlignment = HorizontalAlignment.Stretch,
				Spacing = 8,

				Items =
				{
					new StackLayoutItem(new GroupBox()
					{
						Text = "Select a resource type to add",
						Content = (dropDown = new EnumDropDown<ResourceType>())
					}, true),
					new StackLayoutItem(new StackLayout()
					{
						Orientation = Orientation.Horizontal,
						VerticalContentAlignment = VerticalAlignment.Stretch,
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

			dropDown.SelectedValueChanged += (sender, e) => DefaultButton.Enabled = true;
		}
	
		protected void OnConfirm(object sender, EventArgs e)
		{
			if (dropDown.SelectedIndex > -1)
			{
				Visible = false;
				ParentWindow.BringToFront();
				Close(Constants.RESOURCE_DIALOGS[dropDown.SelectedValue](null));
			}
		}
		
		protected void OnCancel(object sender, EventArgs e)
		{
			Close(null);
		}
	}
}
