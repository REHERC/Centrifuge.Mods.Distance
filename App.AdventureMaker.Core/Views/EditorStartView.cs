using App.AdventureMaker.Core.Commands;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System.Diagnostics;
using System.IO;

namespace App.AdventureMaker.Core.Views
{
	public class EditorStartView : StackLayout
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly StackLayout recent;
		private readonly LinkButton discordLink;

		public EditorStartView(MainView editor)
		{
			this.editor = editor;

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
						Text = "Recent projects",
						Content = new Scrollable() { Content = (recent = new StackLayout()
						{
							Orientation = Orientation.Vertical,
							HorizontalContentAlignment = HorizontalAlignment.Stretch,
							Spacing = 4,

							Items = { "No recent items found." }
						})}
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
			
			RecentProjects.OnChanged += RecentFilesUpdated;
			RecentFilesUpdated();

			editor.OnLoaded += OnLoaded;
		}

		private void OnLoaded(IEditor<CampaignFile> editor)
		{
			if (editor.CurrentFile != null)
			{
				RecentProjects.Update(editor.CurrentFile);
			}
		}

		private void RecentFilesUpdated()
		{
			recent.Items.Clear();

			foreach (FileInfo file in RecentProjects.Get())
			{
				string projectTitle = "<Could not read campaign title>";

				try
				{
					projectTitle = Json.Load<CampaignFile>(file).metadata.title;
				}
				finally
				{
					LinkButton button;
					recent.Items.Add(button = new LinkButton() { Text = $"{projectTitle} ({file.Directory.FullName})" });

					button.Click += (sender, e) =>
					{
						RecentProjects.Update(file);
						editor.LoadFile(file);
					};
				}
			}
		}
	}
}
