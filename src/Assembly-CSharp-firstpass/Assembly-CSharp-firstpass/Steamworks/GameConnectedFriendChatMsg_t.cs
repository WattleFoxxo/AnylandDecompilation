using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C8 RID: 456
	[CallbackIdentity(343)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedFriendChatMsg_t
	{
		// Token: 0x040005A4 RID: 1444
		public const int k_iCallback = 343;

		// Token: 0x040005A5 RID: 1445
		public CSteamID m_steamIDUser;

		// Token: 0x040005A6 RID: 1446
		public int m_iMessageID;
	}
}
