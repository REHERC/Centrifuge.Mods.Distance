using System;
using System.Collections.Generic;

namespace CustomCar
{
    public static class ErrorList
    {
        private static readonly List<string> errors = new List<string>();

        public static void Add(string error)
        {
            errors.Add(error);
            Console.Out.WriteLine("Custom car - Error: " + error);
        }

        public static List<string> Get()
        {
            return errors;
        }

        public static void Clear()
        {
            errors.Clear();
        }

        public static bool HaveErrors()
        {
            return errors.Count > 0;
        }

        public static void Show()
        {
            if (errors.Count == 0)
            {
                return;
            }

            string error = "Can't load the cars correctly:" + errors.Count + " error(s)\n";
            for (int i = errors.Count - 1; i >= 0; i--)
            {
                error += errors[i] + "\n";
            }

            G.Sys.MenuPanelManager_.ShowError(error, "Custom cars error");
        }
    }
}