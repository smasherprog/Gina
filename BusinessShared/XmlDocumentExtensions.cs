using System;
using System.IO;
using System.Xml;

namespace BusinessShared
{
	// Token: 0x02000004 RID: 4
	public static class XmlDocumentExtensions
	{
		// Token: 0x06000008 RID: 8 RVA: 0x000023B4 File Offset: 0x000005B4
		public static void SafeLoad(this XmlDocument doc, string filename)
		{
			using (XmlTextReader xmlTextReader = new XmlTextReader(filename))
			{
				xmlTextReader.DtdProcessing = DtdProcessing.Prohibit;
				doc.Load(xmlTextReader);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void SafeLoad(this XmlDocument doc, Stream stream)
		{
			using (XmlTextReader xmlTextReader = new XmlTextReader(stream))
			{
				xmlTextReader.DtdProcessing = DtdProcessing.Prohibit;
				doc.Load(xmlTextReader);
			}
		}
	}
}
