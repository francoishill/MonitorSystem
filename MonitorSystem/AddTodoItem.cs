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
    public partial class AddTodoItem : Form
    {
        public AddTodoItem()
        {
            InitializeComponent();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (textBoxCategory.Text.Trim().Length == 0) MessageBox.Show("Please enter a Category", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBoxSubcat.Text.Trim().Length == 0) MessageBox.Show("Please enter a subcategory", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBoxItems.Text.Trim().Length == 0) MessageBox.Show("Please enter the items", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //else if (textBoxDescription.Text.Trim().Length == 0 && MessageBox.Show("Accept a blank description?", "No description", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) { }
            else if (textBoxDescription.Text.Trim().Length == 0) MessageBox.Show("Please enter a description", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
