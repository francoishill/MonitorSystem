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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewBackups));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.richTextBox_Description = new System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripDropDownButton_Filter = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.richTextBox_FileContents = new ScintillaNet.Scintilla();
			this.contextMenu_FileNode = new System.Windows.Forms.ContextMenu();
			this.menuItem_DiscardEmptyBackups = new System.Windows.Forms.MenuItem();
			this.contextMenu_ModificationNode = new System.Windows.Forms.ContextMenu();
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
			((System.ComponentModel.ISupportInitialize)(this.richTextBox_FileContents)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.richTextBox_Description);
			this.splitContainer1.Size = new System.Drawing.Size(705, 500);
			this.splitContainer1.SplitterDistance = 189;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Location = new System.Drawing.Point(411, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Filter by description:";
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(255)))), ((int)(((byte)(220)))));
			this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBox1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(517, 3);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(185, 21);
			this.comboBox1.TabIndex = 3;
			this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.HideSelection = false;
			this.treeView1.HotTracking = true;
			this.treeView1.Indent = 30;
			this.treeView1.Location = new System.Drawing.Point(0, 30);
			this.treeView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.treeView1.Name = "treeView1";
			this.treeView1.ShowLines = false;
			this.treeView1.ShowRootLines = false;
			this.treeView1.Size = new System.Drawing.Size(705, 159);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.treeView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			// 
			// richTextBox_Description
			// 
			this.richTextBox_Description.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.richTextBox_Description.ForeColor = System.Drawing.Color.DarkGreen;
			this.richTextBox_Description.Location = new System.Drawing.Point(0, 0);
			this.richTextBox_Description.Name = "richTextBox_Description";
			this.richTextBox_Description.ReadOnly = true;
			this.richTextBox_Description.Size = new System.Drawing.Size(705, 305);
			this.richTextBox_Description.TabIndex = 0;
			this.richTextBox_Description.Text = "";
			this.richTextBox_Description.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.richTextBox_Description.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton_Filter});
			this.statusStrip1.Location = new System.Drawing.Point(0, 500);
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
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// toolStripDropDownButton_Filter
			// 
			this.toolStripDropDownButton_Filter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton_Filter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
			this.toolStripDropDownButton_Filter.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton_Filter.Image")));
			this.toolStripDropDownButton_Filter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton_Filter.Name = "toolStripDropDownButton_Filter";
			this.toolStripDropDownButton_Filter.Size = new System.Drawing.Size(110, 20);
			this.toolStripDropDownButton_Filter.Text = "Click to add filter";
			// 
			// toolStripTextBox1
			// 
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.richTextBox_FileContents);
			this.splitContainer2.Size = new System.Drawing.Size(1228, 500);
			this.splitContainer2.SplitterDistance = 705;
			this.splitContainer2.TabIndex = 2;
			// 
			// richTextBox_FileContents
			// 
			this.richTextBox_FileContents.CausesValidation = false;
			this.richTextBox_FileContents.ConfigurationManager.Language = "mssql";
			this.richTextBox_FileContents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox_FileContents.Folding.Flags = ((ScintillaNet.FoldFlag)((ScintillaNet.FoldFlag.LineBeforeContracted | ScintillaNet.FoldFlag.LineAfterContracted)));
			this.richTextBox_FileContents.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.richTextBox_FileContents.IsBraceMatching = true;
			this.richTextBox_FileContents.IsReadOnly = true;
			this.richTextBox_FileContents.LineWrap.Mode = ScintillaNet.WrapMode.Word;
			this.richTextBox_FileContents.LineWrap.VisualFlags = ((ScintillaNet.WrapVisualFlag)((ScintillaNet.WrapVisualFlag.End | ScintillaNet.WrapVisualFlag.Start)));
			this.richTextBox_FileContents.Location = new System.Drawing.Point(0, 0);
			this.richTextBox_FileContents.Margins.FoldMarginColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.richTextBox_FileContents.Margins.Margin0.Width = 30;
			this.richTextBox_FileContents.Margins.Margin2.Width = 20;
			this.richTextBox_FileContents.Name = "richTextBox_FileContents";
			this.richTextBox_FileContents.Size = new System.Drawing.Size(519, 500);
			this.richTextBox_FileContents.TabIndex = 0;
			this.richTextBox_FileContents.UseFont = true;
			this.richTextBox_FileContents.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ViewBackups_PreviewKeyDown);
			// 
			// contextMenu_FileNode
			// 
			this.contextMenu_FileNode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_DiscardEmptyBackups});
			// 
			// menuItem_DiscardEmptyBackups
			// 
			this.menuItem_DiscardEmptyBackups.Index = 0;
			this.menuItem_DiscardEmptyBackups.Text = "Discard &empty backups";
			this.menuItem_DiscardEmptyBackups.Click += new System.EventHandler(this.menuItem_DiscardEmptyBackups_Click);
			// 
			// contextMenu_ModificationNode
			// 
			this.contextMenu_ModificationNode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_DiscardBackup,
            this.menuItem_AddDescription});
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
			this.ClientSize = new System.Drawing.Size(1228, 522);
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
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.richTextBox_FileContents)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		public System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		public System.Windows.Forms.RichTextBox richTextBox_Description;
		private System.Windows.Forms.MenuItem menuItem_DiscardEmptyBackups;
		public System.Windows.Forms.ContextMenu contextMenu_FileNode;
		public System.Windows.Forms.ContextMenu contextMenu_ModificationNode;
		private System.Windows.Forms.MenuItem menuItem_DiscardBackup;
		private System.Windows.Forms.MenuItem menuItem_AddDescription;
		private ScintillaNet.Scintilla richTextBox_FileContents;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton_Filter;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
	}
}