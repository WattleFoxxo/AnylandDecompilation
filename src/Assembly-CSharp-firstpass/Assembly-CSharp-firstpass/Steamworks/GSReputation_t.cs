using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D6 RID: 470
	[CallbackIdentity(209)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSReputation_t
	{
		// Token: 0x040005D5 RID: 1493
		public const int k_iCallback = 209;

		// Token: 0x040005D6 RID: 1494
		public EResult m_eResult;

		// Token: 0x040005D7 RID: 1495
		public uint m_unReputationScore;

		// Token: 0x040005D8 RID: 1496
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x040005D9 RID: 1497
		public uint m_unBannedIP;

		// Token: 0x040005DA RID: 1498
		public ushort m_usBannedPort;

		// Token: 0x040005DB RID: 1499
		public ulong m_ulBannedGameID;

		// Token: 0x040005DC RID: 1500
		public uint m_unBanExpires;
	}
}
