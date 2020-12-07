using Eto.Forms;
using Eto.Drawing;

namespace App.CustomDeathMessages.Core.Forms
{
	public class FormattingHelpWindow : Dialog
	{
		private void InitializeComponent()
		{
			Title = "Message Formatting Help";
			ClientSize = new Size(386, 256);

			Content = new TextArea()
			{
				Text = Resources.GetText("Formatting.txt"),
				Font = new Font(FontFamilies.Monospace, 10),
				ReadOnly = true
			};
		}

		public FormattingHelpWindow()
		{
			InitializeComponent();
		}
	}
}
