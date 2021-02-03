using App.AdventureMaker.Core.Commands;
using Eto.Forms;
using System.Diagnostics;

namespace App.AdventureMaker.Core.Views
{
	public class EditorStartView : StackLayout
	{
		private readonly ListBox recentItems;
		private readonly LinkButton discordLink;

		public EditorStartView(MainView editor)
		{
			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;
			Spacing = 4;

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Orientation = Orientation.Horizontal,
				VerticalContentAlignment = VerticalAlignment.Stretch,
				Spacing = 4,

				Items =
				{
					new StackLayoutItem(new GroupBox()
					{
						Text = "Get started",
						Width = 256,

						Content = new StackLayout()
						{
							Style = "no-padding",
							Orientation = Orientation.Vertical,
							HorizontalContentAlignment = HorizontalAlignment.Stretch,
							Spacing = 2,

							Items =
							{
								new Button()
								{
									Command = new NewFileCommand(editor),
									Text = "New project"
								},
								new Button()
								{
									Command = new OpenFileCommand(editor),
									Text = "Open existing project"
								},
								new Button()
								{
									Command = new TutorialsCommand(),
									Text = "Online wiki",
								},
								null
							}
						}
					}, false),
					new StackLayoutItem(new GroupBox()
					{
						Text = "Recent files",
						Content = (recentItems = new ListBox()
						{
							Items = { "The recent items list is currently not implemented." },
							Enabled = false
						})
					}, true)
				}
			}, true));

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Orientation = Orientation.Horizontal,
				VerticalContentAlignment = VerticalAlignment.Bottom,
				Spacing = 4,

				Items =
				{
					(discordLink = new LinkButton()
					{
						Text = "Distance discord - Join the community!",
					}),
					null,
					new Button()
					{
						Command = new FeedbackCommand(),
						Text = "Bug reports and feedback",
						Image = Resources.GetIcon("Feedback.ico", 16)
					},
					new Button()
					{
						Command = new SettingsCommand(),
						Text = "Settings",
						Image = Resources.GetIcon("Settings.ico", 16)
					}
				}
			}, false));

			discordLink.Click += (sender, e) =>
			{
				new Process()
				{
					StartInfo = new ProcessStartInfo(Constants.DISCORD_DISTANCE)
					{
						UseShellExecute = true
					}
				}.Start();
			};
		}
	}
}
