using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C4 RID: 452
	[CallbackIdentity(339)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameConnectedChatJoin_t
	{
		// Token: 0x04000597 RID: 1431
		public const int k_iCallback = 339;

		// Token: 0x04000598 RID: 1432
		public CSteamID m_steamIDClanChat;

		// Token: 0x04000599 RID: 1433
		public CSteamID m_steamIDUser;
	}
}
