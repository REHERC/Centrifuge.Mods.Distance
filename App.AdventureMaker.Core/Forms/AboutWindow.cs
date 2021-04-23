using Eto.Drawing;
using Eto.Forms;

namespace App.AdventureMaker.Core.Forms
{
	public class AboutWindow : Dialog
	{
		private void InitializeComponent()
		{
			Title = "About";
			MinimumSize = new Size(512, 100);
			Resizable = false;

			Content = new StackLayout()
			{
				Style = "vertical",
				Spacing = 12,
				Padding = new Padding(16),

				//Spacing = new Size(15, 15),
				Items =
				{
					new ImageView()
					{
						Width = 128,
						Height = 128,
						Image = Resources.GetIcon("Toolbox.ico")
					},
					new Label()
					{
						Text = "Distance Campaign Editor by Reherc",
						Font = new Font(FontFamilies.Sans, 16),
						TextAlignment = TextAlignment.Center
					},
					/*new Label()
					{
						Text = "by Reherc",
						TextAlignment = TextAlignment.Center
					},*/
					new Label()
					{
						Text = "Community content making tool for Distance",
						TextAlignment = TextAlignment.Center
					},
					new Label()
					{
						Text = Main.GetFullVersionString(),
						TextAlignment = TextAlignment.Center
					},
					//new StackLayout()
					//{
					//	Orientation = Orientation.Vertical,
					//	HorizontalContentAlignment = HorizontalAlignment.Center,

					//	Items = 
					//	{
							(AbortButton = new Button()
							{
								Text = "Close"
							})
					//	}
					//}

					//TableLayout.AutoSized(, centered: true),
				},
				ClientSize = new Size(MinimumSize.Width, 96)

				// github.com/reherc/centrifuge.mods.distance
			};

			AbortButton.Click += (sender, e) => Close();
			DefaultButton = AbortButton;
		}

		public AboutWindow()
		{
			InitializeComponent();
		}
	}
}
