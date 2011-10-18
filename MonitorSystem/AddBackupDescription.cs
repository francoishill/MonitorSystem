using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MonitorSystem
{
	public partial class AddBackupDescription : Form
	{
		public AddBackupDescription(Form1.FileChangedDetails fileChangedDetails = null)
		{
			InitializeComponent();

			if (fileChangedDetails != null)
			{
				toolStripStatusLabel1.Text = Path.GetFileName(fileChangedDetails.OriginalFileName) + "   (" + fileChangedDetails.HumanFriendlyLastwriteDateString() + ")";
				if (fileChangedDetails.HasDescription()) textBox_Description.Text = fileChangedDetails.Description;
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}

		private void label2_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void textBox1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
			{
				e.Effect = DragDropEffects.All;
			}
		}

		private void textBox1_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			if (files.Length == 0) MessageBox.Show(this, "No files found");
			else if (files.Length > 1) MessageBox.Show(this, "Only one file at a time");
			else if (!File.Exists(files[0])) MessageBox.Show(this, "File not found: " + files[0]);
			else
			{
				toolStripStatusLabel1.Text = files[0];
				textBox_Description.Text = "This function is not incorporated yet"; textBox_Description.Enabled = false;
			}
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)10)//Ctrl + Enter
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
			}
		}

		private void textBox_Description_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//Note this event is used for multiple controls
			if (e.KeyCode == Keys.Escape)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				this.Close();
			}
		}
	}
}
