using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001FC RID: 508
	[CallbackIdentity(505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyDataUpdate_t
	{
		// Token: 0x0400067C RID: 1660
		public const int k_iCallback = 505;

		// Token: 0x0400067D RID: 1661
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400067E RID: 1662
		public ulong m_ulSteamIDMember;

		// Token: 0x0400067F RID: 1663
		public byte m_bSuccess;
	}
}
