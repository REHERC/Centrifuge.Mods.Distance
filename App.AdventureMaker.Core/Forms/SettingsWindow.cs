using App.AdventureMaker.Core.Controls;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Forms
{
	public class SettingsWindow : Dialog
	{
		public const int TCP_PORT_MAX_NUMBER = 65535;

		private readonly TabControl tabs;

		private readonly RadioButtonList previewModeRunMethod;
		private readonly TextBoxWithButton previewModeRunExecutable;
		private readonly CheckBox previewModeEnableRemoteConsole;
		private readonly NumericStepper previewModeRemoteConsolePort;

		#region Constructors
		public SettingsWindow()
		{
			#region Main Layout Init
			Title = "Settings";
			Padding = new Padding(8);

			MinimumSize = new Size(512, 386);

			Content = new StackLayout()
			{
				Orientation = Orientation.Vertical,
				HorizontalContentAlignment = HorizontalAlignment.Stretch,
				Spacing = 8,
				Items =
				{
					new StackLayoutItem(tabs = new TabControl())
					{
						Expand = true
					},
					new StackLayoutItem(new StackLayout()
					{
						Orientation = Orientation.Horizontal,
						VerticalContentAlignment = VerticalAlignment.Stretch,
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
			tabs.Pages.Add(new TabPage()
			{
				Text = "Campaign Preview Mode",
				Content = new Scrollable()
				{
					Content = new StackLayout()
					{
						Orientation = Orientation.Vertical,
						HorizontalContentAlignment = HorizontalAlignment.Stretch,
						Spacing = 4,

						Items =
						{
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
								Text = "Intercept log messages from the game (set TCP port below)",
							}),
							(previewModeRemoteConsolePort = new NumericStepper()
							{
								DecimalPlaces = 0,
								MinValue = 0,
								MaxValue = TCP_PORT_MAX_NUMBER,
								Enabled = AppSettings.Instance.EnableRcon
							})
						}
					}
				}
			});
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

			previewModeRunMethod.SelectedIndexChanged += (sender, e) =>
			{
				previewModeRunExecutable.Enabled = previewModeRunMethod.SelectedIndex == 1;
			};

			previewModeEnableRemoteConsole.CheckedChanged += (sender, e) =>
			{
				previewModeRemoteConsolePort.Enabled = previewModeEnableRemoteConsole.Checked == true;
			};
		}

		public SettingsWindow(int pageIndex) : this()
		{
			tabs.SelectedIndex = pageIndex;
		}
		#endregion

		#region Save / Load
		private void LoadSettings()
		{
			previewModeRunMethod.SelectedIndex = AppSettings.Instance.PreviewModeRunMethod;
			previewModeRunExecutable.Text = AppSettings.Instance.GameExe;
			previewModeEnableRemoteConsole.Checked = AppSettings.Instance.EnableRcon;
			previewModeRemoteConsolePort.Value = AppSettings.Instance.RconPort;
		}

		private void SaveSettings()
		{
			AppSettings.Instance.PreviewModeRunMethod = previewModeRunMethod.SelectedIndex;
			AppSettings.Instance.GameExe = previewModeRunExecutable.Text;
			AppSettings.Instance.EnableRcon = previewModeEnableRemoteConsole.Checked == true;
			AppSettings.Instance.RconPort = (int)previewModeRemoteConsolePort.Value;

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