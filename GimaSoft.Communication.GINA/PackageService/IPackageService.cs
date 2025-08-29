using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using GimaSoft.Service.GINA;

namespace GimaSoft.Communication.GINA.PackageService
{
	// Token: 0x02000016 RID: 22
	[ServiceContract(ConfigurationName = "PackageService.IPackageService")]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public interface IPackageService
	{
		// Token: 0x0600004C RID: 76
		[OperationContract(Action = "http://tempuri.org/IPackageService/UploadPackageInit", ReplyAction = "http://tempuri.org/IPackageService/UploadPackageInitResponse")]
		UploadResult UploadPackageInit(int totalSize, bool isRepositorySubmission, string username, string password, int subCategoryId, string name, string comment);

		// Token: 0x0600004D RID: 77
		[OperationContract(Action = "http://tempuri.org/IPackageService/UploadPackageChunk", ReplyAction = "http://tempuri.org/IPackageService/UploadPackageChunkResponse")]
		UploadResult UploadPackageChunk(Guid sessionId, byte[] packageData);

		// Token: 0x0600004E RID: 78
		[OperationContract(Action = "http://tempuri.org/IPackageService/GetTotalDownloadSize", ReplyAction = "http://tempuri.org/IPackageService/GetTotalDownloadSizeResponse")]
		DownloadResult GetTotalDownloadSize(Guid[] sessionIds);

		// Token: 0x0600004F RID: 79
		[OperationContract(Action = "http://tempuri.org/IPackageService/DownloadPackageChunk", ReplyAction = "http://tempuri.org/IPackageService/DownloadPackageChunkResponse")]
		DownloadResult DownloadPackageChunk(Guid sessionId, int chunkNumber);

		// Token: 0x06000050 RID: 80
		[OperationContract(Action = "http://tempuri.org/IPackageService/GetConnectionInfo", ReplyAction = "http://tempuri.org/IPackageService/GetConnectionInfoResponse")]
		ConnectionInfo GetConnectionInfo(DateTimeOffset? lastRepositoryDate);

		// Token: 0x06000051 RID: 81
		[OperationContract(Action = "http://tempuri.org/IPackageService/GetMaxSize", ReplyAction = "http://tempuri.org/IPackageService/GetMaxSizeResponse")]
		int GetMaxSize();

		// Token: 0x06000052 RID: 82
		[OperationContract(Action = "http://tempuri.org/IPackageService/GetMaxChunkSize", ReplyAction = "http://tempuri.org/IPackageService/GetMaxChunkSizeResponse")]
		int GetMaxChunkSize();

		// Token: 0x06000053 RID: 83
		[OperationContract(Action = "http://tempuri.org/IPackageService/GetRepositoryList", ReplyAction = "http://tempuri.org/IPackageService/GetRepositoryListResponse")]
		RepositoryEntry[] GetRepositoryList();

		// Token: 0x06000054 RID: 84
		[OperationContract(Action = "http://tempuri.org/IPackageService/GetRepositorySubCategories", ReplyAction = "http://tempuri.org/IPackageService/GetRepositorySubCategoriesResponse")]
		RepositorySubCategory[] GetRepositorySubCategories();

		// Token: 0x06000055 RID: 85
		[OperationContract(Action = "http://tempuri.org/IPackageService/UploadErrorInfo", ReplyAction = "http://tempuri.org/IPackageService/UploadErrorInfoResponse")]
		void UploadErrorInfo(byte[] errorInfo);
	}
}
