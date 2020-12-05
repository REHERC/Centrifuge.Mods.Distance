using Eto.Drawing;
using Eto.Forms;

namespace App.CustomDeathMessages.Core.Forms
{
	public class AboutDialog : Dialog
	{
		private Label AppName;
		private Label AppAuthor;
		private Label AppRepository;
		private Button CloseButton;

		private void InitializeComponent()
		{
			Title = "About";
			MinimumSize = new Size(386, 0);
			Resizable = false;

			AppName = new Label()
			{
				Text = "Custom Death Messages Editor",
				Font = new Font(FontFamilies.Sans, 16),
				TextAlignment = TextAlignment.Center
			};

			AppAuthor = new Label()
			{
				Text = "Original mod by Larnin (Nico), currently maintained by Reherc",
				TextAlignment = TextAlignment.Center
			};

			AppRepository = new Label()
			{
				Text = "Contribute at: github.com/reherc/centrifuge.mods.distance",
				TextAlignment = TextAlignment.Center
			};

			CloseButton = new Button()
			{
				Text = "Close"
			};
			CloseButton.Click += (sender, e) => Close();

			Content = new TableLayout()
			{
				Padding = new Padding(20),
				Spacing = new Size(15, 15),
				Rows =
				{
					AppName,
					AppAuthor,
					AppRepository,
					TableLayout.AutoSized(CloseButton, centered: true),
				},
			};

			AbortButton = DefaultButton = CloseButton;
		}

		public AboutDialog()
		{
			InitializeComponent();
		}
	}
}
