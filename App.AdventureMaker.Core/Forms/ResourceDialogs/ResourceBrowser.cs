using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.AdventureMaker.Core.Forms.ResourceDialogs
{
	public class ResourceBrowser : Dialog<CampaignResource>
	{
		private readonly IEditor<CampaignFile> editor;
		private readonly ResourceType type;

		private readonly GridView<CampaignResource> resourcePicker;
		private readonly Button importResource;

		private List<CampaignResource> resourceList;

		public ResourceBrowser(IEditor<CampaignFile> editor, ResourceType type)
		{
			this.editor = editor;
			this.type = type;

			Size = MinimumSize = new Size(650, 300);

			Title = $"Select a {type.ToString().ToLower()}";

			Content = new StackLayout()
			{
				Style = "vertical",
				Spacing = 4,

				Items =
				{
					new StackLayoutItem(resourcePicker = new GridView<CampaignResource>()
					{
						GridLines = GridLines.Both,

						Columns =
						{
							new GridColumn()
							{
								HeaderText = "File",
								Editable = false,
								Resizable = true,

								DataCell = new TextBoxCell()
								{
									Binding = Binding.Property<CampaignResource, string>(res => $"{res.file}")
								}
							},
							/*new GridColumn()
							{
								HeaderText = "Guid",
								Editable = false,
								Resizable = true,
								
								DataCell = new TextBoxCell()
								{
									Binding = Binding.Property<CampaignResource, string>(res => $"{res.guid}")
								}
							}*/
						}
					}, true),
					new StackLayoutItem(new StackLayout()
					{
						Style = "horizontal",
						Spacing = 4,

						Items =
						{
							(importResource = new Button(ImportNew)
							{
								Text = $"Import new {type.ToString().ToLower()}",
								Image = Resources.GetIcon("AddGreen.ico", 16),
								Enabled = false
							}),
							null,
							(DefaultButton = new Button(Confirm)
							{
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16),
								Enabled = false
							}),
							(AbortButton = new Button(Cancel)
							{
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)
							})
						}
					}, false)
				}
			};

			resourcePicker.SelectedRowsChanged += OnRowSelected;

			UpdateDataStore();
		}

		private void UpdateDataStore()
		{
			resourceList = editor.Document.Data.Resources.Where(res => Equals(res.resource_type, type)).ToList();
			resourceList.Insert(0, new CampaignResource.Dummy());

			resourcePicker.DataStore = resourceList;

			resourcePicker.UpdateBindings();
			resourcePicker.Invalidate();
		}

		private void OnRowSelected(object sender, EventArgs e)
		{
			DefaultButton.Enabled = resourcePicker.SelectedRow > -1;
		}

		private void Confirm(object sender, EventArgs e)
		{
			Close(resourceList[resourcePicker.SelectedRow]);
		}

		private void Cancel(object sender, EventArgs e)
		{
			Close(null);
		}

		private void ImportNew(object sender, EventArgs e)
		{
			CampaignResource imported = Constants.RESOURCE_DIALOGS[type](null);

			if (!Equals(imported, null))
			{
				CampaignFile project = editor.Document;

				project.Data.Resources.Add(imported);

				//TODO: Fix level sets page unselecting current object after assigning document
				editor.Document = project;

				UpdateDataStore();
			}
		}
	}
}