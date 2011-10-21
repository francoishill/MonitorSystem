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

namespace MonitorSystem
{
	public partial class MonitoredFilesChanged : Form
	{
		public MonitoredFilesChanged()
		{
			InitializeComponent();
			AutoCompleteInterop.EnableRichTextboxAutocomplete(richTextBox_Description);//, AutoCompleteInterop.GetWordlistOfFileContents(richTextBox_FileContents.Text));
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null && e.Node.Tag is Form1.FileChangedDetails)
			{
				richTextBox_Description.Enabled = true;
				richTextBox_FileContents.Enabled = true;
				Form1.FileChangedDetails details = e.Node.Tag as Form1.FileChangedDetails;
				if (details.QueueStatus == Form1.FileChangedDetails.QueueStatusEnum.New)
				{
					details.SetNewQueueStatusAndUpdateNodeFontandcolor(Form1.FileChangedDetails.QueueStatusEnum.Read, e.Node);
					//details.QueueStatus = Form1.FileChangedDetails.QueueStatusEnum.Read;
					//e.Node.NodeFont = new Font(e.Node.NodeFont, FontStyle.Strikeout);
				}
				AllowTextchangeCallback = false;
				richTextBox_Description.Text = details.Description;
				//TODO: get a way to incorporate the ScintillaNET.dll file so that it does not have to be in the Path Environment Variables.
				richTextBox_FileContents.IsReadOnly = false;
				richTextBox_FileContents.Text = File.ReadAllText(details.GetBackupFileName());
				AutoCompleteInterop.SetFullAutocompleteListOfRichTextbox(richTextBox_Description, AutoCompleteInterop.GetWordlistOfFileContents(richTextBox_FileContents.Text));
				richTextBox_FileContents.IsReadOnly = true;
				if (details.OriginalFileName.ToLower().EndsWith(".sql"))
				{
					if (richTextBox_FileContents.ConfigurationManager.Language != "mssql")
						richTextBox_FileContents.ConfigurationManager.Language = "mssql";
				}
				else UserMessages.ShowWarningMessage("Filetype not recognized cannot implement syntax highlighting: " + details.OriginalFileName);
				//richTextBox_FileContents.LoadFile(details.GetBackupFileName(), RichTextBoxStreamType.PlainText);
				AllowTextchangeCallback = true;
			}
			else
			{
				richTextBox_Description.Enabled = false;
				richTextBox_Description.Text = null;
				richTextBox_FileContents.Enabled = false;
				richTextBox_FileContents.Text = null;
			}
		}

		public bool AllowTextchangeCallback = true;
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (AllowTextchangeCallback)
			{
				if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
				{
					Form1.FileChangedDetails details =  treeView1.SelectedNode.Tag as Form1.FileChangedDetails;
					details.Description = richTextBox_Description.Text;
					//details.UpdateNodeFontandcolorFromQueueStatus(treeView1.SelectedNode);
					if (richTextBox_Description.Text.Length > 0)
						details.SetNewQueueStatusAndUpdateNodeFontandcolor(details.QueueStatus == Form1.FileChangedDetails.QueueStatusEnum.Read ? Form1.FileChangedDetails.QueueStatusEnum.Accepted : details.QueueStatus, treeView1.SelectedNode);
					else
						details.SetNewQueueStatusAndUpdateNodeFontandcolor(details.QueueStatus == Form1.FileChangedDetails.QueueStatusEnum.Accepted ? Form1.FileChangedDetails.QueueStatusEnum.Read : details.QueueStatus, treeView1.SelectedNode);
					//treeView1.SelectedNode.ForeColor = textBox1.Text == null || textBox1.Text.Trim().Length == 0 ? Color.Red : Color.Green;
				}
			}
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			treeView1.SelectedNode = e.Node;
		}

		private void MonitoredFilesChanged_Shown(object sender, EventArgs e)
		{
			this.TopMost = true;
			this.Activate();
			this.TopMost = false;
			if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
				richTextBox_Description.Focus();
			else treeView1.Focus();

			StylingInterop.SetTreeviewVistaStyle(treeView1);
		}

		private void MonitoredFilesChanged_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
            //Note this event is used for multiple controls
			if (e.KeyCode == Keys.Escape) this.Close();
		}

		private void AcceptSelectedItem()
		{
			if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
			{
				Form1.FileChangedDetails details =  treeView1.SelectedNode.Tag as Form1.FileChangedDetails;
				details.SetNewQueueStatusAndUpdateNodeFontandcolor(Form1.FileChangedDetails.QueueStatusEnum.Accepted, treeView1.SelectedNode);
			}
		}

		private void DiscardItem(TreeNode fileChangedNode)
		{
			if (fileChangedNode != null && fileChangedNode.Tag is Form1.FileChangedDetails)
			{
				Form1.FileChangedDetails details =  fileChangedNode.Tag as Form1.FileChangedDetails;
				details.SetNewQueueStatusAndUpdateNodeFontandcolor(Form1.FileChangedDetails.QueueStatusEnum.Discard, fileChangedNode);
			}
		}

		private void DiscardEmptySubnodes()
		{
			if (treeView1.SelectedNode != null)
			{
				foreach (TreeNode subnode in treeView1.SelectedNode.Nodes)
					if (subnode.Tag is Form1.FileChangedDetails)
					{
						Form1.FileChangedDetails details = subnode.Tag as Form1.FileChangedDetails;
						if (details.Description == null || details.Description.Trim().Length == 0)
							DiscardItem(subnode);
					}
			}
		}

		private void menuItem_ClearMessages_Click(object sender, EventArgs e)
		{
			UserMessages.ShowErrorMessage("Function not incorporated yet");
		}

		private void menuItem_DiscardEmpty_Click(object sender, EventArgs e)
		{
			DiscardEmptySubnodes();
		}

		private void menuItem_ClearThisMessage_Click(object sender, EventArgs e)
		{
			UserMessages.ShowErrorMessage("Function not incorporated yet");
		}

		private void menuItem_Accept_Click(object sender, EventArgs e)
		{
			AcceptSelectedItem();
		}

		private void menuItem_Discard_Click(object sender, EventArgs e)
		{
			DiscardItem(treeView1.SelectedNode);
		}

        private void textBox_Description_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Control && !e.Alt && !e.Shift)
            {
                e.Handled = true;
                richTextBox_Description.Paste(DataFormats.GetFormat(DataFormats.Text));
            }
        }
	}
}
