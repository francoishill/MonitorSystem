namespace MonitorSystem
{
    partial class AddTodoItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxCategory = new System.Windows.Forms.TextBox();
            this.textBoxSubcat = new System.Windows.Forms.TextBox();
            this.textBoxItems = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.dateTimePickerRemindOn = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxAutosnooze = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownAutosnoozeInterval = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutosnoozeInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxCategory
            // 
            this.textBoxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCategory.Location = new System.Drawing.Point(128, 12);
            this.textBoxCategory.Name = "textBoxCategory";
            this.textBoxCategory.Size = new System.Drawing.Size(135, 20);
            this.textBoxCategory.TabIndex = 0;
            this.textBoxCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCategory_KeyDown);
            // 
            // textBoxSubcat
            // 
            this.textBoxSubcat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSubcat.Location = new System.Drawing.Point(128, 38);
            this.textBoxSubcat.Name = "textBoxSubcat";
            this.textBoxSubcat.Size = new System.Drawing.Size(135, 20);
            this.textBoxSubcat.TabIndex = 1;
            this.textBoxSubcat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCategory_KeyDown);
            // 
            // textBoxItems
            // 
            this.textBoxItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxItems.Location = new System.Drawing.Point(128, 64);
            this.textBoxItems.Name = "textBoxItems";
            this.textBoxItems.Size = new System.Drawing.Size(187, 20);
            this.textBoxItems.TabIndex = 2;
            this.textBoxItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCategory_KeyDown);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.AcceptsReturn = true;
            this.textBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDescription.Location = new System.Drawing.Point(128, 90);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDescription.Size = new System.Drawing.Size(281, 135);
            this.textBoxDescription.TabIndex = 3;
            this.textBoxDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCategory_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Category";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Subcategory";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Items";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Description";
            // 
            // buttonAccept
            // 
            this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAccept.Location = new System.Drawing.Point(334, 310);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 8;
            this.buttonAccept.Text = "&Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            this.buttonAccept.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCategory_KeyDown);
            // 
            // dateTimePickerRemindOn
            // 
            this.dateTimePickerRemindOn.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerRemindOn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerRemindOn.Location = new System.Drawing.Point(128, 234);
            this.dateTimePickerRemindOn.Name = "dateTimePickerRemindOn";
            this.dateTimePickerRemindOn.ShowCheckBox = true;
            this.dateTimePickerRemindOn.Size = new System.Drawing.Size(163, 20);
            this.dateTimePickerRemindOn.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Remind me on";
            // 
            // checkBoxAutosnooze
            // 
            this.checkBoxAutosnooze.AutoSize = true;
            this.checkBoxAutosnooze.Location = new System.Drawing.Point(11, 264);
            this.checkBoxAutosnooze.Name = "checkBoxAutosnooze";
            this.checkBoxAutosnooze.Size = new System.Drawing.Size(111, 17);
            this.checkBoxAutosnooze.TabIndex = 13;
            this.checkBoxAutosnooze.Text = "Autosnooze every";
            this.checkBoxAutosnooze.UseVisualStyleBackColor = true;
            this.checkBoxAutosnooze.CheckedChanged += new System.EventHandler(this.checkBoxAutosnooze_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(192, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "minutes";
            // 
            // numericUpDownAutosnoozeInterval
            // 
            this.numericUpDownAutosnoozeInterval.Enabled = false;
            this.numericUpDownAutosnoozeInterval.Location = new System.Drawing.Point(128, 261);
            this.numericUpDownAutosnoozeInterval.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownAutosnoozeInterval.Name = "numericUpDownAutosnoozeInterval";
            this.numericUpDownAutosnoozeInterval.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownAutosnoozeInterval.TabIndex = 15;
            // 
            // AddTodoItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 345);
            this.Controls.Add(this.numericUpDownAutosnoozeInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBoxAutosnooze);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePickerRemindOn);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.textBoxItems);
            this.Controls.Add(this.textBoxSubcat);
            this.Controls.Add(this.textBoxCategory);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(385, 252);
            this.Name = "AddTodoItem";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add todo item";
            this.Shown += new System.EventHandler(this.AddTodoItem_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutosnoozeInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBoxCategory;
        public System.Windows.Forms.TextBox textBoxSubcat;
        public System.Windows.Forms.TextBox textBoxItems;
        public System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.DateTimePicker dateTimePickerRemindOn;
        public System.Windows.Forms.CheckBox checkBoxAutosnooze;
        public System.Windows.Forms.NumericUpDown numericUpDownAutosnoozeInterval;
    }
}