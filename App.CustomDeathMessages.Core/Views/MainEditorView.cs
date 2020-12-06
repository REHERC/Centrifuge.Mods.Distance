using App.CustomDeathMessages.Core.Data;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Text.RegularExpressions;

namespace App.CustomDeathMessages.Core.Views
{
	public class MainEditorView : Splitter
	{
		public event EventHandler<EventArgs> Modified;

		public ListBox Sections { get; protected set; }

		public TextArea TextField { get; protected set; }

		internal EditorData Data { get; set; }

		public MainEditorView()
		{
			Sections = new ListBox()
			{
				Font = new Font(FontFamilies.Sans, 10)
			};

			foreach (string section in EditorData.Sections)
			{
				Sections.Items.Add(section);
			}

			TextField = new TextArea()
			{
				Font = new Font(FontFamilies.Monospace, 10),
				Wrap = false
			};

			Panel1 = Sections;
			Panel2 = TextField;
			FixedPanel = SplitterFixedPanel.Panel1;
			RelativePosition = 200;
			Panel1MinimumSize = 100;
			Panel2MinimumSize = 50;

			Data = new EditorData();

			Sections.SelectedIndexChanged += OnSectionChanged;
			Sections.SelectedIndex = 0;

			TextField.TextChanged += OnTextChanged;
		}

		internal void OnSectionChanged(object sender, EventArgs e)
		{
			string section = Sections.SelectedValue.ToString();
			TextField.Text = string.Join('\n', Data[section]);
		}

		internal void OnTextChanged(object sender, EventArgs e)
		{
			string section = Sections.SelectedValue.ToString();
			Data[section] = Regex.Split(TextField.Text, "\r\n|\r|\n");

			Modified(this, new EventArgs());
		}
	}
}