﻿using App.AdventureMaker.Core.Controls;
using Eto.Drawing;
using Eto.Forms;
using System;

namespace App.AdventureMaker.Core.Forms.ResourceDialogs
{
	public abstract class ResourceDialogBase<T> : Dialog<T> where T : class, new()
	{
		protected T Data { get; set; }

		private readonly StackLayout stackLayout;
		protected readonly PropertiesListBase properties;

		protected ResourceDialogBase(T data)
		{
			Data = data.CloneObject() ?? new T();
			Size = MinimumSize = new Size(360, 180);
			Resizable = false;

			Content = (stackLayout = new StackLayout()
			{
				Style = "vertical",
				Spacing = 16,

				Items =
				{
					new StackLayoutItem(new Scrollable()
					{
						Content = properties = new PropertiesListBase()
					}, true),
					new StackLayoutItem(new StackLayout()
					{
						Style = "horizontal",
						Spacing = 4,

						Items =
						{
							null,
							(DefaultButton = new Button(Confirm)
							{
								Text = "OK",
								Image = Resources.GetIcon("CheckGreen.ico", 16)
							}),
							(AbortButton = new Button(Cancel)
							{
								Text = "Cancel",
								Image = Resources.GetIcon("CloseRed.ico", 16)

							})
						}
					}, false)
				}
			});

			Title = string.Format("{0} {1}", Equals(data, null) ? "Add new" : "Edit", typeof(T).Name.ToLower());

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
