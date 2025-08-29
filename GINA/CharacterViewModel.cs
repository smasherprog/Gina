using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200001F RID: 31
	public class CharacterViewModel : GINAViewModel
	{
		// Token: 0x0600036E RID: 878 RVA: 0x0000BC64 File Offset: 0x00009E64
		public CharacterViewModel(GINACharacter character)
		{
			this.Character = character;
			this.Character.TriggerMatched += this.Character_TriggerMatched;
			this.Character.ShareDetected += this.Character_ShareDetected;
			this.Character.PropertyChanged += this.Character_PropertyChanged;
			Configuration.Current.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "MasterVolume")
				{
					this.SetVolume();
				}
			};
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		// (set) Token: 0x06000370 RID: 880 RVA: 0x0000BCED File Offset: 0x00009EED
		public GINACharacter Character
		{
			get
			{
				return base.Get<GINACharacter>("Character");
			}
			set
			{
				base.Set("Character", value);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000371 RID: 881 RVA: 0x0000BCFB File Offset: 0x00009EFB
		// (set) Token: 0x06000372 RID: 882 RVA: 0x0000BD08 File Offset: 0x00009F08
		public bool IsSelected
		{
			get
			{
				return base.Get<bool>("IsSelected");
			}
			set
			{
				base.Set("IsSelected", value);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000373 RID: 883 RVA: 0x0000BD1B File Offset: 0x00009F1B
		private SpeechSynthesizer Synthesizer
		{
			get
			{
				if (this._Synthesizer == null)
				{
					this._Synthesizer = new SpeechSynthesizer();
					this.SetSynthesizer();
					this.SetVolume();
				}
				return this._Synthesizer;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000374 RID: 884 RVA: 0x0000BD42 File Offset: 0x00009F42
		private MediaPlayer MediaPlayer
		{
			get
			{
				if (this._MediaPlayer == null)
				{
					this._MediaPlayer = new MediaPlayer();
					this.SetVolume();
				}
				return this._MediaPlayer;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000BD64 File Offset: 0x00009F64
		private void SetSynthesizer()
		{
			try
			{
				this.Synthesizer.SelectVoice(this.Character.VoiceName);
			}
			catch
			{
			}
			this.Synthesizer.Rate = this.Character.VoiceSpeed;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		private void SetVolume()
		{
			if (this._Synthesizer != null)
			{
				this._Synthesizer.Volume = Convert.ToInt32((double)(this.Character.Volume * Configuration.Current.MasterVolume) * 0.01);
			}
			if (this._MediaPlayer != null)
			{
				this._MediaPlayer.Volume = (double)(this.Character.Volume * Configuration.Current.MasterVolume) * 0.0001;
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000BE50 File Offset: 0x0000A050
		public void SayText(string text)
		{
			PhoneticTransform phoneticTransform = Configuration.Current.PhoneticDictionary.FirstOrDefault((PhoneticTransform o) => o.ActualWord.ToUpper() == this.Character.Name.ToUpper());
			this.Synthesizer.SpeakAsync(text.Replace("{C}", (phoneticTransform != null) ? phoneticTransform.PhoneticWord : this.Character.Name));
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
		public void PlayStream(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
			{
				return;
			}
			try
			{
				this.MediaPlayer.Open(new Uri(fileName));
				this.MediaPlayer.Play();
			}
			catch
			{
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000379 RID: 889 RVA: 0x0000BF41 File Offset: 0x0000A141
		public GenericCommand CharacterInstructionCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					string text;
					if ((text = p as string) != null)
					{
						if (text == "stop")
						{
							this.Character.StopMatches();
							return;
						}
						if (!(text == "resetcounters"))
						{
							return;
						}
						this.Character.ResetMatchCounters();
					}
				});
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C010 File Offset: 0x0000A210
		private void Character_TriggerMatched(object sender, TriggerMatchedEventArgs e)
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				if (Configuration.Current.EnableSound)
				{
					if (e.UseTextToVoice && !string.IsNullOrWhiteSpace(e.VoiceText))
					{
						if (e.InterruptSpeech)
						{
							this.Synthesizer.SpeakAsyncCancelAll();
						}
						this.Synthesizer.SpeakAsync(e.VoiceText);
					}
					else if (e.PlayMediaFile)
					{
						this.PlayStream(e.MediaFileName);
					}
				}
				this.App.Data.LogTriggerMatch(e);
			}));
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000C060 File Offset: 0x0000A260
		private void Character_ShareDetected(object sender, ShareDetectedEventArgs e)
		{
			base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				Package.AddDetectedShare(e);
			}));
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000C094 File Offset: 0x0000A294
		private void Character_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName;
			if ((propertyName = e.PropertyName) != null)
			{
				if (propertyName == "VoiceName" || propertyName == "VoiceSpeed")
				{
					this.SetSynthesizer();
					return;
				}
				if (!(propertyName == "Volume"))
				{
					return;
				}
				this.SetVolume();
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000C0E0 File Offset: 0x0000A2E0
		public override void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				this._Disposed = true;
				if (disposing && this.Synthesizer != null)
				{
					this.Synthesizer.Dispose();
				}
			}
		}

		// Token: 0x040000C5 RID: 197
		private SpeechSynthesizer _Synthesizer;

		// Token: 0x040000C6 RID: 198
		private MediaPlayer _MediaPlayer;
	}
}
