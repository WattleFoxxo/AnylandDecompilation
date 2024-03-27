using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001CA RID: 458
	[CallbackIdentity(345)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsIsFollowing_t
	{
		// Token: 0x040005AB RID: 1451
		public const int k_iCallback = 345;

		// Token: 0x040005AC RID: 1452
		public EResult m_eResult;

		// Token: 0x040005AD RID: 1453
		public CSteamID m_steamID;

		// Token: 0x040005AE RID: 1454
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bIsFollowing;
	}
}
