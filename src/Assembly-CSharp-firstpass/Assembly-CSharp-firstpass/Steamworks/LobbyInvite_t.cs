using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001FA RID: 506
	[CallbackIdentity(503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyInvite_t
	{
		// Token: 0x04000673 RID: 1651
		public const int k_iCallback = 503;

		// Token: 0x04000674 RID: 1652
		public ulong m_ulSteamIDUser;

		// Token: 0x04000675 RID: 1653
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000676 RID: 1654
		public ulong m_ulGameID;
	}
}
