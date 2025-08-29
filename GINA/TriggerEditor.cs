using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200000A RID: 10
	public class TriggerEditor : BindableControl
	{
		// Token: 0x0600019B RID: 411 RVA: 0x000062DC File Offset: 0x000044DC
		static TriggerEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TriggerEditor), new FrameworkPropertyMetadata(typeof(TriggerEditor)));
			BindableControl.SetDependentProperties(typeof(TriggerEditor));
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000063C0 File Offset: 0x000045C0
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "AllowSave", "TimerType", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "TimerMillisecondDuration", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "TriggerName", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "TriggerText", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "EnableRegex", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "UseCounterResetTimer", null);
			BindableControl.RegisterDependentProperty(type, "AllowSave", "CounterResetDuration", null);
			BindableControl.RegisterDependentProperty(type, "UsingTokens", "TriggerText", null);
			BindableControl.RegisterDependentProperty(type, "EnableRegex", "TriggerText", null);
			BindableControl.RegisterDependentProperty(type, "UseFastCheck", "EnableRegex", null);
			BindableControl.RegisterDependentProperty(type, "TimerEnabled", "TimerType", null);
			BindableControl.RegisterDependentProperty(type, "TimerCountsDown", "TimerType", null);
			BindableControl.RegisterDependentProperty(type, "UseTimerSpan", "TriggerText", null);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006554 File Offset: 0x00004754
		public TriggerEditor()
		{
			this.TimerEarlyEnders = new ObservableCollection<GimaSoft.Business.GINA.Trigger.TimerEarlyEnder>();
			this.MatchHelpers = new ObservableCollection<MatchHelper>();
			base.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "TriggerText" || e.PropertyName == "EnableRegex" || e.PropertyName == "UseFastCheck")
				{
					foreach (MatchHelper matchHelper in this.MatchHelpers)
					{
						try
						{
							matchHelper.GenerateFilter(this.TriggerText, this.EnableRegex, this.UseFastCheck);
						}
						catch
						{
						}
					}
				}
			};
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00006596 File Offset: 0x00004796
		// (set) Token: 0x0600019F RID: 415 RVA: 0x000065A8 File Offset: 0x000047A8
		public GimaSoft.Business.GINA.Trigger Trigger
		{
			get
			{
				return (GimaSoft.Business.GINA.Trigger)base.GetValue(TriggerEditor.TriggerProperty);
			}
			set
			{
				base.SetValue(TriggerEditor.TriggerProperty, value);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000065B6 File Offset: 0x000047B6
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000065C8 File Offset: 0x000047C8
		public TriggerGroup TriggerGroup
		{
			get
			{
				return (TriggerGroup)base.GetValue(TriggerEditor.TriggerGroupProperty);
			}
			set
			{
				base.SetValue(TriggerEditor.TriggerGroupProperty, value);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000065D6 File Offset: 0x000047D6
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x000065E8 File Offset: 0x000047E8
		public string SuggestedSearchText
		{
			get
			{
				return (string)base.GetValue(TriggerEditor.SuggestedSearchTextProperty);
			}
			set
			{
				base.SetValue(TriggerEditor.SuggestedSearchTextProperty, value);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x000065F6 File Offset: 0x000047F6
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00006603 File Offset: 0x00004803
		public string TriggerName
		{
			get
			{
				return base.Get<string>("TriggerName");
			}
			set
			{
				base.Set("TriggerName", value);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006611 File Offset: 0x00004811
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000661E File Offset: 0x0000481E
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000662C File Offset: 0x0000482C
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00006639 File Offset: 0x00004839
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00006647 File Offset: 0x00004847
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00006654 File Offset: 0x00004854
		public ObservableCollection<GimaSoft.Business.GINA.Trigger.TimerEarlyEnder> TimerEarlyEnders
		{
			get
			{
				return base.Get<ObservableCollection<GimaSoft.Business.GINA.Trigger.TimerEarlyEnder>>("TimerEarlyEnders");
			}
			set
			{
				base.Set("TimerEarlyEnders", value);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00006662 File Offset: 0x00004862
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000666F File Offset: 0x0000486F
		public ObservableCollection<MatchHelper> MatchHelpers
		{
			get
			{
				return base.Get<ObservableCollection<MatchHelper>>("MatchHelpers");
			}
			set
			{
				base.Set("MatchHelpers", value);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000667D File Offset: 0x0000487D
		public bool UsingTokens
		{
			get
			{
				return MatchHelper.RegexRequired(this.TriggerText);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000668A File Offset: 0x0000488A
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x000066A1 File Offset: 0x000048A1
		public bool EnableRegex
		{
			get
			{
				return this.UsingTokens || base.Get<bool>("EnableRegex");
			}
			set
			{
				base.Set("EnableRegex", value);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000066B4 File Offset: 0x000048B4
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x000066CB File Offset: 0x000048CB
		public bool UseFastCheck
		{
			get
			{
				return this.EnableRegex && base.Get<bool>("UseFastCheck");
			}
			set
			{
				base.Set("UseFastCheck", value);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000066DE File Offset: 0x000048DE
		public bool UseTimerSpan
		{
			get
			{
				return this.TriggerText.ToLower().Contains("{ts}");
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000066F5 File Offset: 0x000048F5
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00006704 File Offset: 0x00004904
		public TimerTypes TimerType
		{
			get
			{
				return base.Get<TimerTypes>("TimerType");
			}
			set
			{
				base.Set("TimerType", value);
				if (value != TimerTypes.NoTimer && string.IsNullOrWhiteSpace(this.TimerName))
				{
					this.TimerName = this.TriggerName;
				}
				if (!value.CountsDown())
				{
					this.TimerMillisecondDuration = 0L;
				}
				this.UseTimerEnding &= value.CountsDown();
				this.UseTimerEnded &= value.CountsDown();
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006774 File Offset: 0x00004974
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00006781 File Offset: 0x00004981
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000678F File Offset: 0x0000498F
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000679C File Offset: 0x0000499C
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000067AF File Offset: 0x000049AF
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000067BC File Offset: 0x000049BC
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000067CF File Offset: 0x000049CF
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000067DC File Offset: 0x000049DC
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000067EF File Offset: 0x000049EF
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000067FC File Offset: 0x000049FC
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000680F File Offset: 0x00004A0F
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000681C File Offset: 0x00004A1C
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

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000682F File Offset: 0x00004A2F
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x0000683C File Offset: 0x00004A3C
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000684F File Offset: 0x00004A4F
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000685C File Offset: 0x00004A5C
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000686F File Offset: 0x00004A6F
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000687C File Offset: 0x00004A7C
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000688F File Offset: 0x00004A8F
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000689C File Offset: 0x00004A9C
		public string ErrorMessage
		{
			get
			{
				return base.Get<string>("ErrorMessage");
			}
			set
			{
				base.Set("ErrorMessage", value);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001CA RID: 458 RVA: 0x000068AA File Offset: 0x00004AAA
		public TriggerNotificationEditor MainTriggerEditor
		{
			get
			{
				return (TriggerNotificationEditor)base.Template.FindName("MainTriggerEditor", this);
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000068C2 File Offset: 0x00004AC2
		public TriggerNotificationEditor TimerEndingTriggerEditor
		{
			get
			{
				return (TriggerNotificationEditor)base.Template.FindName("TimerEndingTriggerEditor", this);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000068DA File Offset: 0x00004ADA
		public TriggerNotificationEditor TimerEndedTriggerEditor
		{
			get
			{
				return (TriggerNotificationEditor)base.Template.FindName("TimerEndedTriggerEditor", this);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000068F2 File Offset: 0x00004AF2
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000068FF File Offset: 0x00004AFF
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

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000690D File Offset: 0x00004B0D
		public IEnumerable<TriggerCategory> TriggerCategories
		{
			get
			{
				return TriggerCategory.All;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00006914 File Offset: 0x00004B14
		public bool AllowSave
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.TriggerName) && !string.IsNullOrWhiteSpace(this.TriggerText) && (!this.UseCounterResetTimer || this.CounterResetDuration > 0L) && (!this.TimerType.CountsDown() || this.TimerMillisecondDuration > 0L || this.UseTimerSpan);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000696E File Offset: 0x00004B6E
		public bool TimerEnabled
		{
			get
			{
				return this.TimerType != TimerTypes.NoTimer;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000697C File Offset: 0x00004B7C
		public bool TimerCountsDown
		{
			get
			{
				return this.TimerType.CountsDown();
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000698C File Offset: 0x00004B8C
		private static void TriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TriggerEditor triggerEditor = d as TriggerEditor;
			GimaSoft.Business.GINA.Trigger trigger = e.NewValue as GimaSoft.Business.GINA.Trigger;
			if (trigger != null && triggerEditor.TriggerGroup != null)
			{
				triggerEditor.TriggerGroup = null;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000069C0 File Offset: 0x00004BC0
		private static void TriggerGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TriggerEditor triggerEditor = d as TriggerEditor;
			TriggerGroup triggerGroup = e.NewValue as TriggerGroup;
			if (triggerGroup != null && triggerEditor.Trigger != null)
			{
				triggerEditor.Trigger = null;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000069F4 File Offset: 0x00004BF4
		public void SetFields()
		{
			if (this.Trigger != null)
			{
				this.TriggerName = this.Trigger.Name;
				this.TriggerText = this.SuggestedSearchText ?? this.Trigger.TriggerText;
				this.Comments = this.Trigger.Comments;
				this.EnableRegex = this.Trigger.EnableRegex;
				this.TimerType = this.Trigger.TimerType;
				this.TimerName = this.Trigger.TimerName;
				this.RestartBasedOnTimerName = this.Trigger.RestartBasedOnTimerName;
				this.TimerEarlyEnders.Clear();
				foreach (GimaSoft.Business.GINA.Trigger.TimerEarlyEnder timerEarlyEnder in this.Trigger.TimerEarlyEnders)
				{
					this.TimerEarlyEnders.Add(timerEarlyEnder.Clone());
				}
				this.TimerStartBehavior = this.Trigger.TimerStartBehavior;
				this.TimerMillisecondDuration = this.Trigger.TimerMillisecondDuration;
				this.UseTimerEnding = this.Trigger.UseTimerEnding;
				this.UseTimerEnded = this.Trigger.UseTimerEnded;
				this.TimerEndingTime = this.Trigger.TimerEndingTime;
				this.Category = this.Trigger.Category;
				this.UseCounterResetTimer = this.Trigger.UseCounterResetTimer;
				this.CounterResetDuration = this.Trigger.CounterResetDuration;
				this.UseFastCheck = this.Trigger.UseFastCheck;
			}
			else
			{
				this.TriggerName = "";
				this.TriggerText = this.SuggestedSearchText ?? "";
				this.Comments = "";
				this.EnableRegex = false;
				this.TimerType = TimerTypes.NoTimer;
				this.TimerName = "";
				this.RestartBasedOnTimerName = true;
				this.TimerEarlyEnders.Clear();
				this.TimerStartBehavior = TimerStartBehaviors.StartNewTimer;
				this.TimerMillisecondDuration = 0L;
				this.UseTimerEnding = false;
				this.UseTimerEnded = false;
				this.TimerEndingTime = 1;
				this.Category = TriggerCategory.DefaultCategory;
				this.UseCounterResetTimer = false;
				this.CounterResetDuration = 0L;
				this.UseFastCheck = true;
			}
			if (this.MainTriggerEditor != null)
			{
				this.MainTriggerEditor.SetFields(this.Trigger);
				this.TimerEndingTriggerEditor.SetFields((this.Trigger == null) ? null : this.Trigger.TimerEndingTrigger);
				this.TimerEndedTriggerEditor.SetFields((this.Trigger == null) ? null : this.Trigger.TimerEndedTrigger);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00006C94 File Offset: 0x00004E94
		private void SetTriggerFields(GimaSoft.Business.GINA.Trigger trigger)
		{
			trigger.Name = this.TriggerName;
			trigger.TriggerText = this.TriggerText;
			trigger.Comments = this.Comments;
			trigger.EnableRegex = this.EnableRegex;
			trigger.TimerType = this.TimerType;
			trigger.TimerName = this.TimerName;
			trigger.RestartBasedOnTimerName = this.RestartBasedOnTimerName;
			trigger.TimerEarlyEnders.Clear();
			foreach (GimaSoft.Business.GINA.Trigger.TimerEarlyEnder timerEarlyEnder in this.TimerEarlyEnders.Where((GimaSoft.Business.GINA.Trigger.TimerEarlyEnder o) => !string.IsNullOrWhiteSpace(o.EarlyEndText)))
			{
				trigger.TimerEarlyEnders.Add(timerEarlyEnder.Clone());
			}
			trigger.TimerStartBehavior = this.TimerStartBehavior;
			trigger.TimerMillisecondDuration = this.TimerMillisecondDuration;
			trigger.UseTimerEnding = this.UseTimerEnding;
			trigger.UseTimerEnded = this.UseTimerEnded;
			trigger.TimerEndingTime = this.TimerEndingTime;
			trigger.UseCounterResetTimer = this.UseCounterResetTimer;
			trigger.CounterResetDuration = this.CounterResetDuration;
			trigger.Modified = DateTime.Now;
			trigger.UseFastCheck = this.UseFastCheck;
			this.MainTriggerEditor.SetTriggerFields(trigger);
			if (trigger.UseTimerEnding)
			{
				if (trigger.TimerEndingTrigger == null)
				{
					trigger.TimerEndingTrigger = new GimaSoft.Business.GINA.Trigger();
				}
				this.TimerEndingTriggerEditor.SetTriggerFields(trigger.TimerEndingTrigger);
			}
			else
			{
				trigger.TimerEndingTrigger = null;
			}
			if (trigger.UseTimerEnded)
			{
				if (trigger.TimerEndedTrigger == null)
				{
					trigger.TimerEndedTrigger = new GimaSoft.Business.GINA.Trigger();
				}
				this.TimerEndedTriggerEditor.SetTriggerFields(trigger.TimerEndedTrigger);
			}
			else
			{
				trigger.TimerEndedTrigger = null;
			}
			trigger.Category = this.Category;
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00006FC0 File Offset: 0x000051C0
		public GenericCommand SaveCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.SuggestedSearchText = null;
						if (this.EnableRegex)
						{
							try
							{
								new Regex(this.TriggerText);
							}
							catch
							{
								this.ErrorMessage = "The string in the Trigger Text field is not a valid regular expression.";
								return;
							}
						}
						if (this.TriggerGroup != null)
						{
							TriggerGroup triggerGroup = this.TriggerGroup ?? this.Trigger.ParentGroup;
							if (triggerGroup.Triggers.Any((GimaSoft.Business.GINA.Trigger o) => o.Name.ToLower() == this.TriggerName.ToLower()))
							{
								this.ErrorMessage = "A trigger with that name already exists.";
								return;
							}
							GimaSoft.Business.GINA.Trigger trigger = new GimaSoft.Business.GINA.Trigger();
							this.SetTriggerFields(trigger);
							triggerGroup.AddTrigger(trigger, null);
							Configuration.SaveConfiguration(false);
							base.Visibility = Visibility.Collapsed;
							return;
						}
						else if (this.Trigger != null)
						{
							if (this.Trigger.ParentGroup.Triggers.Any((GimaSoft.Business.GINA.Trigger o) => o != this.Trigger && o.Name.ToLower() == this.TriggerName.ToLower()))
							{
								this.ErrorMessage = "A trigger with that name already exists.";
								return;
							}
							this.SetTriggerFields(this.Trigger);
							Configuration.SaveConfiguration(false);
							base.Visibility = Visibility.Collapsed;
						}
					}
				};
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00006FF8 File Offset: 0x000051F8
		public GenericCommand CancelCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						base.Visibility = Visibility.Collapsed;
						this.SuggestedSearchText = null;
					}
				};
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007134 File Offset: 0x00005334
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
							this.MatchHelpers.Clear();
							this.SetFields();
							if (this.MainTriggerEditor != null && this.MainTriggerEditor.SelectedVoiceCharacter == null)
							{
								this.MainTriggerEditor.SelectedVoiceCharacter = GINAData.Current.Characters.FirstOrDefault<CharacterViewModel>();
							}
							if (this.TimerEndedTriggerEditor != null && this.TimerEndingTriggerEditor.SelectedVoiceCharacter == null)
							{
								this.TimerEndingTriggerEditor.SelectedVoiceCharacter = GINAData.Current.Characters.FirstOrDefault<CharacterViewModel>();
							}
							if (this.TimerEndedTriggerEditor != null && this.TimerEndedTriggerEditor.SelectedVoiceCharacter == null)
							{
								this.TimerEndedTriggerEditor.SelectedVoiceCharacter = GINAData.Current.Characters.FirstOrDefault<CharacterViewModel>();
							}
							foreach (GINACharacter ginacharacter in GINACharacter.All)
							{
								this.MatchHelpers.Add(new MatchHelper(ginacharacter, this.TriggerText, this.EnableRegex, this.UseFastCheck));
							}
						}
					}
				};
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00007164 File Offset: 0x00005364
		public GenericCommand ClearErrorCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						this.ErrorMessage = null;
					}
				};
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000718C File Offset: 0x0000538C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			TextBox textBox = base.Template.FindName("tbTriggerName", this) as TextBox;
			if (textBox != null)
			{
				textBox.Focus();
			}
		}

		// Token: 0x0400003E RID: 62
		public static readonly DependencyProperty TriggerProperty = DependencyProperty.Register("Trigger", typeof(GimaSoft.Business.GINA.Trigger), typeof(TriggerEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TriggerEditor.TriggerChanged)));

		// Token: 0x0400003F RID: 63
		public static readonly DependencyProperty TriggerGroupProperty = DependencyProperty.Register("TriggerGroup", typeof(TriggerGroup), typeof(TriggerEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TriggerEditor.TriggerGroupChanged)));

		// Token: 0x04000040 RID: 64
		public static readonly DependencyProperty SuggestedSearchTextProperty = DependencyProperty.Register("SuggestedSearchText", typeof(string), typeof(TriggerEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
	}
}
