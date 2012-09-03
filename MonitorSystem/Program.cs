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
			
			MainForm mainform = new MainForm();
			AutoUpdating.CheckForUpdates(
				//AutoUpdatingForm.CheckForUpdates(
				//exitApplicationAction: () => Application.Exit(),
				ActionIfUptoDate_Versionstring: uptodateversion => ThreadingInterop.UpdateGuiFromThread(mainform, () => mainform.Text += " (up to date version " + uptodateversion + ")"));//,
				//ActionIfUnableToCheckForUpdates: errmsg => ThreadingInterop.UpdateGuiFromThread(mainform, () => mainform.Text += " (" + errmsg + ")"));

            Application.Run(mainform);
        }
    }
}
