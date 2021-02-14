using App.AdventureMaker.Core.Controls;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Forms.ResourceDialogs
{
	public abstract class ResourceDialogBase<T> : Dialog<T> where T : class, new()
	{
		protected T Data { get; set; }

		private readonly Button okButton;
		private readonly Button cancelButton;
		private readonly StackLayout stackLayout;
		protected readonly PropertiesListBase properties;

		protected ResourceDialogBase(T data)
		{
			Data = data.CloneObject() ?? new T();

			Size = MinimumSize = new Size(360, 180);

			Content = (stackLayout = new StackLayout()
			{
				Orientation = Orientation.Vertical,
				HorizontalContentAlignment = HorizontalAlignment.Stretch,
				Spacing = 16,

				Items =
				{
					new StackLayoutItem(new Scrollable()
					{
						Content = (properties = new PropertiesListBase())
					}, true),
					new StackLayoutItem(new StackLayout()
					{
						Orientation = Orientation.Horizontal,
						VerticalContentAlignment = VerticalAlignment.Stretch,
						Spacing = 4,

						Items =
						{
							null,
							(okButton = new Button(Confirm)
							{
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16)
							}),
							(cancelButton = new Button(Cancel)
							{
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)

							})
						}
					}, false)
				}
			});

			Title = string.Format("{0} {1}", Equals(data, null) ? "Add new" : "Edit", typeof(T).Name.ToLower());

			DefaultButton = okButton;
			AbortButton = cancelButton;

			InitializeComponent();

			properties.CompleteRows();
		}

		private void Confirm(object sender, EventArgs e)
		{
			Close(Data);
		}

		private void Cancel(object sender, EventArgs e)
		{
			Close(null);
		}

		protected abstract void InitializeComponent();
	}
}
