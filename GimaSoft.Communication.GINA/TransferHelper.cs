using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using GimaSoft.Business.GINA;
using GimaSoft.Communication.GINA.PackageService;
using GimaSoft.Service.GINA;
using WPFShared;

namespace GimaSoft.Communication.GINA
{
	// Token: 0x02000019 RID: 25
	public class TransferHelper : BindableObject
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000065 RID: 101 RVA: 0x000022A4 File Offset: 0x000004A4
		// (remove) Token: 0x06000066 RID: 102 RVA: 0x000022DC File Offset: 0x000004DC
		public event ErrorReportUploadedHandler ErrorReportUploaded = delegate(ErrorReportUploadedEventArgs e)
		{
		};

		// Token: 0x06000067 RID: 103 RVA: 0x00002311 File Offset: 0x00000511
		protected virtual void OnErrorReportUploaded(bool success)
		{
			Configuration.LogDebug("Fired OnErrorReportUploaded");
			this.ErrorReportUploaded(new ErrorReportUploadedEventArgs(success));
			Configuration.LogDebug("Completed OnErrorReportUploaded");
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000068 RID: 104 RVA: 0x00002338 File Offset: 0x00000538
		// (remove) Token: 0x06000069 RID: 105 RVA: 0x00002370 File Offset: 0x00000570
		public event ConnectionEstablishedHandler ConnectionEstablished;

		// Token: 0x0600006A RID: 106 RVA: 0x000023A5 File Offset: 0x000005A5
		protected virtual void OnConnectionEstablished(ConnectionEstablishedEventArgs e)
		{
			Configuration.LogDebug("Fired OnConnectionEstablished");
			if (this.ConnectionEstablished != null)
			{
				this.ConnectionEstablished(this, e);
			}
			Configuration.LogDebug("Completed OnConnectionEstablished");
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600006B RID: 107 RVA: 0x000023D0 File Offset: 0x000005D0
		// (remove) Token: 0x0600006C RID: 108 RVA: 0x00002408 File Offset: 0x00000608
		public event ConnectionErrorHandler ConnectionError = delegate(object o, ConnectionErrorEventArgs e)
		{
		};

		// Token: 0x0600006D RID: 109 RVA: 0x0000243D File Offset: 0x0000063D
		protected virtual void OnConnectionError(ConnectionErrorEventArgs e)
		{
			Configuration.LogDebug("Fired OnConnectionError");
			this.ConnectionError(this, e);
			Configuration.LogDebug("Completed OnConnectionError");
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600006E RID: 110 RVA: 0x00002460 File Offset: 0x00000660
		// (remove) Token: 0x0600006F RID: 111 RVA: 0x00002498 File Offset: 0x00000698
		public event ConnectionVersionFailedHandler ConnectionVersionFailed;

		// Token: 0x06000070 RID: 112 RVA: 0x000024CD File Offset: 0x000006CD
		protected virtual void OnConnectionVersionFailed(ConnectionVersionFailedEventArgs e)
		{
			Configuration.LogDebug("Fired OnConnectionVersionFailed");
			if (this.ConnectionVersionFailed != null)
			{
				this.ConnectionVersionFailed(this, e);
			}
			Configuration.LogDebug("Completed OnConnectionVersionFailed");
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000071 RID: 113 RVA: 0x000024F8 File Offset: 0x000006F8
		// (remove) Token: 0x06000072 RID: 114 RVA: 0x00002530 File Offset: 0x00000730
		public event ConnectionFailedHandler ConnectionFailed;

		// Token: 0x06000073 RID: 115 RVA: 0x00002565 File Offset: 0x00000765
		protected virtual void OnConnectionFailed(ConnectionFailedEventArgs e)
		{
			Configuration.LogDebug("Fired OnConnectionFailed");
			if (this.ConnectionFailed != null)
			{
				this.ConnectionFailed(this, e);
			}
			Configuration.LogDebug("Completed OnConnectionFailed");
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000074 RID: 116 RVA: 0x00002590 File Offset: 0x00000790
		// (remove) Token: 0x06000075 RID: 117 RVA: 0x000025C8 File Offset: 0x000007C8
		public event ChunkTransferredHandler ChunkTransferred;

		// Token: 0x06000076 RID: 118 RVA: 0x000025FD File Offset: 0x000007FD
		protected virtual void OnChunkTransferred(ChunkTransferredEventArgs e)
		{
			Configuration.LogDebug("Fired OnChunkTransferred");
			if (this.ChunkTransferred != null)
			{
				this.ChunkTransferred(this, e);
			}
			Configuration.LogDebug("Completed OnChunkTransferred");
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000077 RID: 119 RVA: 0x00002628 File Offset: 0x00000828
		// (remove) Token: 0x06000078 RID: 120 RVA: 0x00002660 File Offset: 0x00000860
		public event ChunkFailedHandler ChunkFailed;

		// Token: 0x06000079 RID: 121 RVA: 0x00002695 File Offset: 0x00000895
		protected virtual void OnChunkFailed(ChunkFailedEventArgs e)
		{
			Configuration.LogDebug("Fired OnChunkFailed");
			if (this.ChunkFailed != null)
			{
				this.ChunkFailed(this, e);
			}
			Configuration.LogDebug("Completed OnChunkFailed");
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600007A RID: 122 RVA: 0x000026C0 File Offset: 0x000008C0
		// (remove) Token: 0x0600007B RID: 123 RVA: 0x000026F8 File Offset: 0x000008F8
		public event TransferCompletedHandler UploadCompleted;

		// Token: 0x0600007C RID: 124 RVA: 0x0000272D File Offset: 0x0000092D
		protected virtual void OnUploadCompleted(TransferCompletedEventArgs e)
		{
			Configuration.LogDebug("Fired OnUploadCompleted");
			if (this.UploadCompleted != null)
			{
				this.UploadCompleted(this, e);
			}
			Configuration.LogDebug("Completed OnUploadCompleted");
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600007D RID: 125 RVA: 0x00002758 File Offset: 0x00000958
		// (remove) Token: 0x0600007E RID: 126 RVA: 0x00002790 File Offset: 0x00000990
		public event TransferCompletedHandler DownloadCompleted;

		// Token: 0x0600007F RID: 127 RVA: 0x000027C5 File Offset: 0x000009C5
		protected virtual void OnDownloadCompleted(TransferCompletedEventArgs e)
		{
			Configuration.LogDebug("Fired OnDownloadCompleted");
			if (this.DownloadCompleted != null)
			{
				this.DownloadCompleted(this, e);
			}
			Configuration.LogDebug("Completed OnDownloadCompleted");
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000080 RID: 128 RVA: 0x000027F0 File Offset: 0x000009F0
		// (remove) Token: 0x06000081 RID: 129 RVA: 0x00002828 File Offset: 0x00000A28
		public event RepositoryLoadedHandler RepositoryLoaded;

		// Token: 0x06000082 RID: 130 RVA: 0x0000285D File Offset: 0x00000A5D
		protected virtual void OnRepositoryLoaded(RepositoryLoadedEventArgs e)
		{
			Configuration.LogDebug("Fired OnRepositoryLoaded");
			if (this.RepositoryLoaded != null)
			{
				this.RepositoryLoaded(this, e);
			}
			Configuration.LogDebug("Completed OnRepositoryLoaded");
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000083 RID: 131 RVA: 0x00002888 File Offset: 0x00000A88
		// (remove) Token: 0x06000084 RID: 132 RVA: 0x000028C0 File Offset: 0x00000AC0
		public event RepositorySubCategoriesLoadedHandler RepositorySubCategoriesLoaded;

		// Token: 0x06000085 RID: 133 RVA: 0x000028F5 File Offset: 0x00000AF5
		protected virtual void OnRepositorySubCategoriesLoaded(RepositorySubCategoriesLoadedEventArgs e)
		{
			Configuration.LogDebug("Fired OnRepositorySubCategoriesLoaded");
			if (this.RepositorySubCategoriesLoaded != null)
			{
				this.RepositorySubCategoriesLoaded(this, e);
			}
			Configuration.LogDebug("Completed OnRepositorySubCategoriesLoaded");
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002920 File Offset: 0x00000B20
		private PackageServiceClient Client
		{
			get
			{
				return this._Client;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002928 File Offset: 0x00000B28
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00002935 File Offset: 0x00000B35
		public List<RepositoryEntry> RepositoryEntries
		{
			get
			{
				return base.Get<List<RepositoryEntry>>("RepositoryEntries");
			}
			set
			{
				base.Set("RepositoryEntries", value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00002943 File Offset: 0x00000B43
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000294B File Offset: 0x00000B4B
		public Package DownloadedPackage { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002954 File Offset: 0x00000B54
		public IEnumerable<RepositorySubCategory> RepositorySubCategories
		{
			get
			{
				if (this._RepositorySubCategories == null)
				{
					this._RepositorySubCategories = new List<RepositorySubCategory>();
					this.GetRepositorySubCategories();
				}
				return this._RepositorySubCategories;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002978 File Offset: 0x00000B78
		public void GetRepositorySubCategories()
		{
			Configuration.LogDebug("Retrieving repository sub-categories");
			PackageServiceClient packageServiceClient = new PackageServiceClient("BasicHttpBinding_IPackageService", Configuration.Current.ShareServiceUri);
			this._RepositorySubCategories = packageServiceClient.GetRepositorySubCategories();
			Configuration.LogDebug("Closing connection");
			packageServiceClient.Close();
			this.OnRepositorySubCategoriesLoaded(new RepositorySubCategoriesLoadedEventArgs());
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002A84 File Offset: 0x00000C84
		public void ReportError(ErrorReport error)
		{
			if (this._Client != null)
			{
				this.Disconnect();
			}
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object o, DoWorkEventArgs e)
			{
				this._Client = new PackageServiceClient("BasicHttpBinding_IPackageService", Configuration.Current.ShareServiceUri);
				try
				{
					this._Client.Open();
					this.OnConnectionEstablished(new ConnectionEstablishedEventArgs(null));
					this._Client.UploadErrorInfo(error.GetBytes());
					this.OnErrorReportUploaded(true);
				}
				catch
				{
					this.OnErrorReportUploaded(false);
				}
				finally
				{
					this.Disconnect();
				}
			};
			backgroundWorker.RunWorkerAsync();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002AD4 File Offset: 0x00000CD4
		private void Connect()
		{
			if (this._Client != null)
			{
				this.Disconnect();
			}
			this._Client = new PackageServiceClient("BasicHttpBinding_IPackageService", Configuration.Current.ShareServiceUri);
			try
			{
				BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
				basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
				if (Configuration.Current.ShareServiceUri.ToLower().StartsWith("https:"))
				{
					basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
					basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
				}
				this._Client.Endpoint.Binding = basicHttpBinding;
				Configuration.LogDebug("Opening connection");
				this._Client.Open();
				Configuration.LogDebug("Retrieving connection info");
				ConnectionInfo connectionInfo = this._Client.GetConnectionInfo(Configuration.Current.RepositoryLastViewedAtStartup);
				Configuration.LogDebug("Validation version");
				if (new Version(Application.ProductVersion) < connectionInfo.MinimumClientVersion)
				{
					this.Disconnect();
					this.ConnectionVersionFailed(this, new ConnectionVersionFailedEventArgs(connectionInfo.MinimumClientVersion));
				}
				else
				{
					this.OnConnectionEstablished(new ConnectionEstablishedEventArgs(connectionInfo));
				}
			}
			catch
			{
				this.Disconnect();
				this.OnConnectionFailed(new ConnectionFailedEventArgs());
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002C0C File Offset: 0x00000E0C
		private void Disconnect()
		{
			try
			{
				if (this._Client != null && this._Client.State != CommunicationState.Closed)
				{
					Configuration.LogDebug("Closing connection");
					this._Client.Close();
					Configuration.LogDebug("Closed connection");
				}
				this._Client = null;
			}
			catch
			{
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public void UploadPackage(byte[] packageData)
		{
			this.UploadData = packageData;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object o, DoWorkEventArgs e)
			{
				this.Connect();
				if (this._Client == null)
				{
					return;
				}
				UploadResult uploadResult = this._Client.UploadPackageInit(this.UploadData.Length, false, null, null, 0, null, null);
				if (uploadResult.Success)
				{
					this.OnChunkTransferred(new ChunkTransferredEventArgs(uploadResult.SessionId, 0, this.UploadData.Length));
					int num = 0;
					while (uploadResult.CumulativeBytesUploaded < this.UploadData.Length)
					{
						uploadResult = this._Client.UploadPackageChunk(uploadResult.SessionId, this.UploadData.Skip(num * uploadResult.ChunkSize).Take(uploadResult.ChunkSize).ToArray<byte>());
						if (!uploadResult.Success)
						{
							this.OnChunkFailed(new ChunkFailedEventArgs(uploadResult.SessionId, uploadResult.Error, this.UploadData.Length));
							this.Disconnect();
							return;
						}
						this.OnChunkTransferred(new ChunkTransferredEventArgs(uploadResult.SessionId, uploadResult.CumulativeBytesUploaded, this.UploadData.Length));
						num++;
					}
					this.Disconnect();
					this.OnUploadCompleted(new TransferCompletedEventArgs(uploadResult.SessionId));
					return;
				}
				this.OnChunkFailed(new ChunkFailedEventArgs(Guid.Empty, uploadResult.Error, this.UploadData.Length));
				this.Disconnect();
			};
			backgroundWorker.RunWorkerAsync(null);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003284 File Offset: 0x00001484
		public void Download(ShareDetectedEventArgs args)
		{
			this.DownloadedPackage = null;
			List<Package> packages = new List<Package>();
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object o, DoWorkEventArgs e)
			{
				this.Connect();
				if (this._Client == null)
				{
					return;
				}
				int num = 0;
				try
				{
					string text = "Initalizing download for {0}";
					object[] array = new object[1];
					array[0] = string.Join(", ", args.EffectiveSessionIds.Select((Guid n) => n.ToString()).ToArray<string>());
					Configuration.LogDebug(text, array);
					DownloadResult downloadResult = this._Client.GetTotalDownloadSize(args.EffectiveSessionIds.ToArray<Guid>());
					if (!downloadResult.Success)
					{
						this.OnChunkFailed(new ChunkFailedEventArgs(downloadResult.SessionId, downloadResult.Error, 0));
						this.Disconnect();
					}
					else
					{
						int value = downloadResult.TotalSize.Value;
						Configuration.LogDebug("Initialized, expecting {0} bytes", new object[] { value });
						foreach (Guid guid in args.EffectiveSessionIds)
						{
							MemoryStream memoryStream = new MemoryStream();
							int num2 = 0;
							long length;
							int? totalSize;
							do
							{
								Configuration.LogDebug("Downloading chunk {0} from {1}", new object[] { num2, guid });
								downloadResult = this._Client.DownloadPackageChunk(guid, num2++);
								if (!downloadResult.Success)
								{
									goto IL_0228;
								}
								Configuration.LogDebug("Read {0} bytes", new object[] { downloadResult.ChunkData.Length });
								memoryStream.Write(downloadResult.ChunkData, 0, downloadResult.ChunkData.Length);
								Configuration.LogDebug("Current progress: {0} / {1}", new object[] { memoryStream.Length, downloadResult.TotalSize });
								num += downloadResult.ChunkData.Length;
								Configuration.LogDebug("Overall progress: {0} / {1}", new object[] { num, value });
								this.OnChunkTransferred(new ChunkTransferredEventArgs(downloadResult.SessionId, num, value));
								if (downloadResult == null || !downloadResult.Success)
								{
									break;
								}
								length = memoryStream.Length;
								totalSize = downloadResult.TotalSize;
							}
							while (length < (long)totalSize.GetValueOrDefault() && totalSize != null);
							Configuration.LogDebug("Completed downloading {0}", new object[] { guid });
							if (downloadResult != null && downloadResult.Success)
							{
								memoryStream.Position = 0L;
								Configuration.LogDebug("Creating package");
								packages.Add(Package.OpenPackage(memoryStream.ToArray()));
								Configuration.LogDebug("Disposing downloadData");
								memoryStream.Dispose();
								continue;
							}
							continue;
							IL_0228:
							this.OnChunkFailed(new ChunkFailedEventArgs(guid, downloadResult.Error, value));
							Configuration.LogDebug("Disposing packages");
							foreach (Package package in packages)
							{
								package.Dispose();
							}
							Configuration.LogDebug("Disposing downloadData");
							memoryStream.Dispose();
							this.Disconnect();
							return;
						}
						this.Disconnect();
						Package package2 = null;
						Configuration.LogDebug("Merged packages");
						foreach (Package package3 in packages)
						{
							if (package2 == null)
							{
								package2 = package3;
							}
							else
							{
								package2.Merge(package3);
							}
						}
						Configuration.LogDebug("Setting downloaded package");
						this.DownloadedPackage = package2;
						this.OnDownloadCompleted(new TransferCompletedEventArgs(args.SessionId));
					}
				}
				catch (Exception ex)
				{
					Configuration.LogDebug("Connection error: {0}", new object[] { ex.Message });
					this.OnConnectionError(new ConnectionErrorEventArgs(ex.Message));
				}
			};
			backgroundWorker.RunWorkerAsync(null);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000339C File Offset: 0x0000159C
		public void GetRepositoryEntries()
		{
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object o, DoWorkEventArgs e)
			{
				this.Connect();
				if (this._Client == null)
				{
					return;
				}
				this.RepositoryEntries = (from oi in this.Client.GetRepositoryList()
					orderby oi.Name, oi.Category, oi.SubCategory
					select oi).ToList<RepositoryEntry>();
				this.Disconnect();
				this.OnRepositoryLoaded(new RepositoryLoadedEventArgs());
			};
			backgroundWorker.RunWorkerAsync(null);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000033C8 File Offset: 0x000015C8
		public override void Dispose(bool disposing)
		{
			if (!this._Disposed)
			{
				this._Disposed = true;
				if (disposing)
				{
					try
					{
						this.Disconnect();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x04000018 RID: 24
		private PackageServiceClient _Client;

		// Token: 0x04000019 RID: 25
		private byte[] UploadData;

		// Token: 0x0400001A RID: 26
		private IEnumerable<RepositorySubCategory> _RepositorySubCategories;
	}
}
