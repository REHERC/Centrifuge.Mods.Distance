using System;
using System.Windows.Forms;

namespace Tools.External
{
    public struct StateProvider
    {
        private readonly ToolStripProgressBar progress_;
        private readonly ToolStripStatusLabel label_;

        public StateProvider(ToolStripProgressBar progress, ToolStripStatusLabel label)
        {
            progress_ = progress;
            label_ = label;
        }

        public string Progress => GetProgress(progress_.Value);

        public string GetProgress(float value)
        {
            float current = Math.Min(Math.Max(value, 0), progress_.Maximum);

            const int decimals = 2;
            double shift = Math.Pow(10, decimals);

            float range = (float)(Math.Round(current / progress_.Maximum * 100 * shift) / shift);

            string number = range.ToString();

            string integer = Math.Floor(range).ToString();
            string floating = number.Substring(integer.Length).Replace(".", "").Replace(",", "");


            while (integer.Length < 3)
            {
                integer = $"0{integer}";
            }


            while (floating.Length < decimals)
            {
                floating = $"{floating}0";
            }

            return $"{integer}.{floating}";
        }

        public void SetStatus(string value)
        {
            label_.Text = value;
        }

        public void SetMaxProgress(int value)
        {
            progress_.Maximum = Math.Max(value, 0);
        }

        public void SetProgress(int value)
        {
            progress_.Value = Math.Max(Math.Min(value, progress_.Maximum), 0);
        }

        public void AddProgress(int increment)
        {
            SetProgress(progress_.Value + increment);
        }

        public void IncreaseProgress()
        {
            AddProgress(1);
        }

        public void ResetProgress()
        {
            SetProgress(0);
        }
    }
}
