using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonitorSystem
{
	public partial class MonitoredFilesChanged : Form
	{
		public MonitoredFilesChanged()
		{
			InitializeComponent();
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null && e.Node.Tag is Form1.FileChangedDetails)
			{
				textBox1.Enabled = true;
				Form1.FileChangedDetails details = e.Node.Tag as Form1.FileChangedDetails;
				if (details.QueueStatus == Form1.FileChangedDetails.QueueStatusEnum.New)
				{
					details.SetNewQueueStatusAndUpdateNodeFontandcolor(Form1.FileChangedDetails.QueueStatusEnum.Read, e.Node);
					//details.QueueStatus = Form1.FileChangedDetails.QueueStatusEnum.Read;
					//e.Node.NodeFont = new Font(e.Node.NodeFont, FontStyle.Strikeout);
				}
				AllowTextchangeCallback = false;
				textBox1.Text = details.Description;
				AllowTextchangeCallback = true;
			}
			else
			{
				textBox1.Enabled = false;
				textBox1.Text = null;
			}
		}

		public bool AllowTextchangeCallback = true;
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			//TODO: Eventually add functionality to delete files (according to date, empty description, timeafter previous backup, etc)
			if (AllowTextchangeCallback)
			{
				if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
				{
					Form1.FileChangedDetails details =  treeView1.SelectedNode.Tag as Form1.FileChangedDetails;
					details.Description = textBox1.Text;
					//details.UpdateNodeFontandcolorFromQueueStatus(treeView1.SelectedNode);
					if (textBox1.Text.Length > 0)
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

		private void dismissToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
			{
				Form1.FileChangedDetails details =  treeView1.SelectedNode.Tag as Form1.FileChangedDetails;
				details.SetNewQueueStatusAndUpdateNodeFontandcolor(Form1.FileChangedDetails.QueueStatusEnum.Accepted, treeView1.SelectedNode);
			}
		}

		private void MonitoredFilesChanged_Shown(object sender, EventArgs e)
		{
			this.TopMost = true;
			this.Activate();
			this.TopMost = false;
			if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
				textBox1.Focus();
			else treeView1.Focus();
		}

		private void MonitoredFilesChanged_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) this.Close();
		}
	}
}
