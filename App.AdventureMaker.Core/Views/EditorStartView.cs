using App.AdventureMaker.Core.Commands;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Eto.Drawing;
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

			Style = "vertical";
			Spacing = 4;

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Style = "horizontal",
				Spacing = 4,

				Items =
				{
					new StackLayoutItem(new GroupBox()
					{
						Text = "Get started",
						Width = 256,

						Content = new StackLayout()
						{
							Style = "no-padding vertical",
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
									Text = "Import campaign package",
									Enabled = false
								},
								new Button()
								{
									Command = new TutorialsCommand(),
									Text = "Online wiki",
								},
								null,
								#if PREVIEW
								TableLayout.AutoSized(new Label()
								{
									Text = "Campaign editor preview version,\nuse for feedback purposes only!",
									TextAlignment = TextAlignment.Center,
									Font = new Font(SystemFont.Bold),
									TextColor = Colors.Red
								}, new Padding(8), centered: true)
								
								#endif
							}
						}
					}, false),
					new StackLayoutItem(new GroupBox()
					{
						Text = "Recent projects",
						Content = new Scrollable() { Content = (recent = new StackLayout()
						{
							Style = "no-padding vertical",
							Spacing = 0,

							Items = { "No recent items found." },
						})}
					}, true)
				}
			}, true));

			Items.Add(new StackLayoutItem(new StackLayout()
			{
				Style = "horizontal",
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
					Button button;

					recent.Items.Add(new StackLayout()
					{
						Style = "no-padding vertical",
						Spacing = 2,

						Items =
						{
							(button = new Button()
							{ 
								Text = $"{projectTitle} ({file.Directory.FullName})"
							})
						},

						ContextMenu = new ContextMenu()
						{
							Items =
							{
								(new ButtonMenuItem((sender, e) => 
								{ 
									RecentProjects.Remove(file);
								})
								{
									Text = "Remove",
									Image = Resources.GetIcon("CloseRed.ico")
								}),
								(new ButtonMenuItem((sender, e) =>
								{
									RecentProjects.Clear();
								})
								{
									Text = "Clear all",
									Image = Resources.GetIcon("CloseGray.ico")
								})
							}
						}
					});

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
