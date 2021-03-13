using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using Eto.Drawing;
using System;
using App.AdventureMaker.Core.Controls;

namespace App.AdventureMaker.Core.Forms
{
	public class LevelPropertiesWindow : Dialog<CampaignLevel>
	{
		private CampaignLevel Data { get; }

		private readonly ExtendedTabControl tabs;
		private readonly DynamicLayout generalProperties;
		private readonly DynamicLayout loadingScreenProperties;
		private readonly DynamicLayout introSequenceProperties;
		private readonly DynamicLayout gameplayProperties;

		public LevelPropertiesWindow(CampaignLevel level)
		{
			Data = level.CloneObject()
			?? new CampaignLevel()
			{
				Guid = Guid.NewGuid().ToString()
			};

			Title = $"{(Equals(level, null) ? "Add" : "Edit")} level";
			Size = MinimumSize = new Size(600, 400);
			Padding = new Padding(8);

			Resizable = true;

			Content = new StackLayout()
			{
				Style = "vertical",
				Spacing = 8,

				Items =
				{
					new StackLayoutItem(tabs = new ExtendedTabControl(), true),
					new StackLayoutItem(new StackLayout()
					{
						Style = "horizontal",
						Spacing = 8,
						Padding = new Padding(0, 8, 0, 0),

						Items =
						{
							null,
							(DefaultButton = new Button(OnConfirm)
							{
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16)
							}),
							(AbortButton = new Button(OnCancel)
							{
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)
							})
						}
					}, false)
				}
			};

			#region General Tab
			tabs.AddPage("General", generalProperties = new DynamicLayout());

			generalProperties.BeginScrollable();

			for (int groupID = 1; groupID < 10; ++groupID)
			{
				DynamicGroup dynGP = null;
				CheckBox dynGPenabled;
				generalProperties.AddSeparateRow($"Enable group #{groupID}", dynGPenabled = new CheckBox());

				dynGPenabled.CheckedChanged += (sender, e) => dynGP.GroupBox.Enabled = dynGPenabled.Checked == true;

				dynGP = generalProperties.BeginGroup($"Group #{groupID}");
				for (int propID = 1; propID < 10; ++propID)
				{
					generalProperties.AddRow($"Property {groupID}.{propID}", new TextBox());
				}
				generalProperties.EndGroup();
			}

			generalProperties.EndScrollable();
			#endregion

			tabs.AddPage("Gameplay", gameplayProperties = new DynamicLayout());
			tabs.AddPage("Loading screen", loadingScreenProperties = new DynamicLayout());
			tabs.AddPage("Intro sequence", introSequenceProperties = new DynamicLayout());
		}

		private void OnCancel(object sender, EventArgs e)
		{
			Close(null);
		}

		private void OnConfirm(object sender, EventArgs e)
		{
			Close(Data);
		}
	}
}
