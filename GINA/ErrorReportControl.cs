using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using GimaSoft.Business.GINA;
using GimaSoft.Communication.GINA;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000013 RID: 19
	public class ErrorReportControl : BindableControl
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00008EA0 File Offset: 0x000070A0
		static ErrorReportControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorReportControl), new FrameworkPropertyMetadata(typeof(ErrorReportControl)));
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008EF8 File Offset: 0x000070F8
		public ErrorReportControl()
		{
			this._Helper = new TransferHelper();
			this._Helper.ErrorReportUploaded += delegate(ErrorReportUploadedEventArgs e)
			{
				base.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
				{
					this.Status = ErrorReportControl.Statuses.Completed;
					base.Visibility = Visibility.Collapsed;
					this.ClearError();
				}));
			};
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00008F34 File Offset: 0x00007134
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00008F41 File Offset: 0x00007141
		public ErrorReport ErrorData
		{
			get
			{
				return base.Get<ErrorReport>("ErrorData");
			}
			set
			{
				base.Set("ErrorData", value);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00008F4F File Offset: 0x0000714F
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00008F5C File Offset: 0x0000715C
		public bool ShowErrorDetail
		{
			get
			{
				return base.Get<bool>("ShowErrorDetail");
			}
			set
			{
				base.Set("ShowErrorDetail", value);
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00008F6F File Offset: 0x0000716F
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x00008F7C File Offset: 0x0000717C
		public ErrorReportControl.Statuses Status
		{
			get
			{
				return base.Get<ErrorReportControl.Statuses>("Status");
			}
			set
			{
				base.Set("Status", value);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00008F90 File Offset: 0x00007190
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == UIElement.VisibilityProperty)
			{
				if (base.Visibility == Visibility.Visible && this.ErrorData == null)
				{
					try
					{
						if (File.Exists(Configuration.CrashLogFilePre))
						{
							this.ErrorData = new ErrorReport();
							this.ErrorData.ErrorData = File.ReadAllText(Configuration.CrashLogFilePre);
						}
					}
					catch
					{
					}
					if (this.ErrorData == null)
					{
						base.Visibility = Visibility.Collapsed;
						return;
					}
				}
				else
				{
					this.ErrorData = null;
				}
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000901C File Offset: 0x0000721C
		public void ClearError()
		{
			try
			{
				if (File.Exists(Configuration.CrashLogFilePost))
				{
					File.Delete(Configuration.CrashLogFilePost);
				}
				if (File.Exists(Configuration.CrashLogFilePre))
				{
					File.Move(Configuration.CrashLogFilePre, Configuration.CrashLogFilePost);
				}
			}
			catch
			{
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00009083 File Offset: 0x00007283
		public GenericCommand ShowDetailCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.ShowErrorDetail = bool.Parse(p as string);
				});
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x000090C5 File Offset: 0x000072C5
		public GenericCommand SendErrorCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					this.Status = ErrorReportControl.Statuses.Uploading;
					this.ErrorData.Version = global::System.Windows.Forms.Application.ProductVersion.ToString();
					this._Helper.ReportError(this.ErrorData);
				});
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x000090E7 File Offset: 0x000072E7
		public GenericCommand CloseCommand
		{
			get
			{
				return new GenericCommand(delegate(object p)
				{
					base.Visibility = Visibility.Collapsed;
					this.ClearError();
				});
			}
		}

		// Token: 0x0400005C RID: 92
		private TransferHelper _Helper;

		// Token: 0x02000014 RID: 20
		public enum Statuses
		{
			// Token: 0x0400005E RID: 94
			Uploading = 1,
			// Token: 0x0400005F RID: 95
			Completed
		}
	}
}
