using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MonitorSystem
{
	public partial class CustomBalloonTip : Form
	{
		public enum IconTypes { Error, Information, Question, Shield, Warning, None };
		public CustomBalloonTip(string Title, string Message, int Duration, CustomBalloonTip.IconTypes iconType, Form1.SimpleDelegate VoidDelegateToRunOnClick)
		{
			InitializeComponent();

			this.label_Title.Text = Title;
			this.label_Message.Text = Message;
			this.timer_ShowDuration.Interval = Duration;
			switch (iconType)
			{
				case IconTypes.Error: this.pictureBox_Icon.Image = SystemIcons.Error.ToBitmap(); break;
				case IconTypes.Information: this.pictureBox_Icon.Image = SystemIcons.Information.ToBitmap(); break;
				case IconTypes.Question: this.pictureBox_Icon.Image = SystemIcons.Question.ToBitmap(); break;
				case IconTypes.Shield: this.pictureBox_Icon.Image = SystemIcons.Shield.ToBitmap(); break;
				case IconTypes.Warning: this.pictureBox_Icon.Image = SystemIcons.Warning.ToBitmap(); break;
				default: break;
			}

			controls = new Control[] { this, label_Title, label_Message, pictureBox_Icon };

			AddDelgateToRelevantControls(VoidDelegateToRunOnClick);
		}

		Control[] controls;
		private void AddDelgateToRelevantControls(Form1.SimpleDelegate VoidDelegateToRunOnClick)
		{
			foreach (Control c in controls)
				c.Click += delegate
				{
					this.Close();
					VoidDelegateToRunOnClick.Invoke();
				};
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		delegate void DecreaseOpacityCallback();
		private void CustomBalloonTip_Shown(object sender, EventArgs e)
		{
			if (timer_ShowDuration.Interval != 0)
			{
				timer_ShowDuration.Tick += delegate
				{
					Rectangle bounds = new Rectangle(this.Location, this.Size);
					PerformVoidFunctionSeperateThread(() =>
						{
							for (int i = 0; i <= 10; i++)
							{
								Thread.Sleep(30);
								while (Bounds.Contains(MousePosition))
									Thread.Sleep(1);
								if (this.InvokeRequired)
								{
									DecreaseOpacityCallback d = new DecreaseOpacityCallback(delegate
									{
										this.Opacity -= 0.1;
									});
									this.Invoke(d, new object[] { });
								}
								else
								{
									this.Opacity -= 0.1;
								}
							}
						});

					//TODO: Should implement to NOT close if mouse is inside form
					this.Close();
				};
				timer_ShowDuration.Start();
			}
		}

		public static void PerformVoidFunctionSeperateThread(MethodInvoker method)
		{
			System.Threading.Thread th = new System.Threading.Thread(() =>
			{
				method.Invoke();
			});
			th.Start();
			//th.Join();
			while (th.IsAlive) { Application.DoEvents(); }
		}

		private void CustomBalloonTip_Resize(object sender, EventArgs e)
		{
			label_Message.MaximumSize = new System.Drawing.Size(this.Width - label_Message.Location.X - button_Close.Width, label_Message.MaximumSize.Height);
			label_Message.Width = this.Width - label_Message.Location.X - button_Close.Width;
		}
	}
}
