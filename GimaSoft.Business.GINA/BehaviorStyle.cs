using System;
using System.Windows.Media;
using System.Xml;
using WPFShared;

namespace GimaSoft.Business.GINA
{
	// Token: 0x02000005 RID: 5
	public class BehaviorStyle : BindableObject
	{
		// Token: 0x06000049 RID: 73 RVA: 0x000035BA File Offset: 0x000017BA
		public BehaviorStyle()
		{
			this.FontColor = new SolidColorBrush(Colors.Yellow);
			this.ShadowColor = Colors.Black;
			this.ShadowDepth = 5;
			this.TimerBarColor = Colors.Maroon;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000035EF File Offset: 0x000017EF
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000035FC File Offset: 0x000017FC
		public SolidColorBrush FontColor
		{
			get
			{
				return base.Get<SolidColorBrush>("FontColor");
			}
			set
			{
				base.Set("FontColor", value);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000360A File Offset: 0x0000180A
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003617 File Offset: 0x00001817
		public Color ShadowColor
		{
			get
			{
				return base.Get<Color>("ShadowColor");
			}
			set
			{
				base.Set("ShadowColor", value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000362A File Offset: 0x0000182A
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003637 File Offset: 0x00001837
		public int ShadowDepth
		{
			get
			{
				return base.Get<int>("ShadowDepth");
			}
			set
			{
				base.Set("ShadowDepth", value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000050 RID: 80 RVA: 0x0000364A File Offset: 0x0000184A
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003657 File Offset: 0x00001857
		public Color TimerBarColor
		{
			get
			{
				return base.Get<Color>("TimerBarColor");
			}
			set
			{
				base.Set("TimerBarColor", value);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000366C File Offset: 0x0000186C
		public void LoadFromXml(XmlElement element)
		{
			if (element == null)
			{
				return;
			}
			this.FontColor = new SolidColorBrush(default(Color).SetFromHtml(element.GetElementValue("FontColor", "Yellow"), "White"));
			this.ShadowColor = default(Color).SetFromHtml(element.GetElementValue("ShadowColor", "Black"), "White");
			this.ShadowDepth = element.GetElementValue("ShadowDepth", 5);
			this.TimerBarColor = default(Color).SetFromHtml(element.GetElementValue("TimerBarColor", "Maroon"), "White");
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003710 File Offset: 0x00001910
		public XmlElement SaveToXml(XmlElement element)
		{
			XmlDocument ownerDocument = element.OwnerDocument;
			element.AppendChild(ownerDocument.NewElement("FontColor", this.FontColor.Color.ToHexString(true)));
			element.AppendChild(ownerDocument.NewElement("ShadowColor", this.ShadowColor.ToHexString(true)));
			element.AppendChild(ownerDocument.NewElement("ShadowDepth", this.ShadowDepth));
			element.AppendChild(ownerDocument.NewElement("TimerBarColor", this.TimerBarColor.ToHexString(true)));
			return element;
		}
	}
}
