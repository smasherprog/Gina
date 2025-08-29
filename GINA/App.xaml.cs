using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using GimaSoft.Business.GINA;
using Microsoft.Shell;

namespace GimaSoft.GINA
{
	// Token: 0x02000025 RID: 37
	public partial class App : global::System.Windows.Application, ISingleInstanceApp
	{
		// Token: 0x060003BF RID: 959 RVA: 0x0000CFB0 File Offset: 0x0000B1B0
		private void WriteException(StreamWriter stream, Exception ex)
		{
			if (ex == null)
			{
				return;
			}
			if (ex.InnerException != null)
			{
				this.WriteException(stream, ex.InnerException);
			}
			if (!string.IsNullOrWhiteSpace(ex.Message))
			{
				stream.WriteLine(ex.Message);
			}
			if (!string.IsNullOrWhiteSpace(ex.StackTrace))
			{
				stream.WriteLine(ex.StackTrace);
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000D074 File Offset: 0x0000B274
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs args)
			{
				try
				{
					Exception ex = args.ExceptionObject as Exception;
					if (ex != null)
					{
						StreamWriter streamWriter = File.CreateText(Configuration.CrashLogFilePre);
						this.WriteException(streamWriter, ex);
						streamWriter.WriteLine();
						streamWriter.WriteLine("Crash version: {0}", global::System.Windows.Forms.Application.ProductVersion.ToString());
						streamWriter.Close();
						streamWriter.Dispose();
					}
				}
				catch
				{
				}
			};
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000D093 File Offset: 0x0000B293
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			if (!this.Restarting)
			{
				Configuration.SaveConfiguration(false);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0000D0AA File Offset: 0x0000B2AA
		public GINAData Data
		{
			get
			{
				return GINAData.Current;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0000D0B1 File Offset: 0x0000B2B1
		public Configuration Configuration
		{
			get
			{
				return Configuration.Current;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060003C4 RID: 964 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		// (set) Token: 0x060003C5 RID: 965 RVA: 0x0000D0C0 File Offset: 0x0000B2C0
		public bool Restarting { get; set; }

		// Token: 0x060003C6 RID: 966 RVA: 0x0000D0C9 File Offset: 0x0000B2C9
		private void Application_Startup(object sender, StartupEventArgs e)
		{
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000D100 File Offset: 0x0000B300
		public bool SignalExternalCommandLineArgs(IList<string> args)
		{
			if (args != null)
			{
				foreach (string text in args)
				{
					Package.OpenFilePackage(text);
				}
			}
			return true;
		}

		// Token: 0x040000D4 RID: 212
		private static string Unique = "Gimagukk_GINA_Application" + global::System.Windows.Forms.Application.ProductVersion;
	}
}
