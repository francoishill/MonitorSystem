namespace MonitorSystem
{
    partial class Form1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.textBoxLogs = new System.Windows.Forms.TextBox();
			this.linkLabel_AddEmailAndPassword = new System.Windows.Forms.LinkLabel();
			this.linkLabelGetCurrentTodolist = new System.Windows.Forms.LinkLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelCurrentStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.treeViewTodolist = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.checkBoxAutoUpload = new System.Windows.Forms.CheckBox();
			this.checkBoxComplete = new System.Windows.Forms.CheckBox();
			this.dateTimePickerCreated = new System.Windows.Forms.DateTimePicker();
			this.dateTimePickerDue = new System.Windows.Forms.DateTimePicker();
			this.numericUpDownRemindedCount = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownAutosnoozeInterval = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.checkBoxStopSnooze = new System.Windows.Forms.CheckBox();
			this.timer_PhpCronJob = new System.Windows.Forms.Timer(this.components);
			this.fileSystemWatcher_SqlFiles = new System.IO.FileSystemWatcher();
			this.contextMenuItemsNode = new System.Windows.Forms.ContextMenu();
			this.menuItem_DeleteThisItem = new System.Windows.Forms.MenuItem();
			this.contextMenuItemsSubcat = new System.Windows.Forms.ContextMenu();
			this.menuItem_AddItemToThisCategory = new System.Windows.Forms.MenuItem();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRemindedCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutosnoozeInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher_SqlFiles)).BeginInit();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked_1);
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(12, 9);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(148, 13);
			this.linkLabel1.TabIndex = 0;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "C:\\Francois\\tmp\\Screenshots";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// textBoxLogs
			// 
			this.textBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLogs.Location = new System.Drawing.Point(15, 65);
			this.textBoxLogs.Multiline = true;
			this.textBoxLogs.Name = "textBoxLogs";
			this.textBoxLogs.ReadOnly = true;
			this.textBoxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxLogs.Size = new System.Drawing.Size(780, 115);
			this.textBoxLogs.TabIndex = 1;
			// 
			// linkLabel_AddEmailAndPassword
			// 
			this.linkLabel_AddEmailAndPassword.AutoSize = true;
			this.linkLabel_AddEmailAndPassword.Location = new System.Drawing.Point(12, 34);
			this.linkLabel_AddEmailAndPassword.Name = "linkLabel_AddEmailAndPassword";
			this.linkLabel_AddEmailAndPassword.Size = new System.Drawing.Size(122, 13);
			this.linkLabel_AddEmailAndPassword.TabIndex = 2;
			this.linkLabel_AddEmailAndPassword.TabStop = true;
			this.linkLabel_AddEmailAndPassword.Text = "Add email and password";
			this.linkLabel_AddEmailAndPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_AddEmailAndPassword_LinkClicked);
			// 
			// linkLabelGetCurrentTodolist
			// 
			this.linkLabelGetCurrentTodolist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabelGetCurrentTodolist.AutoSize = true;
			this.linkLabelGetCurrentTodolist.Location = new System.Drawing.Point(699, 9);
			this.linkLabelGetCurrentTodolist.Name = "linkLabelGetCurrentTodolist";
			this.linkLabelGetCurrentTodolist.Size = new System.Drawing.Size(96, 13);
			this.linkLabelGetCurrentTodolist.TabIndex = 3;
			this.linkLabelGetCurrentTodolist.TabStop = true;
			this.linkLabelGetCurrentTodolist.Text = "Get current todolist";
			this.linkLabelGetCurrentTodolist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGetCurrentTodolist_LinkClicked);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCurrentStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 416);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(807, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabelCurrentStatus
			// 
			this.toolStripStatusLabelCurrentStatus.Name = "toolStripStatusLabelCurrentStatus";
			this.toolStripStatusLabelCurrentStatus.Size = new System.Drawing.Size(79, 17);
			this.toolStripStatusLabelCurrentStatus.Text = "CurrentStatus";
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(232, 9);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(39, 38);
			this.dataGridView1.TabIndex = 5;
			this.dataGridView1.Visible = false;
			// 
			// treeViewTodolist
			// 
			this.treeViewTodolist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeViewTodolist.HideSelection = false;
			this.treeViewTodolist.Location = new System.Drawing.Point(0, 0);
			this.treeViewTodolist.Name = "treeViewTodolist";
			this.treeViewTodolist.Size = new System.Drawing.Size(228, 204);
			this.treeViewTodolist.TabIndex = 6;
			this.treeViewTodolist.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewTodolist_BeforeSelect);
			this.treeViewTodolist.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTodolist_AfterSelect);
			this.treeViewTodolist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewTodolist_MouseDown);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(15, 186);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeViewTodolist);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBoxDescription);
			this.splitContainer1.Size = new System.Drawing.Size(547, 204);
			this.splitContainer1.SplitterDistance = 228;
			this.splitContainer1.TabIndex = 7;
			this.splitContainer1.TabStop = false;
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxDescription.Location = new System.Drawing.Point(0, 0);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxDescription.Size = new System.Drawing.Size(315, 204);
			this.textBoxDescription.TabIndex = 0;
			this.textBoxDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDescription_KeyDown);
			// 
			// linkLabel2
			// 
			this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(723, 34);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(72, 13);
			this.linkLabel2.TabIndex = 8;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Add todo item";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// checkBoxAutoUpload
			// 
			this.checkBoxAutoUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxAutoUpload.AutoSize = true;
			this.checkBoxAutoUpload.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxAutoUpload.Checked = true;
			this.checkBoxAutoUpload.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxAutoUpload.Enabled = false;
			this.checkBoxAutoUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxAutoUpload.Location = new System.Drawing.Point(669, 396);
			this.checkBoxAutoUpload.Name = "checkBoxAutoUpload";
			this.checkBoxAutoUpload.Size = new System.Drawing.Size(126, 17);
			this.checkBoxAutoUpload.TabIndex = 9;
			this.checkBoxAutoUpload.Text = "Auto upload changes";
			this.checkBoxAutoUpload.UseVisualStyleBackColor = true;
			// 
			// checkBoxComplete
			// 
			this.checkBoxComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxComplete.AutoSize = true;
			this.checkBoxComplete.Location = new System.Drawing.Point(725, 188);
			this.checkBoxComplete.Name = "checkBoxComplete";
			this.checkBoxComplete.Size = new System.Drawing.Size(70, 17);
			this.checkBoxComplete.TabIndex = 11;
			this.checkBoxComplete.Text = "Complete";
			this.checkBoxComplete.UseVisualStyleBackColor = true;
			this.checkBoxComplete.CheckedChanged += new System.EventHandler(this.checkBoxComplete_CheckedChanged);
			// 
			// dateTimePickerCreated
			// 
			this.dateTimePickerCreated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePickerCreated.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePickerCreated.Enabled = false;
			this.dateTimePickerCreated.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePickerCreated.Location = new System.Drawing.Point(639, 236);
			this.dateTimePickerCreated.Name = "dateTimePickerCreated";
			this.dateTimePickerCreated.Size = new System.Drawing.Size(156, 20);
			this.dateTimePickerCreated.TabIndex = 13;
			// 
			// dateTimePickerDue
			// 
			this.dateTimePickerDue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dateTimePickerDue.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dateTimePickerDue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dateTimePickerDue.Location = new System.Drawing.Point(639, 211);
			this.dateTimePickerDue.Name = "dateTimePickerDue";
			this.dateTimePickerDue.Size = new System.Drawing.Size(156, 20);
			this.dateTimePickerDue.TabIndex = 12;
			this.dateTimePickerDue.ValueChanged += new System.EventHandler(this.dateTimePickerDue_ValueChanged);
			// 
			// numericUpDownRemindedCount
			// 
			this.numericUpDownRemindedCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownRemindedCount.Enabled = false;
			this.numericUpDownRemindedCount.Location = new System.Drawing.Point(675, 263);
			this.numericUpDownRemindedCount.Maximum = new decimal(new int[] {
            276447231,
            23283,
            0,
            0});
			this.numericUpDownRemindedCount.Name = "numericUpDownRemindedCount";
			this.numericUpDownRemindedCount.Size = new System.Drawing.Size(120, 20);
			this.numericUpDownRemindedCount.TabIndex = 14;
			// 
			// numericUpDownAutosnoozeInterval
			// 
			this.numericUpDownAutosnoozeInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numericUpDownAutosnoozeInterval.Location = new System.Drawing.Point(675, 312);
			this.numericUpDownAutosnoozeInterval.Maximum = new decimal(new int[] {
            276447231,
            23283,
            0,
            0});
			this.numericUpDownAutosnoozeInterval.Name = "numericUpDownAutosnoozeInterval";
			this.numericUpDownAutosnoozeInterval.Size = new System.Drawing.Size(120, 20);
			this.numericUpDownAutosnoozeInterval.TabIndex = 15;
			this.numericUpDownAutosnoozeInterval.ValueChanged += new System.EventHandler(this.numericUpDownAutosnoozeInterval_ValueChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(606, 217);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Due";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(589, 242);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 13);
			this.label2.TabIndex = 17;
			this.label2.Text = "Created";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(584, 265);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 13);
			this.label3.TabIndex = 18;
			this.label3.Text = "Reminded count";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(568, 314);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 13);
			this.label4.TabIndex = 19;
			this.label4.Text = "Autosnooze Interval";
			// 
			// checkBoxStopSnooze
			// 
			this.checkBoxStopSnooze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxStopSnooze.AutoSize = true;
			this.checkBoxStopSnooze.Location = new System.Drawing.Point(710, 289);
			this.checkBoxStopSnooze.Name = "checkBoxStopSnooze";
			this.checkBoxStopSnooze.Size = new System.Drawing.Size(85, 17);
			this.checkBoxStopSnooze.TabIndex = 20;
			this.checkBoxStopSnooze.Text = "Stop snooze";
			this.checkBoxStopSnooze.UseVisualStyleBackColor = true;
			this.checkBoxStopSnooze.CheckedChanged += new System.EventHandler(this.checkBoxStopSnooze_CheckedChanged);
			// 
			// timer_PhpCronJob
			// 
			this.timer_PhpCronJob.Enabled = true;
			this.timer_PhpCronJob.Interval = 60000;
			this.timer_PhpCronJob.Tick += new System.EventHandler(this.timer_PhpCronJob_Tick);
			// 
			// fileSystemWatcher_SqlFiles
			// 
			this.fileSystemWatcher_SqlFiles.EnableRaisingEvents = true;
			this.fileSystemWatcher_SqlFiles.IncludeSubdirectories = true;
			this.fileSystemWatcher_SqlFiles.NotifyFilter = System.IO.NotifyFilters.LastWrite;
			this.fileSystemWatcher_SqlFiles.SynchronizingObject = this;
			this.fileSystemWatcher_SqlFiles.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_SqlFiles_Changed);
			// 
			// contextMenuItemsNode
			// 
			this.contextMenuItemsNode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_DeleteThisItem});
			// 
			// menuItem_DeleteThisItem
			// 
			this.menuItem_DeleteThisItem.Index = 0;
			this.menuItem_DeleteThisItem.Text = "&Delete this item";
			this.menuItem_DeleteThisItem.Click += new System.EventHandler(this.menuItem_DeleteThisItem_Click);
			// 
			// contextMenuItemsSubcat
			// 
			this.contextMenuItemsSubcat.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_AddItemToThisCategory});
			// 
			// menuItem_AddItemToThisCategory
			// 
			this.menuItem_AddItemToThisCategory.Index = 0;
			this.menuItem_AddItemToThisCategory.Text = "&Add item to this category";
			this.menuItem_AddItemToThisCategory.Click += new System.EventHandler(this.menuItem_AddItemToThisCategory_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(807, 438);
			this.Controls.Add(this.checkBoxStopSnooze);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDownAutosnoozeInterval);
			this.Controls.Add(this.numericUpDownRemindedCount);
			this.Controls.Add(this.checkBoxComplete);
			this.Controls.Add(this.dateTimePickerDue);
			this.Controls.Add(this.dateTimePickerCreated);
			this.Controls.Add(this.checkBoxAutoUpload);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.linkLabel_AddEmailAndPassword);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.textBoxLogs);
			this.Controls.Add(this.linkLabelGetCurrentTodolist);
			this.Controls.Add(this.linkLabel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(691, 365);
			this.Name = "Form1";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Monitor System";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Shown += new System.EventHandler(this.Form1_Shown);
			this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRemindedCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAutosnoozeInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher_SqlFiles)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
				private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox textBoxLogs;
        private System.Windows.Forms.LinkLabel linkLabel_AddEmailAndPassword;
        private System.Windows.Forms.LinkLabel linkLabelGetCurrentTodolist;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCurrentStatus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TreeView treeViewTodolist;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.LinkLabel linkLabel2;
				private System.Windows.Forms.CheckBox checkBoxAutoUpload;
        private System.Windows.Forms.CheckBox checkBoxComplete;
        private System.Windows.Forms.DateTimePicker dateTimePickerCreated;
        private System.Windows.Forms.DateTimePicker dateTimePickerDue;
        private System.Windows.Forms.NumericUpDown numericUpDownRemindedCount;
        private System.Windows.Forms.NumericUpDown numericUpDownAutosnoozeInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
				private System.Windows.Forms.CheckBox checkBoxStopSnooze;
				private System.Windows.Forms.Timer timer_PhpCronJob;
				private System.IO.FileSystemWatcher fileSystemWatcher_SqlFiles;
				private System.Windows.Forms.ContextMenu contextMenuItemsNode;
				private System.Windows.Forms.MenuItem menuItem_DeleteThisItem;
				private System.Windows.Forms.ContextMenu contextMenuItemsSubcat;
				private System.Windows.Forms.MenuItem menuItem_AddItemToThisCategory;
    }
}

