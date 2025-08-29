using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200003C RID: 60
	public class Trigger : GINABusinessObject, ITriggerLibraryEntry
	{
		// Token: 0x060002DC RID: 732 RVA: 0x0000DC15 File Offset: 0x0000BE15
		static Trigger()
		{
			BindableObject.SetDependentProperties(typeof(Trigger));
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000DC26 File Offset: 0x0000BE26
		public static void RegisterDependentProperties(Type type)
		{
			BindableObject.RegisterDependentProperty(type, "TimerHasDynamicDuration", "TriggerText", null);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000DC39 File Offset: 0x0000BE39
		public Trigger()
		{
			this.NeedsMerge = true;
			this.Category = TriggerCategory.DefaultCategory;
			this.TimerEarlyEnders = new List<Trigger.TimerEarlyEnder>();
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000DC5E File Offset: 0x0000BE5E
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x0000DC6B File Offset: 0x0000BE6B
		public string Name
		{
			get
			{
				return base.Get<string>("Name");
			}
			set
			{
				base.Set("Name", value);
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x0000DC79 File Offset: 0x0000BE79
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x0000DC86 File Offset: 0x0000BE86
		public string TriggerText
		{
			get
			{
				return base.Get<string>("TriggerText");
			}
			set
			{
				base.Set("TriggerText", value);
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000DC94 File Offset: 0x0000BE94
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000DCA1 File Offset: 0x0000BEA1
		public bool EnableRegex
		{
			get
			{
				return base.Get<bool>("EnableRegex");
			}
			set
			{
				base.Set("EnableRegex", value);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000DCC1 File Offset: 0x0000BEC1
		public bool UseFastCheck
		{
			get
			{
				return base.Get<bool>("UseFastCheck");
			}
			set
			{
				base.Set("UseFastCheck", value);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000DCE1 File Offset: 0x0000BEE1
		public string Comments
		{
			get
			{
				return base.Get<string>("Comments");
			}
			set
			{
				base.Set("Comments", value);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000DCEF File Offset: 0x0000BEEF
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000DCFC File Offset: 0x0000BEFC
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

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000DD0F File Offset: 0x0000BF0F
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000DD1C File Offset: 0x0000BF1C
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000DD2A File Offset: 0x0000BF2A
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000DD37 File Offset: 0x0000BF37
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000DD4A File Offset: 0x0000BF4A
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x0000DD57 File Offset: 0x0000BF57
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

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000DD65 File Offset: 0x0000BF65
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0000DD72 File Offset: 0x0000BF72
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

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000DD85 File Offset: 0x0000BF85
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000DD92 File Offset: 0x0000BF92
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000DDA0 File Offset: 0x0000BFA0
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x0000DDAD File Offset: 0x0000BFAD
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x0000DDCD File Offset: 0x0000BFCD
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0000DDED File Offset: 0x0000BFED
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000DDFB File Offset: 0x0000BFFB
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0000DE03 File Offset: 0x0000C003
		public TimerTypes TimerType
		{
			get
			{
				return this._TimerType;
			}
			set
			{
				this._TimerType = value;
				base.RaisePropertyChanged("TimerType");
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000DE17 File Offset: 0x0000C017
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000DE24 File Offset: 0x0000C024
		public string TimerName
		{
			get
			{
				return base.Get<string>("TimerName");
			}
			set
			{
				base.Set("TimerName", value);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000DE32 File Offset: 0x0000C032
		// (set) Token: 0x06000300 RID: 768 RVA: 0x0000DE3F File Offset: 0x0000C03F
		public bool RestartBasedOnTimerName
		{
			get
			{
				return base.Get<bool>("RestartBasedOnTimerName");
			}
			set
			{
				base.Set("RestartBasedOnTimerName", value);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000DE52 File Offset: 0x0000C052
		// (set) Token: 0x06000302 RID: 770 RVA: 0x0000DE5F File Offset: 0x0000C05F
		public long TimerMillisecondDuration
		{
			get
			{
				return base.Get<long>("TimerMillisecondDuration");
			}
			set
			{
				base.Set("TimerMillisecondDuration", value);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000DE72 File Offset: 0x0000C072
		// (set) Token: 0x06000304 RID: 772 RVA: 0x0000DE7F File Offset: 0x0000C07F
		public int TimerVisibleDuration
		{
			get
			{
				return base.Get<int>("TimerVisibleDuration");
			}
			set
			{
				base.Set("TimerVisibleDuration", value);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000DE92 File Offset: 0x0000C092
		public bool TimerHasDynamicDuration
		{
			get
			{
				return this.TriggerText.ToLower().Contains("{ts}");
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000DEA9 File Offset: 0x0000C0A9
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000DEB6 File Offset: 0x0000C0B6
		public List<Trigger.TimerEarlyEnder> TimerEarlyEnders
		{
			get
			{
				return base.Get<List<Trigger.TimerEarlyEnder>>("TimerEarlyEnders");
			}
			set
			{
				base.Set("TimerEarlyEnders", value);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000DEC4 File Offset: 0x0000C0C4
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000DED1 File Offset: 0x0000C0D1
		public TimerStartBehaviors TimerStartBehavior
		{
			get
			{
				return base.Get<TimerStartBehaviors>("TimerStartBehavior");
			}
			set
			{
				base.Set("TimerStartBehavior", value);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0000DEF1 File Offset: 0x0000C0F1
		public bool UseTimerEnding
		{
			get
			{
				return base.Get<bool>("UseTimerEnding");
			}
			set
			{
				base.Set("UseTimerEnding", value);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000DF04 File Offset: 0x0000C104
		// (set) Token: 0x0600030D RID: 781 RVA: 0x0000DF11 File Offset: 0x0000C111
		public bool UseTimerEnded
		{
			get
			{
				return base.Get<bool>("UseTimerEnded");
			}
			set
			{
				base.Set("UseTimerEnded", value);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000DF24 File Offset: 0x0000C124
		// (set) Token: 0x0600030F RID: 783 RVA: 0x0000DF31 File Offset: 0x0000C131
		public int TimerEndingTime
		{
			get
			{
				return base.Get<int>("TimerEndingTime");
			}
			set
			{
				base.Set("TimerEndingTime", value);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000DF44 File Offset: 0x0000C144
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0000DF54 File Offset: 0x0000C154
		public Trigger TimerEndingTrigger
		{
			get
			{
				return base.Get<Trigger>("TimerEndingTrigger");
			}
			set
			{
				Trigger trigger = base.Get<Trigger>("TimerEndingTrigger");
				if (trigger != null && trigger != value)
				{
					trigger.Dispose();
				}
				base.Set("TimerEndingTrigger", value);
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000DF86 File Offset: 0x0000C186
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0000DF94 File Offset: 0x0000C194
		public Trigger TimerEndedTrigger
		{
			get
			{
				return base.Get<Trigger>("TimerEndedTrigger");
			}
			set
			{
				Trigger trigger = base.Get<Trigger>("TimerEndedTrigger");
				if (trigger != null && trigger != value)
				{
					trigger.Dispose();
				}
				base.Set("TimerEndedTrigger", value);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000DFC6 File Offset: 0x0000C1C6
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000DFD3 File Offset: 0x0000C1D3
		public bool UseCounterResetTimer
		{
			get
			{
				return base.Get<bool>("UseCounterResetTimer");
			}
			set
			{
				base.Set("UseCounterResetTimer", value);
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000DFE6 File Offset: 0x0000C1E6
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000DFF3 File Offset: 0x0000C1F3
		public long CounterResetDuration
		{
			get
			{
				return base.Get<long>("CounterResetDuration");
			}
			set
			{
				base.Set("CounterResetDuration", value);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000E006 File Offset: 0x0000C206
		// (set) Token: 0x06000319 RID: 793 RVA: 0x0000E013 File Offset: 0x0000C213
		public int? MediaFileId
		{
			get
			{
				return base.Get<int?>("MediaFileId");
			}
			set
			{
				base.Set("MediaFileId", value);
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000E026 File Offset: 0x0000C226
		// (set) Token: 0x0600031B RID: 795 RVA: 0x0000E033 File Offset: 0x0000C233
		public TriggerGroup ParentGroup
		{
			get
			{
				return base.Get<TriggerGroup>("ParentGroup");
			}
			set
			{
				base.Set("ParentGroup", value);
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000E041 File Offset: 0x0000C241
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000E04E File Offset: 0x0000C24E
		public bool NeedsMerge
		{
			get
			{
				return base.Get<bool>("NeedsMerge");
			}
			set
			{
				base.Set("NeedsMerge", value);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000E061 File Offset: 0x0000C261
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0000E06E File Offset: 0x0000C26E
		public TriggerCategory Category
		{
			get
			{
				return base.Get<TriggerCategory>("Category");
			}
			set
			{
				base.Set("Category", value);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000E07C File Offset: 0x0000C27C
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000E089 File Offset: 0x0000C289
		public string SuggestedCategory
		{
			get
			{
				return base.Get<string>("SuggestedCategory");
			}
			set
			{
				base.Set("SuggestedCategory", value);
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000E097 File Offset: 0x0000C297
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public DateTime Modified
		{
			get
			{
				return base.Get<DateTime>("Modified");
			}
			set
			{
				base.Set("Modified", value);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
		public IEnumerable<TriggerGroup> GroupPath
		{
			get
			{
				List<TriggerGroup> path = new List<TriggerGroup>();
				TriggerGroup grp = this.ParentGroup;
				while (grp != null && grp.ParentGroup != null)
				{
					path.Add(grp);
					grp = grp.ParentGroup;
				}
				path.Reverse();
				foreach (TriggerGroup g in path)
				{
					yield return g;
				}
				yield break;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000E2C8 File Offset: 0x0000C4C8
		public string TriggerPath
		{
			get
			{
				string text = this.Name;
				TriggerGroup triggerGroup = this.ParentGroup;
				while (triggerGroup != null && triggerGroup.ParentGroup != null)
				{
					text = string.Format("{0} -> {1}", triggerGroup.Name, text);
					triggerGroup = triggerGroup.ParentGroup;
				}
				return text;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000E30A File Offset: 0x0000C50A
		public long TotalMatchComparisons
		{
			get
			{
				return Interlocked.Read(ref this._SuccessComparisons) + Interlocked.Read(ref this._FailureComparisons);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000E323 File Offset: 0x0000C523
		public long TotalMatchCost
		{
			get
			{
				return Interlocked.Read(ref this._SuccessCost) + Interlocked.Read(ref this._FailureCost);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000E33C File Offset: 0x0000C53C
		public double AverageMatchCost
		{
			get
			{
				long num = Interlocked.Read(ref this._SuccessComparisons) + Interlocked.Read(ref this._FailureComparisons) + Interlocked.Read(ref this._EarlyEndComparisons);
				long num2 = Interlocked.Read(ref this._SuccessCost) + Interlocked.Read(ref this._FailureCost) + Interlocked.Read(ref this._EarlyEndCost);
				if (num == 0L)
				{
					return 0.0;
				}
				return Math.Round((double)((float)num2 * 1f / (float)num), 2);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000E3B2 File Offset: 0x0000C5B2
		public long SuccessMatchComparisons
		{
			get
			{
				return Interlocked.Read(ref this._SuccessComparisons);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000E3BF File Offset: 0x0000C5BF
		public long SuccessMatchCost
		{
			get
			{
				return Interlocked.Read(ref this._SuccessCost);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000E3CC File Offset: 0x0000C5CC
		public double AverageSuccessMatchCost
		{
			get
			{
				long num = Interlocked.Read(ref this._SuccessComparisons);
				long num2 = Interlocked.Read(ref this._SuccessCost);
				if (num == 0L)
				{
					return 0.0;
				}
				return Math.Round((double)((float)num2 * 1f / (float)num), 2);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000E412 File Offset: 0x0000C612
		public long FailureMatchComparisons
		{
			get
			{
				return Interlocked.Read(ref this._FailureComparisons);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000E41F File Offset: 0x0000C61F
		public long FailureMatchCost
		{
			get
			{
				return Interlocked.Read(ref this._FailureCost);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000E42C File Offset: 0x0000C62C
		public double AverageFailureMatchCost
		{
			get
			{
				long num = Interlocked.Read(ref this._FailureComparisons);
				long num2 = Interlocked.Read(ref this._FailureCost);
				if (num == 0L)
				{
					return 0.0;
				}
				return Math.Round((double)((float)num2 * 1f / (float)num), 2);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000E472 File Offset: 0x0000C672
		public long EarlyEndComparisons
		{
			get
			{
				return Interlocked.Read(ref this._EarlyEndComparisons);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000E47F File Offset: 0x0000C67F
		public long EarlyEndCost
		{
			get
			{
				return Interlocked.Read(ref this._EarlyEndCost);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000E48C File Offset: 0x0000C68C
		public double AverageEarlyEndCost
		{
			get
			{
				long num = Interlocked.Read(ref this._EarlyEndComparisons);
				long num2 = Interlocked.Read(ref this._EarlyEndCost);
				if (num == 0L)
				{
					return 0.0;
				}
				return Math.Round((double)((float)num2 * 1f / (float)num), 2);
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000E4D2 File Offset: 0x0000C6D2
		public void Move(TriggerGroup group, int? index = null)
		{
			if (group != null)
			{
				group.AddTrigger(this, index);
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
		public void Remove()
		{
			if (this.ParentGroup != null)
			{
				this.ParentGroup.Triggers.Remove(this);
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000E4FC File Offset: 0x0000C6FC
		public Trigger Clone()
		{
			Trigger trigger = new Trigger();
			trigger.CopyFrom(this);
			return trigger;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000E520 File Offset: 0x0000C720
		public void CopyFrom(Trigger trigger)
		{
			this.Name = trigger.Name;
			this.TriggerText = trigger.TriggerText;
			this.EnableRegex = trigger.EnableRegex;
			this.Comments = trigger.Comments;
			this.UseText = trigger.UseText;
			this.DisplayText = trigger.DisplayText;
			this.CopyToClipboard = trigger.CopyToClipboard;
			this.ClipboardText = trigger.ClipboardText;
			this.UseTextToVoice = trigger.UseTextToVoice;
			this.TextToVoiceText = trigger.TextToVoiceText;
			this.InterruptSpeech = trigger.InterruptSpeech;
			this.PlayMediaFile = trigger.PlayMediaFile;
			this.MediaFileName = trigger.MediaFileName;
			this.NeedsMerge = trigger.NeedsMerge && (this.ParentGroup == null || this.ParentGroup.IsUnattached);
			this.MediaFileId = ((this.ParentGroup != null && !this.ParentGroup.IsUnattached) ? null : trigger.MediaFileId);
			this.Category = trigger.Category;
			this.SuggestedCategory = trigger.SuggestedCategory;
			this.UseFastCheck = trigger.UseFastCheck;
			this.TimerType = trigger.TimerType;
			this.UseTimerEnded = trigger.UseTimerEnded;
			this.UseTimerEnding = trigger.UseTimerEnding;
			this.TimerMillisecondDuration = trigger.TimerMillisecondDuration;
			this.TimerEarlyEnders = trigger.TimerEarlyEnders.Select((Trigger.TimerEarlyEnder o) => o.Clone()).ToList<Trigger.TimerEarlyEnder>();
			this.TimerEndedTrigger = ((trigger.TimerEndedTrigger != null) ? trigger.TimerEndedTrigger.Clone() : null);
			this.TimerEndingTime = trigger.TimerEndingTime;
			this.TimerEndingTrigger = ((trigger.TimerEndingTrigger != null) ? trigger.TimerEndingTrigger.Clone() : null);
			this.TimerName = trigger.TimerName;
			this.TimerStartBehavior = trigger.TimerStartBehavior;
			this.TimerVisibleDuration = trigger.TimerVisibleDuration;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000E70C File Offset: 0x0000C90C
		public static Trigger CreateFromGamTextTriggerString(string triggerString)
		{
			if (string.IsNullOrEmpty(triggerString))
			{
				return null;
			}
			Regex regex = new Regex(Configuration.GamTextTriggerImportRegex, RegexOptions.IgnoreCase);
			Match match = regex.Match(triggerString);
			if (!match.Success)
			{
				return null;
			}
			Trigger trigger = new Trigger
			{
				Name = Regex.Replace(match.Groups["TriggerText"].Value ?? "", "^BLANK$", ""),
				TriggerText = match.Groups["TriggerText"].Value,
				Comments = Regex.Replace(match.Groups["Comments"].Value ?? "", "^BLANK$", ""),
				UseText = (bool.Parse(match.Groups["UseText"].Value) || bool.Parse(match.Groups["ShowLine"].Value)),
				DisplayText = (bool.Parse(match.Groups["ShowLine"].Value) ? "{L}" : Regex.Replace(match.Groups["DisplayText"].Value ?? "", "^BLANK$", "")),
				UseTextToVoice = bool.Parse(match.Groups["UseTextToVoice"].Value),
				TextToVoiceText = Regex.Replace(match.Groups["TextToVoiceText"].Value ?? "", "^BLANK$", ""),
				PlayMediaFile = bool.Parse(match.Groups["PlayMediaFile"].Value),
				MediaFileName = Regex.Replace(match.Groups["MediaFileName"].Value, "^BLANK$", "")
			};
			trigger.Name = (string.IsNullOrWhiteSpace(trigger.Name) ? trigger.TriggerText : trigger.Name);
			trigger.EnableRegex = Regex.Match(trigger.TriggerText, "\\{S\\}", RegexOptions.IgnoreCase).Success;
			int num = 0;
			bool flag;
			trigger.TimerType = ((bool.TryParse(match.Groups["UseTimer"].Value, out flag) && flag) ? TimerTypes.Timer : TimerTypes.NoTimer);
			trigger.TimerName = Regex.Replace(match.Groups["TimerText"].Value ?? "", "^BLANK$", "");
			trigger.TimerMillisecondDuration += (long)(int.TryParse(match.Groups["TimerHours"].Value, out num) ? (num * 3600000) : 0);
			trigger.TimerMillisecondDuration += (long)(int.TryParse(match.Groups["TimerMinutes"].Value, out num) ? (num * 60000) : 0);
			trigger.TimerMillisecondDuration += (long)(int.TryParse(match.Groups["TimerSeconds"].Value, out num) ? (num * 1000) : 0);
			string text;
			if ((text = (match.Groups["TimerBehavior"].Value ?? "").ToLower()) != null)
			{
				if (text == "do nothing if timer already running")
				{
					trigger.TimerStartBehavior = TimerStartBehaviors.IgnoreIfRunning;
					goto IL_0390;
				}
				if (text == "restart existing timer")
				{
					trigger.TimerStartBehavior = TimerStartBehaviors.RestartTimer;
					goto IL_0390;
				}
			}
			trigger.TimerStartBehavior = TimerStartBehaviors.StartNewTimer;
			IL_0390:
			string text2 = Regex.Replace(match.Groups["TimerEndEarlyText"].Value, "^BLANK$", "");
			if (!string.IsNullOrWhiteSpace(text2))
			{
				trigger.TimerEarlyEnders.Add(new Trigger.TimerEarlyEnder(text2, false));
			}
			trigger.UseTimerEnded = bool.TryParse(match.Groups["UseTimerCompleteText"].Value, out flag) && flag;
			if (trigger.UseTimerEnded)
			{
				trigger.TimerEndedTrigger = new Trigger
				{
					UseText = true,
					DisplayText = Regex.Replace(match.Groups["TimerCompleteText"].Value, "^BLANK$", "")
				};
			}
			return trigger;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000EB60 File Offset: 0x0000CD60
		public void AddComparison(long microseconds, ProfilerComparisonType comparisonType)
		{
			switch (comparisonType)
			{
			case ProfilerComparisonType.Matched:
				Interlocked.Increment(ref this._SuccessComparisons);
				Interlocked.Add(ref this._SuccessCost, microseconds);
				return;
			case ProfilerComparisonType.Unmatched:
				Interlocked.Increment(ref this._FailureComparisons);
				Interlocked.Add(ref this._FailureCost, microseconds);
				return;
			case ProfilerComparisonType.EarlyEndFilter:
				Interlocked.Increment(ref this._EarlyEndComparisons);
				Interlocked.Add(ref this._EarlyEndCost, microseconds);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
		public void ResetComparisonCounters()
		{
			Interlocked.Exchange(ref this._SuccessCost, 0L);
			Interlocked.Exchange(ref this._SuccessComparisons, 0L);
			Interlocked.Exchange(ref this._FailureCost, 0L);
			Interlocked.Exchange(ref this._FailureComparisons, 0L);
			Interlocked.Exchange(ref this._EarlyEndCost, 0L);
			Interlocked.Exchange(ref this._EarlyEndComparisons, 0L);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000EC68 File Offset: 0x0000CE68
		private static Trigger LoadSubTriggerFromXml(XmlElement element, IEnumerable<Package.PrerecordedFile> files)
		{
			Trigger trig = new Trigger
			{
				UseText = element.GetElementValue("UseText", false),
				DisplayText = element.GetElementValue("DisplayText", null),
				UseTextToVoice = element.GetElementValue("UseTextToVoice", false),
				TextToVoiceText = element.GetElementValue("TextToVoiceText", null),
				InterruptSpeech = element.GetElementValue("InterruptSpeech", false),
				PlayMediaFile = element.GetElementValue("PlayMediaFile", false)
			};
			if (files != null)
			{
				int? num = new int?(element.GetElementValue("MediaFileId", 0));
				trig.MediaFileId = ((num != null && num != 0) ? num : null);
				if (trig.MediaFileId != null)
				{
					trig.MediaFileName = Path.Combine(Configuration.Current.ImportedMediaFileFolder, files.Single((Package.PrerecordedFile o) => o.FileId == trig.MediaFileId.Value).Filename);
				}
			}
			else
			{
				trig.MediaFileName = element.GetElementValue("MediaFileName", null);
			}
			return trig;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000EE14 File Offset: 0x0000D014
		public static Trigger LoadFromXml(XmlElement element, IEnumerable<Package.PrerecordedFile> files = null)
		{
			Trigger trig = new Trigger
			{
				Name = element.GetElementValue("Name", null),
				TriggerText = element.GetElementValue("TriggerText", null),
				Comments = element.GetElementValue("Comments", null),
				EnableRegex = element.GetElementValue("EnableRegex", false),
				UseText = element.GetElementValue("UseText", false),
				DisplayText = element.GetElementValue("DisplayText", null),
				CopyToClipboard = element.GetElementValue("CopyToClipboard", false),
				ClipboardText = element.GetElementValue("ClipboardText", null),
				UseTextToVoice = element.GetElementValue("UseTextToVoice", false),
				TextToVoiceText = element.GetElementValue("TextToVoiceText", null),
				InterruptSpeech = element.GetElementValue("InterruptSpeech", false),
				PlayMediaFile = element.GetElementValue("PlayMediaFile", false),
				TimerName = element.GetElementValue("TimerName", null),
				RestartBasedOnTimerName = element.GetElementValue("RestartBasedOnTimerName", true),
				TimerMillisecondDuration = (long)element.GetElementValue("TimerMillisecondDuration", 0),
				TimerVisibleDuration = element.GetElementValue("TimerVisibleDuration", 0),
				TimerStartBehavior = element.GetElementValue("TimerStartBehavior", TimerStartBehaviors.StartNewTimer),
				UseTimerEnding = element.GetElementValue("UseTimerEnding", false),
				UseTimerEnded = element.GetElementValue("UseTimerEnded", false),
				TimerEndingTime = element.GetElementValue("TimerEndingTime", 0),
				SuggestedCategory = element.GetElementValue("SuggestedCategory", null),
				UseCounterResetTimer = element.GetElementValue("UseCounterResetTimer", false),
				CounterResetDuration = element.GetElementValue("CounterResetDuration", 0L),
				Modified = element.GetElementValue("Modified", DateTime.Today),
				UseFastCheck = element.GetElementValue("UseFastCheck", true)
			};
			trig.TimerEarlyEnders = new List<Trigger.TimerEarlyEnder>();
			List<XmlElement> list = element.SelectNodes("TimerEarlyEnders/EarlyEnder").Cast<XmlElement>().ToList<XmlElement>();
			foreach (XmlElement xmlElement in list)
			{
				trig.TimerEarlyEnders.Add(new Trigger.TimerEarlyEnder(xmlElement.GetElementValue("EarlyEndText", null), xmlElement.GetElementValue("EnableRegex", false)));
			}
			if (!trig.TimerEarlyEnders.Any<Trigger.TimerEarlyEnder>())
			{
				string elementValue = element.GetElementValue("TimerEarlyEndText", null);
				if (!string.IsNullOrWhiteSpace(elementValue))
				{
					foreach (string text in elementValue.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
					{
						trig.TimerEarlyEnders.Add(new Trigger.TimerEarlyEnder(text, false));
					}
				}
			}
			XmlElement xmlElement2 = (from XmlNode o in element.SelectNodes("UseTimer")
				where o is XmlElement
				select o).Cast<XmlElement>().FirstOrDefault<XmlElement>();
			if (xmlElement2 != null)
			{
				trig.TimerType = (element.GetElementValue("UseTimer", false) ? TimerTypes.Timer : TimerTypes.NoTimer);
			}
			else
			{
				trig.TimerType = element.GetElementValue("TimerType", TimerTypes.NoTimer);
			}
			if (trig.TimerType.CountsDown() && trig.TimerMillisecondDuration == 0L)
			{
				trig.TimerMillisecondDuration = (long)(element.GetElementValue("TimerDuration", 0) * 1000);
			}
			if (files != null)
			{
				int? num = new int?(element.GetElementValue("MediaFileId", 0));
				trig.MediaFileId = ((num != null && num != 0) ? num : null);
				if (trig.MediaFileId != null)
				{
					trig.MediaFileName = Path.Combine(Configuration.Current.ImportedMediaFileFolder, files.Single((Package.PrerecordedFile o) => o.FileId == trig.MediaFileId.Value).Filename);
				}
			}
			else
			{
				trig.MediaFileName = element.GetElementValue("MediaFileName", null);
			}
			XmlNode xmlNode = element.SelectSingleNode("TimerEndingTrigger[1]");
			if (xmlNode != null)
			{
				trig.TimerEndingTrigger = Trigger.LoadSubTriggerFromXml(xmlNode as XmlElement, files);
			}
			XmlNode xmlNode2 = element.SelectSingleNode("TimerEndedTrigger[1]");
			if (xmlNode2 != null)
			{
				trig.TimerEndedTrigger = Trigger.LoadSubTriggerFromXml(xmlNode2 as XmlElement, files);
			}
			string desiredCategory = element.GetElementValue("Category", "");
			TriggerCategory triggerCategory = TriggerCategory.All.SingleOrDefault((TriggerCategory o) => o.Name.ToLower() == desiredCategory.ToLower());
			if (triggerCategory == null && !string.IsNullOrWhiteSpace(desiredCategory))
			{
				trig.SuggestedCategory = desiredCategory;
			}
			trig.Category = triggerCategory ?? TriggerCategory.DefaultCategory;
			return trig;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000F394 File Offset: 0x0000D594
		private void SaveSubTriggerToXml(XmlElement element, string subName, Trigger trigger, IEnumerable<Package.PrerecordedFile> files)
		{
			XmlDocument ownerDocument = element.OwnerDocument;
			XmlElement xmlElement = ownerDocument.CreateElement(subName);
			element.AppendChild(xmlElement);
			xmlElement.AppendChild(ownerDocument.NewElement("UseText", trigger.UseText));
			xmlElement.AppendChild(ownerDocument.NewElement("DisplayText", trigger.DisplayText ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("UseTextToVoice", trigger.UseTextToVoice));
			xmlElement.AppendChild(ownerDocument.NewElement("InterruptSpeech", trigger.InterruptSpeech));
			xmlElement.AppendChild(ownerDocument.NewElement("TextToVoiceText", trigger.TextToVoiceText ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("PlayMediaFile", trigger.PlayMediaFile));
			if (!string.IsNullOrWhiteSpace(trigger.MediaFileName))
			{
				if (files != null)
				{
					Package.PrerecordedFile prerecordedFile = files.SingleOrDefault((Package.PrerecordedFile o) => o.FullPath.ToLower() == trigger.MediaFileName.ToLower());
					if (prerecordedFile != null)
					{
						xmlElement.AppendChild(ownerDocument.NewElement("MediaFileId", prerecordedFile.FileId));
						return;
					}
				}
				else
				{
					xmlElement.AppendChild(ownerDocument.NewElement("MediaFileName", trigger.MediaFileName ?? ""));
				}
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000F53C File Offset: 0x0000D73C
		public void SaveToXml(XmlElement element, IEnumerable<Package.PrerecordedFile> files = null)
		{
			XmlDocument ownerDocument = element.OwnerDocument;
			XmlElement xmlElement = ownerDocument.CreateElement("Trigger");
			element.AppendChild(xmlElement);
			xmlElement.AppendChild(ownerDocument.NewElement("Name", this.Name ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("TriggerText", this.TriggerText ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("Comments", this.Comments ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("EnableRegex", this.EnableRegex));
			xmlElement.AppendChild(ownerDocument.NewElement("UseText", this.UseText));
			xmlElement.AppendChild(ownerDocument.NewElement("DisplayText", this.DisplayText ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("CopyToClipboard", this.CopyToClipboard));
			xmlElement.AppendChild(ownerDocument.NewElement("ClipboardText", this.ClipboardText ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("UseTextToVoice", this.UseTextToVoice));
			xmlElement.AppendChild(ownerDocument.NewElement("InterruptSpeech", this.InterruptSpeech));
			xmlElement.AppendChild(ownerDocument.NewElement("TextToVoiceText", this.TextToVoiceText ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("PlayMediaFile", this.PlayMediaFile));
			if (!string.IsNullOrWhiteSpace(this.MediaFileName))
			{
				if (files != null)
				{
					Package.PrerecordedFile prerecordedFile = files.SingleOrDefault((Package.PrerecordedFile o) => o.FullPath.ToLower() == this.MediaFileName.ToLower());
					if (prerecordedFile != null)
					{
						xmlElement.AppendChild(ownerDocument.NewElement("MediaFileId", prerecordedFile.FileId));
					}
				}
				else
				{
					xmlElement.AppendChild(ownerDocument.NewElement("MediaFileName", this.MediaFileName ?? ""));
				}
			}
			xmlElement.AppendChild(ownerDocument.NewElement("TimerType", this.TimerType));
			xmlElement.AppendChild(ownerDocument.NewElement("TimerName", this.TimerName ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("RestartBasedOnTimerName", this.RestartBasedOnTimerName));
			xmlElement.AppendChild(ownerDocument.NewElement("TimerMillisecondDuration", this.TimerMillisecondDuration));
			xmlElement.AppendChild(ownerDocument.NewElement("TimerDuration", (int)(this.TimerMillisecondDuration / 1000L)));
			xmlElement.AppendChild(ownerDocument.NewElement("TimerVisibleDuration", this.TimerVisibleDuration));
			xmlElement.AppendChild(ownerDocument.NewElement("TimerStartBehavior", this.TimerStartBehavior));
			xmlElement.AppendChild(ownerDocument.NewElement("TimerEndingTime", this.TimerEndingTime));
			xmlElement.AppendChild(ownerDocument.NewElement("UseTimerEnding", this.UseTimerEnding));
			xmlElement.AppendChild(ownerDocument.NewElement("UseTimerEnded", this.UseTimerEnded));
			if (this.TimerEndingTrigger != null)
			{
				this.SaveSubTriggerToXml(xmlElement, "TimerEndingTrigger", this.TimerEndingTrigger, files);
			}
			if (this.TimerEndedTrigger != null)
			{
				this.SaveSubTriggerToXml(xmlElement, "TimerEndedTrigger", this.TimerEndedTrigger, files);
			}
			xmlElement.AppendChild(ownerDocument.NewElement("UseCounterResetTimer", this.UseCounterResetTimer));
			xmlElement.AppendChild(ownerDocument.NewElement("CounterResetDuration", this.CounterResetDuration));
			xmlElement.AppendChild(ownerDocument.NewElement("Category", (this.Category != null) ? this.Category.Name : ""));
			xmlElement.AppendChild(ownerDocument.NewElement("Modified", this.Modified));
			xmlElement.AppendChild(ownerDocument.NewElement("UseFastCheck", this.UseFastCheck));
			XmlElement xmlElement2 = ownerDocument.CreateElement("TimerEarlyEnders");
			xmlElement.AppendChild(xmlElement2);
			foreach (Trigger.TimerEarlyEnder timerEarlyEnder in this.TimerEarlyEnders)
			{
				XmlElement xmlElement3 = ownerDocument.CreateElement("EarlyEnder");
				xmlElement2.AppendChild(xmlElement3);
				xmlElement3.AppendChild(ownerDocument.NewElement("EarlyEndText", timerEarlyEnder.EarlyEndText ?? ""));
				xmlElement3.AppendChild(ownerDocument.NewElement("EnableRegex", timerEarlyEnder.EnableRegex));
			}
		}

		// Token: 0x04000141 RID: 321
		private long _SuccessComparisons;

		// Token: 0x04000142 RID: 322
		private long _SuccessCost;

		// Token: 0x04000143 RID: 323
		private long _FailureComparisons;

		// Token: 0x04000144 RID: 324
		private long _FailureCost;

		// Token: 0x04000145 RID: 325
		private long _EarlyEndComparisons;

		// Token: 0x04000146 RID: 326
		private long _EarlyEndCost;

		// Token: 0x04000147 RID: 327
		private TimerTypes _TimerType;

		// Token: 0x0200003D RID: 61
		public class TimerEarlyEnder : BindableObject
		{
			// Token: 0x06000340 RID: 832 RVA: 0x0000F9FC File Offset: 0x0000DBFC
			static TimerEarlyEnder()
			{
				BindableObject.SetDependentProperties(typeof(Trigger.TimerEarlyEnder));
			}

			// Token: 0x06000341 RID: 833 RVA: 0x0000FA0D File Offset: 0x0000DC0D
			public static void RegisterDependentProperties(Type type)
			{
				BindableObject.RegisterDependentProperty(type, "EnableRegex", "EarlyEndText", null);
				BindableObject.RegisterDependentProperty(type, "RegexSelectable", "EarlyEndText", null);
			}

			// Token: 0x06000342 RID: 834 RVA: 0x0000FA31 File Offset: 0x0000DC31
			public TimerEarlyEnder()
			{
				this.EnableRegex = false;
				this.EarlyEndText = string.Empty;
			}

			// Token: 0x06000343 RID: 835 RVA: 0x0000FA4B File Offset: 0x0000DC4B
			public TimerEarlyEnder(string earlyEndText, bool enableRegex)
			{
				this.EnableRegex = enableRegex;
				this.EarlyEndText = earlyEndText;
			}

			// Token: 0x17000130 RID: 304
			// (get) Token: 0x06000344 RID: 836 RVA: 0x0000FA61 File Offset: 0x0000DC61
			// (set) Token: 0x06000345 RID: 837 RVA: 0x0000FA82 File Offset: 0x0000DC82
			public bool EnableRegex
			{
				get
				{
					return this.RegexEligible && (!this.RegexSelectable || base.Get<bool>("EnableRegex"));
				}
				set
				{
					base.Set("EnableRegex", value);
					base.RaisePropertyChanged("EnableRegex");
				}
			}

			// Token: 0x17000131 RID: 305
			// (get) Token: 0x06000346 RID: 838 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
			// (set) Token: 0x06000347 RID: 839 RVA: 0x0000FAAD File Offset: 0x0000DCAD
			public string EarlyEndText
			{
				get
				{
					return base.Get<string>("EarlyEndText");
				}
				set
				{
					base.Set("EarlyEndText", value);
				}
			}

			// Token: 0x17000132 RID: 306
			// (get) Token: 0x06000348 RID: 840 RVA: 0x0000FABB File Offset: 0x0000DCBB
			public bool RegexEligible
			{
				get
				{
					return MatchHelper.RegexValid(this.EarlyEndText);
				}
			}

			// Token: 0x17000133 RID: 307
			// (get) Token: 0x06000349 RID: 841 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
			public bool RegexSelectable
			{
				get
				{
					return !MatchHelper.RegexRequired(this.EarlyEndText) && this.RegexEligible;
				}
			}

			// Token: 0x0600034A RID: 842 RVA: 0x0000FAE0 File Offset: 0x0000DCE0
			public Trigger.TimerEarlyEnder Clone()
			{
				return new Trigger.TimerEarlyEnder
				{
					EnableRegex = this.EnableRegex,
					EarlyEndText = this.EarlyEndText
				};
			}
		}
	}
}
