#pragma warning disable IDE1006
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distance.ExternalModdingTools.Forms
{
    public partial class MainWindow : BaseForm
    {
        private MdiLayout layout = MdiLayout.ArrangeIcons;

        public MainWindow()
        {
            EnableDoubleBuffering();
            InitializeComponent();
        }

        public T OpenWindow<T>(T window) where T : Form
        {
            window.MdiParent = this;
            window.Show();

            window.FormClosed += (s, e) =>
            {
                ForceLayout();
            };

            window.Move += (s, e) =>
            {
                ForceLayout();
            };

            window.Resize += (s, e) =>
            {
                ForceLayout();
            };
            SetBorders(window);
            ForceLayout();
            return window;
        }

        public T OpenWindow<T>() where T : Form, new()
        {
            return OpenWindow(new T());
        }

        private void Reset()
        {
            Progress.Value = 0;
            Status.Text = "No task running...";
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            foreach (Tool tool in Program.ToolManager.Tools)
            {
                string name = Program.ToolManager.GetToolName(tool);

                ToolStripMenuItem button = new ToolStripMenuItem(name)
                {
                    Tag = tool,
                };

                button.Click += (_sender, _e) =>
                {
                    ToolStripMenuItem btn = _sender as ToolStripMenuItem;
                    Tool t = button.Tag as Tool;

                    RunTool(t);
                };

                RunToolMenu.DropDownItems.Add(button);
            }

            Reset();

            //OpenWindow<LoggerWindow>();
        }

        private void RunTool(Tool tool)
        {
            StateProvider provider = new StateProvider(Progress, Status);
            provider.SetProgress(0);
            provider.SetMaxProgress(100);
            provider.SetStatus($"Executing tool \"{Program.ToolManager.GetToolName(tool)}\"...");

            SetToolStatus(false);

            tool.SetParent(this);

            if (tool.Prepare(provider))
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                TaskFactory factory = new TaskFactory(token);

                tool.SetCancelToken(source);

                factory.StartNew(() => {
                    try
                    {

                        tool.SetRunning(true);
                        tool.Run(provider);

                        if (tool.WasCanceled())
                        {
                            tool.CloseResources();
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        tool.SetRunning(false);
                    }
                });
            }
            else
            {
                Reset();
            }
            
            SetToolStatus(true);
        }

        private void SetToolStatus(bool state)
        {
            RunToolMenu.Enabled = state;
        }

        private void verticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayout(MdiLayout.TileVertical);
        }

        private void horizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayout(MdiLayout.TileHorizontal);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayout(MdiLayout.Cascade);
        }

        private void arrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLayout(MdiLayout.ArrangeIcons);
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            ForceLayout();
        }

        void SetLayout(MdiLayout value)
        {
            layout = value;
            ForceLayout();
        }

        void ForceLayout()
        {
            //SetBorders();
            LayoutMdi(layout);
        }

        private void windowFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetBorders();
        }

        private void SetBorders()
        {
            foreach (var child in MdiChildren)
            {
                SetBorders(child);
            }
        }

        private void SetBorders(Form frm)
        {
            SetBorders(frm, windowFrameToolStripMenuItem.Checked);
        }

        private void SetBorders(Form frm, bool value)
        {
            frm.FormBorderStyle = value ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;


            frm.MaximizeBox = value;
            frm.MinimizeBox = value;


            //frm.WindowState = value ? FormWindowState.Normal : FormWindowState.Maximized;
        }
    }
}
