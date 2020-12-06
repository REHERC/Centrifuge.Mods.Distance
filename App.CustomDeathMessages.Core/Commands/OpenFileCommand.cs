#pragma warning disable IDE0052

using App.CustomDeathMessages.Core.Data;
using App.CustomDeathMessages.Core.Forms;
using Eto.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.CustomDeathMessages.Core.Commands
{
	public class OpenFileCommand : Command
	{
		readonly MainWindow form_;
		
		public OpenFileCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&Open";
			ToolBarText = "Open";
			Image = Resources.GetIcon("System.Windows.Forms.open.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.O;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			using OpenFileDialog dialog = new OpenFileDialog
			{
				Title = "Open",
				MultiSelect = false,
				CheckFileExists = true
			};

			dialog.Filters.Add(Constants.DIALOG_FILTER_JSON);
			dialog.Filters.Add(Constants.DIALOG_FILTER_ANY);
			dialog.CurrentFilterIndex = 0;

			if (dialog.ShowDialog(form_) == DialogResult.Ok)
			{
				form_.FilePath = dialog.FileName;

				Dictionary<string, string[]> loadData = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(dialog.FileName));

				form_.Data = new EditorData();

				foreach (var item in loadData)
				{
					if (EditorData.Sections.Contains(item.Key))
					{
						form_.Data[item.Key] = item.Value;
					}
				}

				form_.View.Sections.SelectedIndex = 0;
				form_.View.OnSectionChanged(form_.View, new EventArgs());

				form_.Modified = false;
			}
		}
	}
}
