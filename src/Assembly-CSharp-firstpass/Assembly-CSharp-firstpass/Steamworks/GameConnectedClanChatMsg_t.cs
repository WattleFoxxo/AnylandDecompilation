using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C3 RID: 451
	[CallbackIdentity(338)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameConnectedClanChatMsg_t
	{
		// Token: 0x04000593 RID: 1427
		public const int k_iCallback = 338;

		// Token: 0x04000594 RID: 1428
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000595 RID: 1429
		public CSteamID m_steamIDUser;

		// Token: 0x04000596 RID: 1430
		public int m_iMessageID;
	}
}
