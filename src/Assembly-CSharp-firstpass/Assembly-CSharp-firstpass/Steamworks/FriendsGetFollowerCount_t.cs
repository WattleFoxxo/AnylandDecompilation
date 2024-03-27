using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C9 RID: 457
	[CallbackIdentity(344)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsGetFollowerCount_t
	{
		// Token: 0x040005A7 RID: 1447
		public const int k_iCallback = 344;

		// Token: 0x040005A8 RID: 1448
		public EResult m_eResult;

		// Token: 0x040005A9 RID: 1449
		public CSteamID m_steamID;

		// Token: 0x040005AA RID: 1450
		public int m_nCount;
	}
}
