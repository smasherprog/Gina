using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GimaSoft.GINA.Properties
{
	// Token: 0x02000029 RID: 41
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x060004CE RID: 1230 RVA: 0x000102D2 File Offset: 0x0000E4D2
		internal Resources()
		{
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x000102DC File Offset: 0x0000E4DC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("GimaSoft.GINA.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0001031B File Offset: 0x0000E51B
		// (set) Token: 0x060004D1 RID: 1233 RVA: 0x00010322 File Offset: 0x0000E522
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

		// Token: 0x04000103 RID: 259
		private static ResourceManager resourceMan;

		// Token: 0x04000104 RID: 260
		private static CultureInfo resourceCulture;
	}
}
