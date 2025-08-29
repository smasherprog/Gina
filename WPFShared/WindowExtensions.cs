using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Xml;
using System.Xml.Serialization;

namespace WPFShared
{
	// Token: 0x02000044 RID: 68
	public static class WindowExtensions
	{
		// Token: 0x060001E4 RID: 484
		[DllImport("user32.dll")]
		private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

		// Token: 0x060001E5 RID: 485
		[DllImport("user32.dll")]
		private static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

		// Token: 0x060001E6 RID: 486
		[DllImport("user32.dll")]
		private static extern int GetWindowLong(IntPtr hwnd, int index);

		// Token: 0x060001E7 RID: 487
		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

		// Token: 0x060001E8 RID: 488
		[DllImport("user32.dll")]
		private static extern int SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		// Token: 0x060001E9 RID: 489 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public static void SetPlacement(this Window window, string placementXml)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (string.IsNullOrEmpty(placementXml))
			{
				return;
			}
			byte[] bytes = WindowExtensions.encoding.GetBytes(placementXml);
			try
			{
				WINDOWPLACEMENT windowplacement;
				using (MemoryStream memoryStream = new MemoryStream(bytes))
				{
					windowplacement = (WINDOWPLACEMENT)WindowExtensions.serializer.Deserialize(memoryStream);
				}
				windowplacement.length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
				windowplacement.flags = 0;
				windowplacement.showCmd = ((windowplacement.showCmd == 2) ? 1 : windowplacement.showCmd);
				WindowExtensions.SetWindowPlacement(handle, ref windowplacement);
			}
			catch (InvalidOperationException)
			{
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008B88 File Offset: 0x00006D88
		public static string GetPlacement(this Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			WINDOWPLACEMENT windowplacement = default(WINDOWPLACEMENT);
			WindowExtensions.GetWindowPlacement(handle, out windowplacement);
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			};
			StringWriter stringWriter = new StringWriter();
			using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
			{
				WindowExtensions.serializer.Serialize(xmlWriter, windowplacement, xmlSerializerNamespaces);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008C24 File Offset: 0x00006E24
		public static IntPtr Handle(this Window window)
		{
			return new WindowInteropHelper(window).Handle;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008C34 File Offset: 0x00006E34
		private static void SetWindowStyleProperty(IntPtr handle, int property)
		{
			int windowLong = WindowExtensions.GetWindowLong(handle, -20);
			WindowExtensions.SetWindowLong(handle, -20, windowLong | property);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008C58 File Offset: 0x00006E58
		private static void ClearWindowStyleProperty(IntPtr handle, int property)
		{
			int windowLong = WindowExtensions.GetWindowLong(handle, -20);
			WindowExtensions.SetWindowLong(handle, -20, windowLong & ~property);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008C7B File Offset: 0x00006E7B
		public static void MakeOverlay(this Window window)
		{
			WindowExtensions.SetWindowStyleProperty(window.Handle(), 32);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008C8A File Offset: 0x00006E8A
		public static void ClearOverlay(this Window window)
		{
			WindowExtensions.ClearWindowStyleProperty(window.Handle(), 32);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008C99 File Offset: 0x00006E99
		public static void HideFromAltTab(this Window window)
		{
			WindowExtensions.SetWindowStyleProperty(window.Handle(), 128);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008CAB File Offset: 0x00006EAB
		public static void ShowInAltTab(this Window window)
		{
			WindowExtensions.ClearWindowStyleProperty(window.Handle(), 128);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008CBD File Offset: 0x00006EBD
		public static void BringToTop(this Window window)
		{
			WindowExtensions.SetWindowPos(window.Handle(), IntPtr.Zero, 0, 0, 0, 0, 19U);
		}

		// Token: 0x0400009B RID: 155
		private const int SW_SHOWNORMAL = 1;

		// Token: 0x0400009C RID: 156
		private const int SW_SHOWMINIMIZED = 2;

		// Token: 0x0400009D RID: 157
		private const int WS_EX_TRANSPARENT = 32;

		// Token: 0x0400009E RID: 158
		private const int WS_EX_TOOLWINDOW = 128;

		// Token: 0x0400009F RID: 159
		private const int GWL_EXSTYLE = -20;

		// Token: 0x040000A0 RID: 160
		private const int SWP_NOMOVE = 2;

		// Token: 0x040000A1 RID: 161
		private const int SWP_NOACTIVATE = 16;

		// Token: 0x040000A2 RID: 162
		private const int SWP_NOSIZE = 1;

		// Token: 0x040000A3 RID: 163
		private static Encoding encoding = new UTF8Encoding();

		// Token: 0x040000A4 RID: 164
		private static XmlSerializer serializer = new XmlSerializer(typeof(WINDOWPLACEMENT));
	}
}
