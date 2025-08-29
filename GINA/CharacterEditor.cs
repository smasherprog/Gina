using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000003 RID: 3
	public class CharacterEditor : BindableControl
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002344 File Offset: 0x00000544
		static CharacterEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CharacterEditor), new FrameworkPropertyMetadata(typeof(CharacterEditor)));
			BindableControl.SetDependentProperties(typeof(CharacterEditor));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000023BD File Offset: 0x000005BD
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "AllowSave", "CharacterName", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "DisplayName", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "LogFilePath", null);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000023F2 File Offset: 0x000005F2
		public CharacterEditor()
		{
			this.SampleText = "{C}, change this text and press the play button to hear a sample.";
			this.VoiceName = this.AvailableVoices.FirstOrDefault<string>() ?? "";
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000241F File Offset: 0x0000061F
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002431 File Offset: 0x00000631
		public GINACharacter Character
		{
			get
			{
				return (GINACharacter)base.GetValue(CharacterEditor.CharacterProperty);
			}
			set
			{
				base.SetValue(CharacterEditor.CharacterProperty, value);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000243F File Offset: 0x0000063F
		public IEnumerable<string> AvailableVoices
		{
			get
			{
				return Configuration.InstalledVoices;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002446 File Offset: 0x00000646
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002453 File Offset: 0x00000653
		public string DisplayName
		{
			get
			{
				return base.Get<string>("DisplayName");
			}
			set
			{
				base.Set("DisplayName", value);
				if (string.IsNullOrWhiteSpace(this.CharacterName) && !string.IsNullOrWhiteSpace(value))
				{
					this.CharacterName = value;
				}
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000247D File Offset: 0x0000067D
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000248C File Offset: 0x0000068C
		public string CharacterName
		{
			get
			{
				return base.Get<string>("CharacterName");
			}
			set
			{
				base.Set("CharacterName", value);
				if (string.IsNullOrWhiteSpace(this.DisplayName) && !string.IsNullOrWhiteSpace(value))
				{
					this.DisplayName = value;
				}
				if (string.IsNullOrWhiteSpace(this.PhoneticName) && !string.IsNullOrWhiteSpace(value))
				{
					this.PhoneticName = value;
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024DD File Offset: 0x000006DD
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000024EA File Offset: 0x000006EA
		public string PhoneticName
		{
			get
			{
				return base.Get<string>("PhoneticName");
			}
			set
			{
				base.Set("PhoneticName", value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000024F8 File Offset: 0x000006F8
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00002505 File Offset: 0x00000705
		public bool AutoMonitor
		{
			get
			{
				return base.Get<bool>("AutoMonitor");
			}
			set
			{
				base.Set("AutoMonitor", value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002518 File Offset: 0x00000718
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002525 File Offset: 0x00000725
		public string VoiceName
		{
			get
			{
				return base.Get<string>("VoiceName");
			}
			set
			{
				base.Set("VoiceName", value);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002533 File Offset: 0x00000733
		// (set) Token: 0x0600002C RID: 44 RVA: 0x00002540 File Offset: 0x00000740
		public int VoiceSpeed
		{
			get
			{
				return base.Get<int>("VoiceSpeed");
			}
			set
			{
				base.Set("VoiceSpeed", value);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002553 File Offset: 0x00000753
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002560 File Offset: 0x00000760
		public int Volume
		{
			get
			{
				return base.Get<int>("Volume");
			}
			set
			{
				base.Set("Volume", value);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002573 File Offset: 0x00000773
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002580 File Offset: 0x00000780
		public string SampleText
		{
			get
			{
				return base.Get<string>("SampleText");
			}
			set
			{
				base.Set("SampleText", value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000258E File Offset: 0x0000078E
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000259C File Offset: 0x0000079C
		public string LogFilePath
		{
			get
			{
				return base.Get<string>("LogFilePath");
			}
			set
			{
				base.Set("LogFilePath", value);
				if (string.IsNullOrWhiteSpace(this.CharacterName) && !string.IsNullOrWhiteSpace(value))
				{
					try
					{
						Match match = Regex.Match(Path.GetFileName(this.LogFilePath), "^eqlog_(?'name'[A-Za-z]+)_(?'server'[A-Za-z]+)");
						if (match.Success)
						{
							this.CharacterName = match.Groups["name"].Value;
							this.DisplayName = string.Format("{0} ({1})", match.Groups["name"].Value, match.Groups["server"].Value);
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002658 File Offset: 0x00000858
		public bool AllowSave
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.CharacterName) && !string.IsNullOrWhiteSpace(this.DisplayName) && !string.IsNullOrWhiteSpace(this.LogFilePath);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002684 File Offset: 0x00000884
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0000268C File Offset: 0x0000088C
		private SpeechSynthesizer Synthesizer { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002695 File Offset: 0x00000895
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000026A2 File Offset: 0x000008A2
		public Color TextFontColor
		{
			get
			{
				return base.Get<Color>("TextFontColor");
			}
			set
			{
				base.Set("TextFontColor", value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000026B5 File Offset: 0x000008B5
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000026C2 File Offset: 0x000008C2
		public Color TimerFontColor
		{
			get
			{
				return base.Get<Color>("TimerFontColor");
			}
			set
			{
				base.Set("TimerFontColor", value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000026D5 File Offset: 0x000008D5
		// (set) Token: 0x0600003B RID: 59 RVA: 0x000026E2 File Offset: 0x000008E2
		public Color TimerBarColor
		{
			get
			{
				return base.Get<Color>("TimerBarColor");
			}
			set
			{
				base.Set("TimerBarColor", value);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000026F8 File Offset: 0x000008F8
		private static void CharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CharacterEditor characterEditor = d as CharacterEditor;
			characterEditor.SetFields();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002714 File Offset: 0x00000914
		private void SetCharacterFields(GINACharacter character)
		{
			character.Name = this.CharacterName;
			character.DisplayName = this.DisplayName;
			character.AutoMonitor = this.AutoMonitor;
			character.VoiceName = this.VoiceName;
			character.VoiceSpeed = this.VoiceSpeed;
			character.Volume = this.Volume;
			character.LogFilePath = this.LogFilePath;
			character.TextStyle.FontColor = new SolidColorBrush(this.TextFontColor);
			character.TimerStyle.FontColor = new SolidColorBrush(this.TimerFontColor);
			character.TimerStyle.TimerBarColor = this.TimerBarColor;
			if (character.AutoMonitor && !character.IsEnabled)
			{
				character.SetMonitoringStatus(true, true);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000027CC File Offset: 0x000009CC
		public void SetFields()
		{
			if (this.Character != null)
			{
				this.CharacterName = this.Character.Name;
				this.DisplayName = this.Character.DisplayName;
				this.PhoneticName = this.Character.PhoneticName;
				this.AutoMonitor = this.Character.AutoMonitor;
				this.VoiceName = this.Character.VoiceName;
				this.VoiceSpeed = this.Character.VoiceSpeed;
				this.Volume = this.Character.Volume;
				this.LogFilePath = this.Character.LogFilePath;
				this.TextFontColor = this.Character.TextStyle.FontColor.Color;
				this.TimerFontColor = this.Character.TimerStyle.FontColor.Color;
				this.TimerBarColor = this.Character.TimerStyle.TimerBarColor;
				return;
			}
			this.CharacterName = "";
			this.DisplayName = "";
			this.PhoneticName = "";
			this.AutoMonitor = true;
			this.VoiceName = this.AvailableVoices.FirstOrDefault<string>() ?? "";
			this.VoiceSpeed = 0;
			this.Volume = 100;
			this.LogFilePath = "";
			this.TextFontColor = Colors.Yellow;
			this.TimerFontColor = Colors.Yellow;
			this.TimerBarColor = Colors.Maroon;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002938 File Offset: 0x00000B38
		private void SetSynthesizer()
		{
			if (this.Synthesizer == null)
			{
				this.Synthesizer = new SpeechSynthesizer();
			}
			try
			{
				this.Synthesizer.SelectVoice(this.VoiceName);
			}
			catch
			{
			}
			this.Synthesizer.Rate = this.VoiceSpeed;
			this.Synthesizer.Volume = this.Volume;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002B28 File Offset: 0x00000D28
		public GenericCommand SaveCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (this.Character == null)
						{
							GINACharacter ginacharacter = new GINACharacter();
							this.SetCharacterFields(ginacharacter);
							using (IEnumerator<TriggerGroup> enumerator = TriggerGroup.RootGroup.DescendantTree.Where((TriggerGroup o) => o.EnableByDefault).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									TriggerGroup triggerGroup = enumerator.Current;
									ginacharacter.TriggerGroups.Add(triggerGroup);
								}
								goto IL_007C;
							}
						}
						this.SetCharacterFields(this.Character);
						IL_007C:
						if (string.IsNullOrWhiteSpace(Configuration.Current.EverquestFolder) && !string.IsNullOrWhiteSpace(this.LogFilePath))
						{
							try
							{
								string directoryName = Path.GetDirectoryName(this.LogFilePath);
								if (Path.GetFileName(directoryName).ToLower() == Configuration.EverquestLogFolder.ToLower())
								{
									Configuration.Current.EverquestFolder = Path.GetDirectoryName(directoryName);
								}
							}
							catch
							{
							}
						}
						if (string.IsNullOrWhiteSpace(this.PhoneticName))
						{
							this.PhoneticName = this.CharacterName;
						}
						if (this.CharacterName.ToUpper() == this.PhoneticName.ToUpper())
						{
							PhoneticTransform.Remove(this.CharacterName);
						}
						else if (this.CharacterName.ToUpper() != this.PhoneticName.ToUpper())
						{
							PhoneticTransform.Add(this.CharacterName, this.PhoneticName);
						}
						Configuration.SaveConfiguration(false);
						base.Visibility = Visibility.Collapsed;
					}
				};
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002B58 File Offset: 0x00000D58
		public GenericCommand CancelCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						base.Visibility = Visibility.Collapsed;
					}
				};
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002B90 File Offset: 0x00000D90
		public GenericCommand VisibilityChangedCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if ((Visibility)p == Visibility.Visible)
						{
							this.SetFields();
						}
					}
				};
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public GenericCommand FindFileCommand
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
						openFileDialog.ShowHelp = true;
						openFileDialog.HelpRequest += delegate(object o, EventArgs e)
						{
							global::System.Windows.Forms.MessageBox.Show("Please select the log file of the character that you wish to monitor.");
						};
						try
						{
							openFileDialog.FileName = Path.GetFileName(this.LogFilePath);
						}
						catch
						{
						}
						try
						{
							openFileDialog.InitialDirectory = (string.IsNullOrWhiteSpace(this.LogFilePath) ? Path.Combine(Configuration.Current.EverquestFolder, Configuration.EverquestLogFolder) : Path.GetDirectoryName(this.LogFilePath));
						}
						catch
						{
						}
						openFileDialog.Multiselect = false;
						openFileDialog.Filter = "EverQuest Log Files|eqlog*.txt|All Files|*.*";
						if (openFileDialog.ShowDialog() == DialogResult.OK)
						{
							this.LogFilePath = openFileDialog.FileName;
						}
					}
				};
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002D38 File Offset: 0x00000F38
		public GenericCommand PlaySampleCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (string.IsNullOrWhiteSpace(this.VoiceName) || string.IsNullOrWhiteSpace(this.SampleText))
						{
							return;
						}
						this.SetSynthesizer();
						this.Synthesizer.SpeakAsyncCancelAll();
						this.Synthesizer.SpeakAsync(this.SampleText.Replace("{C}", this.PhoneticName));
					}
				};
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public GenericCommand PlayPhoneticNameCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (string.IsNullOrWhiteSpace(this.VoiceName) || string.IsNullOrWhiteSpace(this.PhoneticName))
						{
							return;
						}
						this.SetSynthesizer();
						this.Synthesizer.SpeakAsyncCancelAll();
						this.Synthesizer.SpeakAsync(this.PhoneticName);
					}
				};
			}
		}

		// Token: 0x0400000B RID: 11
		public static readonly DependencyProperty CharacterProperty = DependencyProperty.Register("Character", typeof(GINACharacter), typeof(CharacterEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(CharacterEditor.CharacterChanged)));
	}
}
