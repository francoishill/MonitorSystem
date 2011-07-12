using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Security.Cryptography;
//using System.Threading;

namespace MonitorSystem
{
    public partial class Form1 : Form
    {
        /// <summary>The GetForegroundWindow function returns a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("msvcr70.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int _fpreset();

        private static string ThisAppName = "MonitorSystem";
        public static readonly string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "FJH" + "\\" + ThisAppName;
        public static string SavedListFileName = LocalAppDataPath + "\\EmailAndPasswordList.fjset";

        private const string ServerAddress = "http://localhost";
        //private const string ServerAddress = "https://fjh.co.za";
        private const string doWorkAddress = ServerAddress + "/other/codeigniter/index.php/desktopapp";
        private string Username = "f";
        private string Password = "f";

        List<string> currentEmailPasswordAndRegexList = new List<string>();

        //Timer timer = new Timer();

        public Form1()
        {
            try { _fpreset(); }
            catch { }

            InitializeComponent();

            if (!Directory.Exists(LocalAppDataPath)) Directory.CreateDirectory(LocalAppDataPath);

            //timer.Interval = 10000;
            //timer.Start();
            //timer.Tick += new EventHandler(timer_Tick);

            notifyIcon1.BalloonTipClicked += new EventHandler(notifyIcon1_BalloonTipClicked);

            if (!System.Diagnostics.Debugger.IsAttached && Environment.GetCommandLineArgs()[0].ToUpper().Contains("Apps\\2.0".ToUpper()))
            {
                Microsoft.Win32.RegistryKey regkeyRUN = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                regkeyRUN.SetValue(ThisAppName, "\"" + System.Windows.Forms.Application.ExecutablePath + "\"", Microsoft.Win32.RegistryValueKind.String);
            }

            RegisterSnarl();
            //NotifySnarl("Halo", "How are you");

            RefreshRegexList();

            //SnarlNetworkProtocol.SNP snarl = new SnarlNetworkProtocol.SNP();
            //snarl.register("127.0.0.1", 8000, "MonitorSystemCS");
            //snarl.addClass("127.0.0.1", 8000, "MonitorSystemCS", "Test", "TestTitle");
            //snarl.notify("127.0.0.1", 8000, "MonitorSystemCS", "Test", "TestTitle", "Text", "1000");
        }

        private void RefreshRegexList()
        {
            List<string> tmpList = new List<string>();
            tmpList = GetLinesFromTextFile(SavedListFileName);
            if (tmpList.Count > 0) currentEmailPasswordAndRegexList = tmpList;
        }

        private void RegisterSnarl()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\full phat\Snarl\tools\heysnarl.exe", "register?app-sig=app/" + ThisAppName + "&title=MonitorSystem in C#");
        }

        private void NotifySnarl(string title, string msg)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\full phat\Snarl\tools\heysnarl.exe", "notify?app-sig=app/" + ThisAppName + "&title=" + title + "&text=" + msg);
        }

        private void UnregisterSnarl()
        {
            System.Diagnostics.Process.Start(@"C:\Program Files (x86)\full phat\Snarl\tools\heysnarl.exe", "unregister?app-sig=app/" + ThisAppName);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Minimized;

            InitializeHooks(true, true);
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
            handle = GetForegroundWindow();
            if (GetWindowText(handle, Buff, nChars) > 0)
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
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            Application.DoEvents();
            Application.Exit();
        }

        private void linkLabel_AddEmailAndPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currentEmailPasswordAndRegexList = GetLinesFromTextFile(SavedListFileName);
            string NewRegexEmailAndPasswordString = GetNewEmailAndPassword();
            if (NewRegexEmailAndPasswordString != null)
                currentEmailPasswordAndRegexList.Add(EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[0]) + "|" + EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[1]) + "|" + EncodeStringHex(NewRegexEmailAndPasswordString.Split('\t')[2]));
            WriteLinesToTextFile(SavedListFileName, currentEmailPasswordAndRegexList);
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

        public static List<string> GetLinesFromTextFile(string FullFilePath, Boolean ShowErrorMessage = true)
        {
            if (System.IO.File.Exists(FullFilePath))
            {
                try
                {
                    List<string> tmpList = new List<string>();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(FullFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null) tmpList.Add(line);
                        reader.Close();
                    }
                    return tmpList;
                }
                catch (Exception exc)
                {
                    if (ShowErrorMessage) System.Windows.Forms.MessageBox.Show("Error occurred when reading file " + FullFilePath + Environment.NewLine + "Error: " + exc.Message, "File read error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return new List<string>();
                }
            }
            else
            {
                //if (ShowErrorMessage) System.Windows.Forms.MessageBox.Show("The file does not exist: " + FullFilePath, "File not found", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return new List<string>();
            }
        }

        public static void WriteLinesToTextFile(string FullFilePath, List<string> LinesToWrite)
        {
            using (StreamWriter writer = new StreamWriter(FullFilePath))
            {
                foreach (string s in LinesToWrite)
                    writer.WriteLine(s);
            }
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
        public void PerformVoidFunctionSeperateThread(MethodInvoker method)
        {
            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                method.Invoke();
            });
            th.Start();
            //th.Join();
            while (th.IsAlive) { Application.DoEvents(); }
        }

        private void linkLabelGetPrivateKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                toolStripStatusLabelCurrentStatus.Text = "Obtaining pvt key...";
                //See App.xaml.cs for bypassing invalid SSL certificates
                string tmpkey = "";

                PerformVoidFunctionSeperateThread(() =>
                {
                    tmpkey = PostPHP(ServerAddress + "/generateprivatekey.php", "username=" + Username + "&password=" + Password);
                });

                string tmpSuccessKeyString = "Success: Key=";
                if (tmpkey != null && tmpkey.Length > 0 && tmpkey.ToUpper().StartsWith(tmpSuccessKeyString.ToUpper()))
                {
                    tmpkey = tmpkey.Substring(tmpSuccessKeyString.Length).Replace("\n", "").Replace("\r", "");
                    toolStripStatusLabelCurrentStatus.Text = tmpkey;
                }

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

                        DataSet ds = new DataSet("Todolist");
                        DataTable dt = new DataTable("Todotable");
                        dt.Columns.Add("Category");
                        dt.Columns.Add("Subcat");
                        dt.Columns.Add("Items");
                        dt.Columns.Add("Description");
                        dt.Columns.Add("Complete");//, typeof(bool));
                        foreach (string line in decryptedstring.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            if (line.Trim('\t', '\n', '\r', ' ', '\0').Length > 0)
                            {
                                dt.Rows.Add(line.Split('\t')); ;
                                AddTabSeperatedLineToTreeview(line);
                            }
                        ds.Tables.Add(dt);
                        dataGridView1.DataSource = ds;
                        dataGridView1.DataMember = "Todotable";

                        PerformVoidFunctionSeperateThread(() =>
                        {
                            //string username = "f";
                            //string query = PhpEncryption.SimpleTripleDesEncrypt("_att_" + "un=" + Username, tmpkey);
                            addrequest = (HttpWebRequest)WebRequest.Create(doWorkAddress + "/dotask/" +
                                PhpEncryption.SimpleTripleDesEncrypt(Username, "123456789abcdefghijklmno") + "/" +
                                PhpEncryption.SimpleTripleDesEncrypt("addtolist", tmpkey) + "/" +
                                PhpEncryption.SimpleTripleDesEncrypt("testcatC#\ttestsubC#\ttestitemsC#\ttestdescC#", tmpkey));// + "/");
                            try
                            {
                                addresponse = (HttpWebResponse)addrequest.GetResponse();
                                input = new StreamReader(addresponse.GetResponseStream());
                                encryptedstring = input.ReadToEnd();

                                decryptedstring = PhpEncryption.SimpleTripleDesDecrypt(encryptedstring, tmpkey);
                                MessageBox.Show(this, decryptedstring);
                                //textBoxLogs.Text = "";
                                //appendLogTextbox(decryptedstring);
                            }
                            catch (Exception exc) { MessageBox.Show(this, "Exception:" + exc.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        });
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
            catch (UriFormatException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return;
            }
            catch (IOException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return;
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
                TreeNode tmpItemsNode = new TreeNode(tmpItems) { Tag = tmpDescription };
                if (!treeViewTodolist.Nodes.ContainsKey(tmpCategory)) treeViewTodolist.Nodes.Add(tmpCategory, tmpCategory);
                if (!treeViewTodolist.Nodes[tmpCategory].Nodes.ContainsKey(tmpSubcat)) treeViewTodolist.Nodes[tmpCategory].Nodes.Add(tmpSubcat, tmpSubcat);
                treeViewTodolist.Nodes[tmpCategory].Nodes[tmpSubcat].Nodes.Add(tmpItemsNode);
            }
            else MessageBox.Show("The following line is invalid todo line: " + line);
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

        private void appendLogTextbox(string str)
        {
            textBoxLogs.Text += (textBoxLogs.Text.Length > 0 ? Environment.NewLine : "") + str;
        }

        private void treeViewTodolist_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBoxDescription.Clear();
            if (e.Node != null && e.Node.Tag != null)
                textBoxDescription.Text = e.Node.Tag.ToString();
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

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
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
