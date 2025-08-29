using System;
using System.Collections.Generic;
using System.Linq;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200001E RID: 30
	public class TimerEarlyEndFilter
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00006605 File Offset: 0x00004805
		public TimerEarlyEndFilter(TriggerMatchedEventArgs args)
		{
			this.TriggerMatchedEventArgs = args;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000661F File Offset: 0x0000481F
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00006628 File Offset: 0x00004828
		public TriggerMatchedEventArgs TriggerMatchedEventArgs
		{
			get
			{
				return this._TriggerMatchedEventArgs;
			}
			set
			{
				this._TriggerMatchedEventArgs = value;
				if (this._TriggerMatchedEventArgs != null && this._TriggerMatchedEventArgs.TriggerFilter != null && this._TriggerMatchedEventArgs.TriggerFilter.Trigger != null && this._TriggerMatchedEventArgs.TriggerFilter.Trigger.TimerEarlyEnders != null && this._TriggerMatchedEventArgs.TriggerFilter.Trigger.TimerEarlyEnders.Any<Trigger.TimerEarlyEnder>())
				{
					this.MatchHelpers.Clear();
					foreach (Trigger.TimerEarlyEnder timerEarlyEnder in this._TriggerMatchedEventArgs.TriggerFilter.Trigger.TimerEarlyEnders)
					{
						this.MatchHelpers.Add(new MatchHelper(this.TriggerMatchedEventArgs.TriggerFilter.Character, this.TriggerMatchedEventArgs.TriggerFilter.ResolveText(this.TriggerMatchedEventArgs.MatchedText, timerEarlyEnder.EarlyEndText, false), timerEarlyEnder.EnableRegex, true));
					}
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006748 File Offset: 0x00004948
		public bool IsMatch(string text)
		{
			foreach (MatchHelper matchHelper in this.MatchHelpers)
			{
				if (matchHelper.IsMatch(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400008A RID: 138
		private string[] TriggerTexts;

		// Token: 0x0400008B RID: 139
		private List<MatchHelper> MatchHelpers = new List<MatchHelper>();

		// Token: 0x0400008C RID: 140
		private TriggerMatchedEventArgs _TriggerMatchedEventArgs;
	}
}
