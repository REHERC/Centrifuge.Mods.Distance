using Tools.External.Attributes;
using Tools.External.Forms;

namespace Tools.External.Tools
{
    //[Tool("Test tool")]
    public class TestTool : Tool
    {
        public override bool Run(StateProvider state)
        {
            var info = OpenWindow<LoggerWindow>().Rename("Output");

            info.WriteLine("Begin task...");

            const int length = 500000;

            state.SetMaxProgress(length);

            for (int i = 0; i <= length; i++)
            {
                state.SetProgress(i);
                state.SetStatus($"Task running... ({state.Progress}%)");
            }

            info.WriteLine("Task complete!");

            return true;
        }
    }
}
