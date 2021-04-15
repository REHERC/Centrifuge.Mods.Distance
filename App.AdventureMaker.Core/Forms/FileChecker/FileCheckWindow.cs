using Distance.AdventureMaker.Common.Enums;
using Distance.AdventureMaker.Common.Validation;
using Distance.AdventureMaker.Common.Validation.Validators;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.AdventureMaker.Core.Forms.FileChecker
{
	public class FileCheckWindow : Form
	{
		private static readonly Dictionary<StatusLevel, Func<Image>> STATUS_IMAGES = new Dictionary<StatusLevel, Func<Image>>()
		{
			{ StatusLevel.Info, () => Resources.GetImage("StatusInfo.png") },
			{ StatusLevel.Warning, () => Resources.GetImage("StatusWarning.png") },
			{ StatusLevel.Error, () => Resources.GetImage("StatusError.png") }
		};

		private readonly GridView messagesView;
		private readonly Label status;

		public FileCheckWindow(CampaignValidator validator)
		{
			MinimumSize = new Size(600, 400);

			messagesView = new GridView()
			{
				GridLines = GridLines.Horizontal,
				ShowHeader = true,

				DataStore = validator?.Messages.Cast<object>(),

				Columns =
				{
					new GridColumn()
					{
						HeaderText = "Severity",
						Editable = false,
						Sortable = true,
						Resizable = false,
						Width = 96,

						DataCell = new ImageTextCell()
						{
							TextBinding = Binding.Property<ValidationItem, string>(message => $"{message.status.ToString().ToUpper()}"),
							ImageBinding = Binding.Property<ValidationItem, Image>(message => STATUS_IMAGES[message.status]()),
							ImageInterpolation = ImageInterpolation.High,
							VerticalAlignment = VerticalAlignment.Center

							//TextAlignment = TextAlignment.Right BRUH
						}
					},
					new GridColumn()
					{
						HeaderText = "Message",
						Editable = false,
						Sortable = true,
						Resizable = true,
						Width = 512,

						DataCell = new TextBoxCell()
						{
							Binding = Binding.Property<ValidationItem, string>(message => $"{message.details}")
						}
					}
				}
			};

			Content = new StackLayout()
			{
				Style = "vertical",
				Spacing = 8,
				Items =
				{
					new StackLayoutItem(messagesView, true),
					new StackLayoutItem(status = new Label(), false)
				}
			};

			status.Text = $"Campaign validation: {validator.GetMessages(StatusLevel.Error).Length} Error(s) | {validator.GetMessages(StatusLevel.Warning).Length} Warning(s) | {validator.GetMessages(StatusLevel.Info).Length} Information(s) | {validator.Messages.Count} Total message(s)";
		}
	}
}
