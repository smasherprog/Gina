using System;
using System.Linq;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x0200003A RID: 58
	public class PhoneticTransform : GINABusinessObject
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000D798 File Offset: 0x0000B998
		public static void Add(PhoneticTransform transform)
		{
			Configuration.Current.PhoneticDictionary.Add(transform);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000D7D0 File Offset: 0x0000B9D0
		public static void Add(string actualWord, string phoneticWord)
		{
			PhoneticTransform phoneticTransform = Configuration.Current.PhoneticDictionary.FirstOrDefault((PhoneticTransform o) => o.ActualWord.ToUpper() == actualWord.ToUpper());
			if (phoneticTransform != null)
			{
				phoneticTransform.ActualWord = actualWord;
				phoneticTransform.PhoneticWord = phoneticWord;
				return;
			}
			Configuration.Current.PhoneticDictionary.Add(new PhoneticTransform(actualWord, phoneticWord));
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000D860 File Offset: 0x0000BA60
		public static void Remove(string actualWord)
		{
			Configuration.Current.PhoneticDictionary.RemoveAll((PhoneticTransform o) => o.ActualWord.ToUpper() == actualWord.ToUpper());
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000D896 File Offset: 0x0000BA96
		public PhoneticTransform(string actualWord, string phoneticWord)
		{
			this.ActualWord = actualWord;
			this.PhoneticWord = phoneticWord;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
		public string FastActualWord { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000D8BD File Offset: 0x0000BABD
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000D8C5 File Offset: 0x0000BAC5
		public string FastPhoneticWord { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000D8CE File Offset: 0x0000BACE
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000D8DB File Offset: 0x0000BADB
		public string ActualWord
		{
			get
			{
				return base.Get<string>("ActualWord");
			}
			set
			{
				base.Set("ActualWord", value);
				this.FastActualWord = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000D8F0 File Offset: 0x0000BAF0
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000D8FD File Offset: 0x0000BAFD
		public string PhoneticWord
		{
			get
			{
				return base.Get<string>("PhoneticWord");
			}
			set
			{
				base.Set("PhoneticWord", value);
				this.FastPhoneticWord = value;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000D912 File Offset: 0x0000BB12
		public void Remove()
		{
			if (Configuration.Current.PhoneticDictionary.Contains(this))
			{
				Configuration.Current.PhoneticDictionary.Remove(this);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000D937 File Offset: 0x0000BB37
		public static PhoneticTransform LoadFromXml(XmlElement element, bool loadToAll = true)
		{
			return new PhoneticTransform(element.GetElementValue("ActualWord", ""), element.GetElementValue("PhoneticWord", ""));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000D960 File Offset: 0x0000BB60
		public void SaveToXml(XmlElement element)
		{
			XmlDocument ownerDocument = element.OwnerDocument;
			XmlElement xmlElement = ownerDocument.CreateElement("Transform");
			element.AppendChild(xmlElement);
			xmlElement.AppendChild(ownerDocument.NewElement("ActualWord", this.ActualWord ?? ""));
			xmlElement.AppendChild(ownerDocument.NewElement("PhoneticWord", this.PhoneticWord ?? ""));
		}
	}
}
