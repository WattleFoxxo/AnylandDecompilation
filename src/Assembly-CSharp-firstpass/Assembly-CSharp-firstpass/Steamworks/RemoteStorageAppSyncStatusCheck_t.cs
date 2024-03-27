using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200021A RID: 538
	[CallbackIdentity(1305)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncStatusCheck_t
	{
		// Token: 0x040006C9 RID: 1737
		public const int k_iCallback = 1305;

		// Token: 0x040006CA RID: 1738
		public AppId_t m_nAppID;

		// Token: 0x040006CB RID: 1739
		public EResult m_eResult;
	}
}
