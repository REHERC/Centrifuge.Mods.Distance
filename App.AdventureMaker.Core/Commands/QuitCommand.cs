﻿using App.AdventureMaker.Core.Forms;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Commands
{
	public class QuitCommand : Command
	{
		private readonly MainWindow form_;

		public QuitCommand(MainWindow form)
		{
			form_ = form;
			MenuText = "&Quit";
			ToolBarText = "Quit";
			Image = Resources.GetIcon("CloseRed.ico");
			Shortcut = Application.Instance.CommonModifier | Keys.Q;
		}

		protected override void OnExecuted(EventArgs e)
		{
			base.OnExecuted(e);

			form_.Close();
		}
	}
}
