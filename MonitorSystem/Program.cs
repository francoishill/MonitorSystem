using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharedClasses;

namespace MonitorSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			AutoUpdatingForm.CheckForUpdates(delegate { Application.Exit(); });
            Application.Run(new MainForm());
        }
    }
}
