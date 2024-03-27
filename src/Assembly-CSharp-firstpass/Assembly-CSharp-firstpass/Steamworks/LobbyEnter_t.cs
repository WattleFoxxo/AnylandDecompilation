using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001FB RID: 507
	[CallbackIdentity(504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyEnter_t
	{
		// Token: 0x04000677 RID: 1655
		public const int k_iCallback = 504;

		// Token: 0x04000678 RID: 1656
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000679 RID: 1657
		public uint m_rgfChatPermissions;

		// Token: 0x0400067A RID: 1658
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocked;

		// Token: 0x0400067B RID: 1659
		public uint m_EChatRoomEnterResponse;
	}
}
