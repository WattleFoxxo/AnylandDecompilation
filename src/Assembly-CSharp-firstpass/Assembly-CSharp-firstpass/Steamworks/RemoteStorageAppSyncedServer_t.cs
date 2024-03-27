using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000218 RID: 536
	[CallbackIdentity(1302)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncedServer_t
	{
		// Token: 0x040006BF RID: 1727
		public const int k_iCallback = 1302;

		// Token: 0x040006C0 RID: 1728
		public AppId_t m_nAppID;

		// Token: 0x040006C1 RID: 1729
		public EResult m_eResult;

		// Token: 0x040006C2 RID: 1730
		public int m_unNumUploads;
	}
}
