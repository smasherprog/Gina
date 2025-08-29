using System;
using System.Windows;
using System.Windows.Threading;
using WPFShared;

namespace GimaSoft.GINA
{
	// Token: 0x02000019 RID: 25
	public class GINAViewModel : BindableObject
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000A75A File Offset: 0x0000895A
		public App App
		{
			get
			{
				return (App)Application.Current;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000A766 File Offset: 0x00008966
		protected new Dispatcher Dispatcher
		{
			get
			{
				if (this._Dispatcher == null)
				{
					this._Dispatcher = ((Application.Current != null) ? Application.Current.Dispatcher : Dispatcher.CurrentDispatcher);
				}
				return this._Dispatcher;
			}
		}

		// Token: 0x04000095 RID: 149
		private Dispatcher _Dispatcher;
	}
}
