using System.IO;
using System.IO.Compression;

namespace WPFShared
{
	// Token: 0x0200003E RID: 62
	public static class MemoryStreamExtensions
	{
		// Token: 0x060001DB RID: 475 RVA: 0x000088E4 File Offset: 0x00006AE4
		public static void Compress(this MemoryStream ms)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
				{
					byte[] array = ms.ToArray();
					gzipStream.Write(array, 0, array.Length);
				}
				ms.SetLength(0L);
				memoryStream.Position = 0L;
				memoryStream.CopyTo(ms);
				ms.Position = 0L;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00008968 File Offset: 0x00006B68
		public static void Decompress(this MemoryStream ms)
		{
			ms.Position = 0L;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(ms, CompressionMode.Decompress, true))
				{
					gzipStream.CopyTo(memoryStream);
				}
				ms.SetLength(0L);
				ms.Capacity = (int)memoryStream.Length;
				memoryStream.Position = 0L;
				memoryStream.CopyTo(ms);
				ms.Position = 0L;
			}
		}
	}
}
