using App.AdventureMaker.Core.Forms;
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

		private readonly GridView<CampaignResource> resourceGrid;
		private readonly GridColumn resourceTypeColumn;
		//private readonly GridColumn resourceIdColumn;
		private readonly GridColumn resourceFileColumn;
		private readonly GridColumn resourceDependenciesColumn;

		private readonly StackLayout actionsLayout;
		private readonly Button addResourceButton;
		private readonly ButtonMenuItem addResourceMenuButton;
		private readonly Button editResourceButton;
		private readonly ButtonMenuItem editResourceMenuButton;
		private readonly Button removeResourceButton;
		private readonly ButtonMenuItem removeResourceMenuButton;

		public ResourcesPage(IEditor<CampaignFile> editor_)
		{
			editor = editor_;

			Style = "no-padding vertical";
			Spacing = 4;

			Items.Add(new StackLayoutItem(actionsLayout = new StackLayout()
			{
				Style = "no-padding horizontal",
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

			Items.Add(new StackLayoutItem(resourceGrid = new GridView<CampaignResource>()
			{
				DataStore = (collection = new ObservableCollection<CampaignResource>()),
				GridLines = GridLines.Both,

				ContextMenu = new ContextMenu()
				{
					Items =
					{
						(addResourceMenuButton = new ButtonMenuItem(OnAddResource)
						{
							Text = "Add new",
							Image = Resources.GetIcon("AddGreen.ico")
						}),
						(editResourceMenuButton = new ButtonMenuItem(OnEditResource)
						{
							Text = "Edit",
							Image = Resources.GetIcon("Pencil.ico")
						}),
						(removeResourceMenuButton = new ButtonMenuItem(OnRemoveResource)
						{
							Text = "Delete",
							Image = Resources.GetIcon("CloseRed.ico")
						})
					}
				},

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
					/*(resourceIdColumn = new GridColumn()
					{
						HeaderText = "Unique ID",
						Width = 230,
						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<CampaignResource, string>(res => $"{res.guid}"),
						},
						Editable = false,
						Resizable = true
					}),*/
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
			resourceGrid.CellDoubleClick += OnEditResource;
		}

		#region Event Handlers
		private void OnSelectedResourceChanged(object sender, EventArgs e)
		{
			int index = resourceGrid.SelectedRow;

			editResourceButton.Enabled
			= removeResourceButton.Enabled
			= editResourceMenuButton.Enabled
			= removeResourceMenuButton.Enabled
			= index >= 0;
		}

		private void OnAddResource(object sender, EventArgs e)
		{
			CampaignResource res = new AddResourceWindow().ShowModal();

			if (!Equals(res, null))
			{
				res.guid = Guid.NewGuid().ToString();

				collection.Add(res);

				SortItems();
				editor.Modified = true;
			}
		}

		private void OnEditResource(object sender, EventArgs e)
		{
			int row = resourceGrid.SelectedRow;

			CampaignResource res = collection[row];

			CampaignResource edited = Constants.RESOURCE_DIALOGS[res.resource_type](res);

			if (!Equals(edited, null))
			{
				collection[row] = edited;

				SortItems();

				editor.Modified = true;
			}
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
		private void SortItems()
		{
			var modified = editor.Modified;

			resourceGrid.SuspendLayout();

			var items = collection.OrderBy(res => res.resource_type).ThenBy(res => res.file).ToArray();

			collection.Clear();

			foreach (var item in items)
			{
				collection.Add(item);
			}

			resourceGrid.ResumeLayout();

			editor.Modified = modified;
		}

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

			project.Data.Resources = collection.ToList();
		}

		void ISaveLoad<CampaignFile>.LoadData(CampaignFile project, bool resetUI)
		{
			resourceGrid.UnselectAll();
			resourceGrid.SelectedRow = -1;

			collection.Clear();

			foreach (CampaignResource res in project.Data.Resources)
			{
				collection.Add(res);
			}

			SortItems();

			UpdateBindings();
			Invalidate();
		}
		#endregion
	}
}