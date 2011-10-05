namespace MonitorSystem
{
	partial class ViewBackups
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.textBoxDescription = new System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.richTextBox_FileContents = new System.Windows.Forms.RichTextBox();
			this.contextMenu_FileNode = new System.Windows.Forms.ContextMenu();
			this.contextMenu_ModificationNode = new System.Windows.Forms.ContextMenu();
			this.menuItem_DiscardEmptyBackups = new System.Windows.Forms.MenuItem();
			this.menuItem_DiscardBackup = new System.Windows.Forms.MenuItem();
			this.menuItem_AddDescription = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBoxDescription);
			this.splitContainer1.Size = new System.Drawing.Size(1228, 272);
			this.splitContainer1.SplitterDistance = 409;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 0;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.HideSelection = false;
			this.treeView1.HotTracking = true;
			this.treeView1.Indent = 30;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.treeView1.Name = "treeView1";
			this.treeView1.ShowLines = false;
			this.treeView1.ShowRootLines = false;
			this.treeView1.Size = new System.Drawing.Size(409, 272);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.treeView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.textBoxDescription.ForeColor = System.Drawing.Color.DarkGreen;
			this.textBoxDescription.Location = new System.Drawing.Point(0, 0);
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.Size = new System.Drawing.Size(813, 272);
			this.textBoxDescription.TabIndex = 0;
			this.textBoxDescription.Text = "";
			this.textBoxDescription.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.textBoxDescription.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 571);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(1228, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			this.statusStrip1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(117, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.richTextBox_FileContents);
			this.splitContainer2.Size = new System.Drawing.Size(1228, 571);
			this.splitContainer2.SplitterDistance = 272;
			this.splitContainer2.TabIndex = 2;
			// 
			// richTextBox_FileContents
			// 
			this.richTextBox_FileContents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox_FileContents.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox_FileContents.ForeColor = System.Drawing.Color.Gray;
			this.richTextBox_FileContents.Location = new System.Drawing.Point(0, 0);
			this.richTextBox_FileContents.Name = "richTextBox_FileContents";
			this.richTextBox_FileContents.ReadOnly = true;
			this.richTextBox_FileContents.Size = new System.Drawing.Size(1228, 295);
			this.richTextBox_FileContents.TabIndex = 0;
			this.richTextBox_FileContents.Text = "";
			// 
			// contextMenu_FileNode
			// 
			this.contextMenu_FileNode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_DiscardEmptyBackups});
			// 
			// contextMenu_ModificationNode
			// 
			this.contextMenu_ModificationNode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_DiscardBackup,
            this.menuItem_AddDescription});
			// 
			// menuItem_DiscardEmptyBackups
			// 
			this.menuItem_DiscardEmptyBackups.Index = 0;
			this.menuItem_DiscardEmptyBackups.Text = "Discard &empty backups";
			this.menuItem_DiscardEmptyBackups.Click += new System.EventHandler(this.menuItem_DiscardEmptyBackups_Click);
			// 
			// menuItem_DiscardBackup
			// 
			this.menuItem_DiscardBackup.Index = 0;
			this.menuItem_DiscardBackup.Text = "Dis&card backup";
			this.menuItem_DiscardBackup.Click += new System.EventHandler(this.menuItem_DiscardBackup_Click);
			// 
			// menuItem_AddDescription
			// 
			this.menuItem_AddDescription.Index = 1;
			this.menuItem_AddDescription.Text = "Add &description";
			this.menuItem_AddDescription.Click += new System.EventHandler(this.menuItem_AddDescription_Click);
			// 
			// ViewBackups
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1228, 593);
			this.Controls.Add(this.splitContainer2);
			this.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "ViewBackups";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ViewBackups";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Shown += new System.EventHandler(this.ViewBackups_Shown);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		public System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.RichTextBox richTextBox_FileContents;
		public System.Windows.Forms.RichTextBox textBoxDescription;
		private System.Windows.Forms.MenuItem menuItem_DiscardEmptyBackups;
		public System.Windows.Forms.ContextMenu contextMenu_FileNode;
		public System.Windows.Forms.ContextMenu contextMenu_ModificationNode;
		private System.Windows.Forms.MenuItem menuItem_DiscardBackup;
		private System.Windows.Forms.MenuItem menuItem_AddDescription;
	}
}