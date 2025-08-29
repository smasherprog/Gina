using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Microsoft.Shell
{
	// Token: 0x02000055 RID: 85
	public static class SingleInstance<TApplication> where TApplication : Application, ISingleInstanceApp
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00009514 File Offset: 0x00007714
		public static IList<string> CommandLineArgs
		{
			get
			{
				return SingleInstance<TApplication>.commandLineArgs;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000951C File Offset: 0x0000771C
		public static bool InitializeAsFirstInstance(string uniqueName)
		{
			SingleInstance<TApplication>.commandLineArgs = SingleInstance<TApplication>.GetCommandLineArgs(uniqueName);
			string text = uniqueName + Environment.UserName;
			string text2 = text + ":" + "SingeInstanceIPCChannel";
			bool flag;
			SingleInstance<TApplication>.singleInstanceMutex = new Mutex(true, text, out flag);
			if (flag)
			{
				SingleInstance<TApplication>.CreateRemoteService(text2);
			}
			else
			{
				SingleInstance<TApplication>.SignalFirstInstance(text2, SingleInstance<TApplication>.commandLineArgs);
			}
			return flag;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009576 File Offset: 0x00007776
		public static void Cleanup()
		{
			if (SingleInstance<TApplication>.singleInstanceMutex != null)
			{
				SingleInstance<TApplication>.singleInstanceMutex.Close();
				SingleInstance<TApplication>.singleInstanceMutex = null;
			}
			if (SingleInstance<TApplication>.channel != null)
			{
				ChannelServices.UnregisterChannel(SingleInstance<TApplication>.channel);
				SingleInstance<TApplication>.channel = null;
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000095A8 File Offset: 0x000077A8
		private static IList<string> GetCommandLineArgs(string uniqueApplicationName)
		{
			string[] array;
			if (AppDomain.CurrentDomain.ActivationContext == null)
			{
				array = Environment.GetCommandLineArgs();
			}
			else
			{
				array = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
			}
			if (array == null)
			{
				array = new string[0];
			}
			return new List<string>(array);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000095F0 File Offset: 0x000077F0
		private static void CreateRemoteService(string channelName)
		{
			BinaryServerFormatterSinkProvider binaryServerFormatterSinkProvider = new BinaryServerFormatterSinkProvider();
			binaryServerFormatterSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;
			IDictionary dictionary = new Dictionary<string, string>();
			dictionary["name"] = channelName;
			dictionary["portName"] = channelName;
			dictionary["exclusiveAddressUse"] = "false";
			SingleInstance<TApplication>.channel = new IpcServerChannel(dictionary, binaryServerFormatterSinkProvider);
			ChannelServices.RegisterChannel(SingleInstance<TApplication>.channel, true);
			SingleInstance<TApplication>.IPCRemoteService ipcremoteService = new SingleInstance<TApplication>.IPCRemoteService();
			RemotingServices.Marshal(ipcremoteService, "SingleInstanceApplicationService");
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009664 File Offset: 0x00007864
		private static void SignalFirstInstance(string channelName, IList<string> args)
		{
			IpcClientChannel ipcClientChannel = new IpcClientChannel();
			ChannelServices.RegisterChannel(ipcClientChannel, true);
			string text = "ipc://" + channelName + "/SingleInstanceApplicationService";
			SingleInstance<TApplication>.IPCRemoteService ipcremoteService = (SingleInstance<TApplication>.IPCRemoteService)RemotingServices.Connect(typeof(SingleInstance<TApplication>.IPCRemoteService), text);
			if (ipcremoteService != null)
			{
				ipcremoteService.InvokeFirstInstance(args);
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000096B0 File Offset: 0x000078B0
		private static object ActivateFirstInstanceCallback(object arg)
		{
			IList<string> list = arg as IList<string>;
			SingleInstance<TApplication>.ActivateFirstInstance(list);
			return null;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000096CC File Offset: 0x000078CC
		private static void ActivateFirstInstance(IList<string> args)
		{
			if (Application.Current == null)
			{
				return;
			}
			TApplication tapplication = (TApplication)((object)Application.Current);
			tapplication.SignalExternalCommandLineArgs(args);
		}

		// Token: 0x04000114 RID: 276
		private const string Delimiter = ":";

		// Token: 0x04000115 RID: 277
		private const string ChannelNameSuffix = "SingeInstanceIPCChannel";

		// Token: 0x04000116 RID: 278
		private const string RemoteServiceName = "SingleInstanceApplicationService";

		// Token: 0x04000117 RID: 279
		private const string IpcProtocol = "ipc://";

		// Token: 0x04000118 RID: 280
		private static Mutex singleInstanceMutex;

		// Token: 0x04000119 RID: 281
		private static IpcServerChannel channel;

		// Token: 0x0400011A RID: 282
		private static IList<string> commandLineArgs;

		// Token: 0x02000056 RID: 86
		private class IPCRemoteService : MarshalByRefObject
		{
			// Token: 0x06000234 RID: 564 RVA: 0x000096FB File Offset: 0x000078FB
			public void InvokeFirstInstance(IList<string> args)
			{
				if (Application.Current != null)
				{
					Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(SingleInstance<TApplication>.ActivateFirstInstanceCallback), args);
				}
			}

			// Token: 0x06000235 RID: 565 RVA: 0x00009723 File Offset: 0x00007923
			public override object InitializeLifetimeService()
			{
				return null;
			}
		}
	}
}
