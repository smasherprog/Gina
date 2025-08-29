using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200001F RID: 31
	public class GamTextTriggerTranslation
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000067A4 File Offset: 0x000049A4
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000067AC File Offset: 0x000049AC
		public bool IsCompatible { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000067B5 File Offset: 0x000049B5
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000067BD File Offset: 0x000049BD
		public GamTextTriggerTranslation.IncompatabilityReason Reason { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000067C6 File Offset: 0x000049C6
		// (set) Token: 0x06000158 RID: 344 RVA: 0x000067CE File Offset: 0x000049CE
		private Trigger Trigger { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000067D7 File Offset: 0x000049D7
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000067DF File Offset: 0x000049DF
		private bool UseSTagForCTag { get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000067E8 File Offset: 0x000049E8
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000067F0 File Offset: 0x000049F0
		private bool UseYouForCTag { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000067F9 File Offset: 0x000049F9
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00006801 File Offset: 0x00004A01
		public bool TimerEndingCompatible { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000680A File Offset: 0x00004A0A
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00006812 File Offset: 0x00004A12
		public bool TimerEndedCompatible { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000681C File Offset: 0x00004A1C
		public string Translation
		{
			get
			{
				if (!this.IsCompatible)
				{
					return null;
				}
				string text = "";
				switch (this.Trigger.TimerStartBehavior)
				{
				case TimerStartBehaviors.StartNewTimer:
					text = "Always start a new timer";
					break;
				case TimerStartBehaviors.RestartTimer:
					text = "Restart existing behavior";
					break;
				case TimerStartBehaviors.IgnoreIfRunning:
					text = "Do nothing if timer already running";
					break;
				}
				string text2 = this.TranslateText(this.Trigger.DisplayText);
				return Configuration.GamTextTriggerExportFormat.Replace("{TriggerText}", this.TranslateText(this.Trigger.TriggerText)).Replace("{Comments}", string.IsNullOrWhiteSpace(this.Trigger.Comments) ? "BLANK" : this.Trigger.Comments).Replace("{UseText}", this.Trigger.UseText.ToString())
					.Replace("{ShowText}", (this.Trigger.UseText && !string.IsNullOrWhiteSpace(text2)).ToString())
					.Replace("{DisplayText}", string.IsNullOrWhiteSpace(text2) ? "BLANK" : text2)
					.Replace("{ShowLine}", (this.Trigger.DisplayText != null && this.Trigger.DisplayText.ToUpper().Contains("{L}")) ? "True" : "False")
					.Replace("{OverlayColor}", "Yellow")
					.Replace("{UseTimer}", (this.Trigger.TimerType == TimerTypes.Timer) ? "True" : "False")
					.Replace("{TimerText}", (this.Trigger.TimerType == TimerTypes.Timer && !string.IsNullOrWhiteSpace(this.Trigger.TimerName)) ? this.TranslateText(this.Trigger.TimerName) : "BLANK")
					.Replace("{TimerHours}", TimeSpan.FromMilliseconds((double)this.Trigger.TimerMillisecondDuration).Hours.ToString())
					.Replace("{TimerMinutes}", TimeSpan.FromMilliseconds((double)this.Trigger.TimerMillisecondDuration).Minutes.ToString())
					.Replace("{TimerSeconds}", TimeSpan.FromMilliseconds((double)this.Trigger.TimerMillisecondDuration).Seconds.ToString())
					.Replace("{TimerBehavior}", text)
					.Replace("{UseTimerCompleteText}", (this.Trigger.UseTimerEnded && this.Trigger.TimerEndedTrigger != null && this.Trigger.TimerEndedTrigger.UseText) ? "True" : "False")
					.Replace("{TimerCompleteText}", (this.Trigger.UseTimerEnded && this.Trigger.TimerEndedTrigger != null && this.Trigger.TimerEndedTrigger.UseText) ? (this.TranslateText(this.Trigger.TimerEndedTrigger.DisplayText) ?? "BLANK") : "BLANK")
					.Replace("{UseTimerEndEarlyText}", (this.Trigger.TimerEarlyEnders.Any<Trigger.TimerEarlyEnder>() && !string.IsNullOrWhiteSpace(this.Trigger.TimerEarlyEnders[0].EarlyEndText)) ? "True" : "False")
					.Replace("{TimerEndEarlyText}", (this.Trigger.TimerEarlyEnders.Any<Trigger.TimerEarlyEnder>() && !string.IsNullOrWhiteSpace(this.Trigger.TimerEarlyEnders[0].EarlyEndText)) ? this.TranslateText(this.Trigger.TimerEarlyEnders[0].EarlyEndText) : "BLANK")
					.Replace("{UseSound}", (this.Trigger.PlayMediaFile || this.Trigger.UseTextToVoice) ? "True" : "False")
					.Replace("{PlayMediaFile}", this.Trigger.PlayMediaFile.ToString())
					.Replace("{MediaFileName}", string.IsNullOrWhiteSpace(this.Trigger.MediaFileName) ? "BLANK" : this.Trigger.MediaFileName)
					.Replace("{UseTextToVoice}", (this.Trigger.UseTextToVoice && !string.IsNullOrWhiteSpace(this.TranslateText(this.Trigger.TextToVoiceText))).ToString())
					.Replace("{TextToVoiceText}", string.IsNullOrWhiteSpace(this.TranslateText(this.Trigger.TextToVoiceText)) ? "BLANK" : this.TranslateText(this.Trigger.TextToVoiceText));
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006CAC File Offset: 0x00004EAC
		private string TranslateText(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return input;
			}
			if (this.UseSTagForCTag)
			{
				input = Regex.Replace(input, "{C}", "{S}", RegexOptions.IgnoreCase);
			}
			if (this.UseYouForCTag)
			{
				input = Regex.Replace(input, "{C}", Configuration.Current.ReferenceToSelf, RegexOptions.IgnoreCase);
			}
			input = Regex.Replace(input, "{L}", "", RegexOptions.IgnoreCase);
			return input;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006D20 File Offset: 0x00004F20
		public GamTextTriggerTranslation(Trigger trigger)
		{
			this.Trigger = trigger;
			this.IsCompatible = true;
			bool flag = this.Trigger.TriggerText.ToUpper().Contains("{C}");
			bool flag2 = this.Trigger.TriggerText.ToUpper().Contains("{S}");
			if (flag && flag2)
			{
				this.IsCompatible = false;
				this.Reason = GamTextTriggerTranslation.IncompatabilityReason.UsesCandSTags;
			}
			if (Regex.Match(this.Trigger.TriggerText, "\\{S\\d+\\}", RegexOptions.IgnoreCase).Success)
			{
				this.IsCompatible = false;
				this.Reason = GamTextTriggerTranslation.IncompatabilityReason.UsesNumberedSTags;
				return;
			}
			if (this.Trigger.EnableRegex)
			{
				Regex regex = new Regex(this.Trigger.TriggerText);
				string[] groupNames = regex.GetGroupNames();
				if (groupNames.Any((string o) => o != "0"))
				{
					this.IsCompatible = false;
					this.Reason = GamTextTriggerTranslation.IncompatabilityReason.UsesRegexGroups;
					return;
				}
			}
			if (Regex.Match(trigger.TriggerText, "\\{N\\d*[\\>\\=\\<]*\\d*\\}", RegexOptions.IgnoreCase).Success)
			{
				this.IsCompatible = false;
				this.Reason = GamTextTriggerTranslation.IncompatabilityReason.UsesNTags;
			}
			if (this.Trigger.TimerType != TimerTypes.Timer && this.Trigger.TimerType != TimerTypes.NoTimer)
			{
				this.IsCompatible = false;
				this.Reason = GamTextTriggerTranslation.IncompatabilityReason.UnsupportedTimerType;
			}
			if (flag)
			{
				this.UseSTagForCTag = true;
				return;
			}
			this.UseYouForCTag = true;
		}

		// Token: 0x02000020 RID: 32
		public enum IncompatabilityReason
		{
			// Token: 0x04000096 RID: 150
			None,
			// Token: 0x04000097 RID: 151
			UsesCandSTags,
			// Token: 0x04000098 RID: 152
			UsesNumberedSTags,
			// Token: 0x04000099 RID: 153
			UsesRegexGroups,
			// Token: 0x0400009A RID: 154
			UsesNTags,
			// Token: 0x0400009B RID: 155
			UnsupportedTimerType
		}
	}
}
