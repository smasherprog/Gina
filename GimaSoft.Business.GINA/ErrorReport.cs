using System;
using System.IO;
using System.Xml.Serialization;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000017 RID: 23
	public class ErrorReport : BindableObject
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000055F1 File Offset: 0x000037F1
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000055FE File Offset: 0x000037FE
		public string Comments
		{
			get
			{
				return base.Get<string>("Comments");
			}
			set
			{
				base.Set("Comments", value);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000560C File Offset: 0x0000380C
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00005619 File Offset: 0x00003819
		public string ErrorData
		{
			get
			{
				return base.Get<string>("ErrorData");
			}
			set
			{
				base.Set("ErrorData", value);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005627 File Offset: 0x00003827
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00005634 File Offset: 0x00003834
		public string Version
		{
			get
			{
				return base.Get<string>("Version");
			}
			set
			{
				base.Set("Version", value);
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005644 File Offset: 0x00003844
		public byte[] GetBytes()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ErrorReport));
			MemoryStream memoryStream = new MemoryStream();
			xmlSerializer.Serialize(memoryStream, this);
			return memoryStream.ToArray();
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005678 File Offset: 0x00003878
		public static ErrorReport Create(byte[] bytes)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ErrorReport));
			return (ErrorReport)xmlSerializer.Deserialize(new MemoryStream(bytes)
			{
				Position = 0L
			});
		}
	}
}
