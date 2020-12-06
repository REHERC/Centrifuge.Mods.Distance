using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Views
{
	public class MainEditorView : Splitter
	{
		protected ListBox Sections { get; set; }
		
		protected TextArea TextField { get; set; }

		public MainEditorView()
		{
			Sections = new ListBox()
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

			TextField = new TextArea()
			{
				Font = new Font(FontFamilies.Monospace, 10)
			};

			Panel1 = Sections;
			Panel2 = TextField;
			FixedPanel = SplitterFixedPanel.Panel1;
			RelativePosition = 200;
			Panel1MinimumSize = 100;
			Panel2MinimumSize = 50;

			Sections.SelectedIndexChanged += OnSectionChanged;
			Sections.SelectedIndex = 0;
		}

		private void OnSectionChanged(object sender, EventArgs e)
		{

		}
	}
}
