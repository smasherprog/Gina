using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000006 RID: 6
	public class EverquestCharacterSetSelector : BindableControl
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00004228 File Offset: 0x00002428
		static EverquestCharacterSetSelector()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EverquestCharacterSetSelector), new FrameworkPropertyMetadata(typeof(EverquestCharacterSetSelector)));
			BindableControl.SetDependentProperties(typeof(EverquestCharacterSetSelector));
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000042AF File Offset: 0x000024AF
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "CanExport", "SelectedTriggerSet", null);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000042C2 File Offset: 0x000024C2
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000042D4 File Offset: 0x000024D4
		public TriggerGroup SelectedTriggerGroup
		{
			get
			{
				return (TriggerGroup)base.GetValue(EverquestCharacterSetSelector.SelectedTriggerGroupProperty);
			}
			set
			{
				base.SetValue(EverquestCharacterSetSelector.SelectedTriggerGroupProperty, value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000042E2 File Offset: 0x000024E2
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000042F4 File Offset: 0x000024F4
		public GimaSoft.Business.GINA.Trigger SelectedTrigger
		{
			get
			{
				return (GimaSoft.Business.GINA.Trigger)base.GetValue(EverquestCharacterSetSelector.SelectedTriggerProperty);
			}
			set
			{
				base.SetValue(EverquestCharacterSetSelector.SelectedTriggerProperty, value);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004302 File Offset: 0x00002502
		// (set) Token: 0x060000BB RID: 187 RVA: 0x0000430F File Offset: 0x0000250F
		public IEnumerable<EverquestFolderViewModel> Characters
		{
			get
			{
				return base.Get<IEnumerable<EverquestFolderViewModel>>("Characters");
			}
			set
			{
				base.Set("Characters", value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000431D File Offset: 0x0000251D
		// (set) Token: 0x060000BD RID: 189 RVA: 0x0000432A File Offset: 0x0000252A
		public IEnumerable<string> TriggerSets
		{
			get
			{
				return base.Get<IEnumerable<string>>("TriggerSets");
			}
			set
			{
				base.Set("TriggerSets", value);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004338 File Offset: 0x00002538
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004345 File Offset: 0x00002545
		public string SelectedTriggerSet
		{
			get
			{
				return base.Get<string>("SelectedTriggerSet");
			}
			set
			{
				base.Set("SelectedTriggerSet", value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004364 File Offset: 0x00002564
		public bool CanExport
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.SelectedTriggerSet))
				{
					return this.Characters.SelectMany((EverquestFolderViewModel o) => o.Children).Any((EverquestFolderViewModel o) => o.IsSelected);
				}
				return false;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000044B0 File Offset: 0x000026B0
		public GenericCommand VisibilityChangedCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if ((Visibility)p == Visibility.Visible && !DesignerProperties.GetIsInDesignMode(this))
						{
							this.Characters = EverquestFolderViewModel.GetCharacters();
							this.TriggerSets = EverquestFolderViewModel.GetTriggerSets();
							List<EverquestFolderViewModel> list = this.Characters.SelectMany((EverquestFolderViewModel o) => o.Children).ToList<EverquestFolderViewModel>();
							foreach (EverquestFolderViewModel everquestFolderViewModel in list)
							{
								everquestFolderViewModel.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
								{
									if (e.PropertyName == "IsSelected")
									{
										base.RaisePropertyChanged("CanExport");
									}
								};
							}
						}
					}
				};
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004602 File Offset: 0x00002802
		public GenericCommand ExportCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					if (!this.CanExport)
					{
						return;
					}
					List<Package.EQCharacterTriggerSet> list = (from o in this.Characters.SelectMany((EverquestFolderViewModel o) => o.Children)
						where o.IsSelected
						select new Package.EQCharacterTriggerSet
						{
							Server = o.Server,
							Character = o.Character,
							TriggerSet = this.SelectedTriggerSet
						}).ToList<Package.EQCharacterTriggerSet>();
					List<GimaSoft.Business.GINA.Trigger> list2 = new List<GimaSoft.Business.GINA.Trigger>();
					if (this.SelectedTriggerGroup != null)
					{
						list2.AddRange(this.SelectedTriggerGroup.DescendantTree.SelectMany((TriggerGroup o) => o.Triggers));
					}
					if (this.SelectedTrigger != null)
					{
						list2.Add(this.SelectedTrigger);
					}
					Package.SaveTriggersToEQTriggers(list, list2);
					base.Visibility = Visibility.Collapsed;
				});
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000461E File Offset: 0x0000281E
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

		// Token: 0x0400002E RID: 46
		public static readonly DependencyProperty SelectedTriggerGroupProperty = DependencyProperty.Register("SelectedTriggerGroup", typeof(TriggerGroup), typeof(EverquestCharacterSetSelector), null);

		// Token: 0x0400002F RID: 47
		public static readonly DependencyProperty SelectedTriggerProperty = DependencyProperty.Register("SelectedTrigger", typeof(GimaSoft.Business.GINA.Trigger), typeof(EverquestCharacterSetSelector), null);
	}
}
