using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using GimaSoft.Business.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000007 RID: 7
	public class EverquestTriggerSetSelector : BindableControl
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00004639 File Offset: 0x00002839
		static EverquestTriggerSetSelector()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EverquestTriggerSetSelector), new FrameworkPropertyMetadata(typeof(EverquestTriggerSetSelector)));
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000465E File Offset: 0x0000285E
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x0000466B File Offset: 0x0000286B
		public IEnumerable<EverquestFolderViewModel> TriggerSets
		{
			get
			{
				return base.Get<IEnumerable<EverquestFolderViewModel>>("TriggerSets");
			}
			set
			{
				base.Set("TriggerSets", value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004679 File Offset: 0x00002879
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00004686 File Offset: 0x00002886
		public EverquestFolderViewModel SelectedTriggerSet
		{
			get
			{
				return base.Get<EverquestFolderViewModel>("SelectedTriggerSet");
			}
			set
			{
				base.Set("SelectedTriggerSet", value);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x000046B4 File Offset: 0x000028B4
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
							this.TriggerSets = EverquestFolderViewModel.GetCharacterSets();
						}
					}
				};
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004722 File Offset: 0x00002922
		public GenericCommand ImportCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					if (this.SelectedTriggerSet == null)
					{
						return;
					}
					Package package = Package.OpenPackageFromEQTriggers(this.SelectedTriggerSet.Filename, this.SelectedTriggerSet.TriggerSet);
					Package.AddDetectedShare(new ShareDetectedEventArgs(PackageShareType.GINAPackageFile, package, null));
					base.Visibility = Visibility.Collapsed;
				});
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000473E File Offset: 0x0000293E
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000477D File Offset: 0x0000297D
		public GenericCommand SelectedTriggerSetChangedCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					EverquestFolderViewModel everquestFolderViewModel = p as EverquestFolderViewModel;
					this.SelectedTriggerSet = ((everquestFolderViewModel != null && everquestFolderViewModel.IsTriggerSet) ? everquestFolderViewModel : null);
				});
			}
		}
	}
}
