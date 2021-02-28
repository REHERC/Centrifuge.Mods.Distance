using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using Eto.Drawing;
using System;
using App.AdventureMaker.Core.Controls;

namespace App.AdventureMaker.Core.Forms
{
	public class LevelPropertiesWindow : Dialog<CampaignLevel>
	{
		private CampaignLevel Data { get; set; }

		private readonly ExtendedTabControl tabs;
		private readonly PropertiesListBase generalProperties;
		private readonly PropertiesListBase loadingScreenProperties;
		private readonly PropertiesListBase introSequenceProperties;
		private readonly PropertiesListBase gameplayProperties;

		public LevelPropertiesWindow(CampaignLevel level)
		{
			Data = level.CloneObject()
			?? new CampaignLevel()
			{
				guid = Guid.NewGuid().ToString()
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
							})                      }
					}, false)
				}
			};



			tabs.AddPage("General", generalProperties = new PropertiesListBase(), scrollable: true);
			tabs.AddPage("Gameplay", gameplayProperties = new PropertiesListBase(), scrollable: true);
			tabs.AddPage("Loading screen", loadingScreenProperties = new PropertiesListBase(), scrollable: true);
			tabs.AddPage("Intro sequence", introSequenceProperties = new PropertiesListBase(), scrollable: true);


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
