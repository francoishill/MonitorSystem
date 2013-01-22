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

			/*for (int i = 0; i < 5; i++)
				CustomBalloonTipwpf.ShowCustomBalloonTip(
					"Title"+ i,
					"Message" + i,
					TimeSpan.FromSeconds(0),
					CustomBalloonTipwpf.IconTypes.Information);
			Application.Run(new Form());
			return;*/

			MainForm mainform = new MainForm();
			AutoUpdating.CheckForUpdates_ExceptionHandler(delegate
			{
				ThreadingInterop.UpdateGuiFromThread(mainform, () => mainform.Text += " (up to date version " + AutoUpdating.GetThisAppVersionString() + ")");
			});
			/*AutoUpdating.CheckForUpdates(
				//AutoUpdatingForm.CheckForUpdates(
				//exitApplicationAction: () => Application.Exit(),
				ActionIfUptoDate_Versionstring: uptodateversion => ThreadingInterop.UpdateGuiFromThread(mainform, () => mainform.Text += " (up to date version " + uptodateversion + ")"));//,
				//ActionIfUnableToCheckForUpdates: errmsg => ThreadingInterop.UpdateGuiFromThread(mainform, () => mainform.Text += " (" + errmsg + ")"));*/

            Application.Run(mainform);
        }
    }
}
