using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Sockets;

namespace MonitorSystem
{
	public partial class TransferDropWindow : Form
	{
		private const int WM_WINDOWPOSCHANGING = 0x0046;
		private const int WM_GETMINMAXINFO = 0x0024;

		NetworkInterop.TextFeedbackEventHandler textFeedbackEvent;
		NetworkInterop.ProgressChangedEventHandler progressChangedEvent;
		NotifyIcon mainNotifyIcon;

		public TransferDropWindow(ref NotifyIcon mainNotifyIconOfApplication)
		{
			InitializeComponent();
			mainNotifyIcon = mainNotifyIconOfApplication;
		}

		protected override void WndProc(ref Message m)
		{
			//if (m.Msg == WM_WINDOWPOSCHANGING)
			//{
			//	WindowPos windowPos = (WindowPos)m.GetLParam(typeof(WindowPos));
			//	// Make changes to windowPos
			//	// Then marshal the changes back to the message
			//	Marshal.StructureToPtr(windowPos, m.LParam, true);
			//}
			base.WndProc(ref m);
			// Make changes to WM_GETMINMAXINFO after it has been handled by the underlying
			// WndProc, so we only need to repopulate the minimum size constraints
			if (m.Msg == WM_GETMINMAXINFO)
			{
				//Need this to override the minimum heigth of the form enforced by the operating system:
				//http://stackoverflow.com/questions/992352/overcome-os-imposed-windows-form-minimum-size-limit
				MinMaxInfo minMaxInfo = (MinMaxInfo)m.GetLParam(typeof(MinMaxInfo));
				minMaxInfo.ptMinTrackSize.x = this.MinimumSize.Width;
				minMaxInfo.ptMinTrackSize.y = this.MinimumSize.Height;
				Marshal.StructureToPtr(minMaxInfo, m.LParam, true);
			}
		}

		private void TransferDropWindow_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
			{
				e.Effect = DragDropEffects.All;
			}
		}

		private void TransferDropWindow_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			if (files.Length == 0) MessageBox.Show(this, "No files found");
			else if (files.Length > 1) MessageBox.Show(this, "Only one file at a time");
			else if (!File.Exists(files[0])) MessageBox.Show(this, "File not found: " + files[0]);
			else
			{
				Socket socket;

				textFeedbackEvent += (snder, evtargs) =>
				{
					mainNotifyIcon.ShowBalloonTip(3000, "Textfeedback file transfer", evtargs.FeedbackText, ToolTipIcon.Info);
				};
				progressChangedEvent += (snder, evtargs) =>
				{
					ThreadingInterop.UpdateGuiFromThread(this, () =>
					{
						int currentValue = evtargs.CurrentValue;
						int maxValue = evtargs.MaximumValue;
						if (currentValue == 0 && maxValue == 100)
						{
							this.progressBar1.Maximum = maxValue;
							this.progressBar1.Value = currentValue;
							this.progressBar1.Visible = false;
							this.Opacity = 0.5;
						}
						else if (currentValue > 0 && maxValue > 0)
						{
							this.progressBar1.Visible = true;
							this.progressBar1.Maximum = maxValue;
							this.progressBar1.Value = currentValue;
							this.Opacity = 1;
						}
						Application.DoEvents();
					});
				};
				this.progressBar1.Visible = true;
				this.Opacity = 1;
				Application.DoEvents();
				ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
				{
					NetworkInterop.TransferFile_FileStream(
						files[0],
						out socket,

						ipAddress: null,
						//NetworkInterop.GetIPAddressFromString("fjh.dyndns.org"),

						TextFeedbackEvent: textFeedbackEvent,
						ProgressChangedEvent: progressChangedEvent);
				});
			}
		}

		struct WindowPos
		{
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int width;
			public int height;
			public uint flags;
		}

		struct POINT
		{
			public int x;
			public int y;
		}

		struct MinMaxInfo
		{
			public POINT ptReserved;
			public POINT ptMaxSize;
			public POINT ptMaxPosition;
			public POINT ptMinTrackSize;
			public POINT ptMaxTrackSize;
		}
	}
}
