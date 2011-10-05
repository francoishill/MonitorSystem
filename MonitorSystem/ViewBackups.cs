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
				toolStripStatusLabel1.Text = fcd.GetBackupFileName();//.OriginalFileName;
			}
			else toolStripStatusLabel1.Text = "";
		}

		private void ViewBackups_Shown(object sender, EventArgs e)
		{
			StylingInterop.SetTreeviewVistaStyle(treeView1);
		}
	}
}
