using System.Collections.Generic;

namespace Microsoft.Shell
{
	// Token: 0x02000054 RID: 84
	public interface ISingleInstanceApp
	{
		// Token: 0x0600022B RID: 555
		bool SignalExternalCommandLineArgs(IList<string> args);
	}
}
