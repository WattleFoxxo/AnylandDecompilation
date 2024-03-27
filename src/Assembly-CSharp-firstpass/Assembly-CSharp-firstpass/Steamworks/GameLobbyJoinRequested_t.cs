using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001BE RID: 446
	[CallbackIdentity(333)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameLobbyJoinRequested_t
	{
		// Token: 0x04000581 RID: 1409
		public const int k_iCallback = 333;

		// Token: 0x04000582 RID: 1410
		public CSteamID m_steamIDLobby;

		// Token: 0x04000583 RID: 1411
		public CSteamID m_steamIDFriend;
	}
}
