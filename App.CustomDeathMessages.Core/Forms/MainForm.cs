using Eto.Forms;
using Eto.Drawing;

namespace App.CustomDeathMessages.Core.Forms
{
	public partial class MainForm : Form
	{
		private Command FileNew;
		private Command FileOpen;
		private Command FileSave;
		private Command FileSaveAs;
		private Command FileSettings;
		private Command FileQuit;
		private Command HelpAbout;
		private Splitter MainLayout;
		private ListBox SectionList;
		private TextArea SectionEdit;

		private void InitializeComponent()
		{
			Title = "Distance - Custom Death Messages Editor";
			ClientSize = new Size(640, 480);

			#region Menus
			#region Commands Initialisation
			FileNew = new Command()
			{
				MenuText = "&New",
				Shortcut = Application.Instance.CommonModifier | Keys.N
			};
			FileOpen = new Command()
			{
				MenuText = "&Open",
				Shortcut = Application.Instance.CommonModifier | Keys.O
			};
			FileSave = new Command()
			{
				MenuText = "&Save",
				Shortcut = Application.Instance.CommonModifier | Keys.S
			};
			FileSaveAs = new Command()
			{
				MenuText = "Sa&ve As",
				Shortcut = Application.Instance.CommonModifier | Keys.Shift | Keys.S
			};
			FileSettings = new Command()
			{
				MenuText = "Settin&gs"
			};
			FileQuit = new Command()
			{
				MenuText = "&Quit",
				Shortcut = Application.Instance.CommonModifier | Keys.Q
			};
			HelpAbout = new Command()
			{
				MenuText = "&About"
			};
			HelpAbout.Executed += OnExecuteHelpAboutCommand;
			#endregion
			#region Toolbars Layout Creation
			Menu = new MenuBar()
			{
				Items =
				{
					new ButtonMenuItem()
					{
						Text = "&File",
						Items =
						{
							FileNew,
							FileOpen,
							FileSave,
							FileSaveAs,
							new SeparatorMenuItem(),
							FileSettings,
							new SeparatorMenuItem(),
							FileQuit
						}
					},
					new ButtonMenuItem()
					{
						Text = "&Help",
						Items =
						{
							HelpAbout
						}
					}
				}
			};
			#endregion
			#endregion

			#region Layout
			#region Control Initialisation
			SectionList = new ListBox()
			{
				Font = new Font(FontFamilies.Sans, 10),
				Items =
				{
					"Anti Tunnel Squish",
					"Finished",
					"Impact",
					"Laser Overheated",
					"Kick No Level",
					"Kill Grid",
					"Not Ready",
					"Overheated",
					"Self Termination",
					"Spectate",
					"Stunt Collect",
					"Tag Points Lead",
				}
			};
			SectionEdit = new TextArea()
			{
				Font = new Font(FontFamilies.Monospace, 10)
			};
			MainLayout = new Splitter()
			{
				Panel1 = SectionList,
				Panel2 = SectionEdit,
				FixedPanel = SplitterFixedPanel.Panel1,
				RelativePosition = 200,
				Panel1MinimumSize = 100,
				Panel2MinimumSize = 50
			};
			#endregion

			SectionList.SelectedIndexChanged += (sender, e) =>
			{
				string name = SectionList.Items[SectionList.SelectedIndex].ToString();
				SectionEdit.Text = name;
			};

			SectionList.SelectedIndex = 0;

			Content = MainLayout;
			#endregion
		}

		public MainForm()
		{
			InitializeComponent();
		}
	}
}