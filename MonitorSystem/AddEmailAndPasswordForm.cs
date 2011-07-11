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
    public partial class AddEmailAndPasswordForm : Form
    {
        public AddEmailAndPasswordForm()
        {
            InitializeComponent();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (textBoxRegex.Text.Length == 0) MessageBox.Show("Please enter a regex (regular expression) for the window title.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBoxUsername.Text.Length == 0) MessageBox.Show("Please enter a username.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBoxPassword.Text.Length == 0 && MessageBox.Show("No password is entered, confirm?", "Empty password", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No) { }
            else this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void checkBoxHidePassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = checkBoxHidePassword.Checked ? '*' : (char)0;
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.SelectNextControl((Control)sender, true, true, true, true);
        }
    }
}
