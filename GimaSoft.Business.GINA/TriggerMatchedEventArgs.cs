using System;
using System.Linq;
using System.Text.RegularExpressions;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200001C RID: 28
	public class TriggerMatchedEventArgs : BindableObject, IDisposable
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00005950 File Offset: 0x00003B50
		public TriggerMatchedEventArgs(TriggerFilter filter, string line, MatchTypes matchType = MatchTypes.Standard)
		{
			this.MatchedTime = DateTime.Now;
			this.TriggerFilter = filter;
			this.MatchType = matchType;
			this.CounterInstance = filter.Matches;
			this.TimerSpanMilliseconds = filter.ResolveTimerSpan(line);
			Match match = Configuration.Current.LogFormatRegex.Match(line);
			if (match.Success)
			{
				this.MatchedText = match.Groups["text"].Value;
				string text = string.Format("{0} {1} {2} {3}:{4}:{5}", new object[]
				{
					match.Groups["month"].Value,
					match.Groups["date"].Value,
					match.Groups["year"].Value,
					match.Groups["hour"].Value,
					match.Groups["minute"].Value,
					match.Groups["second"].Value
				});
				DateTime dateTime = new DateTime(this.MatchedTime.Ticks);
				DateTime.TryParse(text, out dateTime);
				this.LoggedTime = dateTime;
				return;
			}
			this.LoggedTime = this.MatchedTime;
			this.MatchedText = line;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005AAE File Offset: 0x00003CAE
		public TriggerMatchedEventArgs()
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005AB8 File Offset: 0x00003CB8
		public TriggerMatchedEventArgs Clone(MatchTypes matchType)
		{
			TriggerMatchedEventArgs triggerMatchedEventArgs = new TriggerMatchedEventArgs
			{
				TriggerFilter = this.TriggerFilter,
				MatchedTime = this.MatchedTime,
				LoggedTime = this.LoggedTime,
				MatchedText = this.MatchedText,
				MatchType = matchType,
				TargetDateTime = this.TargetDateTime,
				TimerEndingDone = this.TimerEndingDone,
				VisibleDateTime = this.VisibleDateTime,
				CounterInstance = this.CounterInstance,
				TimerSpanMilliseconds = this.TimerSpanMilliseconds
			};
			if (matchType == MatchTypes.Timer)
			{
				if (this.TriggerFilter.Trigger.TimerType.CountsDown())
				{
					triggerMatchedEventArgs.TargetDateTime = new DateTime?(triggerMatchedEventArgs.MatchedTime.AddMilliseconds((double)(this.TimerSpanMilliseconds ?? this.TriggerFilter.Trigger.TimerMillisecondDuration)));
					triggerMatchedEventArgs.VisibleDateTime = new DateTime?((this.TriggerFilter.Trigger.TimerVisibleDuration == 0) ? this.MatchedTime : this.TargetDateTime.Value.AddSeconds((double)(-1 * this.TriggerFilter.Trigger.TimerVisibleDuration)));
					if (this.TriggerFilter.Trigger.UseTimerEnding)
					{
						triggerMatchedEventArgs.EndingDateTime = new DateTime?(triggerMatchedEventArgs.TargetDateTime.Value.AddSeconds((double)(-1 * this.TriggerFilter.Trigger.TimerEndingTime)));
					}
				}
				if (this.TriggerFilter.Trigger.TimerEarlyEnders.Any<Trigger.TimerEarlyEnder>())
				{
					this.TriggerFilter.Character.AddTimerEarlyEndFilter(new TimerEarlyEndFilter(triggerMatchedEventArgs));
				}
			}
			else if (matchType == MatchTypes.TimerEnding)
			{
				triggerMatchedEventArgs.MatchedTime = DateTime.Now;
			}
			else if (matchType == MatchTypes.TimerEnded)
			{
				triggerMatchedEventArgs.MatchedTime = DateTime.Now;
			}
			return triggerMatchedEventArgs;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005C8C File Offset: 0x00003E8C
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00005C94 File Offset: 0x00003E94
		public TriggerFilter TriggerFilter { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005C9D File Offset: 0x00003E9D
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00005CA5 File Offset: 0x00003EA5
		public DateTime MatchedTime { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00005CAE File Offset: 0x00003EAE
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00005CB6 File Offset: 0x00003EB6
		public DateTime LoggedTime { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005CBF File Offset: 0x00003EBF
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00005CC7 File Offset: 0x00003EC7
		public string MatchedText { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005CD0 File Offset: 0x00003ED0
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public MatchTypes MatchType { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005CE1 File Offset: 0x00003EE1
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00005CE9 File Offset: 0x00003EE9
		public DateTime? TargetDateTime { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00005CF2 File Offset: 0x00003EF2
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00005CFA File Offset: 0x00003EFA
		public DateTime? VisibleDateTime { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005D03 File Offset: 0x00003F03
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00005D0B File Offset: 0x00003F0B
		public DateTime? EndingDateTime { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005D14 File Offset: 0x00003F14
		// (set) Token: 0x06000133 RID: 307 RVA: 0x00005D1C File Offset: 0x00003F1C
		public int CounterInstance { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005D25 File Offset: 0x00003F25
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00005D2D File Offset: 0x00003F2D
		public long? TimerSpanMilliseconds { get; set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00005D38 File Offset: 0x00003F38
		public string ExportText
		{
			get
			{
				return string.Format("{0}\t{1}\t{2}\t{3:yyyy/MM/dd hh:mm:ss tt}\t{4:yyyy/MM/dd hh:mm:ss tt}", new object[]
				{
					this.TriggerFilter.Character.DisplayName,
					this.TriggerFilter.Trigger.TriggerPath,
					this.MatchedText,
					this.LoggedTime,
					this.MatchedTime
				});
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public string DisplayText
		{
			get
			{
				if (this._DisplayText == null)
				{
					string text = "";
					switch (this.MatchType)
					{
					case MatchTypes.Standard:
						text = this.TriggerFilter.Trigger.DisplayText;
						break;
					case MatchTypes.Timer:
						text = this.TriggerFilter.Trigger.TimerName;
						break;
					case MatchTypes.TimerEnding:
						text = ((this.TriggerFilter.Trigger.TimerEndingTrigger != null) ? this.TriggerFilter.Trigger.TimerEndingTrigger.DisplayText : text);
						break;
					case MatchTypes.TimerEnded:
						text = ((this.TriggerFilter.Trigger.TimerEndedTrigger != null) ? this.TriggerFilter.Trigger.TimerEndedTrigger.DisplayText : text);
						break;
					}
					this._DisplayText = this.ResolveText(this.MatchedText, text ?? "", false);
				}
				return this._DisplayText;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00005E84 File Offset: 0x00004084
		public string VoiceText
		{
			get
			{
				if (this._VoiceText == null)
				{
					string text = "";
					switch (this.MatchType)
					{
					case MatchTypes.Standard:
						text = this.TriggerFilter.Trigger.TextToVoiceText;
						break;
					case MatchTypes.Timer:
						text = this.TriggerFilter.Trigger.TimerName;
						break;
					case MatchTypes.TimerEnding:
						text = ((this.TriggerFilter.Trigger.TimerEndingTrigger != null) ? this.TriggerFilter.Trigger.TimerEndingTrigger.TextToVoiceText : text);
						break;
					case MatchTypes.TimerEnded:
						text = ((this.TriggerFilter.Trigger.TimerEndedTrigger != null) ? this.TriggerFilter.Trigger.TimerEndedTrigger.TextToVoiceText : text);
						break;
					}
					this._VoiceText = this.ResolveText(this.MatchedText, text ?? "", true);
				}
				return this._VoiceText;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00005F64 File Offset: 0x00004164
		public string ClipboardText
		{
			get
			{
				if (this._ClipboardText == null)
				{
					string text = "";
					switch (this.MatchType)
					{
					case MatchTypes.Standard:
						text = this.TriggerFilter.Trigger.ClipboardText;
						break;
					case MatchTypes.Timer:
						text = this.TriggerFilter.Trigger.TimerName;
						break;
					case MatchTypes.TimerEnding:
						text = ((this.TriggerFilter.Trigger.TimerEndingTrigger != null) ? this.TriggerFilter.Trigger.TimerEndingTrigger.ClipboardText : text);
						break;
					case MatchTypes.TimerEnded:
						text = ((this.TriggerFilter.Trigger.TimerEndedTrigger != null) ? this.TriggerFilter.Trigger.TimerEndedTrigger.ClipboardText : text);
						break;
					}
					this._ClipboardText = this.ResolveText(this.MatchedText, text ?? "", false);
				}
				return this._ClipboardText;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006044 File Offset: 0x00004244
		public bool UseTextToVoice
		{
			get
			{
				switch (this.MatchType)
				{
				case MatchTypes.Standard:
					return this.TriggerFilter.Trigger.UseTextToVoice;
				case MatchTypes.Timer:
					return false;
				case MatchTypes.TimerEnding:
					return this.TriggerFilter.Trigger.TimerEndingTrigger != null && this.TriggerFilter.Trigger.TimerEndingTrigger.UseTextToVoice;
				case MatchTypes.TimerEnded:
					return this.TriggerFilter.Trigger.TimerEndedTrigger != null && this.TriggerFilter.Trigger.TimerEndedTrigger.UseTextToVoice;
				default:
					return false;
				}
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000060DC File Offset: 0x000042DC
		public bool InterruptSpeech
		{
			get
			{
				switch (this.MatchType)
				{
				case MatchTypes.Standard:
					return this.TriggerFilter.Trigger.InterruptSpeech;
				case MatchTypes.Timer:
					return false;
				case MatchTypes.TimerEnding:
					return this.TriggerFilter.Trigger.TimerEndingTrigger != null && this.TriggerFilter.Trigger.TimerEndingTrigger.InterruptSpeech;
				case MatchTypes.TimerEnded:
					return this.TriggerFilter.Trigger.TimerEndedTrigger != null && this.TriggerFilter.Trigger.TimerEndedTrigger.InterruptSpeech;
				default:
					return false;
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006174 File Offset: 0x00004374
		public bool PlayMediaFile
		{
			get
			{
				switch (this.MatchType)
				{
				case MatchTypes.Standard:
					return this.TriggerFilter.Trigger.PlayMediaFile;
				case MatchTypes.Timer:
					return false;
				case MatchTypes.TimerEnding:
					return this.TriggerFilter.Trigger.TimerEndingTrigger != null && this.TriggerFilter.Trigger.TimerEndingTrigger.PlayMediaFile;
				case MatchTypes.TimerEnded:
					return this.TriggerFilter.Trigger.TimerEndedTrigger != null && this.TriggerFilter.Trigger.TimerEndedTrigger.PlayMediaFile;
				default:
					return false;
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000620C File Offset: 0x0000440C
		public string MediaFileName
		{
			get
			{
				switch (this.MatchType)
				{
				case MatchTypes.Standard:
					return this.TriggerFilter.Trigger.MediaFileName;
				case MatchTypes.Timer:
					return null;
				case MatchTypes.TimerEnding:
					if (this.TriggerFilter.Trigger.TimerEndingTrigger == null)
					{
						return null;
					}
					return this.TriggerFilter.Trigger.TimerEndingTrigger.MediaFileName;
				case MatchTypes.TimerEnded:
					if (this.TriggerFilter.Trigger.TimerEndedTrigger == null)
					{
						return null;
					}
					return this.TriggerFilter.Trigger.TimerEndedTrigger.MediaFileName;
				default:
					return null;
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000062A4 File Offset: 0x000044A4
		public int Delay
		{
			get
			{
				return (this.MatchedTime - this.LoggedTime).Seconds;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000062CA File Offset: 0x000044CA
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000062D7 File Offset: 0x000044D7
		public bool TimerEndingDone
		{
			get
			{
				return base.Get<bool>("TimerEndingDone");
			}
			set
			{
				base.Set("TimerEndingDone", value);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000062EA File Offset: 0x000044EA
		// (set) Token: 0x06000142 RID: 322 RVA: 0x000062F7 File Offset: 0x000044F7
		public bool TimerVisible
		{
			get
			{
				return base.Get<bool>("TimerVisible");
			}
			set
			{
				base.Set("TimerVisible", value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000630C File Offset: 0x0000450C
		public int SecondsRemaining
		{
			get
			{
				if (this.MatchType != MatchTypes.Timer)
				{
					return 0;
				}
				if (this.TriggerFilter.Trigger.TimerType.CountsDown())
				{
					return (int)Math.Max(0.0, Math.Ceiling((this.TargetDateTime.Value - DateTime.Now).TotalMilliseconds / 1000.0));
				}
				return (int)(DateTime.Now - this.MatchedTime).TotalSeconds;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00006394 File Offset: 0x00004594
		public int MillisecondsRemaining
		{
			get
			{
				if (this.MatchType != MatchTypes.Timer || !this.TriggerFilter.Trigger.TimerType.CountsDown())
				{
					return 0;
				}
				return (int)Math.Max(0.0, (this.TargetDateTime.Value - DateTime.Now).TotalMilliseconds);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000063F4 File Offset: 0x000045F4
		public int TotalMilliseconds
		{
			get
			{
				if (this.MatchType != MatchTypes.Timer || !this.TriggerFilter.Trigger.TimerType.CountsDown())
				{
					return 1;
				}
				return (int)Math.Max(1.0, (this.TargetDateTime.Value - this.MatchedTime).TotalMilliseconds);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006454 File Offset: 0x00004654
		public void DoTick()
		{
			if (!this.TimerVisible && DateTime.Now >= this.VisibleDateTime)
			{
				this.TimerVisible = true;
			}
			if (this.EndingDateTime != null && !this.TimerEndingDone && this.TriggerFilter.Trigger.UseTimerEnding && DateTime.Now >= this.EndingDateTime)
			{
				this.TimerEndingDone = true;
				this.TriggerFilter.Character.RethrowMatch(this.Clone(MatchTypes.TimerEnding));
			}
			base.RaisePropertyChanged("SecondsRemaining");
			base.RaisePropertyChanged("MillisecondsRemaining");
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006520 File Offset: 0x00004720
		public void EndTimer()
		{
			if (this.TriggerFilter.Trigger.UseTimerEnded)
			{
				this.TriggerFilter.Character.RethrowMatch(this.Clone(MatchTypes.TimerEnded));
			}
			this.TriggerFilter.Character.RemoveTimerEarlyEndFilter(this);
			if (this.TriggerFilter.Trigger.Category != null && this.TriggerFilter.TimerOverlay != null)
			{
				this.TriggerFilter.TimerOverlay.RemoveMatch(this, true);
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006598 File Offset: 0x00004798
		public void AbortTimer()
		{
			this.TriggerFilter.Character.RemoveTimerEarlyEndFilter(this);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000065AC File Offset: 0x000047AC
		public string ResolveText(string srcLine, string destLine, bool isVoice = false)
		{
			string text = this.TriggerFilter.ResolveText(srcLine, destLine, isVoice);
			return Regex.Replace(text ?? "", "\\{COUNTER\\}", this.CounterInstance.ToString(), RegexOptions.IgnoreCase);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000065EB File Offset: 0x000047EB
		public override void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				this._Disposed = true;
				if (disposing)
				{
					this.AbortTimer();
				}
			}
		}

		// Token: 0x0400007D RID: 125
		private string _DisplayText;

		// Token: 0x0400007E RID: 126
		private string _VoiceText;

		// Token: 0x0400007F RID: 127
		private string _ClipboardText;
	}
}
