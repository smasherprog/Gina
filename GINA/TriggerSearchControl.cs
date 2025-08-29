using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200000D RID: 13
	public class TriggerSearchControl : BindableControl
	{
		// Token: 0x06000220 RID: 544 RVA: 0x00007B56 File Offset: 0x00005D56
		static TriggerSearchControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TriggerSearchControl), new FrameworkPropertyMetadata(typeof(TriggerSearchControl)));
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00007B7B File Offset: 0x00005D7B
		public TriggerSearchControl()
		{
			this.ModifiedSince = new DateTime?(DateTime.Today);
			this.Matches = new ObservableCollection<GimaSoft.Business.GINA.Trigger>();
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00007B9E File Offset: 0x00005D9E
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00007BAB File Offset: 0x00005DAB
		public bool ShowTriggerEditor
		{
			get
			{
				return base.Get<bool>("ShowTriggerEditor");
			}
			set
			{
				base.Set("ShowTriggerEditor", value);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00007BBE File Offset: 0x00005DBE
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00007BCB File Offset: 0x00005DCB
		public GimaSoft.Business.GINA.Trigger EditingTrigger
		{
			get
			{
				return base.Get<GimaSoft.Business.GINA.Trigger>("EditingTrigger");
			}
			set
			{
				base.Set("EditingTrigger", value);
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00007BD9 File Offset: 0x00005DD9
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00007BE6 File Offset: 0x00005DE6
		public bool SearchInProgress
		{
			get
			{
				return base.Get<bool>("SearchInProgress");
			}
			set
			{
				base.Set("SearchInProgress", value);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00007BF9 File Offset: 0x00005DF9
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00007C06 File Offset: 0x00005E06
		public string SearchText
		{
			get
			{
				return base.Get<string>("SearchText");
			}
			set
			{
				base.Set("SearchText", value);
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00007C14 File Offset: 0x00005E14
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00007C21 File Offset: 0x00005E21
		public DateTime? ModifiedSince
		{
			get
			{
				return base.Get<DateTime?>("ModifiedSince");
			}
			set
			{
				base.Set("ModifiedSince", value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00007C34 File Offset: 0x00005E34
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00007C41 File Offset: 0x00005E41
		public ObservableCollection<GimaSoft.Business.GINA.Trigger> Matches
		{
			get
			{
				return base.Get<ObservableCollection<GimaSoft.Business.GINA.Trigger>>("Matches");
			}
			set
			{
				base.Set("Matches", value);
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000806C File Offset: 0x0000626C
		public GenericCommand SearchCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.SearchInProgress = true;
					this.Matches = null;
					List<GimaSoft.Business.GINA.Trigger> list = new List<GimaSoft.Business.GINA.Trigger>();
					try
					{
						List<GimaSoft.Business.GINA.Trigger> list2 = TriggerGroup.RootGroup.DescendantTree.SelectMany((TriggerGroup o) => o.Triggers).ToList<GimaSoft.Business.GINA.Trigger>();
						string text = (this.SearchText ?? "").ToLower();
						using (List<GimaSoft.Business.GINA.Trigger>.Enumerator enumerator = list2.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								GimaSoft.Business.GINA.Trigger trigger = enumerator.Current;
								bool flag = false;
								if (this.ModifiedSince == null || !(trigger.Modified < this.ModifiedSince))
								{
									if (string.IsNullOrEmpty(this.SearchText))
									{
										list.Add(trigger);
									}
									else if (trigger.TriggerText.ToLower().Contains(text) || trigger.Name.ToLower().Contains(text) || (trigger.Comments ?? "").ToLower().Contains(text) || (trigger.DisplayText ?? "").ToLower().Contains(text) || (trigger.TextToVoiceText ?? "").ToLower().Contains(text))
									{
										list.Add(trigger);
									}
									else
									{
										foreach (GINACharacter ginacharacter in GINACharacter.All)
										{
											if (trigger.TriggerText.ToLower().Replace("{c}", ginacharacter.Name.ToLower()).Contains(text) || trigger.Name.ToLower().Contains(text) || (trigger.Comments ?? "").ToLower().Contains(text) || (trigger.DisplayText ?? "").ToLower().Replace("{c}", ginacharacter.Name.ToLower()).Contains(text) || (trigger.TextToVoiceText ?? "").ToLower().Replace("{c}", ginacharacter.Name.ToLower()).Contains(text))
											{
												flag = true;
												list.Add(trigger);
												break;
											}
										}
										if (!flag)
										{
											List<TriggerFilter> list3 = GINACharacter.All.Select((GINACharacter o) => new TriggerFilter(o, trigger)).ToList<TriggerFilter>();
											foreach (TriggerFilter triggerFilter in list3)
											{
												if (triggerFilter.IsMatch(this.SearchText))
												{
													list.Add(trigger);
													break;
												}
											}
										}
									}
								}
							}
						}
					}
					catch
					{
					}
					this.Matches = list.OrderBy((GimaSoft.Business.GINA.Trigger o) => o.Name).ToObservableCollection<GimaSoft.Business.GINA.Trigger>();
					this.SearchInProgress = false;
				});
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00008088 File Offset: 0x00006288
		public GenericCommand CloseCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					base.Visibility = Visibility.Collapsed;
				});
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000230 RID: 560 RVA: 0x000080C2 File Offset: 0x000062C2
		public GenericCommand EditTriggerCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					GimaSoft.Business.GINA.Trigger trigger = p as GimaSoft.Business.GINA.Trigger;
					if (p == null)
					{
						return;
					}
					this.EditingTrigger = trigger;
					this.ShowTriggerEditor = true;
				});
			}
		}
	}
}
