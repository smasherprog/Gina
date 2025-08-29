using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200000B RID: 11
	public class TriggerGroupEditor : BindableControl
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x000071C0 File Offset: 0x000053C0
		static TriggerGroupEditor()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TriggerGroupEditor), new FrameworkPropertyMetadata(typeof(TriggerGroupEditor)));
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007264 File Offset: 0x00005464
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00007276 File Offset: 0x00005476
		public TriggerGroup TriggerGroup
		{
			get
			{
				return (TriggerGroup)base.GetValue(TriggerGroupEditor.TriggerGroupProperty);
			}
			set
			{
				base.SetValue(TriggerGroupEditor.TriggerGroupProperty, value);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007284 File Offset: 0x00005484
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007296 File Offset: 0x00005496
		public TriggerGroup ParentGroup
		{
			get
			{
				return (TriggerGroup)base.GetValue(TriggerGroupEditor.ParentGroupProperty);
			}
			set
			{
				base.SetValue(TriggerGroupEditor.ParentGroupProperty, value);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000072A4 File Offset: 0x000054A4
		// (set) Token: 0x060001EA RID: 490 RVA: 0x000072B1 File Offset: 0x000054B1
		public string GroupName
		{
			get
			{
				return base.Get<string>("GroupName");
			}
			set
			{
				base.Set("GroupName", value);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000072BF File Offset: 0x000054BF
		// (set) Token: 0x060001EC RID: 492 RVA: 0x000072CC File Offset: 0x000054CC
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

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000072DA File Offset: 0x000054DA
		// (set) Token: 0x060001EE RID: 494 RVA: 0x000072E7 File Offset: 0x000054E7
		public bool EnableByDefault
		{
			get
			{
				return base.Get<bool>("EnableByDefault");
			}
			set
			{
				base.Set("EnableByDefault", value);
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001EF RID: 495 RVA: 0x000072FA File Offset: 0x000054FA
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007307 File Offset: 0x00005507
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

		// Token: 0x060001F1 RID: 497 RVA: 0x00007318 File Offset: 0x00005518
		private static void TriggerGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TriggerGroupEditor triggerGroupEditor = d as TriggerGroupEditor;
			TriggerGroup triggerGroup = e.NewValue as TriggerGroup;
			if (triggerGroup != null && triggerGroupEditor.ParentGroup != null)
			{
				triggerGroupEditor.ParentGroup = null;
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000734C File Offset: 0x0000554C
		private static void ParentGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TriggerGroupEditor triggerGroupEditor = d as TriggerGroupEditor;
			TriggerGroup triggerGroup = e.NewValue as TriggerGroup;
			if (triggerGroup != null && triggerGroupEditor.TriggerGroup != null)
			{
				triggerGroupEditor.TriggerGroup = null;
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007380 File Offset: 0x00005580
		public void SetFields()
		{
			if (this.TriggerGroup != null)
			{
				this.GroupName = this.TriggerGroup.Name;
				this.Comments = this.TriggerGroup.Comments;
				this.EnableByDefault = this.TriggerGroup.EnableByDefault;
				return;
			}
			this.GroupName = "";
			this.Comments = "";
			this.EnableByDefault = true;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000073E8 File Offset: 0x000055E8
		private void SetTriggerGroupFields(TriggerGroup group)
		{
			bool flag = this.EnableByDefault && !group.EnableByDefault;
			group.Name = this.GroupName;
			group.SelfCommented = !string.IsNullOrWhiteSpace(this.Comments) && (this.Comments ?? "") != (group.Comments ?? "");
			group.Comments = this.Comments;
			group.EnableByDefault = this.EnableByDefault;
			if (flag)
			{
				foreach (GINACharacter ginacharacter in GINACharacter.All)
				{
					ginacharacter.TriggerGroups.Add(group);
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x000075E0 File Offset: 0x000057E0
		public GenericCommand SaveCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (string.IsNullOrWhiteSpace(this.GroupName))
						{
							this.ErrorMessage = "A name is required for the new group.";
							return;
						}
						if (this.ParentGroup == null)
						{
							if (this.TriggerGroup != null)
							{
								if (this.TriggerGroup.ParentGroup.Groups.Any((TriggerGroup o) => o != this.TriggerGroup && o.Name.ToLower() == this.GroupName.ToLower()))
								{
									this.ErrorMessage = "A group with that name already exists.";
									return;
								}
								this.SetTriggerGroupFields(this.TriggerGroup);
								Configuration.SaveConfiguration(false);
								base.Visibility = Visibility.Collapsed;
							}
							return;
						}
						if (this.ParentGroup.Groups.Any((TriggerGroup o) => o.Name.ToLower() == this.GroupName.ToLower()))
						{
							this.ErrorMessage = "A group with that name already exists.";
							return;
						}
						TriggerGroup triggerGroup = new TriggerGroup();
						this.SetTriggerGroupFields(triggerGroup);
						this.ParentGroup.AddGroup(triggerGroup, null);
						Configuration.SaveConfiguration(false);
						base.Visibility = Visibility.Collapsed;
					}
				};
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00007610 File Offset: 0x00005810
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000767C File Offset: 0x0000587C
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
							this.ErrorMessage = null;
							this.SetFields();
							TextBox textBox = base.Template.FindName("tbGroupName", this) as TextBox;
							if (textBox != null)
							{
								textBox.Focus();
							}
						}
					}
				};
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x000076AC File Offset: 0x000058AC
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

		// Token: 0x04000042 RID: 66
		public static readonly DependencyProperty TriggerGroupProperty = DependencyProperty.Register("TriggerGroup", typeof(TriggerGroup), typeof(TriggerGroupEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TriggerGroupEditor.TriggerGroupChanged)));

		// Token: 0x04000043 RID: 67
		public static readonly DependencyProperty ParentGroupProperty = DependencyProperty.Register("ParentGroup", typeof(TriggerGroup), typeof(TriggerGroupEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TriggerGroupEditor.ParentGroupChanged)));
	}
}
