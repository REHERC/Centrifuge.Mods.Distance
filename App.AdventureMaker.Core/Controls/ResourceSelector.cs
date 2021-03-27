using App.AdventureMaker.Core.Forms.ResourceDialogs;
using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using System;
using System.Linq;

namespace App.AdventureMaker.Core.Controls
{
	public class ResourceSelector : TextBoxWithButton
	{
		public event EventHandler<EventArgs> ResourceSelected;

		private readonly IEditor<CampaignFile> editor;
		private readonly ResourceType type;

		private string resource_ = string.Empty;
		public CampaignResource Resource
		{
			get
			{
				var items = editor.Document.Data.Resources.Where(res => Equals(res.guid, resource_) && Equals(res.resource_type, type));
				return items.FirstOrDefault();
			}
			set
			{
				resource_ = value?.guid;
				Control.Text = value is null ? "<no element selected>" : value.file;

				ResourceSelected?.Invoke(this, EventArgs.Empty);
			}
		}

		public ResourceSelector(IEditor<CampaignFile> editor, ResourceType type)
		: base()
		{
			this.editor = editor;
			this.type = type;

			ControlEnabled = false;
		}

		protected override void OnButtonClicked()
		{
			base.OnButtonClicked();

			CampaignResource resource = new ResourceBrowser(editor, type).ShowModal();

			if (resource is CampaignResource.Dummy)
			{
				Resource = null;
			}
			else if (resource != null)
			{
				Resource = resource;
			}
		}
	}
}
