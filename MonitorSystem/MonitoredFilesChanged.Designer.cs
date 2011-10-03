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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.contextMenu_TotalFile = new System.Windows.Forms.ContextMenu();
			this.menuItem_ClearMessages = new System.Windows.Forms.MenuItem();
			this.menuItem_DiscardEmpty = new System.Windows.Forms.MenuItem();
			this.contextMenu_FileModification = new System.Windows.Forms.ContextMenu();
			this.menuItem_ClearThisMessage = new System.Windows.Forms.MenuItem();
			this.menuItem_Accept = new System.Windows.Forms.MenuItem();
			this.menuItem_Discard = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
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
			this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			// contextMenu_TotalFile
			// 
			this.contextMenu_TotalFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_ClearMessages,
            this.menuItem_DiscardEmpty});
			// 
			// menuItem_ClearMessages
			// 
			this.menuItem_ClearMessages.Checked = true;
			this.menuItem_ClearMessages.Index = 0;
			this.menuItem_ClearMessages.Text = "&Clear messages";
			this.menuItem_ClearMessages.Click += new System.EventHandler(this.menuItem_ClearMessages_Click);
			// 
			// menuItem_DiscardEmpty
			// 
			this.menuItem_DiscardEmpty.Index = 1;
			this.menuItem_DiscardEmpty.Text = "Discard &empty";
			this.menuItem_DiscardEmpty.Click += new System.EventHandler(this.menuItem_DiscardEmpty_Click);
			// 
			// contextMenu_FileModification
			// 
			this.contextMenu_FileModification.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_ClearThisMessage,
            this.menuItem_Accept,
            this.menuItem_Discard});
			// 
			// menuItem_ClearThisMessage
			// 
			this.menuItem_ClearThisMessage.Index = 0;
			this.menuItem_ClearThisMessage.Text = "Clear &this message";
			this.menuItem_ClearThisMessage.Click += new System.EventHandler(this.menuItem_ClearThisMessage_Click);
			// 
			// menuItem_Accept
			// 
			this.menuItem_Accept.Index = 1;
			this.menuItem_Accept.Text = "&Accept";
			this.menuItem_Accept.Click += new System.EventHandler(this.menuItem_Accept_Click);
			// 
			// menuItem_Discard
			// 
			this.menuItem_Discard.Index = 2;
			this.menuItem_Discard.Text = "Dis&card";
			this.menuItem_Discard.Click += new System.EventHandler(this.menuItem_Discard_Click);
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
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		public System.Windows.Forms.TreeView treeView1;
		public System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.MenuItem menuItem_ClearMessages;
		private System.Windows.Forms.MenuItem menuItem_DiscardEmpty;
		public System.Windows.Forms.ContextMenu contextMenu_TotalFile;
		public System.Windows.Forms.ContextMenu contextMenu_FileModification;
		private System.Windows.Forms.MenuItem menuItem_ClearThisMessage;
		private System.Windows.Forms.MenuItem menuItem_Accept;
		private System.Windows.Forms.MenuItem menuItem_Discard;
	}
}