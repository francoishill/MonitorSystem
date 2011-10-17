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
using FileChangedDetails = MonitorSystem.Form1.FileChangedDetails;

namespace MonitorSystem
{
	public partial class ViewBackups : Form
	{
		public ViewBackups()
		{
			InitializeComponent();

			toolStripStatusLabel1.Text = "";
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
			if (e.Node != null && e.Node.Tag != null && e.Node.Tag is Form1.FileChangedDetails)
			{
				Form1.FileChangedDetails fcd = e.Node.Tag as Form1.FileChangedDetails;
				textBoxDescription.Text = fcd.Description;
				richTextBox_FileContents.IsReadOnly = false;
				richTextBox_FileContents.Text = File.ReadAllText(fcd.GetBackupFileName());
				richTextBox_FileContents.IsReadOnly = true;
				if (fcd.OriginalFileName.ToLower().EndsWith(".sql")) richTextBox_FileContents.ConfigurationManager.Language = "mssql";
				else UserMessages.ShowWarningMessage("Filetype not recognized cannot implement syntax highlighting: " + fcd.OriginalFileName);
				//richTextBox_FileContents.LoadFile(fcd.GetBackupFileName(), RichTextBoxStreamType.PlainText);
				toolStripStatusLabel1.Text = fcd.GetBackupFileName();//.OriginalFileName;
			}
			else
			{
				toolStripStatusLabel1.Text = "";
				textBoxDescription.Text = null;
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
					textBoxDescription.Text = formAddBackup.textBox_Description.Text;
					treeView1.SelectedNode.ForeColor = tmpfcd.GetColorBasedOnDescription();
				}
			}
		}
	}
}
