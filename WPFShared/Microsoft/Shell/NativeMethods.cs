﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Shell
{
	// Token: 0x02000052 RID: 82
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		// Token: 0x06000224 RID: 548
		[DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "CommandLineToArgvW")]
		private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine, out int numArgs);

		// Token: 0x06000225 RID: 549
		[DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
		private static extern IntPtr _LocalFree(IntPtr hMem);

		// Token: 0x06000226 RID: 550 RVA: 0x0000948C File Offset: 0x0000768C
		public static string[] CommandLineToArgvW(string cmdLine)
		{
			IntPtr intPtr = IntPtr.Zero;
			string[] array2;
			try
			{
				int num = 0;
				intPtr = NativeMethods._CommandLineToArgvW(cmdLine, out num);
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, i * Marshal.SizeOf(typeof(IntPtr)));
					array[i] = Marshal.PtrToStringUni(intPtr2);
				}
				array2 = array;
			}
			finally
			{
				NativeMethods._LocalFree(intPtr);
			}
			return array2;
		}

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x06000228 RID: 552
		public delegate IntPtr MessageHandler(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);
	}
}
