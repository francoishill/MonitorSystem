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
	public partial class ViewBackups : Form
	{
		public ViewBackups()
		{
			InitializeComponent();

			StylingInterop.SetTreeviewVistaStyle(treeView1);
		}

		public bool AllowTextchangeCallback = true;
		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (AllowTextchangeCallback)
			{
			}
		}
	}
}
