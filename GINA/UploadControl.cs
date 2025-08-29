using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GimaSoft.Business.GINA;
using GimaSoft.Communication.GINA;
using GimaSoft.Service.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x0200000E RID: 14
	public class UploadControl : BindableControl
	{
		// Token: 0x06000236 RID: 566 RVA: 0x000080D8 File Offset: 0x000062D8
		static UploadControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(UploadControl), new FrameworkPropertyMetadata(typeof(UploadControl)));
			BindableControl.SetDependentProperties(typeof(UploadControl));
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00008184 File Offset: 0x00006384
		public static void RegisterDependentProperties(Type type)
		{
			BindableControl.RegisterDependentProperty(type, "ShareToken", "SessionId", null);
			BindableControl.RegisterDependentProperty(type, "SubCategories", "SelectedCategory", null);
			BindableControl.RegisterDependentProperty(type, "CanUpload", "SelectedSubCategory", null);
			BindableControl.RegisterDependentProperty(type, "CanUpload", "SubmissionName", null);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000081E4 File Offset: 0x000063E4
		public UploadControl()
		{
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(UIElement.VisibilityProperty, typeof(UploadControl));
			dependencyPropertyDescriptor.AddValueChanged(this, delegate(object o, EventArgs e)
			{
				(o as UploadControl).StartUpload();
			});
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00008230 File Offset: 0x00006430
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00008242 File Offset: 0x00006442
		public IEnumerable<ITriggerLibraryEntry> SelectedEntries
		{
			get
			{
				return (IEnumerable<ITriggerLibraryEntry>)base.GetValue(UploadControl.SelectedEntriesProperty);
			}
			set
			{
				base.SetValue(UploadControl.SelectedEntriesProperty, value);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00008250 File Offset: 0x00006450
		// (set) Token: 0x0600023C RID: 572 RVA: 0x00008262 File Offset: 0x00006462
		public bool IsRepositoryUpload
		{
			get
			{
				return (bool)base.GetValue(UploadControl.IsRepositoryUploadProperty);
			}
			set
			{
				base.SetValue(UploadControl.IsRepositoryUploadProperty, value);
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00008275 File Offset: 0x00006475
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00008287 File Offset: 0x00006487
		public bool AutoClipboard
		{
			get
			{
				return (bool)base.GetValue(UploadControl.AutoClipboardProperty);
			}
			set
			{
				base.SetValue(UploadControl.AutoClipboardProperty, value);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000829A File Offset: 0x0000649A
		// (set) Token: 0x06000240 RID: 576 RVA: 0x000082A7 File Offset: 0x000064A7
		public Guid SessionId
		{
			get
			{
				return base.Get<Guid>("SessionId");
			}
			set
			{
				base.Set("SessionId", value);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000082BA File Offset: 0x000064BA
		public string ShareToken
		{
			get
			{
				return Package.GetPackagePasteText(this.SessionId);
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000082C7 File Offset: 0x000064C7
		// (set) Token: 0x06000243 RID: 579 RVA: 0x000082D4 File Offset: 0x000064D4
		public int TotalBytes
		{
			get
			{
				return base.Get<int>("TotalBytes");
			}
			set
			{
				base.Set("TotalBytes", value);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000082E7 File Offset: 0x000064E7
		// (set) Token: 0x06000245 RID: 581 RVA: 0x000082F4 File Offset: 0x000064F4
		public int UploadedBytes
		{
			get
			{
				return base.Get<int>("UploadedBytes");
			}
			set
			{
				base.Set("UploadedBytes", value);
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00008307 File Offset: 0x00006507
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00008314 File Offset: 0x00006514
		public int Status
		{
			get
			{
				return base.Get<int>("Status");
			}
			set
			{
				base.Set("Status", value);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00008327 File Offset: 0x00006527
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00008334 File Offset: 0x00006534
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

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00008342 File Offset: 0x00006542
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000834F File Offset: 0x0000654F
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000835D File Offset: 0x0000655D
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000836A File Offset: 0x0000656A
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00008378 File Offset: 0x00006578
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00008385 File Offset: 0x00006585
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00008398 File Offset: 0x00006598
		// (set) Token: 0x06000251 RID: 593 RVA: 0x000083A5 File Offset: 0x000065A5
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

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000252 RID: 594 RVA: 0x000083B3 File Offset: 0x000065B3
		// (set) Token: 0x06000253 RID: 595 RVA: 0x000083C0 File Offset: 0x000065C0
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

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000254 RID: 596 RVA: 0x000083CE File Offset: 0x000065CE
		// (set) Token: 0x06000255 RID: 597 RVA: 0x000083DB File Offset: 0x000065DB
		public string SelectedCategory
		{
			get
			{
				return base.Get<string>("SelectedCategory");
			}
			set
			{
				base.Set("SelectedCategory", value);
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000256 RID: 598 RVA: 0x000083E9 File Offset: 0x000065E9
		// (set) Token: 0x06000257 RID: 599 RVA: 0x000083F6 File Offset: 0x000065F6
		public RepositorySubCategory SelectedSubCategory
		{
			get
			{
				return base.Get<RepositorySubCategory>("SelectedSubCategory");
			}
			set
			{
				base.Set("SelectedSubCategory", value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00008404 File Offset: 0x00006604
		public bool CanUpload
		{
			get
			{
				return this.SelectedSubCategory != null && !string.IsNullOrWhiteSpace(this.SubmissionName);
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000842C File Offset: 0x0000662C
		public IEnumerable<string> Categories
		{
			get
			{
				if (this._Transfer == null || this._Transfer.RepositorySubCategories == null)
				{
					return null;
				}
				return from o in this._Transfer.RepositorySubCategories.Select((RepositorySubCategory o) => o.CategoryName).Distinct<string>()
					orderby o
					select o;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600025A RID: 602 RVA: 0x000084C0 File Offset: 0x000066C0
		public IEnumerable<RepositorySubCategory> SubCategories
		{
			get
			{
				if (this._Transfer == null || this._Transfer.RepositorySubCategories == null)
				{
					return null;
				}
				return from o in this._Transfer.RepositorySubCategories
					where o.CategoryName == this.SelectedCategory
					orderby o.SubCategoryName
					select o;
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000085C4 File Offset: 0x000067C4
		public void StartUpload()
		{
			if (base.Visibility != Visibility.Visible)
			{
				return;
			}
			if (this._Executing)
			{
				return;
			}
			this._Executing = true;
			this.ErrorMessage = null;
			if (this.SelectedEntries == null || !this.SelectedEntries.Any<ITriggerLibraryEntry>())
			{
				return;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
			{
				if (this._Transfer == null)
				{
					this._Transfer = new TransferHelper();
					this.SetEvents();
				}
				bool isRepositoryUpload = this.IsRepositoryUpload;
				using (Package package = Package.CreatePackage(this.SelectedEntries))
				{
					this._PackageBytes = package.GetBytes();
				}
				this.UploadedBytes = 0;
				this.TotalBytes = this._PackageBytes.Length;
				if (this.IsRepositoryUpload)
				{
					this.Status = 5;
					return;
				}
				this.Status = 1;
				this._Transfer.UploadPackage(this._PackageBytes);
			}));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000871C File Offset: 0x0000691C
		private void SetEvents()
		{
			this._Transfer.ConnectionEstablished += delegate(object o, ConnectionEstablishedEventArgs e)
			{
				this.Status = 2;
			};
			this._Transfer.ConnectionVersionFailed += delegate(object o, ConnectionVersionFailedEventArgs e)
			{
				this.Status = 7;
				this.ErrorMessage = e.VersionMessage;
			};
			this._Transfer.ConnectionFailed += delegate(object o, ConnectionFailedEventArgs e)
			{
				this.ErrorMessage = "Unable to connect to Sharing Service";
				this._Executing = false;
			};
			this._Transfer.ChunkTransferred += delegate(object o, ChunkTransferredEventArgs e)
			{
				this.UploadedBytes = e.TotalTransferred;
			};
			this._Transfer.ChunkFailed += delegate(object o, ChunkFailedEventArgs e)
			{
				this._Executing = false;
				if (e.Error == ShareErrors.TooLarge)
				{
					this.ErrorMessage = "Selected package is too large to upload.";
					return;
				}
				this.ErrorMessage = "Error occurred while uploading package";
			};
			this._Transfer.UploadCompleted += delegate(object o, TransferCompletedEventArgs e)
			{
				this.Status = 3;
				this.SessionId = e.SessionId;
				Package.AddCreatedShare(e.SessionId);
				this._Executing = false;
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(delegate
				{
					if (this.AutoClipboard)
					{
						this.CopyToClipboardCommand.Execute(null);
						this.CloseCommand.Execute(null);
					}
				}));
			};
			this._Transfer.RepositorySubCategoriesLoaded += delegate(object o, RepositorySubCategoriesLoadedEventArgs e)
			{
				base.RaisePropertyChanged("Categories");
				base.RaisePropertyChanged("SubCategories");
			};
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600025D RID: 605 RVA: 0x000087F4 File Offset: 0x000069F4
		public GenericCommand CopyToClipboardCommand
		{
			get
			{
				return new GenericCommand
				{
					Execute = delegate(object p)
					{
						if (!string.IsNullOrWhiteSpace(this.ShareToken))
						{
							(Application.Current as App).Data.CopyTextToClipboard(this.ShareToken);
						}
					}
				};
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00008824 File Offset: 0x00006A24
		public GenericCommand CloseCommand
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

		// Token: 0x04000046 RID: 70
		private const int Connecting = 1;

		// Token: 0x04000047 RID: 71
		private const int Transferring = 2;

		// Token: 0x04000048 RID: 72
		private const int Completed = 3;

		// Token: 0x04000049 RID: 73
		private const int Errored = 4;

		// Token: 0x0400004A RID: 74
		private const int GatherRepositoryInfo = 5;

		// Token: 0x0400004B RID: 75
		private const int GatherCredentials = 6;

		// Token: 0x0400004C RID: 76
		private const int InvalidVersion = 7;

		// Token: 0x0400004D RID: 77
		public static readonly DependencyProperty SelectedEntriesProperty = DependencyProperty.Register("SelectedEntries", typeof(IEnumerable<ITriggerLibraryEntry>), typeof(UploadControl), null);

		// Token: 0x0400004E RID: 78
		public static readonly DependencyProperty IsRepositoryUploadProperty = DependencyProperty.Register("IsRepositoryUpload", typeof(bool), typeof(UploadControl), null);

		// Token: 0x0400004F RID: 79
		public static readonly DependencyProperty AutoClipboardProperty = DependencyProperty.Register("AutoClipboard", typeof(bool), typeof(UploadControl), null);

		// Token: 0x04000050 RID: 80
		private TransferHelper _Transfer;

		// Token: 0x04000051 RID: 81
		private bool _Executing;

		// Token: 0x04000052 RID: 82
		private byte[] _PackageBytes;
	}
}
