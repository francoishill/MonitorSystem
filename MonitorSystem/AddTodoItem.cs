﻿using System;
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
        Control ControlToFocusFirst = null;

        public AddTodoItem(string Category = "", string Subcat = "")
        {
            InitializeComponent();
			if (Category != "") { comboBoxCategory.Text = Category; ControlToFocusFirst = comboBoxSubcat; }
            if (Subcat != "") { comboBoxSubcat.Text = Subcat; ControlToFocusFirst = textBoxItems; }
            dateTimePickerRemindOn.Value = DateTime.Now.AddMinutes(30);
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            AcceptNow();
        }

        private void AcceptNow()
        {
            if (comboBoxCategory.Text.Trim().Length == 0) MessageBox.Show("Please enter a Category", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			else if (comboBoxSubcat.Text.Trim().Length == 0) MessageBox.Show("Please enter a subcategory", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBoxItems.Text.Trim().Length == 0) MessageBox.Show("Please enter the items", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //else if (textBoxDescription.Text.Trim().Length == 0 && MessageBox.Show("Accept a blank description?", "No description", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) { }
			//else if (textBoxDescription.Text.Trim().Length == 0) MessageBox.Show("Please enter a description", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void AddTodoItem_Shown(object sender, EventArgs e)
        {
            if (ControlToFocusFirst != null) ControlToFocusFirst.Focus();

        }

        private void textBoxCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //if ((ModifierKeys & Keys.Control) == Keys.Control)
                //    AcceptNow();
                if ((sender != textBoxDescription && sender != buttonAccept) || ((ModifierKeys & Keys.Shift) == Keys.Shift))
                {
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
            }
            //else if (e.KeyCode == Keys.Escape)
            //    this.Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && (ModifierKeys & Keys.Control) == Keys.Control) { e.Handled = true; AcceptNow(); }
            if (e.KeyCode == Keys.Escape) { e.Handled = true; this.Close(); }
        }

        private void checkBoxAutosnooze_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxAutosnooze.Checked) {
                numericUpDownAutosnoozeInterval.Enabled = true;
                numericUpDownAutosnoozeInterval.Minimum = 1;
                label6.Enabled = true;
                if (numericUpDownAutosnoozeInterval.Value == 0) numericUpDownAutosnoozeInterval.Value = 15;
            }
            else {
                numericUpDownAutosnoozeInterval.Minimum = 0;
                numericUpDownAutosnoozeInterval.Enabled = false;
                label6.Enabled = false;
            }
        }

		private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBoxSubcat.Items.Clear();

			var tmpCatAndSubcat = comboBoxCategory.SelectedItem as TodoCategoryAndSubcats;
			if (tmpCatAndSubcat == null)
				return;

			comboBoxSubcat.Items.AddRange(tmpCatAndSubcat.Subcategories.ToArray());
		}
    }

	public class TodoCategoryAndSubcats
	{
		public string Category;
		public List<string> Subcategories;
		public TodoCategoryAndSubcats(string Category, List<string> Subcategories)
		{
			this.Category = Category;
			this.Subcategories = Subcategories;
		}
		public override string ToString()
		{
			return Category;
		}
	}
}
