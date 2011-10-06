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
	public partial class TestAnimations : Form
	{
		public TestAnimations()
		{
			InitializeComponent();
		}

		private void TestAnimations_Shown(object sender, EventArgs e)
		{
			Screen screen = Screen.FromHandle(this.Handle);
			Rectangle bounds = screen.WorkingArea;
			this.Location = new Point(bounds.Left + (bounds.Width - this.Width) / 2, screen.Bounds.Bottom);
			ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
			{
				int EndTop = bounds.Top + (bounds.Height - this.Height) / 2;//this.Top + this.Height;
				while (this.Top > EndTop)
				{
					Thread.Sleep(3);
					ThreadingInterop.UpdateGuiFromThread(this, () =>
					{
						if (this.Top - 5 < EndTop) this.Top = EndTop;
						else this.Top -= 5;
					});
				}
			});

			/*int iterations = 100;
			//int originalFontSize = (int)this.Font.SizeInPoints;
			//int fontsizeDecreaseFactor = 2;
			float maxScaleFactor = 2;
			
			float originalOpacity = 0.4F;
			float finalOpacity = 0.8F;

			this.Scale(new SizeF(maxScaleFactor, maxScaleFactor));
			foreach (Control cntrl in this.Controls)
				cntrl.Scale(new SizeF(maxScaleFactor, maxScaleFactor));
			//this.Font = new System.Drawing.Font(this.Font.FontFamily, originalFontSize + iterations * fontsizeDecreaseFactor);
			this.Opacity = originalOpacity;
			ThreadingInterop.PerformVoidFunctionSeperateThread(() =>
			{
				//int EndTop = this.Top + this.Height;
				//while (this.Font.Size > 12)
				for (int i = 0; i < iterations; i++)
				{
					Thread.Sleep(1);
					ThreadingInterop.UpdateGuiFromThread(this, () =>
					{
						//this.Font = new Font(this.Font.FontFamily, this.Font.SizeInPoints - 1);//fontsizeDecreaseFactor);
						//this.PerformAutoScale();

						//if (Math.IEEERemainder(i, 10) == 0)
						//	CustomBalloonTip.ShowCustomBalloonTip("", "Scale: " + (maxScaleFactor - ((maxScaleFactor - 1) * i / iterations)), 3000, CustomBalloonTip.IconTypes.Information, delegate { });

						//this.Scale(new SizeF(1, 1));
						//this.Scale(new SizeF(maxScaleFactor - ((maxScaleFactor - 1) * i / iterations), maxScaleFactor - ((maxScaleFactor - 1) * i / iterations)));
						float fact = 0.98F;
						this.Scale(new SizeF(fact, fact));
						foreach (Control cntrl in this.Controls)
							cntrl.Scale(new SizeF(fact, fact));
						//this.PerformAutoScale();
						this.Opacity += (finalOpacity - originalOpacity) / iterations;
						//if (Math.IEEERemainder(i, 10) == 0)
						//  CustomBalloonTip.ShowCustomBalloonTip("", "Scale: " + this.AutoScaleFactor, 3000, CustomBalloonTip.IconTypes.Information, delegate { });
						//if (this.Top + 5 > EndTop) this.Top = EndTop;
						//else this.Top += 5;
					});
				}
			});*/
		}

		TestAnimations tmp = null;
		private void button1_Click(object sender, EventArgs e)
		{
			if (tmp == null)
			{
				tmp = new TestAnimations();
				//tmp.Show();
			}
			else
			Win32Api.AnimateWindow(tmp.Handle, 750, Win32Api.AW_ACTIVATE | Win32Api.AW_BLEND);
		}
	}
}
