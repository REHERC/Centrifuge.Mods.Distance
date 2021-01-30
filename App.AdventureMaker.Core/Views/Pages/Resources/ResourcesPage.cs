using App.AdventureMaker.Core.Interfaces;
using Distance.AdventureMaker.Common.Models;
using Distance.AdventureMaker.Common.Models.Resources;
using Eto.Forms;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace App.AdventureMaker.Core.Views
{
	public class ResourcesPage : GridView, ISaveLoad<CampaignFile>
	{
		private readonly ObservableCollection<CampaignResource> collection;

		private readonly GridColumn resourceTypeColumn;
		private readonly GridColumn resourceIdColumn;
		private readonly GridColumn resourceFileColumn;
		private readonly GridColumn resourceDependenciesColumn;

		public ResourcesPage(IEditor<CampaignFile> editor)
		{
			collection = new ObservableCollection<CampaignResource>();

			DataStore = collection;

			GridLines = GridLines.Both;


			//Columns.Add(new GridColumn() { Resizable = true, Width = 100, HeaderText = "Resource type" });
			//Columns.Add(new GridColumn() { Resizable = true, Width = 250, HeaderText = "File path" });
			//Columns.Add(new GridColumn() { Resizable = true, Width = 250, HeaderText = "Dependencies" });


			Columns.Add(resourceTypeColumn = new GridColumn()
			{
				HeaderText = "Ressource Type",
				Width = 100,
				DataCell = new TextBoxCell()
				{
					Binding = Binding.Property<CampaignResource, string>(res => $"{res.resource_type}"),
				},
				Editable = false,
				Resizable = false
			});

			Columns.Add(resourceIdColumn = new GridColumn()
			{
				HeaderText = "Unique ID",
				Width = 240,
				DataCell = new TextBoxCell()
				{
					Binding = Binding.Property<CampaignResource, string>(res => $"{res.guid}"),
				},
				Editable = false,
				Resizable = false
			});

			Columns.Add(resourceFileColumn = new GridColumn()
			{
				HeaderText = "Main File",
				DataCell = new TextBoxCell()
				{
					Binding = Binding.Property<CampaignResource, string>(res => $"{res.file}"),
				},
				Editable = false,
				Resizable = false
			});

			Columns.Add(resourceDependenciesColumn = new GridColumn()
			{
				HeaderText = "Dependencies",
				Width = 128,
				DataCell = new TextBoxCell()
				{
					Binding = Binding.Property<CampaignResource, string>(res => $"{(res.dependencies_count == 0 ? "none" : $"{res.dependencies_count} other resource(s)")}"),
				},
				Editable = false,
				Resizable = false
			});
		}

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
			collection.Clear();

			foreach (CampaignResource res in project.data.resources)
			{
				collection.Add(res);
			}

			UpdateBindings();
			Invalidate();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			resourceFileColumn.Width
			= Size.Width 
			- resourceTypeColumn.Width 
			- resourceIdColumn.Width 
			- resourceDependenciesColumn.Width 
			- 4;
		}
	}
}