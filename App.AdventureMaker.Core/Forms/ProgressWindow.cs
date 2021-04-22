#pragma warning disable CS0649
using App.AdventureMaker.Core.Interfaces;
using App.AdventureMaker.Core.Tasks;
using Eto.Drawing;
using Eto.Forms;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace App.AdventureMaker.Core.Forms
{
	public class ProgressWindow : Dialog, IProgressData
	{
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
			get => progress.Value;
			set
			{
				progress.Value = value;
				Invalidate();
			}
		}

		public int Maximum
		{
			get => progress.MaxValue;
			set
			{
				progress.MaxValue = value;
				Invalidate();
			}
		}

		private readonly ProgressBar progress;
		private readonly Label status;

		public ProgressWindow()
		{
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
					new StackLayoutItem(progress = new ProgressBar(), false),
				}
			};


			WindowStyle = WindowStyle.Utility;
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

		public void NotifyCompleted()
		{
			CanClose = true;
			Close();
		}

		public bool Run(TaskBase task)
		{
			bool result = false;

			Thread thread = new Thread(() =>
			{
				ShowModal(null);
				result = task.Execute(this);
			})
			{
				IsBackground = true
			};

			thread.Start();

			while (thread.IsAlive)
			{
				//Task.Delay(1);
				Thread.Yield();
			}

			return result;
		}
	}
}
