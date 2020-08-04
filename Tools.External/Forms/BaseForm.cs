using System.Windows.Forms;

namespace Tools.External.Forms
{
    public class BaseForm : Form
    {
        public void EnableDoubleBuffering()
        {
            if (!DesignMode)
            {
                SetStyle(ControlStyles.DoubleBuffer |
                   ControlStyles.UserPaint |
                   ControlStyles.AllPaintingInWmPaint,
                   true);
                UpdateStyles();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (!DesignMode)
                {
                    cp.ExStyle |= 0x02000000;
                }

                return cp;
            }
        }
    }
}
