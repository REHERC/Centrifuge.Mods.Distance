using App.AdventureMaker.Core.Controls;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Forms
{
	public class SettingsWindow : Dialog
	{
		private readonly TabControl tabs;

		private readonly CheckBox generalOpenLastProject;
		private readonly Label previewModeInfoLabel;
		private readonly RadioButtonList previewModeRunMethod;
		private readonly TextBoxWithButton previewModeRunExecutable;
		private readonly CheckBox previewModeEnableRemoteConsole;

		#region Constructors
		public SettingsWindow()
		{
			#region Main Layout Init
			Title = "Settings";
			Padding = new Padding(8);

			MinimumSize = new Size(512, 386);

			Content = new StackLayout()
			{
				Style = "vertical",
				Items =
				{
					new StackLayoutItem(tabs = new TabControl())
					{
						Expand = true
					},
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
					})
					{
						Expand = false
					}
				}
			};
			#endregion

			#region Tab Initialization
			#region General
			tabs.Pages.Add(new TabPage()
			{
				Text = "General",
				Content = new Scrollable()
				{
					Content = new StackLayout()
					{
						Style = "vertical",
						Spacing = 4,

						Items =
						{
							(generalOpenLastProject = new CheckBox()
							{
								Text = "Open most recent project on startup"
							})
						}
					}
				}
			});
			#endregion
			#region Campaign Preview Mode
			tabs.Pages.Add(new TabPage()
			{
				Text = "In-game campaign preview",
				Content = new Scrollable()
				{
					Content = new StackLayout()
					{
						Style = "vertical",
						Spacing = 4,

						Items =
						{
							(previewModeInfoLabel = new Label()
							{
								Text = "The preview mode requires the AdventureMaker mod to be installed.",
								TextColor = Colors.Blue
							}),
							"Select the method that will be used to run the game:",
							(previewModeRunMethod = new RadioButtonList()
							{
								Style = "no-padding",
								Orientation = Orientation.Vertical,
								Spacing = new Size(0, 2),
								Items =
								{
									"Use steam:// links (if you own the game on steam)",
									"Open game executable (for drm-free distance players)",
								}
							}),
							(previewModeRunExecutable = new TextBoxWithButton()),
							(previewModeEnableRemoteConsole = new CheckBox()
							{
								Text = "Intercept log messages from the game (using named memory pipes)",
							})
						}
					}
				}
			});
			#endregion
			#endregion

			previewModeRunExecutable.ButtonClick += () =>
			{
				using OpenFileDialog dialog = new OpenFileDialog()
				{
					Title = "Select the Distance game executable",
					Filters =
					{
						Constants.DIALOG_FILTER_ANY
					}
				};
				if (dialog.ShowDialog(this) == DialogResult.Ok)
				{
					previewModeRunExecutable.Text = dialog.FileName;
				}
			};

			previewModeRunMethod.SelectedIndexChanged += (sender, e) => previewModeRunExecutable.Enabled = previewModeRunMethod.SelectedIndex == 1;

			previewModeInfoLabel.Font = new Font(previewModeInfoLabel.Font.Family, previewModeInfoLabel.Font.Size, FontStyle.Bold);
		}

		public SettingsWindow(int pageIndex) : this()
		{
			tabs.SelectedIndex = pageIndex;
		}
		#endregion

		#region Save / Load
		private void LoadSettings()
		{
			generalOpenLastProject.Checked = AppSettings.Instance.OpenLastProject;

			previewModeRunMethod.SelectedIndex = AppSettings.Instance.PreviewModeRunMethod;
			previewModeRunExecutable.Text = AppSettings.Instance.GameExe;
			previewModeEnableRemoteConsole.Checked = AppSettings.Instance.EnableRcon;
		}

		private void SaveSettings()
		{
			AppSettings.Instance.OpenLastProject = generalOpenLastProject.Checked == true;

			AppSettings.Instance.PreviewModeRunMethod = previewModeRunMethod.SelectedIndex;
			AppSettings.Instance.GameExe = previewModeRunExecutable.Text;
			AppSettings.Instance.EnableRcon = previewModeEnableRemoteConsole.Checked == true;

			AppSettings.Save();
		}

		#endregion

		#region Standard Methods
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			LoadSettings();
		}

		private void OnConfirm(object sender, EventArgs e)
		{
			SaveSettings();
			Close();
		}

		private void OnCancel(object sender, EventArgs e)
		{
			Close();
		}
		#endregion
	}
}