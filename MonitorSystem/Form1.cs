using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//using System.Threading;

namespace MonitorSystem
{
	public partial class Form1 : Form
	{
		private static string ThisAppName = "MonitorSystem";
		//public static readonly string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "FJH" + "\\" + ThisAppName;
		public static string SavedListFileName = WindowsInterop.LocalAppDataPath + "\\EmailAndPasswordList.fjset";
		public static string LastAutobackupStateFileName =  WindowsInterop.LocalAppDataPath + "\\LastAutobackupState.fjset";

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

		public static string AutoBackupDir
		{
			get
			{
				if (Directory.Exists(@"C:\ProgramData\GLS\ReportSQLqueries"))
					return @"C:\ProgramData\GLS\ReportSQLqueries";
				else if (Directory.Exists(@"C:\Francois\other\Test\SqlFilesAutobackup"))
					return @"C:\Francois\other\Test\SqlFilesAutobackup";
				else return @"c:\windows\system32";
			}
		}

		public Form1()
		{
			try { Win32Api._fpreset(); }
			catch { }

			InitializeComponent();
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

			RegisterSnarl();
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

			PopulateNotifyIconContextMenu();
		}

		private void PopulateNotifyIconContextMenu()
		{
			MenuItem addBackupDescriptionMenuItem = new MenuItem("Add backup &description", delegate
				{
					AddBackupDescription formAddBackupDescription = new AddBackupDescription();
					formAddBackupDescription.Show();
				});
			MenuItem exitMenuItem = new MenuItem("E&xit", delegate { RequestApplicationQuit(); });
			MenuItem viewallbackupsMenuItem = new MenuItem("View &all backups", delegate { ViewAllBackupsNow(); });
			MenuItem testcustomballoontipMenuItem = new MenuItem("Test &custom balloontip", delegate { ShowCustomBalloonTip("Hallo", "This is a custom balloon tip for 3 seconds...", 3000, CustomBalloonTip.IconTypes.Shield); });

			notifyIcon1.ContextMenu = new ContextMenu(new MenuItem[]
			{
				addBackupDescriptionMenuItem,
				viewallbackupsMenuItem,
				new MenuItem("-"),
				exitMenuItem,				
				testcustomballoontipMenuItem
			});
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
							MessageBox.Show("Could not get date from pipesplit 1: " + line);
							success = false;
							continue;
						}
						FileChangedDetails.QueueStatusEnum queueStatus;
						if (!Enum.TryParse(line.Split('|')[2], true, out queueStatus))
						{
							MessageBox.Show("Could not get QueueStatus from pipesplit 2: " + line);
							success = false;
							continue;
						}
						if (!QueuedFileChanges.ContainsKey(fullPath)) QueuedFileChanges.Add(fullPath, new Dictionary<DateTime, FileChangedDetails>());
						FileInfo fi = new FileInfo(fullPath);
						lastWrite = new DateTime(lastWrite.Year, lastWrite.Month, lastWrite.Day, lastWrite.Hour, lastWrite.Minute, lastWrite.Second);
						if (!QueuedFileChanges[fullPath].ContainsKey(lastWrite))
						{
							FileChangedDetails fcd = new FileChangedDetails(fullPath, null);
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
			tmpList = TextFilesInterop.GetLinesFromTextFile(SavedListFileName);
			if (tmpList.Count > 0) currentEmailPasswordAndRegexList = tmpList;
		}

		private void RegisterSnarl()
		{
			try
			{
				System.Diagnostics.Process.Start(@"C:\Program Files (x86)\full phat\Snarl\tools\heysnarl.exe", "register?app-sig=app/" + ThisAppName + "&title=MonitorSystem in C#");
			}
			catch { }
		}

		private void NotifySnarl(string title, string msg)
		{
			try
			{
				System.Diagnostics.Process.Start(@"C:\Program Files (x86)\full phat\Snarl\tools\heysnarl.exe", "notify?app-sig=app/" + ThisAppName + "&title=" + title + "&text=" + msg);
			}
			catch { }
		}

		private void UnregisterSnarl()
		{
			try
			{
				System.Diagnostics.Process.Start(@"C:\Program Files (x86)\full phat\Snarl\tools\heysnarl.exe", "unregister?app-sig=app/" + ThisAppName);
			}
			catch { }
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			this.ShowInTaskbar = true;
			this.WindowState = FormWindowState.Minimized;

			//InitializeHooks(true, true);
			//notifyIcon1.ShowBalloonTip(3000, "Hooks disabled", "Hooks were disabled in code", ToolTipIcon.Info);
			if (!Win32Api.RegisterHotKey(this.Handle, Win32Api.Hotkey1, Win32Api.MOD_WIN, (int)Keys.Q)) MessageBox.Show("QuickAccess could not register hotkey WinKey + Q");

			ReadLastQueuedStatusIfFileExist();

			StylingInterop.SetTreeviewVistaStyle(treeViewTodolist);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == Win32Api.WM_HOTKEY)
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
			actHook.OnMouseActivity += new MouseEventHandler(actHook_OnMouseActivity);
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
		void actHook_OnMouseActivity(object sender, MouseEventArgs e)
		{
			switch (e.Button)
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
						string tmpRegex = DecodeStringHex(RegexUsernamePassword.Split('|')[0]);
						string tmpUsername = DecodeStringHex(RegexUsernamePassword.Split('|')[1]);
						string tmpPassword = DecodeStringHex(RegexUsernamePassword.Split('|')[2]);

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
						string tmpRegex = DecodeStringHex(RegexUsernamePassword.Split('|')[0]);
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
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.Activate();
			notifyIcon1.Visible = false;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("explorer", @"C:\Francois\tmp\Screenshots");
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
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
				PerformVoidFunctionSeperateThread(() =>
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
				Application.Exit();
			}
			else
				ShowBalloonTipNotification("Please process unread notifications", BalloonTipActionIn: BalloonTipActionEnum.ChangedFileList);
			return false;
		}

		private void linkLabel_AddEmailAndPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			currentEmailPasswordAndRegexList = TextFilesInterop.GetLinesFromTextFile(SavedListFileName);
			string NewRegexEmailAndPasswordString = GetNewEmailAndPassword();
			if (NewRegexEmailAndPasswordString != null)
				currentEmailPasswordAndRegexList.Add(EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[0]) + "|" + EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[1]) + "|" + EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[2]));
			TextFilesInterop.WriteLinesToTextFile(SavedListFileName, currentEmailPasswordAndRegexList);
			RefreshRegexList();
			foreach (string RegexUsernamePassword in currentEmailPasswordAndRegexList)
			{
				string tmpRegex = DecodeStringHex(RegexUsernamePassword.Split('|')[0]);
				string tmpUsername = DecodeStringHex(RegexUsernamePassword.Split('|')[1]);
				string tmpPassword = DecodeStringHex(RegexUsernamePassword.Split('|')[2]);
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

		static string password = "abcdefhkiljqwpmz";
		private string EncodeStringHex(string StringToEncode)
		{
			string tmpstr = "";
			foreach (char c in StringToEncode.ToCharArray())
			{
				int remainder;
				int div;
				try
				{
					if ((int)c == 8211) div = Math.DivRem((int)'-', 16, out remainder);
					else div = Math.DivRem((int)c, 16, out remainder);
					tmpstr += password[div].ToString() + password[remainder].ToString();
				}
				catch (Exception exc)
				{
					MessageBox.Show("Error, could not encode hex, char " + c.ToString() + ", (int)char = " + (int)c + ": " + Environment.NewLine + exc.Message, "Exception error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return tmpstr;
		}

		//enum StringTypeEnum { Regex, Username, Password };
		private string DecodeStringHex(string StringToDecode)//, StringTypeEnum StringType)
		{
			string tmpstr = "";
			for (int i = 0; i <= StringToDecode.Length - 2; i = i + 2)
			{
				tmpstr += (char)(password.IndexOf(StringToDecode[i]) * 16 + password.IndexOf(StringToDecode[i + 1]));
			}
			return tmpstr;
		}

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

		//PerformVoidFunctionSeperateThread(() => { MessageBox.Show("Test"); MessageBox.Show("Test1"); });
		public static void PerformVoidFunctionSeperateThread(MethodInvoker method, bool WaitUntilFinish = true)
		{
			System.Threading.Thread th = new System.Threading.Thread(() =>
			{
				method.Invoke();
			});
			th.Start();
			//th.Join();
			if (WaitUntilFinish)
				while (th.IsAlive) { Application.DoEvents(); }
		}

		private string GetPrivateKey()
		{
			try
			{
				toolStripStatusLabelCurrentStatus.Text = "Obtaining pvt key...";
				string tmpkey = null;

				PerformVoidFunctionSeperateThread(() =>
				{
					tmpkey = PostPHP(PhpInterop.ServerAddress + "/generateprivatekey.php", "username=" + PhpInterop.Username + "&password=" + PhpInterop.Password);
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

		private void AddTodoItemNow(string Category = null, string Subcat = null)
		{
			AddTodoItem addform = new AddTodoItem(Category, Subcat);
			if (addform.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				bool successfulAdd = PerformDesktopAppDoTask(
						PhpInterop.Username,
						"addtolist",
						new List<string>()
                {
                    addform.textBoxCategory.Text,
                    addform.textBoxSubcat.Text,
                    addform.textBoxItems.Text,
                    addform.textBoxDescription.Text,
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
							addform.textBoxCategory.Text,
							addform.textBoxSubcat.Text,
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
			if (line.Contains('\t') && line.Split('\t').Length >= 4)
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
				AdditemToTreeview(tmpCategory, tmpSubcat, tmpItems, tmpDescription, tmpCompleted, tmpDue, tmpCreated, tmpRemindedCount, tmpStopSnooze, tmpAutosnoozeInterval);
			}
			else MessageBox.Show("The following line is invalid todo line: " + line);
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

		/// <summary>
		/// Post data to php, maximum length of data is 8Mb
		/// </summary>
		/// <param name="url">The url of the php, do not include the ?</param>
		/// <param name="data">The data, i.e. "name=koos&surname=koekemoer". Note to not include the ?</param>
		/// <returns>Returns the data received from the php (usually the "echo" statements in the php.</returns>
		public string PostPHP(string url, string data)
		{
			string vystup = "";
			try
			{
				data = data.Replace("+", "[|]");
				//Our postvars
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				//Initialisation, we use localhost, change if appliable
				HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
				//Our method is post, otherwise the buffer (postvars) would be useless
				WebReq.Method = "POST";
				//We use form contentType, for the postvars.
				WebReq.ContentType = "application/x-www-form-urlencoded";
				//The length of the buffer (postvars) is used as contentlength.
				WebReq.ContentLength = buffer.Length;
				//We open a stream for writing the postvars
				Stream PostData = WebReq.GetRequestStream();
				//Now we write, and afterwards, we close. Closing is always important!
				PostData.Write(buffer, 0, buffer.Length);
				PostData.Close();
				//Get the response handle, we have no true response yet!
				HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
				//Let's show some information about the response
				//System.Windows.Forms.MessageBox.Show(WebResp.StatusCode.ToString());
				//System.Windows.Forms.MessageBox.Show(WebResp.Server);

				//Now, we read the response (the string), and output it.
				Stream Answer = WebResp.GetResponseStream();
				StreamReader _Answer = new StreamReader(Answer);
				vystup = _Answer.ReadToEnd();

				//Congratulations, you just requested your first POST page, you
				//can now start logging into most login forms, with your application
				//Or other examples.
				string tmpresult = vystup.Trim() + "\n";
			}
			catch (Exception exc)
			{
				if (!exc.Message.ToUpper().StartsWith("The remote name could not be resolved:".ToUpper()))
					//LoggingClass.AddToLogList(UserMessages.MessageTypes.PostPHP, exc.Message);
					appendLogTextbox("Post php: " + exc.Message);
				else //LoggingClass.AddToLogList(UserMessages.MessageTypes.PostPHPremotename, exc.Message);
					appendLogTextbox("Post php remote name: " + exc.Message);
				//SysWinForms.MessageBox.Show("Error (092892): " + Environment.NewLine + exc.Message, "Exception error", SysWinForms.MessageBoxButtons.OK, SysWinForms.MessageBoxIcon.Error);
			}
			return vystup;
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
						PerformVoidFunctionSeperateThread(() =>
						{
							string ArgumentListTabSeperated = "";
							foreach (string s in ArgumentList)
								ArgumentListTabSeperated += (ArgumentListTabSeperated.Length > 0 ? "\t" : "") + s;

							addrequest = (HttpWebRequest)WebRequest.Create(PhpInterop.doWorkAddress + "/dotask/" +
									PhpEncryption.SimpleTripleDesEncrypt(UsernameIn, "123456789abcdefghijklmno") + "/" +
									PhpEncryption.SimpleTripleDesEncrypt(TaskName, tmpkey) + "/" +
									PhpEncryption.SimpleTripleDesEncrypt(ArgumentListTabSeperated, tmpkey));// + "/");
							//appendLogTextbox(addrequest.RequestUri.ToString());
							try
							{
								addresponse = (HttpWebResponse)addrequest.GetResponse();
								input = new StreamReader(addresponse.GetResponseStream());
								encryptedstring = input.ReadToEnd();
								//appendLogTextbox("Encrypted response: " + encryptedstring);

								decryptedstring = PhpEncryption.SimpleTripleDesDecrypt(encryptedstring, tmpkey);
								//appendLogTextbox("Decrypted response: " + decryptedstring);
								decryptedstring = decryptedstring.Replace("\0", "").Trim();
								//MessageBox.Show(this, decryptedstring);
								if (MustWriteResultToLogsTextbox) appendLogTextbox("Result for " + TaskName + ": " + decryptedstring);
								mustreturn = true;

								//else
								//{
								//    //appendLogTextbox("Uploading changes failed: " + decryptedstring);
								//    string s = "";
								//    foreach (char c in decryptedstring.ToCharArray()) s += (int)c + ",";
								//    appendLogTextbox("Uploading changes failed: " + s);
								//}
							}
							catch (Exception exc) { MessageBox.Show(this, "Exception:" + exc.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

			PerformVoidFunctionSeperateThread(() =>
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
			public string FileName;
			public string Description;
			public QueueStatusEnum QueueStatus;
			public FileChangedDetails(string FileNameIn, string DescriptionIn, QueueStatusEnum QueueStatusIn = QueueStatusEnum.New)
			{
				FileName = FileNameIn;
				Description = DescriptionIn;
				QueueStatus = QueueStatusIn;
			}

			private bool IsStringEmpty(string str)
			{
				return str == null || str.Trim().Length == 0;
			}

			public void UpdateNodeFontandcolorFromQueueStatus(TreeNode node)
			{
				SetNewQueueStatusAndUpdateNodeFontandcolor(QueueStatus, node);
			}

			public void SetNewQueueStatusAndUpdateNodeFontandcolor(QueueStatusEnum QueueStatusIn, TreeNode node)
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
				node.NodeFont = new Font(FontFamily.GenericSansSerif, 12, nodeFontStyle);
			}

			public static string dateFormat = @"yyyy MM dd (HH\hmm ss)";
			public static string SavetofileDateFormat = "yyyyMMddHHmmss";
			private string GetFileNameStart(DateTime lastWriteTime)
			{
				return FileName + "_" + lastWriteTime.ToString(dateFormat);
			}

			public static string backupExt = ".bac";
			public static string descrExt = ".desc";
			public string GetBackupFileName(DateTime lastWriteTime)
			{
				return GetFileNameStart(lastWriteTime) + backupExt;
			}

			public string GetDescriptionFileName(DateTime lastWriteTime)
			{
				return GetFileNameStart(lastWriteTime) + descrExt;
			}
		}

		private bool IsFileInExtionFilter(string filePath)
		{
			foreach (string ex in AutobackupExtensionFilters)
				if (filePath.ToLower().EndsWith(ex.ToLower()))
					return true;
			return false;
		}

		private readonly string[] AutobackupExtensionFilters = new string[] { ".sql", ".xml" };
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
					FileChangedDetails fcd = new FileChangedDetails(e.FullPath, null);
					QueuedFileChanges[e.FullPath].Add(lastWrite, fcd);
					string newFileName = fcd.GetBackupFileName(lastWrite);

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
							PerformVoidFunctionSeperateThread(() => { System.Threading.Thread.Sleep(500); }, true);
							if (unsuccessfulCount >= 5)
							{
								ShowBalloonTipNotification(exc.Message, Title: "Copy failed 5x", icon: ToolTipIcon.Error);
								break;
							}
						}
					}

					try
					{
						File.SetAttributes(newFileName, FileAttributes.System | FileAttributes.Hidden);
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
			ShowCustomBalloonTipNotification(fcd.FileName, 3000, "File changed, click to add description", ToolTipIcon.Info, BalloonTipActionEnum.ChangedFileList);
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
		}

		private void ShowCustomBalloonTipNotification(string Description, int duration = 3000, string Title = "Title", ToolTipIcon icon = ToolTipIcon.Info, BalloonTipActionEnum BalloonTipActionIn = BalloonTipActionEnum.None)
		{
			CustomBalloonTip.IconTypes iconType =
				icon == ToolTipIcon.Error ? CustomBalloonTip.IconTypes.Error :
				icon == ToolTipIcon.Info ? CustomBalloonTip.IconTypes.Information :
				icon == ToolTipIcon.Warning ? CustomBalloonTip.IconTypes.Warning :
				CustomBalloonTip.IconTypes.None;
			BalloonTipAction = BalloonTipActionIn;
			ShowCustomBalloonTip(Title, Description, duration, iconType);
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

		enum BalloonTipActionEnum { ChangedFileList, None };
		private BalloonTipActionEnum BalloonTipAction = BalloonTipActionEnum.None;
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

		public TreeNode PopulateNodeFromPath(string relativePath, TreeNode node)
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
					nodeText = relativePath.Substring(0, relativePath.IndexOf('\\'));
					t = new TreeNode(nodeText);
					returnNode = PopulateNodeFromPath(relativePath.Substring(relativePath.IndexOf('\\') + 1), t) ?? t;
				}
				else
				{
					t = new TreeNode(nodeText);
					returnNode = t;
				}
				node.Nodes.Add(t);
			}
			return returnNode;
		}

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
						TreeNode fileNode = PopulateNodeFromPath(file.Substring(rootDir.Length + 1), rootDirNode);

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
								TreeNode fileModifiedNode = new TreeNode(date.ToString("yyyy-MM-dd HH:mm:ss"));
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

					formMonitoredFilesChanged.ShowDialog();
					formMonitoredFilesChanged.textBox1.Enabled = false;
					formMonitoredFilesChanged.AllowTextchangeCallback = false;
					formMonitoredFilesChanged.textBox1.Text = null;
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
									string fileName = fcd.GetDescriptionFileName(lastWrite);
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
									File.Delete(fcd.GetBackupFileName(lastWrite));
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
				formViewBackups.treeView1.Nodes.Clear();

				Dictionary<string, Dictionary<DateTime, FileChangedDetails>> FilesDict = null;// new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();

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
						string fileText = "";
						using (StreamReader sr = new StreamReader(backupFile)) { fileText = sr.ReadToEnd(); }

						if (FilesDict == null) FilesDict = new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();
						if (!FilesDict.ContainsKey(backupFile)) FilesDict.Add(backupFile, new Dictionary<DateTime, FileChangedDetails>());
						if (!FilesDict[backupFile].ContainsKey(dateOut))
							FilesDict[backupFile].Add(dateOut, new FileChangedDetails(backupFile, fileText));
					}
				}

				if (FilesDict != null)
				{
					TreeNode nodeToSelect = null;
					TreeNode rootDirNode = new TreeNode(rootDir + "\\");
					bool AtleastOneFilechangedNode = false;
					foreach (string file in FilesDict.Keys)
					{
						TreeNode fileNode = PopulateNodeFromPath(file.Substring(rootDir.Length + 1), rootDirNode);
						fileNode.Text = file.Split('\\')[file.Split('\\').Length - 1];
						fileNode.Tag = file;
						//fileNode.ContextMenuStrip = formViewBackups.contextMenuStrip_TotalFile;
						foreach (DateTime date in FilesDict[file].Keys)
						{
							FileChangedDetails fcd = FilesDict[file][date];
							if (fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Accepted && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Complete)
							{
								AtleastOneFilechangedNode = true;
								TreeNode fileModifiedNode = new TreeNode(date.ToString("yyyy-MM-dd HH:mm:ss"));
								fileModifiedNode.Tag = fcd;
								//fileModifiedNode.ContextMenuStrip = formViewBackups.contextMenuStrip_FileModification;
								fcd.UpdateNodeFontandcolorFromQueueStatus(fileModifiedNode);
								fileNode.Nodes.Add(fileModifiedNode);
								string PathExcludingRoot = file.Substring(rootDir.Length + 2);
								while (PathExcludingRoot.EndsWith("\\")) PathExcludingRoot = PathExcludingRoot.Substring(0, PathExcludingRoot.Length);

								TreeNode NodeToAddFileTo = null;
								while (PathExcludingRoot.Contains('\\'))
								{
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

						if (AtleastOneFilechangedNode)
						{
							rootDirNode.Nodes.Add(fileNode);
							formViewBackups.treeView1.Nodes.Add(rootDirNode);
							rootDirNode.ExpandAll();
						}
					}
					formViewBackups.treeView1.SelectedNode = nodeToSelect;

					if (formViewBackups.treeView1.Nodes.Count == 0)
					{
						ShowBalloonTipNotification("No file changes queued");
						return;
					}

					formViewBackups.ShowDialog();
				}
				//formViewBackups.ShowDialog();
			}
		}

		public static void UpdateGuiFromThread(Control controlToUpdate, Action action)
		{
			if (controlToUpdate.InvokeRequired)
				controlToUpdate.Invoke(action, new object[] { });
			else action.Invoke();
		}

		public delegate void SimpleDelegate();
		//delegate void MoveWindowUpCallback();
		List<CustomBalloonTip> VisibleBalloonTipForms = new List<CustomBalloonTip>();
		private void ShowCustomBalloonTip(string Title, string Message, int Duration, CustomBalloonTip.IconTypes iconType)
		{
			CustomBalloonTip cbt = new CustomBalloonTip(Title, Message, Duration, iconType, delegate { PerformBalloonTipClick(); });
			//this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width, Screen.PrimaryScreen.WorkingArea.Bottom - this.Height);
			int TopStart = 0;
			foreach (CustomBalloonTip tmpVisibleFrms in VisibleBalloonTipForms)
				if (tmpVisibleFrms != null && tmpVisibleFrms.Visible)
					TopStart += tmpVisibleFrms.Height;
			int gapFromSide = 100;
			cbt.Location = new Point(Screen.PrimaryScreen.WorkingArea.Left + gapFromSide, Screen.PrimaryScreen.WorkingArea.Top + TopStart - cbt.Height);
			cbt.Width = Screen.PrimaryScreen.WorkingArea.Width - gapFromSide * 2;
			cbt.FormClosed += delegate
			{
				if (VisibleBalloonTipForms.Contains(cbt))
				{
					int indexOfRemoved = VisibleBalloonTipForms.IndexOf(cbt);
					int cbtHeight = cbt.Height;
					for (int i = indexOfRemoved + 1; i < VisibleBalloonTipForms.Count; i++)
					{
						CustomBalloonTip tmpForm = VisibleBalloonTipForms[i];
						PerformVoidFunctionSeperateThread(() =>
						{
							//int StartPoint = VisibleBalloonTipForms[i].Top;
							int EndPoint = tmpForm.Top - cbtHeight;
							while (tmpForm.Top > EndPoint)
							{
								System.Threading.Thread.Sleep(10);
								UpdateGuiFromThread(tmpForm, () =>
								{
									if (tmpForm.Top - 5 > EndPoint) tmpForm.Top -= 5;
									else tmpForm.Top = EndPoint;
								});
								//Action SlideUpAction = (Action)(() =>
								//{
								//  if (tmpForm.Top - 5 > EndPoint) tmpForm.Top -= 5;
								//  else tmpForm.Top = EndPoint;
								//});
								//if (tmpForm.InvokeRequired)
								//  tmpForm.Invoke(SlideUpAction, new object[] { });
								//else SlideUpAction.Invoke();
								//VisibleBalloonTipForms[i].Top -= 5;
							}
						}, false);
						//VisibleBalloonTipForms[i].Top -= cbt.Height;
					}
					VisibleBalloonTipForms.Remove(cbt);
					//foreach (
				}
			};
			VisibleBalloonTipForms.Add(cbt);
			cbt.Show();
			//ShowInactiveTopmost(cbt);
			//cbt.StartTimerForClosing();
			//cbt.Show();
		}

		private static void ShowInactiveTopmost(Form frm)
		{
			Win32Api.ShowWindow(frm.Handle, Win32Api.SW_SHOWNOACTIVATE);
			Win32Api.SetWindowPos(frm.Handle.ToInt32(), Win32Api.HWND_TOPMOST, frm.Left, frm.Top, frm.Width, frm.Height, Win32Api.SWP_NOACTIVATE);
		}

		private void menuItem_DeleteThisItem_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(this, "Are you sure you want to delete " + treeViewTodolist.SelectedNode.Text + "?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
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

	public class StylingInterop
	{
		public const int TV_FIRST = 0x1100;
		public const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
		public const int TVM_GETEXTENDEDSTYLE = TV_FIRST + 45;
		public const int TVS_EX_AUTOHSCROLL = 0x0020;
		public const int TVS_EX_FADEINOUTEXPANDOS = 0x0040;

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		internal static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

		public static void SetVistaStyleOnControlHandle(IntPtr ControlHandle)
		{
			SetWindowTheme(ControlHandle, "explorer", null);
		}

		public static void SetTreeviewVistaStyle(TreeView treeview)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
			{
				Func<TreeView, bool> SetTreeviewVistaStyleNow = new Func<TreeView, bool>((tv) =>
				{
					int dw = SendMessage(tv.Handle, TVM_GETEXTENDEDSTYLE, 0, 0);

					// Update style
					dw |= TVS_EX_AUTOHSCROLL;       // autoscroll horizontaly
					//dw |= TVS_EX_FADEINOUTEXPANDOS; // auto hide the +/- signs

					// set style
					SendMessage(tv.Handle, TVM_SETEXTENDEDSTYLE, 0, dw);

					// little black/empty arrows and blue highlight on treenodes
					SetVistaStyleOnControlHandle(tv.Handle);
					return true;
				});

				if (treeview.Handle != IntPtr.Zero)
					SetTreeviewVistaStyleNow.Invoke(treeview);
				else
					treeview.HandleCreated += delegate { SetTreeviewVistaStyleNow(treeview); };
			}
		}
	}

	public class PhpEncryption
	{
		public static string SimpleTripleDesEncrypt(string Data, string keystring)
		{
			byte[] key = Encoding.ASCII.GetBytes(keystring);
			byte[] iv = Encoding.ASCII.GetBytes("password");
			byte[] data = Encoding.ASCII.GetBytes(Data);
			byte[] enc = new byte[0];
			TripleDES tdes = TripleDES.Create();
			tdes.IV = iv;
			tdes.Key = key;
			tdes.Mode = CipherMode.CBC;
			tdes.Padding = PaddingMode.Zeros;
			ICryptoTransform ict = tdes.CreateEncryptor();
			enc = ict.TransformFinalBlock(data, 0, data.Length);
			return ByteArrayToString(enc);
		}

		public static string SimpleTripleDesDecrypt(string Data, string keystring)
		{
			byte[] key = Encoding.ASCII.GetBytes(keystring);
			byte[] iv = Encoding.ASCII.GetBytes("password");
			byte[] data = StringToByteArray(Data);
			byte[] enc = new byte[0];
			TripleDES tdes = TripleDES.Create();
			tdes.IV = iv;
			tdes.Key = key;
			tdes.Mode = CipherMode.CBC;
			tdes.Padding = PaddingMode.Zeros;
			ICryptoTransform ict = tdes.CreateDecryptor();
			enc = ict.TransformFinalBlock(data, 0, data.Length);
			return Encoding.ASCII.GetString(enc);
		}

		public static string ByteArrayToString(byte[] ba)
		{
			string hex = BitConverter.ToString(ba);
			return hex.Replace("-", "");
		}

		public static string StringToHex(string stringIn)
		{
			return PhpEncryption.ByteArrayToString(Encoding.Default.GetBytes(stringIn));
		}

		public static byte[] StringToByteArray(String hex)
		{
			int NumberChars = hex.Length;
			byte[] bytes = new byte[NumberChars / 2];
			for (int i = 0; i < NumberChars; i += 2)
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			return bytes;
		}

		public static string HexToString(string hexIn)
		{
			return Encoding.Default.GetString(StringToByteArray(hexIn));
		}
	}

	internal struct LASTINPUTINFO
	{
		public uint cbSize;

		public uint dwTime;
	}

	/// <summary>
	/// Summary description for Win32.
	/// </summary>
	public class Win32
	{
		[DllImport("User32.dll")]
		public static extern bool LockWorkStation();

		[DllImport("User32.dll")]
		private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		[DllImport("Kernel32.dll")]
		private static extern uint GetLastError();

		public static uint GetIdleTime()
		{
			LASTINPUTINFO lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
			GetLastInputInfo(ref lastInPut);

			return ((uint)Environment.TickCount - lastInPut.dwTime);
		}

		public static long GetTickCount()
		{
			return Environment.TickCount;
		}

		public static long GetLastInputTime()
		{
			LASTINPUTINFO lastInPut = new LASTINPUTINFO();
			lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
			if (!GetLastInputInfo(ref lastInPut))
			{
				throw new Exception(GetLastError().ToString());
			}

			return lastInPut.dwTime;
		}


	}
}