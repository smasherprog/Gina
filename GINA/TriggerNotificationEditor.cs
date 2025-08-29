using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200000C RID: 12
	public class TriggerNotificationEditor : BindableControl
	{
		// Token: 0x06000200 RID: 512 RVA: 0x000076DA File Offset: 0x000058DA
		static TriggerNotificationEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TriggerNotificationEditor), new FrameworkPropertyMetadata(typeof(TriggerNotificationEditor)));
			BindableControl.SetDependentProperties(typeof(TriggerNotificationEditor));
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000770E File Offset: 0x0000590E
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "DontUseSound", "UseTextToVoice", null);
			BindableControl.RegisterDependentProperty(type, "DontUseSound", "PlayMediaFile", null);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00007732 File Offset: 0x00005932
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000773F File Offset: 0x0000593F
		public bool UseText
		{
			get
			{
				return base.Get<bool>("UseText");
			}
			set
			{
				base.Set("UseText", value);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00007752 File Offset: 0x00005952
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000775F File Offset: 0x0000595F
		public string DisplayText
		{
			get
			{
				return base.Get<string>("DisplayText");
			}
			set
			{
				base.Set("DisplayText", value);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000776D File Offset: 0x0000596D
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000777A File Offset: 0x0000597A
		public bool CopyToClipboard
		{
			get
			{
				return base.Get<bool>("CopyToClipboard");
			}
			set
			{
				base.Set("CopyToClipboard", value);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000778D File Offset: 0x0000598D
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000779A File Offset: 0x0000599A
		public string ClipboardText
		{
			get
			{
				return base.Get<string>("ClipboardText");
			}
			set
			{
				base.Set("ClipboardText", value);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000077A8 File Offset: 0x000059A8
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000077B5 File Offset: 0x000059B5
		public bool UseTextToVoice
		{
			get
			{
				return base.Get<bool>("UseTextToVoice");
			}
			set
			{
				base.Set("UseTextToVoice", value);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000077C8 File Offset: 0x000059C8
		// (set) Token: 0x0600020D RID: 525 RVA: 0x000077D5 File Offset: 0x000059D5
		public string TextToVoiceText
		{
			get
			{
				return base.Get<string>("TextToVoiceText");
			}
			set
			{
				base.Set("TextToVoiceText", value);
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600020E RID: 526 RVA: 0x000077E3 File Offset: 0x000059E3
		// (set) Token: 0x0600020F RID: 527 RVA: 0x000077F0 File Offset: 0x000059F0
		public bool PlayMediaFile
		{
			get
			{
				return base.Get<bool>("PlayMediaFile");
			}
			set
			{
				base.Set("PlayMediaFile", value);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00007803 File Offset: 0x00005A03
		// (set) Token: 0x06000211 RID: 529 RVA: 0x00007810 File Offset: 0x00005A10
		public string MediaFileName
		{
			get
			{
				return base.Get<string>("MediaFileName");
			}
			set
			{
				base.Set("MediaFileName", value);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000781E File Offset: 0x00005A1E
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000782B File Offset: 0x00005A2B
		public bool InterruptSpeech
		{
			get
			{
				return base.Get<bool>("InterruptSpeech");
			}
			set
			{
				base.Set("InterruptSpeech", value);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000783E File Offset: 0x00005A3E
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000784B File Offset: 0x00005A4B
		public CharacterViewModel SelectedVoiceCharacter
		{
			get
			{
				return base.Get<CharacterViewModel>("SelectedVoiceCharacter");
			}
			set
			{
				base.Set("SelectedVoiceCharacter", value);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00007859 File Offset: 0x00005A59
		public App App
		{
			get
			{
				return (App)global::System.Windows.Application.Current;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00007865 File Offset: 0x00005A65
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000787C File Offset: 0x00005A7C
		public bool DontUseSound
		{
			get
			{
				return !this.UseTextToVoice && !this.PlayMediaFile;
			}
			set
			{
				if (value)
				{
					this.UseTextToVoice = (this.PlayMediaFile = false);
				}
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000789C File Offset: 0x00005A9C
		internal void SetFields(GimaSoft.Business.GINA.Trigger trigger)
		{
			if (trigger != null)
			{
				this.UseText = trigger.UseText;
				this.DisplayText = trigger.DisplayText;
				this.CopyToClipboard = trigger.CopyToClipboard;
				this.ClipboardText = trigger.ClipboardText;
				this.UseTextToVoice = trigger.UseTextToVoice;
				this.TextToVoiceText = trigger.TextToVoiceText;
				this.InterruptSpeech = trigger.InterruptSpeech;
				this.PlayMediaFile = trigger.PlayMediaFile;
				this.MediaFileName = trigger.MediaFileName;
				return;
			}
			this.UseText = false;
			this.DisplayText = "";
			this.CopyToClipboard = false;
			this.ClipboardText = "";
			this.UseTextToVoice = false;
			this.TextToVoiceText = "";
			this.InterruptSpeech = false;
			this.PlayMediaFile = false;
			this.MediaFileName = "";
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00007968 File Offset: 0x00005B68
		internal void SetTriggerFields(GimaSoft.Business.GINA.Trigger trigger)
		{
			trigger.UseText = this.UseText;
			trigger.DisplayText = this.DisplayText;
			trigger.CopyToClipboard = this.CopyToClipboard;
			trigger.ClipboardText = this.ClipboardText;
			trigger.UseTextToVoice = this.UseTextToVoice;
			trigger.TextToVoiceText = (this.UseTextToVoice ? this.TextToVoiceText : "");
			trigger.InterruptSpeech = this.UseTextToVoice && this.InterruptSpeech;
			trigger.PlayMediaFile = this.PlayMediaFile;
			trigger.MediaFileName = (this.PlayMediaFile ? this.MediaFileName : "");
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00007A6F File Offset: 0x00005C6F
		public GenericCommand PlaySampleCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					if (this.SelectedVoiceCharacter == null)
					{
						return;
					}
					if (this.UseTextToVoice && !string.IsNullOrWhiteSpace(this.TextToVoiceText))
					{
						this.SelectedVoiceCharacter.SayText(this.TextToVoiceText);
						return;
					}
					if (this.PlayMediaFile && !string.IsNullOrWhiteSpace(this.MediaFileName))
					{
						this.SelectedVoiceCharacter.PlayStream(this.MediaFileName);
					}
				});
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00007B28 File Offset: 0x00005D28
		public GenericCommand FindSoundFileCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						OpenFileDialog openFileDialog = new OpenFileDialog();
						openFileDialog.AddExtension = true;
						openFileDialog.DefaultExt = "txt";
						openFileDialog.CheckFileExists = false;
						openFileDialog.CheckPathExists = true;
						try
						{
							openFileDialog.FileName = Path.GetFileName(this.MediaFileName);
						}
						catch
						{
						}
						try
						{
							openFileDialog.InitialDirectory = Path.GetDirectoryName(this.MediaFileName);
						}
						catch
						{
						}
						openFileDialog.Multiselect = false;
						openFileDialog.Filter = "Wav files|*.wav|All Files|*.*";
						if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							this.MediaFileName = openFileDialog.FileName;
						}
					}
				};
			}
		}
	}
}
