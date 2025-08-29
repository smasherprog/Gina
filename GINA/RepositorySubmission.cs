using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GimaSoft.Communication.GINA;
using GimaSoft.Service.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000015 RID: 21
	public class RepositorySubmission : BindableControl
	{
		// Token: 0x060002AE RID: 686 RVA: 0x000090FA File Offset: 0x000072FA
		static RepositorySubmission()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RepositorySubmission), new FrameworkPropertyMetadata(typeof(RepositorySubmission)));
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000911F File Offset: 0x0000731F
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000912C File Offset: 0x0000732C
		public string Username
		{
			get
			{
				return base.Get<string>("Username");
			}
			set
			{
				base.Set("Username", value);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000913A File Offset: 0x0000733A
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x00009147 File Offset: 0x00007347
		public string Password
		{
			get
			{
				return base.Get<string>("Password");
			}
			set
			{
				base.Set("Password", value);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00009155 File Offset: 0x00007355
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00009162 File Offset: 0x00007362
		public int? SubCategoryId
		{
			get
			{
				return base.Get<int?>("SubCategoryId");
			}
			set
			{
				base.Set("SubCategoryId", value);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00009175 File Offset: 0x00007375
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00009182 File Offset: 0x00007382
		public string SubmissionName
		{
			get
			{
				return base.Get<string>("SubmissionName");
			}
			set
			{
				base.Set("SubmissionName", value);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00009190 File Offset: 0x00007390
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000919D File Offset: 0x0000739D
		public string Comment
		{
			get
			{
				return base.Get<string>("Comment");
			}
			set
			{
				base.Set("Comment", value);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x000091AB File Offset: 0x000073AB
		// (set) Token: 0x060002BA RID: 698 RVA: 0x000091B8 File Offset: 0x000073B8
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060002BB RID: 699 RVA: 0x000091C6 File Offset: 0x000073C6
		// (set) Token: 0x060002BC RID: 700 RVA: 0x000091D3 File Offset: 0x000073D3
		public string SelectedCategory
		{
			get
			{
				return base.Get<string>("SelectedCategory");
			}
			set
			{
				base.Set("SelectedCategory", value);
				base.RaisePropertyChanged("SubCategories");
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060002BD RID: 701 RVA: 0x000091F8 File Offset: 0x000073F8
		public IEnumerable<string> Categories
		{
			get
			{
				if (this._Transfer == null || this._Transfer.RepositorySubCategories == null)
				{
					return null;
				}
				return from o in this._Transfer.RepositorySubCategories.Select((RepositorySubCategory o) => o.CategoryName).Distinct<string>().Union(new string[] { "" })
					orderby o
					select o;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002BE RID: 702 RVA: 0x000092A4 File Offset: 0x000074A4
		public IEnumerable<string> SubCategories
		{
			get
			{
				if (this._Transfer == null || this._Transfer.RepositorySubCategories == null)
				{
					return null;
				}
				return from o in (from o in this._Transfer.RepositorySubCategories
						where o.CategoryName == this.SelectedCategory
						select o.SubCategoryName).Distinct<string>().Union(new string[] { "" })
					orderby o
					select o;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002BF RID: 703 RVA: 0x000093A0 File Offset: 0x000075A0
		public GenericCommand VisibilityChangedCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (base.Visibility != Visibility.Visible)
						{
							return;
						}
						this.ErrorMessage = null;
						if (this._Transfer == null)
						{
							this._Transfer = new TransferHelper();
							this._Transfer.RepositorySubCategoriesLoaded += delegate(object o, RepositorySubCategoriesLoadedEventArgs e)
							{
								base.RaisePropertyChanged("Categories");
							};
						}
					}
				};
			}
		}

		// Token: 0x04000060 RID: 96
		private TransferHelper _Transfer;
	}
}
