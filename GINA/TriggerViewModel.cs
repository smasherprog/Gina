using System;
using System.ComponentModel;
using GimaSoft.Business.GINA;

namespace GimaSoft.GINA
{
	// Token: 0x02000024 RID: 36
	public class TriggerViewModel : TriggerAndGroupViewModel
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x0000CF18 File Offset: 0x0000B118
		public TriggerViewModel(TriggerGroupViewModel groupVM, Trigger trigger)
		{
			this.GroupVM = groupVM;
			this.Trigger = trigger;
			if (this.GroupVM.MainVM != null)
			{
				this.GroupVM.MainVM.PropertyChanged += delegate(object o, PropertyChangedEventArgs e)
				{
					if (e.PropertyName == "SelectedCharacter")
					{
						base.RaisePropertyChanged("CheckboxVisible");
					}
				};
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000CF69 File Offset: 0x0000B169
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x0000CF76 File Offset: 0x0000B176
		public Trigger Trigger
		{
			get
			{
				return base.Get<Trigger>("Trigger");
			}
			set
			{
				base.Set("Trigger", value);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0000CF84 File Offset: 0x0000B184
		// (set) Token: 0x060003BB RID: 955 RVA: 0x0000CF91 File Offset: 0x0000B191
		public TriggerGroupViewModel GroupVM
		{
			get
			{
				return base.Get<TriggerGroupViewModel>("GroupVM");
			}
			set
			{
				base.Set("GroupVM", value);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0000CF9F File Offset: 0x0000B19F
		public override string DisplayName
		{
			get
			{
				return this.Trigger.Name;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000CFAC File Offset: 0x0000B1AC
		public override bool IsTriggerView
		{
			get
			{
				return true;
			}
		}
	}
}
