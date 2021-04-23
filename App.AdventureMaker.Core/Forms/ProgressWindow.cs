#pragma warning disable CS0649
using App.AdventureMaker.Core.Interfaces;
using App.AdventureMaker.Core.Tasks;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Forms
{
	public class ProgressWindow : Dialog, IProgressReporter
	{
		#region Properties
		private bool CanClose { get; set; }

		public string Status
		{
			get => status.Text;
			set
			{
				status.Text = value;
				Invalidate();
			}
		}

		public int Value
		{
			get => bar.Value;
			set
			{
				bar.Value = value;
				Invalidate();
			}
		}

		public int Maximum
		{
			get => bar.MaxValue;
			set
			{
				bar.MaxValue = value;
				Invalidate();
			}
		}
		#endregion

		private readonly ProgressBar bar;
		private readonly Label status;
		private readonly Progress progress;
		private readonly Control owner;

		public ProgressWindow(Control owner) : this()
		{
			this.owner = owner;
		}

		public ProgressWindow()
		{
			progress = new Progress();

			MinimumSize = new Size(384, 128);
			Size = MinimumSize;

			CanClose = false;

			Content = new StackLayout()
			{
				Style = "no-padding vertical",
				Padding = new Padding(12),
				Spacing = 12,

				Items =
				{
					new StackLayoutItem(status = new Label(), false),
					new StackLayoutItem(bar = new ProgressBar(), false),
				}
			};

			WindowStyle = WindowStyle.Utility;

			this.owner = null;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!CanClose)
			{
				e.Cancel = true;
				return;
			}

			base.OnClosing(e);
		}

		public async Task RunTask(TaskBase task)
		{
			progress.PropertyChanged += OnProgressDataChanged;

			Task showDialogTask = ShowModalAsync(owner);

			Task callbackTask = task.Execute(progress).ContinueWith((_) =>
			{
				CanClose = true;
				Application.Instance.Invoke(Close);
			});

			await callbackTask.ConfigureAwait(false);
			await showDialogTask.ConfigureAwait(false);

			//await Task.WhenAll(showDialogTask, callbackTask).ConfigureAwait(false);

			progress.PropertyChanged -= OnProgressDataChanged;
		}

		protected void OnProgressDataChanged(object sender, EventArgs e)
		{
			Application.Instance.Invoke(() =>
			{
				Title = progress.Title;
				status.Text = progress.Status;
				bar.MaxValue = progress.Maximum;
				bar.Value = progress.Value;
			});
		}

		Task IProgressReporter.RunTask(TaskBase task)
		{
			return RunTask(task);
		}

		void IProgressReporter.OnProgressDataChanged(object sender, EventArgs e)
		{
			OnProgressDataChanged(sender, e);
		}
	}
}
