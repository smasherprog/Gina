using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WPFShared.Properties
{
	// Token: 0x0200004E RID: 78
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000218 RID: 536 RVA: 0x00009266 File Offset: 0x00007466
		internal Resources()
		{
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00009270 File Offset: 0x00007470
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("WPFShared.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000092A9 File Offset: 0x000074A9
		// (set) Token: 0x0600021B RID: 539 RVA: 0x000092B0 File Offset: 0x000074B0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x040000AD RID: 173
		private static ResourceManager resourceMan;

		// Token: 0x040000AE RID: 174
		private static CultureInfo resourceCulture;
	}
}
