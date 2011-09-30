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
			this.acceptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.discardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.discardemptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.splitContainer1.TabStop = false;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(600, 306);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.treeView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(0, 0);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(527, 306);
			this.textBox1.TabIndex = 0;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
			// 
			// contextMenuStrip_TotalFile
			// 
			this.contextMenuStrip_TotalFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearMessagesToolStripMenuItem,
            this.discardemptyToolStripMenuItem});
			this.contextMenuStrip_TotalFile.Name = "contextMenuStrip_TotalFile";
			this.contextMenuStrip_TotalFile.Size = new System.Drawing.Size(156, 70);
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
            this.acceptToolStripMenuItem,
            this.discardToolStripMenuItem});
			this.contextMenuStrip_FileModification.Name = "contextMenuStrip_TotalFile";
			this.contextMenuStrip_FileModification.Size = new System.Drawing.Size(173, 70);
			// 
			// clearthisMessageToolStripMenuItem
			// 
			this.clearthisMessageToolStripMenuItem.Name = "clearthisMessageToolStripMenuItem";
			this.clearthisMessageToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.clearthisMessageToolStripMenuItem.Text = "Clear &this message";
			// 
			// acceptToolStripMenuItem
			// 
			this.acceptToolStripMenuItem.Name = "acceptToolStripMenuItem";
			this.acceptToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.acceptToolStripMenuItem.Text = "&Accept";
			this.acceptToolStripMenuItem.Click += new System.EventHandler(this.acceptToolStripMenuItem_Click_1);
			// 
			// discardToolStripMenuItem
			// 
			this.discardToolStripMenuItem.Name = "discardToolStripMenuItem";
			this.discardToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
			this.discardToolStripMenuItem.Text = "Dis&card";
			this.discardToolStripMenuItem.Click += new System.EventHandler(this.discardToolStripMenuItem_Click);
			// 
			// discardemptyToolStripMenuItem
			// 
			this.discardemptyToolStripMenuItem.Name = "discardemptyToolStripMenuItem";
			this.discardemptyToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.discardemptyToolStripMenuItem.Text = "Discard &empty";
			this.discardemptyToolStripMenuItem.Click += new System.EventHandler(this.discardemptyToolStripMenuItem_Click);
			// 
			// MonitoredFilesChanged
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1155, 330);
			this.Controls.Add(this.splitContainer1);
			this.KeyPreview = true;
			this.Name = "MonitoredFilesChanged";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MonitoredFilesChanged";
			this.Shown += new System.EventHandler(this.MonitoredFilesChanged_Shown);
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MonitoredFilesChanged_PreviewKeyDown);
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
		private System.Windows.Forms.ToolStripMenuItem acceptToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem discardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem discardemptyToolStripMenuItem;
	}
}