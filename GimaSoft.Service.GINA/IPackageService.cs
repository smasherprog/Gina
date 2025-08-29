using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace GimaSoft.Service.GINA
{
	// Token: 0x02000004 RID: 4
	[ServiceContract]
	public interface IPackageService
	{
		// Token: 0x06000017 RID: 23
		[OperationContract]
		UploadResult UploadPackageInit(int totalSize, bool isRepositorySubmission, string username, string password, int subCategoryId, string name, string comment);

		// Token: 0x06000018 RID: 24
		[OperationContract]
		UploadResult UploadPackageChunk(Guid sessionId, byte[] packageData);

		// Token: 0x06000019 RID: 25
		[OperationContract]
		DownloadResult GetTotalDownloadSize(Guid[] sessionIds);

		// Token: 0x0600001A RID: 26
		[OperationContract]
		DownloadResult DownloadPackageChunk(Guid sessionId, int chunkNumber);

		// Token: 0x0600001B RID: 27
		[OperationContract]
		ConnectionInfo GetConnectionInfo(DateTimeOffset? lastRepositoryDate);

		// Token: 0x0600001C RID: 28
		[Obsolete]
		[OperationContract]
		int GetMaxSize();

		// Token: 0x0600001D RID: 29
		[Obsolete]
		[OperationContract]
		int GetMaxChunkSize();

		// Token: 0x0600001E RID: 30
		[OperationContract]
		IEnumerable<RepositoryEntry> GetRepositoryList();

		// Token: 0x0600001F RID: 31
		[OperationContract]
		IEnumerable<RepositorySubCategory> GetRepositorySubCategories();

		// Token: 0x06000020 RID: 32
		[OperationContract]
		void UploadErrorInfo(byte[] errorInfo);
	}
}
