using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools.External
{
	public class Tool
	{
		protected Forms.MainWindow parent_;
		protected CancellationTokenSource source;
		protected bool running_ = false;
		protected bool canceled_ = false;

		protected List<IDisposable> disposeResources_ = new List<IDisposable>();

		internal void SetParent(Forms.MainWindow parent)
		{
			parent_ = parent;
		}

		internal void SetRunning(bool value)
		{
			if (!running_)
			{
				canceled_ = false;
			}

			running_ = value;
		}

		public virtual bool Prepare(StateProvider state)
		{
			return true;
		}

		internal void CloseResources()
		{
			SetRunning(false);

			foreach (var obj in disposeResources_)
			{
				switch (obj)
				{
					case Form frm:
						frm.Hide();
						frm.Close();
						break;
				}

				obj.Dispose();
			}
		}

		internal void SetCancelToken(CancellationTokenSource source)
		{
			this.source = source;
		}

		public virtual bool Run(StateProvider state)
		{
			return true;
		}

		internal void Cancel()
		{
			canceled_ = true;
			source.Cancel();
		}

		public void Delay(int value)
		{
			Task.Delay(value).Wait();
		}

		public T OpenWindow<T>(T window) where T : Form
		{
			T result = null;

			parent_.Invoke((MethodInvoker)(() =>
			{
				result = parent_.OpenWindow(window);
				result.FormClosing += (s, e) =>
				{
					if (running_)
					{
						e.Cancel = true;
						Cancel();
						CloseResources();
					}
				};

				//result.WindowState = FormWindowState.Maximized;

				disposeResources_.Add(result);
			}));

			return result;
		}

		public T OpenWindow<T>() where T : Form, new()
		{
			return OpenWindow(new T());
		}

		public bool IsRunning()
		{
			return running_;
		}

		public bool WasCanceled()
		{
			return canceled_;
		}
	}
}
