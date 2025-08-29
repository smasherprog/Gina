using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using GimaSoft.Service.GINA;

namespace GimaSoft.Communication.GINA.PackageService
{
	// Token: 0x02000018 RID: 24
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[DebuggerStepThrough]
	public class PackageServiceClient : ClientBase<IPackageService>, IPackageService
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000021DF File Offset: 0x000003DF
		public PackageServiceClient()
		{
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000021E7 File Offset: 0x000003E7
		public PackageServiceClient(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000021F0 File Offset: 0x000003F0
		public PackageServiceClient(string endpointConfigurationName, string remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000021FA File Offset: 0x000003FA
		public PackageServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002204 File Offset: 0x00000404
		public PackageServiceClient(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000220E File Offset: 0x0000040E
		public UploadResult UploadPackageInit(int totalSize, bool isRepositorySubmission, string username, string password, int subCategoryId, string name, string comment)
		{
			return base.Channel.UploadPackageInit(totalSize, isRepositorySubmission, username, password, subCategoryId, name, comment);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002226 File Offset: 0x00000426
		public UploadResult UploadPackageChunk(Guid sessionId, byte[] packageData)
		{
			return base.Channel.UploadPackageChunk(sessionId, packageData);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002235 File Offset: 0x00000435
		public DownloadResult GetTotalDownloadSize(Guid[] sessionIds)
		{
			return base.Channel.GetTotalDownloadSize(sessionIds);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002243 File Offset: 0x00000443
		public DownloadResult DownloadPackageChunk(Guid sessionId, int chunkNumber)
		{
			return base.Channel.DownloadPackageChunk(sessionId, chunkNumber);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002252 File Offset: 0x00000452
		public ConnectionInfo GetConnectionInfo(DateTimeOffset? lastRepositoryDate)
		{
			return base.Channel.GetConnectionInfo(lastRepositoryDate);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002260 File Offset: 0x00000460
		public int GetMaxSize()
		{
			return base.Channel.GetMaxSize();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000226D File Offset: 0x0000046D
		public int GetMaxChunkSize()
		{
			return base.Channel.GetMaxChunkSize();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000227A File Offset: 0x0000047A
		public RepositoryEntry[] GetRepositoryList()
		{
			return base.Channel.GetRepositoryList();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00002287 File Offset: 0x00000487
		public RepositorySubCategory[] GetRepositorySubCategories()
		{
			return base.Channel.GetRepositorySubCategories();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002294 File Offset: 0x00000494
		public void UploadErrorInfo(byte[] errorInfo)
		{
			base.Channel.UploadErrorInfo(errorInfo);
		}
	}
}
