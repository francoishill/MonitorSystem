namespace MonitorSystem
{
	partial class MonitoredFilesChanged
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.contextMenuStrip_TotalFile = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearMessagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip_FileModification = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.clearthisMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.keepForlaterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenuStrip_TotalFile.SuspendLayout();
			this.contextMenuStrip_FileModification.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(12, 12);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.textBox1);
			this.splitContainer1.Size = new System.Drawing.Size(1131, 306);
			this.splitContainer1.SplitterDistance = 600;
			this.splitContainer1.TabIndex = 2;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(600, 306);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(527, 306);
			this.textBox1.TabIndex = 2;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// contextMenuStrip_TotalFile
			// 
			this.contextMenuStrip_TotalFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearMessagesToolStripMenuItem});
			this.contextMenuStrip_TotalFile.Name = "contextMenuStrip_TotalFile";
			this.contextMenuStrip_TotalFile.Size = new System.Drawing.Size(156, 26);
			// 
			// clearMessagesToolStripMenuItem
			// 
			this.clearMessagesToolStripMenuItem.Name = "clearMessagesToolStripMenuItem";
			this.clearMessagesToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.clearMessagesToolStripMenuItem.Text = "&Clear messages";
			// 
			// contextMenuStrip_FileModification
			// 
			this.contextMenuStrip_FileModification.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearthisMessageToolStripMenuItem,
            this.keepForlaterToolStripMenuItem});
			this.contextMenuStrip_FileModification.Name = "contextMenuStrip_TotalFile";
			this.contextMenuStrip_FileModification.Size = new System.Drawing.Size(173, 48);
			// 
			// clearthisMessageToolStripMenuItem
			// 
			this.clearthisMessageToolStripMenuItem.Name = "clearthisMessageToolStripMenuItem";
			this.clearthisMessageToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.clearthisMessageToolStripMenuItem.Text = "Clear &this message";
			// 
			// keepForlaterToolStripMenuItem
			// 
			this.keepForlaterToolStripMenuItem.Name = "keepForlaterToolStripMenuItem";
			this.keepForlaterToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.keepForlaterToolStripMenuItem.Text = "Keep for &later";
			this.keepForlaterToolStripMenuItem.Click += new System.EventHandler(this.keepForlaterToolStripMenuItem_Click);
			// 
			// MonitoredFilesChanged
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1155, 330);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MonitoredFilesChanged";
			this.Text = "MonitoredFilesChanged";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.contextMenuStrip_TotalFile.ResumeLayout(false);
			this.contextMenuStrip_FileModification.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		public System.Windows.Forms.TreeView treeView1;
		public System.Windows.Forms.TextBox textBox1;
		public System.Windows.Forms.ContextMenuStrip contextMenuStrip_TotalFile;
		public System.Windows.Forms.ContextMenuStrip contextMenuStrip_FileModification;
		private System.Windows.Forms.ToolStripMenuItem clearMessagesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearthisMessageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem keepForlaterToolStripMenuItem;
	}
}