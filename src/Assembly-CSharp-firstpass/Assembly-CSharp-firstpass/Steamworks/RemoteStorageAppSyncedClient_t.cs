using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000217 RID: 535
	[CallbackIdentity(1301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedClient_t
	{
		// Token: 0x040006BB RID: 1723
		public const int k_iCallback = 1301;

		// Token: 0x040006BC RID: 1724
		public AppId_t m_nAppID;

		// Token: 0x040006BD RID: 1725
		public EResult m_eResult;

		// Token: 0x040006BE RID: 1726
		public int m_unNumDownloads;
	}
}
