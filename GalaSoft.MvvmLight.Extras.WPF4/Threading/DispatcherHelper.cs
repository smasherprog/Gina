using System;
using System.Windows.Threading;

namespace GalaSoft.MvvmLight.Threading
{
	// Token: 0x02000002 RID: 2
	public static class DispatcherHelper
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020E6 File Offset: 0x000002E6
		public static Dispatcher UIDispatcher { get; private set; }

		// Token: 0x06000003 RID: 3 RVA: 0x000020F0 File Offset: 0x000002F0
		public static void CheckBeginInvokeOnUI(Action action)
		{
			if (DispatcherHelper.UIDispatcher.CheckAccess())
			{
				action();
			}
			else
			{
				DispatcherHelper.UIDispatcher.BeginInvoke(action, new object[0]);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002130 File Offset: 0x00000330
		public static void Initialize()
		{
			if (DispatcherHelper.UIDispatcher == null)
			{
				DispatcherHelper.UIDispatcher = Dispatcher.CurrentDispatcher;
			}
		}
	}
}
