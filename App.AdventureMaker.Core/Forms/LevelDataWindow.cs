using Distance.AdventureMaker.Common.Models;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.AdventureMaker.Core.Forms
{
	public class LevelDataWindow : Dialog<CampaignLevel>
	{
		private CampaignLevel Data;

		public LevelDataWindow(CampaignLevel date)
		{
			Data = Data.CloneObject();

			Title = Equals(Data, null) ? "Add new playlist level" : "Edit playlist level";
		}

		private void Confirm(object sender, EventArgs e)
		{
			Close(Data);
		}

		private void Cancel(object sender, EventArgs e)
		{
			Close(null);
		}
	}
}
