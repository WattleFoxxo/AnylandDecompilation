using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C1 RID: 449
	[CallbackIdentity(336)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendRichPresenceUpdate_t
	{
		// Token: 0x0400058D RID: 1421
		public const int k_iCallback = 336;

		// Token: 0x0400058E RID: 1422
		public CSteamID m_steamIDFriend;

		// Token: 0x0400058F RID: 1423
		public AppId_t m_nAppID;
	}
}
