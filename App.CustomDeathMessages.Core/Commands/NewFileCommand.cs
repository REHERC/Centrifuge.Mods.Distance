#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Data;
using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using System;

namespace App.CustomDeathMessages.Core.Commands
{
	public class NewFileCommand : Command
	{
		readonly MainWindow form_;
		
		public NewFileCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&New";
			ToolBarText = "New";
			Image = Resources.GetIcon("System.Windows.Forms.new.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.N;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			if (form_.Modified && MessageBox.Show(form_, "You have unsaved changes!\nIf you create a new file they will be discarded.\n\nDo you want to create a new file anyways ?", "New file", MessageBoxButtons.YesNo, MessageBoxType.Warning) != DialogResult.Yes)
			{
				return;
			}

			CreateNewDocument();
		}

		protected void CreateNewDocument()
		{
			form_.Data = new EditorData();
			form_.FilePath = string.Empty;
			
			form_.View.Sections.SelectedIndex = 0;
			form_.View.OnSectionChanged(form_.View, new EventArgs());

			form_.Modified = false;
		}
	}
}
