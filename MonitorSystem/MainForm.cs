using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SharedClasses;
using CustomBalloonTipClass = SharedClasses.CustomBalloonTipwpf.CustomBalloonTipClass;

namespace MonitorSystem
{
	public partial class MainForm : Form
	{
		private static string ThisAppName = "MonitorSystem";
		//public static readonly string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "FJH" + "\\" + ThisAppName;
		public static string SavedListFileName = WindowsInterop.LocalAppDataPath + "\\EmailAndPasswordList.fjset";
		public static string LastAutobackupStateFileName = WindowsInterop.LocalAppDataPath + "\\LastAutobackupState.fjset";

		private TransferDropWindow transferDropWindow = null;

		////private const string ServerAddress = "http://localhost";
		////private const string ServerAddress = "https://fjh.co.za";
		//private const string ServerAddress = "http://firepuma.com";
		////private const string doWorkAddress = ServerAddress + "/other/codeigniter/index.php/desktopapp";
		//private const string doWorkAddress = ServerAddress + "/desktopapp";
		//private string Username = "f";
		//private string Password = "f";

		List<string> currentEmailPasswordAndRegexList = new List<string>();

		//Timer timer = new Timer();
		private const string MySQLdateformat = "yyyy-MM-dd HH:mm:ss";
		DateTime mindate = new DateTime(1800, 1, 1, 0, 0, 0);

		public static string MonitoredAutoBackupPath = "";

		//MouseHooks.MouseHook mouseHook;

		public static string AutoBackupDir
		{
			get
			{
				if (Directory.Exists(@"C:\ProgramData\GLS"))//\ReportSQLqueries"))
					return @"C:\ProgramData\GLS";//\ReportSQLqueries";
				else if (Directory.Exists(@"C:\Francois\other\Test\SqlFilesAutobackup"))
					return @"C:\Francois\other\Test\SqlFilesAutobackup";
				else return @"c:\windows\system32";
			}
		}

		//List<Form> OverlayChildrenList = new List<Form>()
		//{
		//    new Form(),
		//    new Form()
		//};

		//OverlayForm overlayForm = new OverlayForm();

		public MainForm()
		{
			try { Win32Api._fpreset(); }
			catch { }

			InitializeComponent();
			WindowMessagesInterop.InitializeClientMessages();//"MonitorSystem");
			//StartPipeClient();

			RefreshSmallTodoitems();

			fileSystemWatcher_SqlFiles.Path = AutoBackupDir;

			if (!Directory.Exists(WindowsInterop.LocalAppDataPath)) Directory.CreateDirectory(WindowsInterop.LocalAppDataPath);

			//timer.Interval = 10000;
			//timer.Start();
			//timer.Tick += new EventHandler(timer_Tick);

			notifyIcon1.BalloonTipClicked += new EventHandler(notifyIcon1_BalloonTipClicked);

			/*if (!System.Diagnostics.Debugger.IsAttached && Environment.GetCommandLineArgs()[0].ToUpper().Contains("Apps\\2.0".ToUpper()))
			{
				Microsoft.Win32.RegistryKey regkeyRUN = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				regkeyRUN.SetValue(ThisAppName, "\"" + System.Windows.Forms.Application.ExecutablePath + "\"", Microsoft.Win32.RegistryValueKind.String);
			}*/

			//RegisterSnarl();
			//NotifySnarl("Halo", "How are you");

			RefreshRegexList();

			//SnarlNetworkProtocol.SNP snarl = new SnarlNetworkProtocol.SNP();
			//snarl.register("127.0.0.1", 8000, "MonitorSystemCS");
			//snarl.addClass("127.0.0.1", 8000, "MonitorSystemCS", "Test", "TestTitle");
			//snarl.notify("127.0.0.1", 8000, "MonitorSystemCS", "Test", "TestTitle", "Text", "1000");

			dateTimePickerDue.MinDate = mindate;
			dateTimePickerCreated.MinDate = mindate;

			//MonitoredFilesClass.RePopulateFilesAndLastModifiedTimesDictionary(@"C:\ProgramData\GLS\ReportSQLqueries", true);
			MonitoredAutoBackupPath = fileSystemWatcher_SqlFiles.Path;

			ContextMenu notifiIconContextMenu = PopulateNotifyIconContextMenu();

			//mouseHook = new MouseHooks.MouseHook();
			////mouseHook.MouseGestureEvent += (o, gest) => { if (gest.MouseGesture == Win32Api.MouseGestures.RL) UserMessages.ShowErrorMessage("Message"); };
			//mouseHook.MouseMoveEvent += delegate
			//{
			//  if (MousePosition.X < 5)
			//  {
			//    if (overlayForm.IsDisposed) overlayForm = new OverlayForm();
			//    if (!overlayForm.Visible)
			//    {
			//      overlayForm.ListOfChildForms = OverlayChildrenList;
			//      overlayForm.Show();
			//    }
			//  }
			//};
			//mouseHook.Start();
			//DONE TODO: Textbox does not get cleared when showing queued messages
			transferDropWindow = new TransferDropWindow(ref notifyIcon1, ref notifiIconContextMenu);
			Rectangle workingArea = Screen.FromPoint(new Point(0, 0)).WorkingArea;
			transferDropWindow.Location = new Point(workingArea.Left + (workingArea.Width - transferDropWindow.Width), workingArea.Top + (workingArea.Height - transferDropWindow.Height));
			transferDropWindow.Show();//null);
			transferDropWindow.BringToFront();

			//InfoOfTransferToClient i = new InfoOfTransferToClient(true, 3.5D, 17.5D, 10, 300);
			//Stream str = new MemoryStream();
			//SerializationInterop.SerializeCustom(i, str, false, SerializationInterop.SerializationFormat.Xml);
			//InfoOfTransferToClient i2 = (InfoOfTransferToClient)SerializationInterop.DeserializeCustom(str, new InfoOfTransferToClient(), true, SerializationInterop.SerializationFormat.Xml);
			//MessageBox.Show(i2.AverageBytesPerSecond.ToString());

			//myclass mc = new myclass("Francois", "Hill", 23.11);
			//Stream stream = new MemoryStream();
			//SerializationInterop.SerializeCustom(mc, stream, false);
			////MessageBox.Show(mc.ToString());
			////Stream stream2 = new MemoryStream();
			//myclass mc2 = (myclass)SerializationInterop.DeserializeCustom(stream, new myclass());
			//MessageBox.Show(mc2.Name + ", " + mc2.Surname + ", " + mc2.Age);

			//GenericSettings.EnsureAllSettingsAreInitialized();
		}

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			ApplicationRecoveryAndRestart.RegisterForRecoveryAndRestart(
				delegate//On crash
				{
					this.notifyIcon1.Visible = false;
					//File.WriteAllText(@"c:\francois\other\tmp.txt", DateTime.Now.ToString("HH:mm:ss"));
				},
				delegate//On successfull restart
				{
					//MessageBox.Show("Application successfully restarted from crash. No functionality incorporated yet.", "Restarted", MessageBoxButtons.OK, MessageBoxIcon.Information);
					if (Directory.Exists(ApplicationRecoveryAndRestart.CrashReportsDirectory))
					{
						MessageBox.Show("MonitorSystem successfully restarted from crash. See Crash report.", "Restarted successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
						Process.Start("explorer", "\"" + ApplicationRecoveryAndRestart.CrashReportsDirectory + "\"");
					}
					else MessageBox.Show("MonitorSystem successfully restarted from crash. Could not find Crash reports folder ("
						+ ApplicationRecoveryAndRestart.CrashReportsDirectory
						+ ").", "Successfully Restarted", MessageBoxButtons.OK, MessageBoxIcon.Information);
				},
				delegate//After 60 seconds, when restart ready
				{
					this.Invoke((Action)delegate
					{
						labelRecoveryAndRestartSafe.Visible = true;
						notifyIcon1.ShowBalloonTip(3000, "Recovery and Restart", "MonitorSystem is now Restart Safe", ToolTipIcon.Info);
					});
				});
		}

		//class myclass
		//{
		//	public string Name;
		//	public string Surname;
		//	public double Age;
		//	public myclass()
		//	{
		//	}
		//	public myclass(string NameIn, string SurnameIn, double AgeIn)
		//	{
		//		Name = NameIn;
		//		Surname = SurnameIn;
		//		Age = AgeIn;
		//	}
		//}

		private string SmallTodolistFilePath = SettingsInterop.GetFullFilePathInLocalAppdata(ThisAppName, "SmallTodolist.txt");
		private void RefreshSmallTodoitems()
		{
			queuedNotifications.Clear();
			List<string> todoItems = TextFilesInterop.GetLinesFromTextFile(SmallTodolistFilePath, false);
			foreach (string todoitem in todoItems)
				if (!todoitem.StartsWith("//"))
				{
					if (!queuedNotifications.ContainsKey(todoitem)) queuedNotifications.Add(todoitem, new QueuedNotificationClass("Todo item", todoitem));
					else UserMessages.ShowWarningMessage("Todo item already in list, duplicates not allowed: " + todoitem);
				}
		}

		private bool IsApplicationArestartedInstance()
		{
			return System.Environment.GetCommandLineArgs().Length > 1 && System.Environment.GetCommandLineArgs()[1] == "/restart";
		}

		private ContextMenu PopulateNotifyIconContextMenu()
		{
			MenuItem addSmallTodoItem = new MenuItem("Add &small todo item", delegate { AddSmallTodoItem(); });
			MenuItem refreshSmallTodoList = new MenuItem("Re&fresh small todo list", delegate { RefreshSmallTodoitems(); });
			MenuItem showAllSmallTodoItems = new MenuItem("Show &all small todo items", delegate { ShowSmallTodolist(); });
			MenuItem editOnlineTodoList = new MenuItem("Edit online &todo list", delegate { this.ShowForm(); });
			MenuItem addBackupDescriptionMenuItem = new MenuItem("Add backup &description", delegate { (new AddBackupDescription()).Show(); });
			MenuItem exitMenuItem = new MenuItem("E&xit", delegate { RequestApplicationQuit(); });
			MenuItem viewallbackupsMenuItem = new MenuItem("View &all backups", delegate { ViewAllBackupsNow(); });
			MenuItem showqueuedmessagesMenuItem = new MenuItem("Show &queued messages", delegate { ShowQueuedMessages(); });
			//MenuItem testcustomballoontipMenuItem = new MenuItem("Test &custom balloontip", delegate { CustomBalloonTip.ShowCustomBalloonTip("Hallo", "This is a custom balloon tip for 3 seconds...", 3000, CustomBalloonTip.IconTypes.Shield, delegate { PerformBalloonTipClick(); }); });
			MenuItem testWindowAnimations = new MenuItem("Test window &animations", delegate { (new TestAnimations()).Show(); });
			MenuItem testSpeech = new MenuItem("Test &speech", delegate { (new TestSpeech()).Show(); });
			MenuItem transferFileDropWindow = new MenuItem("Transfer Dropwindow", (sndr, evtargs) =>
				{
					bool newCheckedValue = !(sndr as MenuItem).Checked;
					(sndr as MenuItem).Checked = newCheckedValue;
					ToggleTransferDropWindowVisible(!newCheckedValue);
				}) { Checked = true };

			notifyIcon1.ContextMenu = new ContextMenu(new MenuItem[]
			{
				addSmallTodoItem,
				refreshSmallTodoList,
				showAllSmallTodoItems,
				new MenuItem("-"),
				editOnlineTodoList,
				addBackupDescriptionMenuItem,
				viewallbackupsMenuItem,
				new MenuItem("-"),
				showqueuedmessagesMenuItem,
				new MenuItem("-"),
				testWindowAnimations,
				testSpeech,
				transferFileDropWindow,
				new MenuItem("-"),
				exitMenuItem,
			});
			return notifyIcon1.ContextMenu;
		}

		private void ToggleTransferDropWindowVisible(bool newVisibility)
		{
			if (newVisibility) transferDropWindow.Hide();
			else transferDropWindow.Show();
		}

		private void AddSmallTodoItem()
		{
			string newTodoItem = InputBoxWPF.Prompt("Enter new todo item:");
			if (newTodoItem != null && newTodoItem.Trim() != "")
			{
				List<string> todoItemlist = TextFilesInterop.GetLinesFromTextFile(SmallTodolistFilePath, false);
				todoItemlist.Add(newTodoItem);
				TextFilesInterop.WriteLinesToTextFile(SmallTodolistFilePath, todoItemlist);
				RefreshSmallTodoitems();
			}
		}

		private void ReadLastQueuedStatusIfFileExist()
		{
			if (File.Exists(LastAutobackupStateFileName))
			{
				bool success = true;
				using (StreamReader sr = new StreamReader(LastAutobackupStateFileName))
				{
					while (!sr.EndOfStream)
					{
						string line = sr.ReadLine();
						string fullPath = line.Split('|')[0];
						DateTime lastWrite;
						if (!DateTime.TryParseExact(line.Split('|')[1], FileChangedDetails.SavetofileDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out lastWrite))
						{
							UserMessages.ShowErrorMessage("Could not get date from pipesplit 1: " + line);
							success = false;
							continue;
						}
						FileChangedDetails.QueueStatusEnum queueStatus;
						if (!Enum.TryParse(line.Split('|')[2], true, out queueStatus))
						{
							UserMessages.ShowErrorMessage("Could not get QueueStatus from pipesplit 2: " + line);
							success = false;
							continue;
						}
						if (!QueuedFileChanges.ContainsKey(fullPath)) QueuedFileChanges.Add(fullPath, new Dictionary<DateTime, FileChangedDetails>());
						FileInfo fi = new FileInfo(fullPath);
						lastWrite = new DateTime(lastWrite.Year, lastWrite.Month, lastWrite.Day, lastWrite.Hour, lastWrite.Minute, lastWrite.Second);
						if (!QueuedFileChanges[fullPath].ContainsKey(lastWrite))
						{
							FileChangedDetails fcd = new FileChangedDetails(lastWrite, fullPath, null);
							fcd.QueueStatus = queueStatus;
							QueuedFileChanges[fullPath].Add(lastWrite, fcd);
						}
					}
				}
				if (success)
					File.Delete(LastAutobackupStateFileName);
				ShowQueuedMessages();
			}
		}

		private void RefreshRegexList()
		{
			List<string> tmpList = new List<string>();
			tmpList = TextFilesInterop.GetLinesFromTextFile(SavedListFileName, false);
			if (tmpList.Count > 0) currentEmailPasswordAndRegexList = tmpList;
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			this.ShowInTaskbar = true;
			this.WindowState = FormWindowState.Minimized;

			//InitializeHooks(true, true);
			//notifyIcon1.ShowBalloonTip(3000, "Hooks disabled", "Hooks were disabled in code", ToolTipIcon.Info);
			if (!Win32Api.RegisterHotKey(this.Handle, Win32Api.Hotkey1, Win32Api.MOD_WIN, (int)Keys.Q)) UserMessages.ShowWarningMessage("MonitorSystem could not register hotkey WinKey + Q");

			ReadLastQueuedStatusIfFileExist();

			StylingInterop.SetTreeviewVistaStyle(treeViewTodolist);

			if (Environment.CommandLine.ToLower().Contains(@"documents\visual studio 2010\")) labelTestCrash.Visible = true;
		}

		/*NamedPipesInterop.NamedPipeClient pipeclient;
		private void StartPipeClient()
		{
			pipeclient = NamedPipesInterop.NamedPipeClient.StartNewPipeClient(
			ActionOnError: (e) => { Console.WriteLine("Error occured: " + e.GetException().Message); },
			ActionOnMessageReceived: (m) =>
			{
				if (m.MessageType == PipeMessageTypes.AcknowledgeClientRegistration)
					Console.WriteLine("Client successfully registered.");
				else
				{
					if (m.MessageType == PipeMessageTypes.Show)
					{
						this.Show();
						if (this.WindowState == FormWindowState.Minimized)
							this.WindowState = FormWindowState.Normal;
						bool tmptopmost = this.TopMost;
						this.TopMost = true;
						Application.DoEvents();
						this.TopMost = tmptopmost;
						this.Activate();
					}
					else if (m.MessageType == PipeMessageTypes.Hide)
					{
						this.WindowState = FormWindowState.Minimized;
					}
					else if (m.MessageType == PipeMessageTypes.Close)
					{
						ForceClosing = true;
						this.notifyIcon1.Visible = false;
						if (this.InvokeRequired)
							this.Invoke((Action)delegate { RequestApplicationQuit(); });
						else
							RequestApplicationQuit();
					}
				}
			});
		}*/

		protected override void WndProc(ref Message m)
		{
			WindowMessagesInterop.MessageTypes mt;
			WindowMessagesInterop.ClientHandleMessage(m.Msg, m.WParam, m.LParam, out mt);
			if (mt == WindowMessagesInterop.MessageTypes.Show)
			{
				this.Show();
				if (this.WindowState == FormWindowState.Minimized)
					this.WindowState = FormWindowState.Normal;
				bool tmptopmost = this.TopMost;
				this.TopMost = true;
				Application.DoEvents();
				this.TopMost = tmptopmost;
				this.Activate();
			}
			else if (mt == WindowMessagesInterop.MessageTypes.Close)
			{
				ForceClosing = true;
				this.Close();
			}
			else if (mt == WindowMessagesInterop.MessageTypes.Hide)
			{
				this.WindowState = FormWindowState.Minimized;
			}
			else if (m.Msg == Win32Api.WM_HOTKEY)
			{
				if (m.WParam == new IntPtr(Win32Api.Hotkey1))
					ShowQueuedMessages();
			}
			base.WndProc(ref m);
		}

		UserActivityHook actHook;
		private void InitializeHooks(bool InstallMouseHook, bool InstallKeyboardHook)
		{
			actHook = new UserActivityHook(InstallMouseHook, InstallKeyboardHook);
			actHook.OnMouseActivity += new UserActivityHook.MoreMouseEventHandler(actHook_OnMouseActivity);
			actHook.KeyDown += new KeyEventHandler(actHook_KeyDown);
			actHook.KeyPress += new KeyPressEventHandler(actHook_KeyPress);
			actHook.KeyUp += new KeyEventHandler(actHook_KeyUp);
		}

		enum BalloonTipClickActionEnum { None, CopyTagToClipboard };
		BalloonTipClickActionEnum BalloonTipClickAction = BalloonTipClickActionEnum.None;
		private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
		{
			switch (BalloonTipClickAction)
			{
				case BalloonTipClickActionEnum.None: break;

				case BalloonTipClickActionEnum.CopyTagToClipboard:
					Clipboard.SetText(notifyIcon1.Tag == null ? "" : notifyIcon1.Tag.ToString());
					break;

				default:
					break;
			}
			BalloonTipClickAction = BalloonTipClickActionEnum.None;
		}

		enum NextActionEnum { Username, Password };
		NextActionEnum NextAction = NextActionEnum.Username;
		void actHook_OnMouseActivity(object sender, UserActivityHook.MoreMouseEventArgs e)
		{
			switch (e.Button.Button)
			{
				case MouseButtons.Left:
					if ((ModifierKeys & Keys.Control) == Keys.Control && (ModifierKeys & Keys.Shift) == Keys.Shift && (ModifierKeys & Keys.Alt) == Keys.Alt)
					{
						string activeTitle = GetActiveWindowTitle();
						notifyIcon1.Tag = activeTitle;
						BalloonTipClickAction = BalloonTipClickActionEnum.CopyTagToClipboard;
						notifyIcon1.ShowBalloonTip(300, "Active", activeTitle + Environment.NewLine + "Click to copy to clipboard", ToolTipIcon.Info);
					}
					break;
				case MouseButtons.Middle:
					//else notifyIcon1.ShowBalloonTip(300, "Middle", activeTitle, ToolTipIcon.Info);
					break;
				case MouseButtons.None:
					break;
				case MouseButtons.Right:
					break;
				case MouseButtons.XButton1:
					break;
				case MouseButtons.XButton2:
					break;
				default:
					break;
			}
			//if (e.Button != System.Windows.Forms.MouseButtons.None)
			//notifyIcon1.ShowBalloonTip(100, "Mouse button pressed", e.Button.ToString(), ToolTipIcon.Info);
		}

		string LastActiveTitle = "";
		bool WasKeyLiftedAfterPreviousDown = true;
		void actHook_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.LControlKey)
			{
				string activeTitle = GetActiveWindowTitle();
				if (activeTitle != null)
				{
					activeTitle = activeTitle.Replace("\n", "").Replace("\r", "");
					if (LastActiveTitle != activeTitle) NextAction = NextActionEnum.Username;
					LastActiveTitle = activeTitle;

					foreach (string RegexUsernamePassword in currentEmailPasswordAndRegexList)
					{
						string tmpRegex = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[0], hex16CharactersToUseOnlineTodo);
						string tmpUsername = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[1], hex16CharactersToUseOnlineTodo);
						string tmpPassword = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[2], hex16CharactersToUseOnlineTodo);

						//string currText = Clipboard.GetText();
						if (Regex.IsMatch(activeTitle, tmpRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline))
						{
							//if ((NextAction == NextActionEnum.Username && currText != tmpUsername)
							//    || (NextAction == NextActionEnum.Password && currText != tmpPassword))
							if (WasKeyLiftedAfterPreviousDown)
							{
								if (NextAction == NextActionEnum.Username)
								{
									Clipboard.SetText(tmpUsername);
									notifyIcon1.ShowBalloonTip(300, "Ready " + NextAction.ToString(), "Paste ready", ToolTipIcon.Info);
									if (tmpPassword.Length > 0) NextAction = NextActionEnum.Password;
									WasKeyLiftedAfterPreviousDown = false;
								}
								else if (NextAction == NextActionEnum.Password)
								{
									Clipboard.SetText(tmpPassword);
									notifyIcon1.ShowBalloonTip(300, "Ready" + NextAction.ToString(), "Paste ready", ToolTipIcon.Info);
									NextAction = NextActionEnum.Username;
									WasKeyLiftedAfterPreviousDown = false;
									ClearClipboardAfterMilliseconds(1000);
								}
								break;
							}
						}
					}

					/*if (Regex.IsMatch(activeTitle, ".*Remote.*Desktop.*LogMeIn.*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
					{
							if (NextAction == NextActionEnum.Username)
							{
									Clipboard.SetText("francoishill11@gmail.com");
									NextAction = NextActionEnum.Password;
							}
							else if (NextAction == NextActionEnum.Password)
							{
									Clipboard.SetText("bokbokkie");
									NextAction = NextActionEnum.Username;
									ClearClipboardAfterMilliseconds(1000);
							}
					}
					else if (Regex.IsMatch(activeTitle, "LogMeIn - Google Chrome", RegexOptions.IgnoreCase | RegexOptions.Multiline))
					{
							Clipboard.SetText("bokbokkie");
							NextAction = NextActionEnum.Username;
							Timer timerClearClipboard = new Timer();
							timerClearClipboard.Interval = 1000;
							timerClearClipboard.Tick += delegate
							{
									Clipboard.Clear();
									timerClearClipboard.Stop();
									timerClearClipboard.Dispose();
									timerClearClipboard = null;
							};
							timerClearClipboard.Start();
					}*/
				}
			}
		}

		private static void ClearClipboardAfterMilliseconds(int Millisecs)
		{
			Timer timerClearClipboard = new Timer();
			timerClearClipboard.Interval = Millisecs;
			timerClearClipboard.Tick += delegate
			{
				Clipboard.Clear();
				timerClearClipboard.Stop();
				timerClearClipboard.Dispose();
				timerClearClipboard = null;
			};
			timerClearClipboard.Start();
		}

		void actHook_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		void actHook_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.LControlKey)
			{
				WasKeyLiftedAfterPreviousDown = true;
				string activeTitle = GetActiveWindowTitle();
				if (activeTitle != null)
				{
					activeTitle = activeTitle.Replace("\n", "").Replace("\r", "");
					foreach (string RegexUsernamePassword in currentEmailPasswordAndRegexList)
					{
						string tmpRegex = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[0], hex16CharactersToUseOnlineTodo);
						if (Regex.IsMatch(activeTitle, tmpRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline))
							Clipboard.Clear();
					}
				}
			}
		}

		private string GetActiveWindowTitle()
		{
			const int nChars = 256;
			IntPtr handle = IntPtr.Zero;
			StringBuilder Buff = new StringBuilder(nChars);
			handle = Win32Api.GetForegroundWindow();
			if (Win32Api.GetWindowText(handle, Buff, nChars) > 0)
			{
				return Buff.ToString();
			}
			return null;
		}

		//const int SecondsToIdle = 240000;//4minutes
		//const string FilePathStart = @"C:\Francois\tmp\Screenshots\Screenshot_";
		//private void timer_Tick(object sender, EventArgs e)
		//{
		//    try
		//    {
		//        if (Win32.GetIdleTime() >= SecondsToIdle) SaveScreenshotNow();
		//    }
		//    catch { }
		//}

		//private static void SaveScreenshotNow()
		//{
		//    Rectangle bounds = Screen.GetBounds(Point.Empty);
		//    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
		//    {
		//        using (Graphics g = Graphics.FromImage(bitmap))
		//        {
		//            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
		//        }
		//        string filepath = FilePathStart + DateTime.Now.ToString(@"yyyy\ MM\ dd\ \a\t\ HH\hmm_ss_fff") + ".jpg";
		//        bitmap.Save(filepath, ImageFormat.Jpeg);
		//    }
		//}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Hide();
				this.Tag = WindowState;
				notifyIcon1.Visible = true;
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//ShowForm();
			ShowSmallTodolist();
		}

		//TODO: Continue implementing this concept (having a todo list which can be easily accessed by double clicking on tray icon
		class QueuedNotificationClass
		{
			public string Title;
			public string Message;
			public QueuedNotificationClass(string TitleIn, string MessageIn)
			{
				Title = TitleIn;
				Message = MessageIn;
			}
		}
		private Dictionary<string, QueuedNotificationClass> queuedNotifications = new Dictionary<string, QueuedNotificationClass>();
		private void ShowSmallTodolist()
		{
			//TODO: Write app for transferring files (over internet / TCP), must totally manage it and have resume capability.
			if (queuedNotifications.Count == 0) this.notifyIcon1.ShowBalloonTip(3000, "No items", "No todo items currently loaded", ToolTipIcon.Info);
			foreach (string key in queuedNotifications.Keys)
				CustomBalloonTipwpf.ShowCustomBalloonTip(
					queuedNotifications[key].Title,
					queuedNotifications[key].Message,
					5000,
					CustomBalloonTipwpf.IconTypes.None,
					(sndr) =>
					{
						if (sndr != null && sndr is CustomBalloonTipClass)
							if (UserMessages.Confirm("Mark item as Done: " + (sndr as CustomBalloonTipClass).Message + "?"))
							{
								QueueingActionsInterop.EnqueueAction(delegate
								{
									string todoItemText = (sndr as CustomBalloonTipClass).Message;
									List<string> todolistFileLines = TextFilesInterop.GetLinesFromTextFile(SmallTodolistFilePath, true);
									if (!todolistFileLines.Contains(todoItemText)) UserMessages.ShowWarningMessage("Could not mark todo item as done, missing from file: " + todoItemText);
									else
									{
										todolistFileLines[todolistFileLines.IndexOf(todoItemText)] = "//" + todoItemText;
										//queuedNotifications.Remove(sndr.ToString());
									}
									TextFilesInterop.WriteLinesToTextFile(SmallTodolistFilePath, todolistFileLines);
									RefreshSmallTodoitems();
								});
							}
					},
					key,
					false,
					3);
		}

		private void ShowForm()
		{
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.Activate();
			notifyIcon1.Visible = false;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("explorer", @"C:\Francois\tmp\Screenshots");
		}

		private bool ForceClosing = false;
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			ApplicationRecoveryAndRestart.UnregisterForRecoveryAndRestart();
			if (e.CloseReason == CloseReason.UserClosing && !ForceClosing)
			{
				this.WindowState = FormWindowState.Minimized;
				e.Cancel = true;
			}
			else if (!MayApplicationQuit())
			{
				e.Cancel = true;
				this.Show();
				this.WindowState = FormWindowState.Normal;
				//This code prevents shutdown
				//DONE TODO: Should reload the "state" again (and notifying user) on next startup instead of preventing shutdown, i.e. reload the QueuedFileChanges Dictionary
				bool ShouldRatherSaveThis_State_AndReloadOnPcStartupAgain_AndNotifyUser;
				this.Activate();
				toolStripStatusLabelCurrentStatus.Text = "Saving current state, please wait...";
				ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
				{
					using (StreamWriter sw = new StreamWriter(LastAutobackupStateFileName))
					{
						List<string> keys = QueuedFileChanges.Keys.ToList();
						foreach (string key in keys)
						{
							if (QueuedFileChanges[key] == null || QueuedFileChanges[key].Count == 0)
								continue;
							List<DateTime> dates = QueuedFileChanges[key].Keys.ToList();
							foreach (DateTime date in dates)
								if (QueuedFileChanges[key][date].QueueStatus != FileChangedDetails.QueueStatusEnum.Complete)
								{
									sw.WriteLine(
										key + "|" +
										date.ToString(FileChangedDetails.SavetofileDateFormat) + "|" +
										QueuedFileChanges[key][date].QueueStatus.ToString());
									QueuedFileChanges[key].Remove(date);
									if (QueuedFileChanges[key].Count == 0) QueuedFileChanges.Remove(key);
								}
						}
					}
				}, true);
				e.Cancel = false;
				//if (pipeclient != null)
				//	pipeclient.ForceCancelRetryLoop = true;
				//System.Diagnostics.Process.Start("shutdown", "-a");
				//Interaction.Shell("shutdown -a", AppWinStyle.MinimizedFocus, false, -1);
			}
		}

		private bool QueueFileChangesHasUnprocessedItems()
		{
			if (QueuedFileChanges == null || QueuedFileChanges.Count == 0) return false;
			foreach (string key in QueuedFileChanges.Keys)
			{
				if (QueuedFileChanges[key] == null || QueuedFileChanges[key].Count == 0)
					continue;
				foreach (DateTime date in QueuedFileChanges[key].Keys)
					if (QueuedFileChanges[key][date].QueueStatus != FileChangedDetails.QueueStatusEnum.Complete)
						return true;
			}
			return false;
		}

		private bool MayApplicationQuit()
		{
			return !QueueFileChangesHasUnprocessedItems();
		}

		private bool RequestApplicationQuit()
		{
			if (MayApplicationQuit())
			{
				this.notifyIcon1.Visible = false;
				Application.DoEvents();
				ForceClosing = true;
				//if (pipeclient != null)
				//	pipeclient.ForceCancelRetryLoop = true;
				this.Close();
				return true;
				//Application.Exit();
			}
			else
				//ShowBalloonTipNotification("Please process unread notifications", BalloonTipActionIn: BalloonTipActionEnum.ChangedFileList);
				ShowCustomBalloonTipNotification("Please process unread notifications", BalloonTipActionIn: BalloonTipActionEnum.ChangedFileList);
			return false;
		}

		private void linkLabel_AddEmailAndPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			currentEmailPasswordAndRegexList = TextFilesInterop.GetLinesFromTextFile(SavedListFileName, false);
			string NewRegexEmailAndPasswordString = GetNewEmailAndPassword();
			if (NewRegexEmailAndPasswordString != null)
				currentEmailPasswordAndRegexList.Add(
					EncodeAndDecodeInterop.EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[0], (err) => UserMessages.ShowErrorMessage(err), hex16CharactersToUseOnlineTodo)
					+ "|" + EncodeAndDecodeInterop.EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[1], (err) => UserMessages.ShowErrorMessage(err), hex16CharactersToUseOnlineTodo)
					+ "|" + EncodeAndDecodeInterop.EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[2], (err) => UserMessages.ShowErrorMessage(err), hex16CharactersToUseOnlineTodo));
			TextFilesInterop.WriteLinesToTextFile(SavedListFileName, currentEmailPasswordAndRegexList);
			RefreshRegexList();
			foreach (string RegexUsernamePassword in currentEmailPasswordAndRegexList)
			{
				string tmpRegex = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[0], hex16CharactersToUseOnlineTodo);
				string tmpUsername = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[1], hex16CharactersToUseOnlineTodo);
				string tmpPassword = EncodeAndDecodeInterop.DecodeStringHex(RegexUsernamePassword.Split('|')[2], hex16CharactersToUseOnlineTodo);
				string nl = Environment.NewLine;
			}
		}

		private string GetNewEmailAndPassword()
		{
			AddEmailAndPasswordForm tmpform = new AddEmailAndPasswordForm();
			if (tmpform.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				return tmpform.textBoxRegex.Text + "\t" + tmpform.textBoxUsername.Text + "\t" + tmpform.textBoxPassword.Text;
			}
			return null;
		}

		private static string hex16CharactersToUseOnlineTodo = "abcdefhkiljqwpmz";

		//PerformFunctionSeperateThread((Func<int, string>)delegate(int i) { return ""; }, null);
		public object PerformFunctionSeperateThread(Delegate method, object[] param)
		{
			object res = new object();
			System.Threading.Thread th = new System.Threading.Thread(() =>
			{
				res = method.DynamicInvoke(param);
			});
			th.Start();
			//th.Join();
			while (th.IsAlive) { Application.DoEvents(); }
			return res;
		}

		private string GetPrivateKey()
		{
			try
			{
				toolStripStatusLabelCurrentStatus.Text = "Obtaining pvt key...";
				string tmpkey = null;

				ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
				{
					string tmpResult;
					if (WebInterop.PostPHP(PhpInterop.ServerAddress + "/generateprivatekey.php", "username=" + PhpInterop.Username + "&password=" + PhpInterop.Password, out tmpResult))
						tmpkey = tmpResult;
					else
						appendLogTextbox(tmpResult);
				});

				string tmpSuccessKeyString = "Success: Key=";
				if (tmpkey != null && tmpkey.Length > 0 && tmpkey.ToUpper().StartsWith(tmpSuccessKeyString.ToUpper()))
				{
					tmpkey = tmpkey.Substring(tmpSuccessKeyString.Length).Replace("\n", "").Replace("\r", "");
					toolStripStatusLabelCurrentStatus.Text = tmpkey;
				}
				return tmpkey;
			}
			catch (Exception exc)
			{
				appendLogTextbox("Obtain private key exception: " + exc.Message);
				return null;
			}
		}

		private void linkLabelGetCurrentTodolist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			GetCurrentTodolist();
		}

		private void GetCurrentTodolist()
		{
			treeViewTodolist.Nodes.Clear();
			string tmpresult = GetResultOfPerformingDesktopAppDoTask(PhpInterop.Username, "getlist", new List<string>());
			if (tmpresult != null)
			{
				appendLogTextbox("Successfully obtained todo list");
				foreach (string line in tmpresult.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
					if (line.Trim('\t', '\n', '\r', ' ', '\0').Length > 0)
					{
						//dt.Rows.Add(line.Split('\t')); ;
						AddTabSeperatedLineToTreeview(line);
					}
			}
			/*try
			{
					//See App.xaml.cs for bypassing invalid SSL certificates
					string tmpkey = GetPrivateKey();

					if (tmpkey != null)
					{
							HttpWebRequest addrequest = null;
							HttpWebResponse addresponse = null;
							StreamReader input = null;
							try
							{
									if (Username != null && Username.Length > 0
																 && tmpkey != null && tmpkey.Length > 0)
									{
											string encryptedstring;
											string decryptedstring = "";

											PerformVoidFunctionSeperateThread(() =>
											{
													//string username = "f";
													//string query = PhpEncryption.SimpleTripleDesEncrypt("_att_" + "un=" + Username, tmpkey);
													addrequest = (HttpWebRequest)WebRequest.Create(doWorkAddress + "/dotask/" + PhpEncryption.SimpleTripleDesEncrypt(Username, "123456789abcdefghijklmno") + "/" + PhpEncryption.SimpleTripleDesEncrypt("getlist", tmpkey));// + "/");
													addresponse = (HttpWebResponse)addrequest.GetResponse();

													input = new StreamReader(addresponse.GetResponseStream());
													encryptedstring = input.ReadToEnd();

													decryptedstring = PhpEncryption.SimpleTripleDesDecrypt(encryptedstring, tmpkey);
													//textBoxLogs.Text = "";
													//appendLogTextbox(decryptedstring);
											});

											treeViewTodolist.Nodes.Clear();

											//DataSet ds = new DataSet("Todolist");
											//DataTable dt = new DataTable("Todotable");
											//dt.Columns.Add("Category");
											//dt.Columns.Add("Subcat");
											//dt.Columns.Add("Items");
											//dt.Columns.Add("Description");
											//dt.Columns.Add("Complete");//, typeof(bool));
											foreach (string line in decryptedstring.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
													if (line.Trim('\t', '\n', '\r', ' ', '\0').Length > 0)
													{
															//dt.Rows.Add(line.Split('\t')); ;
															AddTabSeperatedLineToTreeview(line);
													}
											//ds.Tables.Add(dt);
											//dataGridView1.DataSource = ds;
											//dataGridView1.DataMember = "Todotable";
									}
							}
							catch (Exception exc)
							{
									appendLogTextbox("Obtain php: " + exc.Message);
							}
							finally
							{
									if (addresponse != null) addresponse.Close();
									if (input != null) input.Close();
							}
					}
			}
			catch (UriFormatException ex)
			{
					System.Windows.Forms.MessageBox.Show(ex.Message);
					return;
			}
			catch (IOException ex)
			{
					System.Windows.Forms.MessageBox.Show(ex.Message);
					return;
			}*/
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			AddTodoItemNow();
		}

		private void AddTodoItemNow(string Category = "", string Subcat = "")
		{
			AddTodoItem addform = new AddTodoItem(Category, Subcat);

			if (treeViewTodolist.Nodes.Count == 0)
				GetCurrentTodolist();

			foreach (TreeNode catnode in treeViewTodolist.Nodes)
			{
				List<string> tmplist = new List<string>();
				foreach (TreeNode subcatnode in catnode.Nodes)
					tmplist.Add(subcatnode.Text);
				addform.comboBoxCategory.Items.Add(new TodoCategoryAndSubcats(catnode.Text, tmplist));
			}

			if (addform.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				bool successfulAdd = PerformDesktopAppDoTask(
						PhpInterop.Username,
						"addtolist",
						new List<string>()
                {
                    addform.comboBoxCategory.Text,
                    addform.comboBoxSubcat.Text,
                    addform.textBoxItems.Text,
                    addform.textBoxDescription.Text ?? "",
                    addform.dateTimePickerRemindOn.Value.ToString(MySQLdateformat),
                    addform.dateTimePickerRemindOn.Checked ? "0" : "1",//This is correct, checbox sais remind but database it is stopsnooze
                    addform.checkBoxAutosnooze.Checked ? ((int)addform.numericUpDownAutosnoozeInterval.Value).ToString() : "0"
                },
						true,
						"1");
				if (successfulAdd)
				{
					appendLogTextbox("Successfully added " + addform.textBoxItems.Text);
					AdditemToTreeview(
							addform.comboBoxCategory.Text,
							addform.comboBoxSubcat.Text,
							addform.textBoxItems.Text,
							addform.textBoxDescription.Text,
							false,
							addform.dateTimePickerRemindOn.Value,
							DateTime.Now,
							0,
							!addform.dateTimePickerRemindOn.Checked,
							addform.checkBoxAutosnooze.Checked ? (int)addform.numericUpDownAutosnoozeInterval.Value : 0);
				}
			}
		}

		private void AddTabSeperatedLineToTreeview(string line)
		{
			if (line.Contains("\t") && line.Split('\t').Length >= 4)
			{
				string tmpCategory = line.Split('\t')[0];
				string tmpSubcat = line.Split('\t')[1];
				string tmpItems = line.Split('\t')[2];
				string tmpDescription = line.Split('\t')[3];
				bool tmpCompleted = line.Split('\t')[4] == "1";
				DateTime tmpDue = line.Split('\t')[5].Length > 0 ? DateTime.ParseExact(line.Split('\t')[5], MySQLdateformat, CultureInfo.InvariantCulture) : mindate;
				DateTime tmpCreated = line.Split('\t')[6].Length > 0 ? DateTime.ParseExact(line.Split('\t')[6], MySQLdateformat, CultureInfo.InvariantCulture) : mindate;
				int tmpRemindedCount = Convert.ToInt32(line.Split('\t')[7]);
				bool tmpStopSnooze = line.Split('\t')[8] == "1";
				int tmpAutosnoozeInterval = Convert.ToInt32(line.Split('\t')[9]);

				if (string.IsNullOrWhiteSpace(tmpCategory) || string.IsNullOrWhiteSpace(tmpSubcat))
					UserMessages.ShowWarningMessage("Cannot obtain item (category/subcat is empty) on line: " + line);
				else
					AdditemToTreeview(tmpCategory, tmpSubcat, tmpItems, tmpDescription, tmpCompleted, tmpDue, tmpCreated, tmpRemindedCount, tmpStopSnooze, tmpAutosnoozeInterval);
			}
			else UserMessages.ShowWarningMessage("The following line is invalid todo line: " + line);
		}

		private void AdditemToTreeview(string Category, string Subcat, string Items, string Description, bool Completed, DateTime Due, DateTime Created, int RemindedCount, bool StopSnooze, int AutosnoozeInterval)
		{
			TreeNode tmpItemsNode = new TreeNode(Items) { Tag = new ItemDetails(Description, Completed, Due, Created, RemindedCount, StopSnooze, AutosnoozeInterval) };
			tmpItemsNode.ContextMenu = contextMenuItemsNode;
			if (!treeViewTodolist.Nodes.ContainsKey(Category)) treeViewTodolist.Nodes.Add(Category, Category);
			if (!treeViewTodolist.Nodes[Category].Nodes.ContainsKey(Subcat)) treeViewTodolist.Nodes[Category].Nodes.Add(Subcat, Subcat);
			treeViewTodolist.Nodes[Category].Nodes[Subcat].Nodes.Add(tmpItemsNode);
			tmpItemsNode.Parent.ContextMenu = contextMenuItemsSubcat;
		}

		private void appendLogTextbox(string str, bool mustUpdateStatusText = true)
		{
			textBoxLogs.Text += (textBoxLogs.Text.Length > 0 ? Environment.NewLine : "") + str;
			if (mustUpdateStatusText) updateStatusText(str);
		}

		private void updateStatusText(string str)
		{
			toolStripStatusLabelCurrentStatus.Text = str;
		}

		private void treeViewTodolist_AfterSelect(object sender, TreeViewEventArgs e)
		{
			MustHandleCheckChanged = false;
			MustHandleDueDateChanged = false;
			MustHandleStopSnoozeChanged = false;
			MustHandleStopAutosnoozeIntervalChanged = false;
			ClearAll();
			if (e.Node != null && e.Node.Tag != null)
			{
				numericUpDownRemindedCount.Minimum = 0;
				numericUpDownAutosnoozeInterval.Minimum = 0;

				ItemDetails details = e.Node.Tag as ItemDetails;
				textBoxDescription.Text = details.Description.Replace("<br>", "\r\n");
				textBoxDescription.Tag = details.Description.Replace("<br>", "\r\n");
				checkBoxComplete.Checked = details.Complete;
				dateTimePickerDue.Value = details.Due;
				dateTimePickerCreated.Value = details.Created;
				numericUpDownRemindedCount.Value = details.RemindedCount;
				checkBoxStopSnooze.Checked = details.StopSnooze;
				numericUpDownAutosnoozeInterval.Value = details.AutosnoozeInterval;
			}
			Application.DoEvents();
			MustHandleCheckChanged = true;
			MustHandleDueDateChanged = true;
			MustHandleStopSnoozeChanged = true;
			MustHandleStopAutosnoozeIntervalChanged = true;
		}

		private void ClearAll()
		{
			textBoxDescription.Text = null;
			textBoxDescription.Tag = null;
			checkBoxComplete.Checked = false;
			dateTimePickerDue.Value = mindate;
			dateTimePickerCreated.Value = mindate;
			numericUpDownRemindedCount.Minimum = -1; numericUpDownRemindedCount.Value = -1;
			checkBoxStopSnooze.Checked = false;
			numericUpDownAutosnoozeInterval.Minimum = -1; numericUpDownAutosnoozeInterval.Value = -1;
		}

		private bool UploadChangesMade(TreeNode node)
		{
			return PerformDesktopAppDoTask(
					PhpInterop.Username,
					"updatedescription",
					new List<string>()
                {
                    node.Parent.Parent.Text,//category
                    node.Parent.Text,//subcat
                    node.Text,//items
                    (node.Tag as ItemDetails).Description//description
                },
					true,
					"1");
		}

		private void treeViewTodolist_MouseDown(object sender, MouseEventArgs e)
		{
			TreeNode ItemOfContextMenu = treeViewTodolist.GetNodeAt(treeViewTodolist.PointToClient(MousePosition));
			if (ItemOfContextMenu == null)
				treeViewTodolist.ContextMenuStrip = null;//contextMenuStrip_ROOT;
			else treeViewTodolist.SelectedNode = ItemOfContextMenu;
		}

		private void textBoxDescription_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A && ((ModifierKeys & Keys.Control) == Keys.Control))
				textBoxDescription.SelectAll();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) { e.Handled = true; this.Close(); }
		}

		private bool PerformDesktopAppDoTask(string UsernameIn, string TaskName, List<string> ArgumentList, bool CheckForSpecificResult = false, string SuccessSpecificResult = "", bool MustWriteResultToLogsTextbox = false)
		{
			string result = GetResultOfPerformingDesktopAppDoTask(UsernameIn, TaskName, ArgumentList, MustWriteResultToLogsTextbox);
			if (CheckForSpecificResult && result == SuccessSpecificResult)
				return true;
			return false;
		}

		private string GetResultOfPerformingDesktopAppDoTask(string UsernameIn, string TaskName, List<string> ArgumentList, bool MustWriteResultToLogsTextbox = false)
		{
			string tmpkey = GetPrivateKey();

			if (tmpkey != null)
			{
				HttpWebRequest addrequest = null;
				HttpWebResponse addresponse = null;
				StreamReader input = null;

				try
				{
					if (UsernameIn != null && UsernameIn.Length > 0
																 && tmpkey != null && tmpkey.Length > 0)
					{
						string encryptedstring;
						string decryptedstring = "";
						bool mustreturn = false;
						ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
						{
							string ArgumentListTabSeperated = "";
							foreach (string s in ArgumentList)
								ArgumentListTabSeperated += (ArgumentListTabSeperated.Length > 0 ? "\t" : "") + s;

							addrequest = (HttpWebRequest)WebRequest.Create(PhpInterop.doWorkAddress + "/dotask/" +
									PhpInterop.PhpEncryption.SimpleTripleDesEncrypt(UsernameIn, "123456789abcdefghijklmno") + "/" +
									PhpInterop.PhpEncryption.SimpleTripleDesEncrypt(TaskName, tmpkey) + "/" +
									PhpInterop.PhpEncryption.SimpleTripleDesEncrypt(ArgumentListTabSeperated, tmpkey));// + "/");
							//appendLogTextbox(addrequest.RequestUri.ToString());
							try
							{
								addresponse = (HttpWebResponse)addrequest.GetResponse();
								input = new StreamReader(addresponse.GetResponseStream());
								encryptedstring = input.ReadToEnd();
								//appendLogTextbox("Encrypted response: " + encryptedstring);

								decryptedstring = PhpInterop.PhpEncryption.SimpleTripleDesDecrypt(encryptedstring, tmpkey);
								//appendLogTextbox("Decrypted response: " + decryptedstring);
								decryptedstring = decryptedstring.Replace("\0", "").Trim();
								//MessageBox.Show(this, decryptedstring);
								if (MustWriteResultToLogsTextbox) appendLogTextbox("Result for " + TaskName + ": " + decryptedstring);
								mustreturn = true;
							}
							catch (Exception exc) { UserMessages.ShowErrorMessage(this, "Exception:" + exc.Message, "Exception"); }
						});
						if (mustreturn) return decryptedstring;
					}
				}
				catch (Exception exc)
				{
					appendLogTextbox("Obtain php: " + exc.Message);
				}
				finally
				{
					if (addresponse != null) addresponse.Close();
					if (input != null) input.Close();
				}
			}
			return null;
		}

		private void treeViewTodolist_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (treeViewTodolist.SelectedNode != null)
				if (textBoxDescription.Tag != null && treeViewTodolist.SelectedNode.Tag != null && textBoxDescription.Text.ToString() != textBoxDescription.Tag.ToString())
				{
					appendLogTextbox("Uploading changes made to " + treeViewTodolist.SelectedNode.ToString());
					//treeViewTodolist.Enabled = false;
					//splitContainer1.Enabled = false;
					(treeViewTodolist.SelectedNode.Tag as ItemDetails).Description = textBoxDescription.Text;
					if (UploadChangesMade(treeViewTodolist.SelectedNode))
					{
						//treeViewTodolist.Enabled = true;
						//splitContainer1.Enabled = true;
						//treeViewTodolist.Focus();
						//treeViewTodolist.SelectedNode = e.Node;
						appendLogTextbox("Changes accepted");
					}
					else
					{
						(treeViewTodolist.SelectedNode.Tag as ItemDetails).Description = textBoxDescription.Tag.ToString();
						//treeViewTodolist.Enabled = true;
						//splitContainer1.Enabled = true;
						textBoxDescription.Text = textBoxDescription.Tag.ToString();
						appendLogTextbox("Changes rejected");
						e.Cancel = true;
					}
				}
			//e.Cancel = true;
		}

		bool MustHandleCheckChanged = true;
		private void checkBoxComplete_CheckedChanged(object sender, EventArgs e)
		{
			if (MustHandleCheckChanged)
			{
				MustHandleCheckChanged = false;

				checkBoxComplete.Enabled = false;
				if (treeViewTodolist.SelectedNode != null)
				{
					TreeNode node = treeViewTodolist.SelectedNode;
					if (!PerformDesktopAppDoTask(
							PhpInterop.Username,
							"updatecomplete",
							new List<string>()
                        {
                            node.Parent.Parent.Text,//category
                            node.Parent.Text,//subcat
                            node.Text,//items
                            checkBoxComplete.Checked ? "1" : "0"
                        },
							true,
							"1"))
						checkBoxComplete.Checked = (node.Tag as ItemDetails).Complete;
					else
					{
						appendLogTextbox("Successfully updated completed state for " + node.Text);
						(node.Tag as ItemDetails).Complete = checkBoxComplete.Checked;
					}
				}
				checkBoxComplete.Enabled = true;

				Application.DoEvents();
				MustHandleCheckChanged = true;
			}
		}

		bool MustHandleDueDateChanged = true;
		private void dateTimePickerDue_ValueChanged(object sender, EventArgs e)
		{
			if (MustHandleDueDateChanged)
			{
				MustHandleDueDateChanged = false;

				dateTimePickerDue.Enabled = false;
				if (treeViewTodolist.SelectedNode != null)
				{
					TreeNode node = treeViewTodolist.SelectedNode;
					if (!PerformDesktopAppDoTask(
							PhpInterop.Username,
							"updatedate",
							new List<string>()
                        {
                            node.Parent.Parent.Text,//category
                            node.Parent.Text,//subcat
                            node.Text,//items
                            dateTimePickerDue.Value.ToString(MySQLdateformat)
                        },
							true,
							"1"))
						dateTimePickerDue.Value = (node.Tag as ItemDetails).Due;
					else
					{
						appendLogTextbox("Successfully updated due date for " + node.Text);
						(node.Tag as ItemDetails).Due = dateTimePickerDue.Value;
					}
				}
				dateTimePickerDue.Enabled = true;

				Application.DoEvents();
				MustHandleDueDateChanged = true;
			}
		}

		bool MustHandleStopSnoozeChanged = true;
		private void checkBoxStopSnooze_CheckedChanged(object sender, EventArgs e)
		{
			if (MustHandleStopSnoozeChanged)
			{
				MustHandleStopSnoozeChanged = false;

				checkBoxStopSnooze.Enabled = false;
				if (treeViewTodolist.SelectedNode != null)
				{
					TreeNode node = treeViewTodolist.SelectedNode;
					if (!PerformDesktopAppDoTask(
							PhpInterop.Username,
							"updatestopsnooze",
							new List<string>()
                        {
                            node.Parent.Parent.Text,//category
                            node.Parent.Text,//subcat
                            node.Text,//items
                            checkBoxStopSnooze.Checked ? "1" : "0"
                        },
							true,
							"1"))
						checkBoxStopSnooze.Checked = (node.Tag as ItemDetails).StopSnooze;
					else
					{
						appendLogTextbox("Successfully updated stopsnooze state for " + node.Text);
						(node.Tag as ItemDetails).StopSnooze = checkBoxStopSnooze.Checked;
					}
				}
				checkBoxStopSnooze.Enabled = true;

				Application.DoEvents();
				MustHandleStopSnoozeChanged = true;
			}
		}

		bool MustHandleStopAutosnoozeIntervalChanged = true;
		private void numericUpDownAutosnoozeInterval_ValueChanged(object sender, EventArgs e)
		{
			if (MustHandleStopAutosnoozeIntervalChanged)
			{
				MustHandleStopAutosnoozeIntervalChanged = false;

				numericUpDownAutosnoozeInterval.Enabled = false;
				if (treeViewTodolist.SelectedNode != null)
				{
					TreeNode node = treeViewTodolist.SelectedNode;
					if (!PerformDesktopAppDoTask(
							PhpInterop.Username,
							"updateautosnoozeinterval",
							new List<string>()
                        {
                            node.Parent.Parent.Text,//category
                            node.Parent.Text,//subcat
                            node.Text,//items
                            ((int)numericUpDownAutosnoozeInterval.Value).ToString()
                        },
							true,
							"1"))
						numericUpDownAutosnoozeInterval.Value = (node.Tag as ItemDetails).AutosnoozeInterval;
					else
					{
						appendLogTextbox("Successfully updated autosnooze interval for " + node.Text);
						(node.Tag as ItemDetails).AutosnoozeInterval = (int)numericUpDownAutosnoozeInterval.Value;
					}
				}
				numericUpDownAutosnoozeInterval.Enabled = true;

				Application.DoEvents();
				MustHandleStopAutosnoozeIntervalChanged = true;
			}
		}

		private void timer_PhpCronJob_Tick(object sender, EventArgs e)
		{

			ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
			{
				try
				{
					//StringBuilder Output = new StringBuilder();

					System.Diagnostics.Process proc = new System.Diagnostics.Process();
					proc.StartInfo.FileName = "php";
					proc.StartInfo.Arguments = @"""c:\francois\websites\firepuma\cron.php""";
					proc.StartInfo.UseShellExecute = false;
					proc.StartInfo.RedirectStandardOutput = true;
					proc.StartInfo.CreateNoWindow = true;
					//proc.OutputDataReceived += delegate(object sendingProcess, DataReceivedEventArgs outLine)
					//{
					//    if (!String.IsNullOrEmpty(outLine.Data))
					//    {
					//        Output.Append(Environment.NewLine + outLine.Data);
					//    }
					//};
					proc.Start();
					proc.BeginOutputReadLine();
					proc.WaitForExit();
					proc.Close();
					//listBox1.Items.AddRange(Output.ToString().Split('\n'));
				}
				catch { }
			});
		}

		/*public class MonitoredFilesClass
		{
			//This class is used because FileSystemWathcer fires the same event twice
			public static Dictionary<string, DateTime> FilesAndLastModifiedTimes;
			public static void RePopulateFilesAndLastModifiedTimesDictionary(string RootPathIn, bool SubdirsIn, string searchPatternsPipeDelimited = "*.sql|*.xml")
			{
				foreach (string splitSearchPattern in searchPatternsPipeDelimited.Split('|'))
					AppendFilesAndLastModifiedTimesToDictionary(RootPathIn, SubdirsIn, splitSearchPattern);
			}

			private static void AppendFilesAndLastModifiedTimesToDictionary(string Rootpath, bool Subdirs, string searchPattern)
			{
				if (FilesAndLastModifiedTimes == null) FilesAndLastModifiedTimes = new Dictionary<string, DateTime>();
				if (Directory.Exists(Rootpath))
					foreach (string f in Directory.GetFiles(Rootpath, searchPattern, Subdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
						if (!searchPattern.StartsWith("*") || (searchPattern.StartsWith("*") && f.ToLower().EndsWith(searchPattern.Substring(1).ToLower())))
							FilesAndLastModifiedTimes.Add(f, new FileInfo(f).LastWriteTime);
			}
		}*/

		public class FileChangedDetails
		{
			public enum QueueStatusEnum { New, Read, Accepted, Discard, Complete };

			public static FileAttributes NormalAttributes = FileAttributes.Normal;
			public static FileAttributes BackupAndDescriptionAttributes = FileAttributes.System | FileAttributes.Hidden;

			public DateTime LastWrite;
			public string OriginalFileName;
			public string Description;
			public QueueStatusEnum QueueStatus;
			public FileChangedDetails(DateTime LastWriteIn, string OriginalFileNameIn, string DescriptionIn, QueueStatusEnum QueueStatusIn = QueueStatusEnum.New)
			{
				LastWrite = LastWriteIn;
				OriginalFileName = OriginalFileNameIn;
				Description = DescriptionIn;
				QueueStatus = QueueStatusIn;
			}

			private bool IsStringEmpty(string str)
			{
				return str == null || str.Trim().Length == 0;
			}

			public bool HasDescription()
			{
				return Description != null && Description.Trim().Length > 0;
			}

			public Color GetColorBasedOnDescription()
			{
				if (HasDescription()) return Color.Green;
				else return Color.LightGray;
			}

			public string HumanFriendlyLastwriteDateString()
			{
				return LastWrite.ToString(humandfriendlyDateFormat);
			}

			public void UpdateNodeFontandcolorFromQueueStatus(TreeNode node, Font fontPrototype = null)
			{
				SetNewQueueStatusAndUpdateNodeFontandcolor(QueueStatus, node, fontPrototype);
			}

			public void SetNewQueueStatusAndUpdateNodeFontandcolor(QueueStatusEnum QueueStatusIn, TreeNode node, Font fontPrototype = null)
			{
				this.QueueStatus = QueueStatusIn;
				node.ForeColor =
									IsStringEmpty(Description) && QueueStatusIn == FileChangedDetails.QueueStatusEnum.Read ? Color.Red :
									QueueStatusIn == FileChangedDetails.QueueStatusEnum.Read ? Color.Green :
									QueueStatusIn == FileChangedDetails.QueueStatusEnum.Accepted ? Color.Green :
									QueueStatusIn == FileChangedDetails.QueueStatusEnum.Discard ? Color.Red :
									Color.Black;
				FontStyle nodeFontStyle =
					QueueStatusIn == FileChangedDetails.QueueStatusEnum.New ? FontStyle.Underline :
					QueueStatusIn == FileChangedDetails.QueueStatusEnum.Read ? FontStyle.Regular :
					QueueStatusIn == FileChangedDetails.QueueStatusEnum.Accepted ? FontStyle.Strikeout :
					QueueStatusIn == FileChangedDetails.QueueStatusEnum.Discard ? FontStyle.Strikeout | FontStyle.Italic :
					FontStyle.Italic;
				if (fontPrototype == null)
					node.NodeFont = new Font(FontFamily.GenericSansSerif, 12, nodeFontStyle);
				else
					node.NodeFont = new Font(fontPrototype, nodeFontStyle);
			}

			public static string humandfriendlyDateFormat = @"ddd dd-MMM-yyyy \a\t HH\hmm:ss";
			public static string dateFormat = @"yyyy MM dd (HH\hmm ss)";
			public static string SavetofileDateFormat = "yyyyMMddHHmmss";

			private static string GetFileNameAppendStringForDate(DateTime lastWriteTime)
			{
				return "_" + lastWriteTime.ToString(dateFormat);
			}

			public static string backupExt = ".bac";
			public static string descrExt = ".desc";
			public string GetBackupFileName()//(DateTime lastWriteTime)
			{
				return OriginalFileName + GetFileNameAppendStringForDate(LastWrite) + backupExt;//lastWriteTime) + backupExt;
			}

			public string GetDescriptionFileName()//DateTime lastWriteTime)
			{
				return OriginalFileName + GetFileNameAppendStringForDate(LastWrite) + descrExt;//;lastWriteTime) + descrExt;
			}

			public static string GetDescriptionFileNameFromBackupFilename(string backupFileName)
			{
				return backupFileName.Substring(0, backupFileName.Length - backupExt.Length) + descrExt;
			}

			public static string GetOriginalNameFromBackupFile(string backupFileName)
			{
				return backupFileName.Substring(0, backupFileName.Length - GetFileNameAppendStringForDate(new DateTime()).Length - backupExt.Length);
			}

			public bool DiscardBackupFileAndDescription()
			{
				try
				{
					File.Delete(GetBackupFileName());
					if (File.Exists(GetDescriptionFileName())) File.Delete(GetDescriptionFileName());
				}
				catch (Exception exc)
				{
					UserMessages.ShowErrorMessage("Could not delete file: ", exc.Message);
				}

				if (File.Exists(GetBackupFileName()) || File.Exists(GetDescriptionFileName()))
					return false;
				else
					return true;
			}

			public void WriteDescriptionFileNow()//string newDescription)
			{
				if (File.Exists(GetDescriptionFileName())) File.SetAttributes(GetDescriptionFileName(), NormalAttributes);
				using (StreamWriter sw = new StreamWriter(GetDescriptionFileName()))
					sw.Write(Description);//newDescription);

				try
				{
					File.SetAttributes(GetDescriptionFileName(), BackupAndDescriptionAttributes);
				}
				catch (Exception exc)
				{
					UserMessages.ShowErrorMessage("Could not set description file attributes: " + GetDescriptionFileName() + Environment.NewLine + exc.Message);
				}
			}
		}

		public static bool IsFileInExtionFilter(string filePath)
		{
			foreach (string ex in AutobackupExtensionFilters)
				if (filePath.ToLower().EndsWith(ex.ToLower()))
					return true;
			return false;
		}

		private static readonly string[] AutobackupExtensionFilters = new string[] { ".sql", ".xml", ".cs" };
		Dictionary<string, Dictionary<DateTime, FileChangedDetails>> QueuedFileChanges = new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();
		private void fileSystemWatcher_SqlFiles_Changed(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Changed
				&& IsFileInExtionFilter(e.FullPath))
			{
				//if (QueuedFileChanges == null) QueuedFileChanges = new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();
				if (!QueuedFileChanges.ContainsKey(e.FullPath)) QueuedFileChanges.Add(e.FullPath, new Dictionary<DateTime, FileChangedDetails>());
				FileInfo fi = new FileInfo(e.FullPath);
				DateTime lastWrite = fi.LastWriteTime;
				lastWrite = new DateTime(lastWrite.Year, lastWrite.Month, lastWrite.Day, lastWrite.Hour, lastWrite.Minute, lastWrite.Second);
				if (!QueuedFileChanges[e.FullPath].ContainsKey(lastWrite))
				{
					FileChangedDetails fcd = new FileChangedDetails(lastWrite, e.FullPath, null);
					QueuedFileChanges[e.FullPath].Add(lastWrite, fcd);
					string newFileName = fcd.GetBackupFileName();//lastWrite);

					bool successfullyCopied = false;
					int unsuccessfulCount = 0;
					while (!successfullyCopied)
					{
						try
						{
							File.Copy(e.FullPath, newFileName);
							successfullyCopied = true;
						}
						catch (Exception exc)
						{
							unsuccessfulCount++;
							ThreadingInterop.PerformVoidFunctionSeperateThread(() => { System.Threading.Thread.Sleep(500); }, true);
							if (unsuccessfulCount >= 5)
							{
								ShowBalloonTipNotification(exc.Message, Title: "Copy failed 5x", icon: ToolTipIcon.Error);
								break;
							}
						}
					}

					try
					{
						File.SetAttributes(newFileName, FileChangedDetails.BackupAndDescriptionAttributes);
					}
					catch (Exception exc)
					{
						ShowBalloonTipNotification("Exception: " + exc.Message, Title: "Could not set system|hidden attributes", icon: ToolTipIcon.Warning);
					}
					ShowFileChangedBalloonTip(fcd);
				}
				Application.DoEvents();
			}
		}

		private void ShowFileChangedBalloonTip(FileChangedDetails fcd)
		{
			//ShowBalloonTipNotification(fcd.FileName, 3000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			ShowCustomBalloonTipNotification(fcd.OriginalFileName, 3000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			//ShowCustomBalloonTipNotification(fcd.FileName, 1000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			//ShowCustomBalloonTipNotification(fcd.FileName, 2000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			//ShowCustomBalloonTipNotification(fcd.FileName, 10000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			//ShowCustomBalloonTipNotification(fcd.FileName, 3000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			//ShowCustomBalloonTipNotification(fcd.FileName, 500, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			//ShowCustomBalloonTipNotification(fcd.FileName, 1000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
			LastFileChangedDetailsAdded = fcd;
		}

		private void ShowBalloonTipNotification(string Description, int duration = 3000, string Title = "Title", ToolTipIcon icon = ToolTipIcon.Info, BalloonTipActionEnum BalloonTipActionIn = BalloonTipActionEnum.None)
		{
			notifyIcon1.ShowBalloonTip(duration, Title, Description, icon);
			BalloonTipAction = BalloonTipActionIn;
			//TODO: Dink bietjie oor speech input
		}

		private void ShowCustomBalloonTipNotification(string Description, int duration = 3000, string Title = "Title", ToolTipIcon icon = ToolTipIcon.Info, BalloonTipActionEnum BalloonTipActionIn = BalloonTipActionEnum.None)
		{
			CustomBalloonTipwpf.IconTypes iconType =
				icon == ToolTipIcon.Error ? CustomBalloonTipwpf.IconTypes.Error :
				icon == ToolTipIcon.Info ? CustomBalloonTipwpf.IconTypes.Information :
				icon == ToolTipIcon.Warning ? CustomBalloonTipwpf.IconTypes.Warning :
				CustomBalloonTipwpf.IconTypes.None;
			BalloonTipAction = BalloonTipActionIn;
			CustomBalloonTipwpf.ShowCustomBalloonTip(Title, Description, duration, iconType, delegate { PerformBalloonTipClick(); });
		}

		//private void tmpShowPopupToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//  ShowCustomBalloonTip(
		//    "Title",
		//    "Message 123",
		//    3000,
		//    CustomBalloonTip.IconTypes.Shield,
		//    delegate
		//    {
		//      PerformBalloonTipClick();
		//    });
		//}

		public enum BalloonTipActionEnum { ChangedFileList, None };
		public static BalloonTipActionEnum BalloonTipAction = BalloonTipActionEnum.None;
		private FileChangedDetails LastFileChangedDetailsAdded = null;
		private void notifyIcon1_BalloonTipClicked_1(object sender, EventArgs e)
		{
			PerformBalloonTipClick();
		}

		private void PerformBalloonTipClick()
		{
			if (BalloonTipAction == BalloonTipActionEnum.ChangedFileList)
			{
				ShowQueuedMessages();
			}
		}

		public static TreeNode PopulateTreeNode(string path, TreeNode rootnode)
		{
			TreeNode lastNode = null;
			string subPathAgg;
			subPathAgg = string.Empty;
			foreach (string subPath in path.Split('\\'))
			{
				subPathAgg += subPath + '\\';
				TreeNode[] nodes = rootnode.Nodes.Find(subPathAgg, true);
				if (nodes.Length == 0)
					if (lastNode == null)
						lastNode = rootnode.Nodes.Add(subPathAgg, subPath);
					else
						lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
				else
					lastNode = nodes[0];
			}
			return lastNode;
		}

		/*public TreeNode PopulateNodeFromPath(string relativePath, TreeNode node)
		{
			TreeNode returnNode = null;
			while (relativePath.EndsWith("\\")) relativePath = relativePath.Substring(0, relativePath.Length - 1).Trim();
			while (relativePath.StartsWith("\\")) relativePath = relativePath.Substring(1);
			TreeNode t = null;
			if (relativePath.Length > 0)
			{
				string nodeText = relativePath;
				if (relativePath.Contains('\\'))
				{
					nodeText = relativePath.Substring(0, relativePath.LastIndexOf('\\'));
					t = new TreeNode(nodeText);
					//t.Nodes.Add(node);
					returnNode = PopulateNodeFromPath(relativePath.Substring(relativePath.IndexOf('\\') + 1), t) ?? t;
					returnNode.Nodes.Add(node);
				}
				else
				{
					t = new TreeNode(nodeText);
					//t.Nodes.Add(node);
					returnNode = t;
				}
				node.Nodes.Add(t);
			}
			return returnNode;
		}*/

		private TreeNode AddFileNode(TreeView tv, string rootPath, string fullFilePath)
		{
			while (rootPath.EndsWith("\\")) rootPath = rootPath.Substring(0, rootPath.Length - 1);

			TreeNode existingRootNode = null;
			foreach (TreeNode node in tv.Nodes)
				if (node.Text == rootPath)
					existingRootNode = node;

			if (existingRootNode == null)
				existingRootNode = new TreeNode(rootPath);

			string fileDir = fullFilePath.Substring(0, fullFilePath.LastIndexOf('\\'));
			//PopulateTree(fileDir, 
			return null;
		}

		MonitoredFilesChanged formMonitoredFilesChanged;
		private void ShowQueuedMessages()
		{
			if (formMonitoredFilesChanged == null) formMonitoredFilesChanged = new MonitoredFilesChanged();
			if (!formMonitoredFilesChanged.Modal)
			{
				formMonitoredFilesChanged.treeView1.Nodes.Clear();
				if (QueuedFileChanges != null)
				{
					string rootDir = fileSystemWatcher_SqlFiles.Path;
					while (rootDir.EndsWith("\\")) rootDir = rootDir.Substring(0, rootDir.Length - 1);

					TreeNode nodeToSelect = null;
					TreeNode rootDirNode = new TreeNode(rootDir + "\\");
					bool AtleastOneFilechangedNode = false;
					foreach (string file in QueuedFileChanges.Keys)
					{
						/*TreeNode folderNode = null;
						string FilePathwithRootfolderDeleted = file.Substring(rootDir.Length + 1);
						while (FilePathwithRootfolderDeleted.EndsWith("\\")) FilePathwithRootfolderDeleted = FilePathwithRootfolderDeleted.Substring(0, FilePathwithRootfolderDeleted.Length - 1);
						while (FilePathwithRootfolderDeleted.Contains("\\"))
						{
							string tmpFoldername = FilePathwithRootfolderDeleted.Split('\\')[0];
							if (folderNode == null)
								folderNode = new TreeNode(tmpFoldername);
							else
							{
								folderNode.Nodes.Add(new TreeNode(tmpFoldername));
								folderNode = folderNode.Nodes[folderNode.Nodes.Count - 1];
							}
							FilePathwithRootfolderDeleted = FilePathwithRootfolderDeleted.Substring(tmpFoldername.Length + 1);
						}*/
						//TreeNode fileNode = new TreeNode(file.Substring(rootDir.Length + 1) + " (" + QueuedFileChanges[file].Count + ")") { Tag = file, ContextMenuStrip = formMonitoredFilesChanged.contextMenuStrip_TotalFile };// { Tag = QueuedFileChanges[file] };
						//TreeNode fileNode = new TreeNode(file.Split('\\')[file.Split('\\').Length - 1] + " (" + QueuedFileChanges[file].Count + ")") { Tag = file, ContextMenuStrip = formMonitoredFilesChanged.contextMenuStrip_TotalFile };// { Tag = QueuedFileChanges[file] };
						//TreeNode fileNode = PopulateNodeFromPath(file.Substring(rootDir.Length + 1), rootDirNode);
						TreeNode fileNode = PopulateTreeNode(file.Substring(rootDir.Length + 1), rootDirNode);

						int FileQueuedCount = 0;
						foreach (DateTime date in QueuedFileChanges[file].Keys)
						{
							FileChangedDetails fcd = QueuedFileChanges[file][date];
							if (fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Accepted && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Complete && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Discard)
								FileQueuedCount++;
						}

						fileNode.Text = file.Split('\\')[file.Split('\\').Length - 1] + " (" + FileQueuedCount + ")";
						fileNode.Tag = file;
						fileNode.ContextMenu = formMonitoredFilesChanged.contextMenu_TotalFile;
						foreach (DateTime date in QueuedFileChanges[file].Keys)
						{
							FileChangedDetails fcd = QueuedFileChanges[file][date];
							if (fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Accepted && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Complete && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Discard)
							{
								AtleastOneFilechangedNode = true;
								TreeNode fileModifiedNode = new TreeNode(date.ToString(FileChangedDetails.humandfriendlyDateFormat));////MySQLdateformat));
								fileModifiedNode.Tag = fcd;
								fileModifiedNode.ContextMenu = formMonitoredFilesChanged.contextMenu_FileModification;
								fcd.UpdateNodeFontandcolorFromQueueStatus(fileModifiedNode);
								fileNode.Nodes.Add(fileModifiedNode);
								string PathExcludingRoot = file.Substring(rootDir.Length + 2);
								while (PathExcludingRoot.EndsWith("\\")) PathExcludingRoot = PathExcludingRoot.Substring(0, PathExcludingRoot.Length);

								TreeNode NodeToAddFileTo = null;
								while (PathExcludingRoot.Contains('\\'))
								{
									//NodeToAddFileTo = new TreeNode(PathExcludingRoot.Split('\\')[PathExcludingRoot.Split('\\').Length - 1]);
									string nodeText = PathExcludingRoot.Split('\\')[PathExcludingRoot.Split('\\').Length - 1];
									TreeNode tmpSubNode = new TreeNode(nodeText);
									if (NodeToAddFileTo == null) NodeToAddFileTo = tmpSubNode;
									else
									{
										NodeToAddFileTo.Nodes.Add(tmpSubNode);
										NodeToAddFileTo = tmpSubNode;
									}
									PathExcludingRoot = PathExcludingRoot.Substring(nodeText.Length + 1);
								}

								if (fcd == LastFileChangedDetailsAdded) nodeToSelect = fileModifiedNode;
							}
						}
						/*if (folderNode == null) rootDirNode.Nodes.Add(fileNode);
						else
						{
							folderNode.Nodes.Add(fileNode);
							rootDirNode.Nodes.Add(folderNode);
						}*/
					}
					if (AtleastOneFilechangedNode)
					{
						formMonitoredFilesChanged.treeView1.Nodes.Add(rootDirNode);
						rootDirNode.ExpandAll();
					}
					formMonitoredFilesChanged.treeView1.SelectedNode = nodeToSelect;

					if (formMonitoredFilesChanged.treeView1.Nodes.Count == 0)
					{
						ShowBalloonTipNotification("No file changes queued");
						return;
					}

					formMonitoredFilesChanged.richTextBox_Description.Text = "";
					formMonitoredFilesChanged.ShowDialog(this);
					formMonitoredFilesChanged.richTextBox_Description.Enabled = false;
					formMonitoredFilesChanged.AllowTextchangeCallback = false;
					formMonitoredFilesChanged.richTextBox_Description.Text = null;
					formMonitoredFilesChanged.AllowTextchangeCallback = true;
					foreach (string file in QueuedFileChanges.Keys)
					{
						Dictionary<DateTime, FileChangedDetails> changes = QueuedFileChanges[file];
						foreach (DateTime lastWrite in changes.Keys)
						{
							FileChangedDetails fcd = changes[lastWrite];
							if (fcd.QueueStatus == FileChangedDetails.QueueStatusEnum.Accepted)
							{
								//Copy file (add datestring at end)
								//Write file if has description
								//Delete from List?
								if (fcd.Description != null && fcd.Description.Trim().Length > 0)
								{
									string fileName = fcd.GetDescriptionFileName();//lastWrite);
									StreamWriter sw = new StreamWriter(fileName);
									try
									{
										sw.Write(fcd.Description);
									}
									finally
									{
										sw.Close();
										try
										{
											File.SetAttributes(fileName, FileAttributes.System | FileAttributes.Hidden);
										}
										catch (Exception exc)
										{
											ShowBalloonTipNotification("Exception: " + exc.Message, Title: "Could not set system|hidden attributes", icon: ToolTipIcon.Warning);
										}
										fcd.QueueStatus = FileChangedDetails.QueueStatusEnum.Complete;
									}
								}
							}
							else if (fcd.QueueStatus == FileChangedDetails.QueueStatusEnum.Discard)
							{
								try
								{
									//TODO: Eventually add functionality to delete files (according to date, empty description, timeafter previous backup, etc)
									File.Delete(fcd.GetBackupFileName());//lastWrite));
									fcd.QueueStatus = FileChangedDetails.QueueStatusEnum.Complete;
								}
								catch (Exception exc)
								{
									ShowBalloonTipNotification(exc.Message, 3000, "Exception delete file", ToolTipIcon.Warning);
								}
							}
						}
					}
					/*if (formMonitoredFilesChanged.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
					}*/
				}
			}
		}

		public static void Compress(FileInfo fi)
		{
			// Get the stream of the source file.
			using (FileStream inFile = fi.OpenRead())
			{
				// Prevent compressing hidden and 
				// already compressed files.
				if ((File.GetAttributes(fi.FullName)
					& FileAttributes.Hidden)
					!= FileAttributes.Hidden & fi.Extension != ".gz")
				{
					// Create the compressed file.
					using (FileStream outFile =
								File.Create(fi.FullName + ".gz"))
					{
						using (GZipStream Compress =
							new GZipStream(outFile,
							CompressionMode.Compress))
						{
							// Copy the source file into 
							// the compression stream.
							inFile.CopyTo(Compress);

							Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
									fi.Name, fi.Length.ToString(), outFile.Length.ToString());
						}
					}
				}
			}
		}

		public static void Decompress(FileInfo fi)
		{
			// Get the stream of the source file.
			using (FileStream inFile = fi.OpenRead())
			{
				// Get original file extension, for example
				// "doc" from report.doc.gz.
				string curFile = fi.FullName;
				string origName = curFile.Remove(curFile.Length -
						fi.Extension.Length);

				//Create the decompressed file.
				using (FileStream outFile = File.Create(origName))
				{
					using (GZipStream Decompress = new GZipStream(inFile,
									CompressionMode.Decompress))
					{
						// Copy the decompression stream 
						// into the output file.
						Decompress.CopyTo(outFile);

						Console.WriteLine("Decompressed: {0}", fi.Name);

					}
				}
			}
		}

		ViewBackups formViewBackups;
		private void ViewAllBackupsNow()
		{
			if (formViewBackups == null) formViewBackups = new ViewBackups();
			if (!formViewBackups.Modal)
			{
				int countNodesAdded =
					formViewBackups.RefreshNodes(null, fileSystemWatcher_SqlFiles.Path, LastFileChangedDetailsAdded, true);
				if (countNodesAdded == 0)
				{
					ShowBalloonTipNotification("No backup files found");
					return;
				}
				formViewBackups.ShowDialog();

				/*formViewBackups.treeView1.Nodes.Clear();

				Dictionary<string, Dictionary<DateTime, FileChangedDetails>> OriginalFilenamesWithModificationsDict = null;// new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();

				string rootDir = fileSystemWatcher_SqlFiles.Path;
				while (rootDir.EndsWith("\\")) rootDir = rootDir.Substring(0, rootDir.Length - 1);
				string[] backupFiles = Directory.GetFiles(rootDir, "*" + FileChangedDetails.backupExt, SearchOption.AllDirectories);
				foreach (string backupFile in backupFiles)
				{
					//asdads.sql_2011 09 28 (13h29 55).bac
					//"yyyy MM dd (HH\hmm ss)"
					string datePartOfFile = backupFile.Substring(backupFile.LastIndexOf('_') + 1);
					datePartOfFile = datePartOfFile.Substring(0, datePartOfFile.Length - FileChangedDetails.backupExt.Length);
					DateTime dateOut;
					if (DateTime.TryParseExact(datePartOfFile, FileChangedDetails.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOut))
					{
						string fileDescription = "";
						string descriptionFileName = FileChangedDetails.GetDescriptionFileNameFromBackupFilename(backupFile);
						if (File.Exists(descriptionFileName))
							using (StreamReader sr = new StreamReader(descriptionFileName)) { fileDescription = sr.ReadToEnd(); }

						string originalFileName = FileChangedDetails.GetOriginalNameFromBackupFile(backupFile);
						if (OriginalFilenamesWithModificationsDict == null) OriginalFilenamesWithModificationsDict = new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();
						if (!OriginalFilenamesWithModificationsDict.ContainsKey(originalFileName)) OriginalFilenamesWithModificationsDict.Add(originalFileName, new Dictionary<DateTime, FileChangedDetails>());
						if (!OriginalFilenamesWithModificationsDict[originalFileName].ContainsKey(dateOut))
							OriginalFilenamesWithModificationsDict[originalFileName].Add(dateOut, new FileChangedDetails(dateOut, originalFileName, fileDescription));
					}
				}

				if (OriginalFilenamesWithModificationsDict != null)
				{
					TreeNode nodeToSelect = null;
					TreeNode rootDirNode = new TreeNode(rootDir + "\\");
					bool AtleastOneFile = false;

					TreeNode FileNodeWithLastSavedTime = null;
					DateTime LastSavedTimeOfAllFileNodes = DateTime.MinValue;
					foreach (string file in OriginalFilenamesWithModificationsDict.Keys)
					{
						string originalFileName = file.Split('\\')[file.Split('\\').Length - 1];// FileChangedDetails.GetOriginalNameFromBackupFile(file.Split('\\')[file.Split('\\').Length - 1]);
						TreeNode fileNode;
						fileNode = new TreeNode();
						fileNode = PopulateTreeNode(file.Substring(rootDir.Length + 1), rootDirNode);
						//fileNode.NodeFont = new System.Drawing.Font(this.Font.FontFamily, 12, this.Font.Style);
						fileNode.Name = originalFileName;
						fileNode.Text = originalFileName;
						fileNode.Tag = file;

						if (FileNodeWithLastSavedTime == null) FileNodeWithLastSavedTime = fileNode;

						fileNode.ContextMenu = formViewBackups.contextMenu_FileNode;

						bool AtleastOneFileModification = false;
						foreach (DateTime date in OriginalFilenamesWithModificationsDict[file].Keys)
						{
							FileChangedDetails fcd = OriginalFilenamesWithModificationsDict[file][date];
							if (fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Accepted && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Complete)
							{
								AtleastOneFile = true;
								AtleastOneFileModification = true;
								TreeNode fileModifiedNode = new TreeNode(date.ToString("yyyy-MM-dd HH:mm:ss"));
								fileModifiedNode.Tag = fcd;
								fileModifiedNode.ContextMenu = formViewBackups.contextMenu_ModificationNode;
								fileModifiedNode.ForeColor = fcd.GetColorBasedOnDescription();

								fileNode.Nodes.Add(fileModifiedNode);
								string PathExcludingRoot = file.Substring(rootDir.Length + 2);
								while (PathExcludingRoot.EndsWith("\\")) PathExcludingRoot = PathExcludingRoot.Substring(0, PathExcludingRoot.Length);

								if (fcd == LastFileChangedDetailsAdded) nodeToSelect = fileModifiedNode;
							}
						}

						DateTime lastSaveTimeInNode = DateTime.MinValue;
						foreach (TreeNode subnode in fileNode.Nodes)
						{
							if (subnode.Tag is FileChangedDetails)
							{
								FileChangedDetails thisChangedDetails = subnode.Tag as FileChangedDetails;
								if (thisChangedDetails.LastWrite > lastSaveTimeInNode) lastSaveTimeInNode = thisChangedDetails.LastWrite;
								if (thisChangedDetails.LastWrite > LastSavedTimeOfAllFileNodes)
								{
									LastSavedTimeOfAllFileNodes = thisChangedDetails.LastWrite;
									FileNodeWithLastSavedTime = fileNode;
								}
							}
						}

						fileNode.Text = fileNode.Name + "  (" + fileNode.GetNodeCount(false) + ")  " + lastSaveTimeInNode.ToString(FileChangedDetails.humandfriendlyDateFormat);

						TreeNode parentNode = fileNode.Parent;
						while (parentNode != null)
						{
							parentNode.Expand();
							//parentNode.NodeFont = new Font(formViewBackups.treeView1.Font.FontFamily, 8);
							parentNode.ForeColor = Color.LightGray;
							parentNode = parentNode.Parent;
						}
						//fileNode.NodeFont = new Font(formViewBackups.treeView1.Font.FontFamily, 8);
					}
					if (FileNodeWithLastSavedTime != null) FileNodeWithLastSavedTime.ForeColor = Color.Green;

					if (AtleastOneFile)
					{
						formViewBackups.treeView1.Nodes.Add(rootDirNode);
						rootDirNode.Expand();//.ExpandAll);
					}
					formViewBackups.treeView1.SelectedNode = nodeToSelect;

					if (formViewBackups.treeView1.Nodes.Count == 0)
					{
						ShowBalloonTipNotification("No backup files found");
						return;
					}

					formViewBackups.ShowDialog();
				}
				//formViewBackups.ShowDialog();*/
			}
		}

		public static void UpdateGuiFromThread(Control controlToUpdate, Action action)
		{
			if (controlToUpdate.InvokeRequired)
				controlToUpdate.Invoke(action, new object[] { });
			else action.Invoke();
		}

		private static void ShowInactiveTopmost(Form frm)
		{
			Win32Api.ShowWindow(frm.Handle, Win32Api.SW_SHOWNOACTIVATE);
			Win32Api.SetWindowPos(frm.Handle.ToInt32(), Win32Api.HWND_TOPMOST, frm.Left, frm.Top, frm.Width, frm.Height, Win32Api.SWP_NOACTIVATE);
		}

		private void menuItem_DeleteThisItem_Click(object sender, EventArgs e)
		{
			if (UserMessages.Confirm(this, "Are you sure you want to delete " + treeViewTodolist.SelectedNode.Text + "?", "Confirm delete"))
			{
				bool successfulDelete = PerformDesktopAppDoTask(
						PhpInterop.Username,
						"deleteitem",
						new List<string>()
                {
                    treeViewTodolist.SelectedNode.Parent.Parent.Text,
                    treeViewTodolist.SelectedNode.Parent.Text,
                    treeViewTodolist.SelectedNode.Text,
                },
						true,
						"1");
				appendLogTextbox("Successfully deleted item: " + treeViewTodolist.SelectedNode.Text);
				treeViewTodolist.SelectedNode.Remove();
			}
		}

		private void menuItem_AddItemToThisCategory_Click(object sender, EventArgs e)
		{
			AddTodoItemNow(treeViewTodolist.SelectedNode.Parent.Text, treeViewTodolist.SelectedNode.Text);
		}

		private void label5_Click(object sender, EventArgs e)
		{
			OwnUnhandledExceptionHandler.TestCrash(true, (msg) => UserMessages.Confirm(msg));
		}
	}



	public class ItemDetails
	{
		public string Description;
		public bool Complete;
		public DateTime Due;
		public DateTime Created;
		public int RemindedCount;
		public bool StopSnooze;
		public int AutosnoozeInterval;
		public ItemDetails(string Description, bool Complete, DateTime Due, DateTime Created, int RemindedCount, bool StopSnooze, int AutosnoozeInterval)
		{
			this.Description = Description;
			this.Complete = Complete;
			this.Due = Due;
			this.Created = Created;
			this.RemindedCount = RemindedCount;
			this.StopSnooze = StopSnooze;
			this.AutosnoozeInterval = AutosnoozeInterval;
		}

		public string DetailsToStringBlock()
		{
			return string.Format("Complete: {0}\r\nDue: {1}\r\nCreated: {2}\r\nRemindedCount: {3}\r\nStopSnooze: {4}\r\nAutosnoozeInterval: {5}", Complete, Due, Created, RemindedCount, StopSnooze, AutosnoozeInterval);
		}
	}
}