using System.Windows.Forms;
using Tools.External.Extensions;

namespace Tools.External.Forms
{
    public partial class LoggerWindow : BaseForm
    {
        //private StringBuilder output;

        public LoggerWindow()
        {
            //output = new StringBuilder();
            EnableDoubleBuffering();
            InitializeComponent();
            Box.SetDoubleBuffered(true);
        }

        public static LoggerWindow Create(string name = "Output")
        {
            return new LoggerWindow().Rename(name);
        }

        public LoggerWindow Rename(string name = "Output")
        {
            Text = name;
            return this;
        }

        public void Clear()
        {
            //output = new StringBuilder();
            SetText(string.Empty);
        }

        public void SetText(string value)
        {
            Box.Invoke((MethodInvoker)delegate ()
            {
                Box.SuspendLayout();
                Box.Text = value;
            });
            ScrollToEnd();
            Box.Invoke((MethodInvoker)delegate ()
            {
                Box.ResumeLayout();
            });
        }

        public void WriteLine(string line)
        {
            //output.AppendLine(line);
            //SetText(output.ToString());

            Box.Invoke((MethodInvoker)delegate ()
            {
                string value = $"{(Box.Text.Length > 0 ? "\n" : string.Empty)}{line}".Replace("\n", System.Environment.NewLine);
                Box.AppendText(value);
            });
        }

        public void ScrollToEnd()
        {
            Box.Invoke((MethodInvoker)delegate ()
            {
                Box.SelectionStart = Box.Text.Length;
                Box.SelectionLength = 0;
                Box.ScrollToCaret();
            });
        }
    }
}
