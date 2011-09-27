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
				textBox1.ReadOnly = false;
				Form1.FileChangedDetails details = e.Node.Tag as Form1.FileChangedDetails;
				if (details.QueueStatus == Form1.FileChangedDetails.QueueStatusEnum.New)
				{
					details.QueueStatus = Form1.FileChangedDetails.QueueStatusEnum.Handled;
					e.Node.NodeFont = new Font(e.Node.NodeFont, FontStyle.Strikeout);
				}
				AllowTextchangeCallback = false;
				textBox1.Text = details.Description;
				AllowTextchangeCallback = true;
			}
			else
			{
				textBox1.ReadOnly = true;
				textBox1.Text = null;
			}
		}

		bool AllowTextchangeCallback = true;
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (AllowTextchangeCallback)
			{
				if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
				{
					Form1.FileChangedDetails details =  treeView1.SelectedNode.Tag as Form1.FileChangedDetails;
					details.Description = textBox1.Text;
					treeView1.SelectedNode.ForeColor = textBox1.Text == null || textBox1.Text.Trim().Length == 0 ? Color.Red : Color.Green;
				}
			}
		}

		private void keepForlaterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is Form1.FileChangedDetails)
			{
				Form1.FileChangedDetails details =  treeView1.SelectedNode.Tag as Form1.FileChangedDetails;
				details.QueueStatus = Form1.FileChangedDetails.QueueStatusEnum.Later;
				treeView1.SelectedNode.NodeFont = new Font(treeView1.SelectedNode.NodeFont, FontStyle.Regular);
			}
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			treeView1.SelectedNode = e.Node;
		}
	}
}
