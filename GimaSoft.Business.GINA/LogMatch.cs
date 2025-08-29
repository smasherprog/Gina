using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000023 RID: 35
	public class LogMatch
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x00009420 File Offset: 0x00007620
		public LogMatch(GINACharacter character, string matchedText, DateTime? loggedTime, IEnumerable<TriggerFilter> inactiveFilters)
		{
			this.Character = character;
			this.MatchedText = matchedText;
			this.LoggedTime = loggedTime;
			foreach (TriggerFilter triggerFilter in character.Filters)
			{
				if (triggerFilter.IsMatch(matchedText))
				{
					this.MatchedTrigger = triggerFilter.Trigger;
					break;
				}
			}
			if (this.MatchedTrigger == null && inactiveFilters != null)
			{
				foreach (TriggerFilter triggerFilter2 in inactiveFilters)
				{
					if (triggerFilter2.IsMatch(matchedText))
					{
						this.MatchedTrigger = triggerFilter2.Trigger;
						this.TriggerIsInactive = true;
						break;
					}
				}
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000094FC File Offset: 0x000076FC
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009504 File Offset: 0x00007704
		public GINACharacter Character { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000950D File Offset: 0x0000770D
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00009515 File Offset: 0x00007715
		public string MatchedText { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000951E File Offset: 0x0000771E
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00009526 File Offset: 0x00007726
		public Trigger MatchedTrigger { get; set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000952F File Offset: 0x0000772F
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00009537 File Offset: 0x00007737
		public bool TriggerIsInactive { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00009540 File Offset: 0x00007740
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00009548 File Offset: 0x00007748
		public DateTime? LoggedTime { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00009554 File Offset: 0x00007754
		public string SuggestedTriggerText
		{
			get
			{
				int num = 0;
				string text = this.MatchedText.Replace(this.Character.Name, "{C}");
				Match match = Regex.Match(text, "\\d+");
				if (match.Success)
				{
					foreach (Group group in match.Groups.Cast<Group>())
					{
						text = text.Remove(group.Index, group.Length).Insert(group.Index, "{S" + ((num == 0) ? "" : num.ToString()) + "}");
					}
				}
				return text;
			}
		}
	}
}
