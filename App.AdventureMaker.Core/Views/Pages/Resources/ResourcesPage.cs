using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Forms;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace App.AdventureMaker.Core.Views
{
	public class ResourcesPage : StackLayout, ISaveLoad<CampaignFile>
	{
		private readonly ObservableCollection<CampaignResource> collection;

		private readonly IEditor<CampaignFile> editor;

		private readonly GridView resourceGrid;
		private readonly GridColumn resourceTypeColumn;
		private readonly GridColumn resourceIdColumn;
		private readonly GridColumn resourceFileColumn;
		private readonly GridColumn resourceDependenciesColumn;

		private readonly StackLayout actionsLayout;
		private readonly Button addResourceButton;
		private readonly Button editResourceButton;
		private readonly Button removeResourceButton;

		public ResourcesPage(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			Style = "no-padding";
			Orientation = Orientation.Vertical;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;
			Spacing = 4;

			Items.Add(new StackLayoutItem(actionsLayout = new StackLayout()
			{
				Style = "no-padding",
				Orientation = Orientation.Horizontal,
				VerticalContentAlignment = VerticalAlignment.Stretch,
				//Height = 32,

				Spacing = 4,

				Items =
				{
					(addResourceButton = new Button(OnAddResource)
					{
						Image = Resources.GetIcon("AddGreen.ico", 16),
						Text = "Add new resource"
					}),
					//null,
					(editResourceButton = new Button(OnEditResource)
					{
						Image = Resources.GetIcon("Pencil.ico", 16),
						Text = "Edit resource properties"
					}),
					(removeResourceButton = new Button(OnRemoveResource)
					{
						Image = Resources.GetIcon("CloseRed.ico", 16),
						Text = "Delete resource"
					})
				}
			}, false));

			Items.Add(new StackLayoutItem(resourceGrid = new GridView()
			{ 
				DataStore = (collection = new ObservableCollection<CampaignResource>()),
				GridLines = GridLines.Both,

				Columns =
				{
					(resourceTypeColumn = new GridColumn()
					{
						HeaderText = "Ressource Type",
						Width = 128,
						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignResource, string>(res => $"{res.resource_type}"),
						},
						Editable = false,
						Resizable = true
					}),
					(resourceIdColumn = new GridColumn()
					{
						HeaderText = "Unique ID",
						Width = 230,
						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignResource, string>(res => $"{res.guid}"),
						},
						Editable = false,
						Resizable = true
					}),
					(resourceFileColumn = new GridColumn()
					{
						HeaderText = "Main File",
						Width = 250,
						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignResource, string>(res => $"{res.file}"),
						},
						Editable = false,
						Resizable = true
					}),
					(resourceDependenciesColumn = new GridColumn()
					{
						HeaderText = "Dependencies",
						Width = 120,
						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignResource, string>(res => $"{(res.dependencies_count == 0 ? "none" : $"{res.dependencies_count} other resource(s)")}"),
						},
						Editable = false,
						Resizable = true
					})
				}
			}, true));

			resourceGrid.SelectionChanged += OnSelectedResourceChanged;
		}

		#region Event Handlers
		private void OnSelectedResourceChanged(object sender, EventArgs e)
		{
			int index = resourceGrid.SelectedRow;
			editResourceButton.Enabled = removeResourceButton.Enabled = index >= 0;
		}

		private void OnAddResource(object sender, EventArgs e)
		{

		}

		private void OnEditResource(object sender, EventArgs e)
		{

		}

		private void OnRemoveResource(object sender, EventArgs e)
		{
			if (Messages.RemoveResource() == DialogResult.Yes)
			{
				DeleteCurrentResourceEntry();
			}
		}
		#endregion

		#region Action Methods
		private void DeleteCurrentResourceEntry()
		{
			collection.RemoveAt(resourceGrid.SelectedRow);

			editor.Modified = true;
		}
		#endregion

		#region Save / Load
		void ISaveLoad<CampaignFile>.SaveData(CampaignFile project)
		{
			/*
			project.data.resources.Add(new CampaignResource.Texture() { file = "logo.png" });
			project.data.resources.Add(new CampaignResource.Level() { file = "file.bytes", thumbnail = "level.bytes.png" });
			project.data.resources.Add(new CampaignResource.Level() { file = "placeholder", thumbnail = "sample text" });
			*/

			project.data.resources = collection.ToList();
		}

		void ISaveLoad<CampaignFile>.LoadData(CampaignFile project)
		{
			resourceGrid.UnselectAll();
			resourceGrid.SelectedRow = -1;

			collection.Clear();

			foreach (CampaignResource res in project.data.resources)
			{
				collection.Add(res);
			}

			UpdateBindings();
			Invalidate();
		}
		#endregion
	}
}