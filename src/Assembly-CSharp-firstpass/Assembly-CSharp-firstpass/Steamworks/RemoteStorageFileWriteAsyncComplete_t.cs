using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000231 RID: 561
	[CallbackIdentity(1331)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileWriteAsyncComplete_t
	{
		// Token: 0x0400073F RID: 1855
		public const int k_iCallback = 1331;

		// Token: 0x04000740 RID: 1856
		public EResult m_eResult;
	}
}
