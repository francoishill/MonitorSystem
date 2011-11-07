using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using FileChangedDetails = MonitorSystem.MainForm.FileChangedDetails;
using System.Globalization;
using System.Windows.Forms.Design;

namespace MonitorSystem
{
	public partial class ViewBackups : Form
	{
		string RootDir = null;
		FileChangedDetails LastFileChangedDetailsAdded;

		//TODO: Should have string search (maybe even regex) functionality on file description. Can filter on file (regex of filename) and/or dates
		public ViewBackups()
		{
			InitializeComponent();

			//toolStripStatusLabel1.Text = "";

			comboBox1.Text = null;
			UpdateFilterBackgroundColor();
			//comboStripItem1.Text = null;
			//comboStripItem1
			comboBox1.LostFocus += delegate
			{
				if (!comboBox1.Items.Contains(comboBox1.Text))
					comboBox1.Items.Insert(0, comboBox1.Text);
				//comboStripItem1.history.Add(comboStripItem1.Text);
			};

			comboBox1.TextChanged += delegate
			//toolStripTextBox1.TextChanged += delegate
			{
				//foreach (TreeNode node in treeView1.Nodes)
				RefreshNodes(comboBox1.Text);//toolStripTextBox1.Text);
			};
		}

		public bool AllowTextchangeCallback = true;
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (AllowTextchangeCallback)
			{
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null && e.Node.Tag != null && e.Node.Tag is MainForm.FileChangedDetails)
			{
				MainForm.FileChangedDetails fcd = e.Node.Tag as MainForm.FileChangedDetails;
				richTextBox_Description.Text = fcd.Description;
				richTextBox_FileContents.IsReadOnly = false;
				richTextBox_FileContents.Text = File.Exists(fcd.GetBackupFileName()) ? File.ReadAllText(fcd.GetBackupFileName()) : "";
				richTextBox_FileContents.IsReadOnly = true;
				if (fcd.OriginalFileName.ToLower().EndsWith(".sql"))
				{
					if (richTextBox_FileContents.ConfigurationManager.Language != "mssql")
						richTextBox_FileContents.ConfigurationManager.Language = "mssql";
				}
				else UserMessages.ShowWarningMessage("Filetype not recognized cannot implement syntax highlighting: " + fcd.OriginalFileName);
				//richTextBox_FileContents.LoadFile(fcd.GetBackupFileName(), RichTextBoxStreamType.PlainText);
				toolStripStatusLabel1.Text = fcd.GetBackupFileName();//.OriginalFileName;
			}
			else
			{
				toolStripStatusLabel1.Text = "";
				richTextBox_Description.Text = null;
				richTextBox_FileContents.Text = null;
			}
		}

		private void ViewBackups_Shown(object sender, EventArgs e)
		{
			StylingInterop.SetTreeviewVistaStyle(treeView1);
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			treeView1.SelectedNode = e.Node;
		}

		private void ViewBackups_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//Note this event is used for multiple controls
			if (e.KeyCode == Keys.Escape) this.Close();
		}

		private void menuItem_DiscardEmptyBackups_Click(object sender, EventArgs e)
		{
			if (UserMessages.Confirm("Delete all backups with empty descriptions?"))
			{
				TreeNodeCollection subnodes = treeView1.SelectedNode.Nodes;
				foreach (TreeNode subnode in subnodes)
				{
					if (subnode.Tag is FileChangedDetails)
					{
						FileChangedDetails tmpsubfcd = subnode.Tag as FileChangedDetails;
						if (!tmpsubfcd.HasDescription())
							if (tmpsubfcd.DiscardBackupFileAndDescription())
							{
								subnode.NodeFont = new Font(treeView1.Font, FontStyle.Strikeout | FontStyle.Italic);
								subnode.ContextMenu = null;
							}
					}
				}
			}
		}

		private void menuItem_DiscardBackup_Click(object sender, EventArgs e)
		{
			FileChangedDetails tmpfcd = treeView1.SelectedNode.Tag as FileChangedDetails;
			if (!tmpfcd.HasDescription() || UserMessages.Confirm("This item (" + tmpfcd.HumanFriendlyLastwriteDateString() + ") has a description, confirm discarding?"))
			{
				if (tmpfcd.DiscardBackupFileAndDescription())
				{
					treeView1.SelectedNode.NodeFont = new System.Drawing.Font(treeView1.Font, FontStyle.Strikeout | FontStyle.Italic);
					treeView1.SelectedNode.ContextMenu = null;
				}
			}
		}

		private void menuItem_AddDescription_Click(object sender, EventArgs e)
		{
			FileChangedDetails tmpfcd = treeView1.SelectedNode.Tag as FileChangedDetails;
			AddBackupDescription formAddBackup = new AddBackupDescription(tmpfcd);
			if (formAddBackup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				if (formAddBackup.textBox_Description.Text != null && formAddBackup.textBox_Description.Text.Trim().Length > 0)
				{
					tmpfcd.Description = formAddBackup.textBox_Description.Text;
					tmpfcd.WriteDescriptionFileNow();
					richTextBox_Description.Text = formAddBackup.textBox_Description.Text;
					treeView1.SelectedNode.ForeColor = tmpfcd.GetColorBasedOnDescription();
				}
			}
		}

		private void CheckFilter(TreeNode node, string filterText)
		{
			//if (node != null && node.Tag != null && node.Tag is MainForm.FileChangedDetails)
			//{
			//	//node.en.IsVisible = (node.Tag as MainForm.FileChangedDetails).Description.ToLower().Contains(filterText);
			//}
		}

		public int RefreshNodes(string filterString, string rootDir = null, FileChangedDetails lastFileChangedDetailsAdded = null)
		{
			if (rootDir != null) RootDir = rootDir;
			if (lastFileChangedDetailsAdded != null) LastFileChangedDetailsAdded = lastFileChangedDetailsAdded;

			if (RootDir == null &&
				UserMessages.ShowWarningMessage("Cannot refresh viewbackups, RootDir is null"))
				return 0;
			//if (LastFileChangedDetailsAdded == null &&
			//	UserMessages.ShowWarningMessage("Cannot refresh viewbackups, LastFileChangedDetailsAdded is null"))
			//	return 0;

			this.treeView1.Nodes.Clear();

			Dictionary<string, Dictionary<DateTime, FileChangedDetails>> OriginalFilenamesWithModificationsDict = null;// new Dictionary<string, Dictionary<DateTime, FileChangedDetails>>();
		
			while (RootDir.EndsWith("\\")) RootDir = RootDir.Substring(0, RootDir.Length - 1);
			string[] backupFiles = Directory.GetFiles(RootDir, "*" + FileChangedDetails.backupExt, SearchOption.AllDirectories);
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

					if (filterString != null && !fileDescription.ToLower().Contains(filterString.ToLower()))
						continue;

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
				TreeNode rootDirNode = new TreeNode(RootDir + "\\");
				bool AtleastOneFile = false;

				TreeNode FileNodeWithLastSavedTime = null;
				DateTime LastSavedTimeOfAllFileNodes = DateTime.MinValue;
				foreach (string file in OriginalFilenamesWithModificationsDict.Keys)
				{
					string originalFileName = file.Split('\\')[file.Split('\\').Length - 1];// FileChangedDetails.GetOriginalNameFromBackupFile(file.Split('\\')[file.Split('\\').Length - 1]);
					TreeNode fileNode;
					fileNode = new TreeNode();
					fileNode = MainForm.PopulateTreeNode(file.Substring(RootDir.Length + 1), rootDirNode);
					//fileNode.NodeFont = new System.Drawing.Font(this.Font.FontFamily, 12, this.Font.Style);
					fileNode.Name = originalFileName;
					fileNode.Text = originalFileName;
					fileNode.Tag = file;

					if (FileNodeWithLastSavedTime == null) FileNodeWithLastSavedTime = fileNode;

					fileNode.ContextMenu = this.contextMenu_FileNode;

					bool AtleastOneFileModification = false;
					foreach (DateTime date in OriginalFilenamesWithModificationsDict[file].Keys)
					{
						FileChangedDetails fcd = OriginalFilenamesWithModificationsDict[file][date];
						if (fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Accepted && fcd.QueueStatus != FileChangedDetails.QueueStatusEnum.Complete)
						{
							AtleastOneFile = true;
							AtleastOneFileModification = true;
							TreeNode fileModifiedNode = new TreeNode(date.ToString(FileChangedDetails.humandfriendlyDateFormat));////MySQLdateformat));
							fileModifiedNode.Tag = fcd;
							fileModifiedNode.ContextMenu = this.contextMenu_ModificationNode;
							fileModifiedNode.ForeColor = fcd.GetColorBasedOnDescription();

							fileNode.Nodes.Add(fileModifiedNode);
							string PathExcludingRoot = file.Substring(RootDir.Length + 2);
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
					this.treeView1.Nodes.Add(rootDirNode);
					rootDirNode.Expand();//.ExpandAll);
				}
				if (filterString != null)
					rootDirNode.ExpandAll();
				this.treeView1.SelectedNode = nodeToSelect;

				if (this.treeView1.Nodes.Count == 0)
				{
					//ShowBalloonTipNotification("No file changes queued");
					return 0;
				}

				//this.ShowDialog();
			}
			return this.treeView1.Nodes.Count;
		}

		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			UpdateFilterBackgroundColor();
		}

		private void UpdateFilterBackgroundColor()
		{
			comboBox1.BackColor = comboBox1.Text.Length == 0 ? SystemColors.Window : Color.FromArgb(220, 255, 220);
		}
	}
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
																			 ToolStripItemDesignerAvailability.ContextMenuStrip |
																			 ToolStripItemDesignerAvailability.StatusStrip)]
	public class ComboStripItem : ToolStripControlHost
	{
		private ComboBox combo;
		public List<string> history;

		public ComboStripItem() : base(new ComboBox())
		{
			this.combo = this.Control as ComboBox;
			history = new List<string>();
		}

		// Add properties, events etc. you want to expose...
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
																			 ToolStripItemDesignerAvailability.ContextMenuStrip |
																			 ToolStripItemDesignerAvailability.StatusStrip)]
	public class ComboTextboxItem : ToolStripControlHost
	{
		private TextBox textbox;

		public ComboTextboxItem()
			: base(new TextBox())
		{
			this.textbox = this.Control as TextBox;
		}

		// Add properties, events etc. you want to expose...
	}

}
