namespace MonitorSystem
{
	partial class CustomBalloonTip
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
			this.button_Close = new System.Windows.Forms.Button();
			this.label_Title = new System.Windows.Forms.Label();
			this.label_Message = new System.Windows.Forms.Label();
			this.pictureBox_Icon = new System.Windows.Forms.PictureBox();
			this.timer_ShowDuration = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_Icon)).BeginInit();
			this.SuspendLayout();
			// 
			// button_Close
			// 
			this.button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Close.BackColor = System.Drawing.Color.Transparent;
			this.button_Close.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button_Close.FlatAppearance.BorderSize = 0;
			this.button_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.button_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.button_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button_Close.ForeColor = System.Drawing.Color.Red;
			this.button_Close.Location = new System.Drawing.Point(635, -1);
			this.button_Close.Name = "button_Close";
			this.button_Close.Size = new System.Drawing.Size(20, 20);
			this.button_Close.TabIndex = 0;
			this.button_Close.Text = "x";
			this.button_Close.UseVisualStyleBackColor = false;
			this.button_Close.Click += new System.EventHandler(this.button1_Click);
			// 
			// label_Title
			// 
			this.label_Title.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label_Title.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_Title.ForeColor = System.Drawing.Color.Green;
			this.label_Title.Location = new System.Drawing.Point(25, 1);
			this.label_Title.Name = "label_Title";
			this.label_Title.Size = new System.Drawing.Size(31, 15);
			this.label_Title.TabIndex = 1;
			this.label_Title.Text = "Title";
			// 
			// label_Message
			// 
			this.label_Message.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label_Message.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.label_Message.Location = new System.Drawing.Point(78, 2);
			this.label_Message.MaximumSize = new System.Drawing.Size(540, 0);
			this.label_Message.MinimumSize = new System.Drawing.Size(0, 17);
			this.label_Message.Name = "label_Message";
			this.label_Message.Size = new System.Drawing.Size(540, 17);
			this.label_Message.TabIndex = 2;
			this.label_Message.Text = "This is the message text, long text should be cut off. Then it should go to next " +
					"line. Then it should go to next line.";
			// 
			// pictureBox_Icon
			// 
			this.pictureBox_Icon.Location = new System.Drawing.Point(3, 3);
			this.pictureBox_Icon.Name = "pictureBox_Icon";
			this.pictureBox_Icon.Size = new System.Drawing.Size(16, 16);
			this.pictureBox_Icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox_Icon.TabIndex = 3;
			this.pictureBox_Icon.TabStop = false;
			// 
			// CustomBalloonTip
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(654, 22);
			this.ControlBox = false;
			this.Controls.Add(this.button_Close);
			this.Controls.Add(this.pictureBox_Icon);
			this.Controls.Add(this.label_Message);
			this.Controls.Add(this.label_Title);
			this.Cursor = System.Windows.Forms.Cursors.Cross;
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomBalloonTip";
			this.Opacity = 0.95D;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.Shown += new System.EventHandler(this.CustomBalloonTip_Shown);
			this.Resize += new System.EventHandler(this.CustomBalloonTip_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_Icon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button button_Close;
		private System.Windows.Forms.Label label_Title;
		private System.Windows.Forms.Label label_Message;
		private System.Windows.Forms.PictureBox pictureBox_Icon;
		private System.Windows.Forms.Timer timer_ShowDuration;
	}
}