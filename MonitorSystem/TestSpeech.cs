using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace MonitorSystem
{
	public partial class TestSpeech : Form
	{
		public TestSpeech()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
			Grammar dictationGrammar = new DictationGrammar();
			recognizer.LoadGrammar(dictationGrammar);
			try
			{
				button1.Text = "Speak Now";
				recognizer.SetInputToDefaultAudioDevice();
				RecognitionResult result = recognizer.Recognize();
				if (result != null)
				{
					button1.Text = "&Click";
					textBox1.Text += (textBox1.Text.Length > 0 ? Environment.NewLine : "") + result.Text;
				}
				else
					UserMessages.ShowWarningMessage("Cannot recognize speech, no result");
			}
			catch (InvalidOperationException exception)
			{
				button1.Text = String.Format("Could not recognize input from default aduio device. Is a microphone or sound card available?\r\n{0} - {1}.", exception.Source, exception.Message);
			}
			finally
			{
				recognizer.UnloadAllGrammars();
			}                          
		}
	}
}
