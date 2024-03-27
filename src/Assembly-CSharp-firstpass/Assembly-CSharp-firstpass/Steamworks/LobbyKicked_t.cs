using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000201 RID: 513
	[CallbackIdentity(512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyKicked_t
	{
		// Token: 0x04000691 RID: 1681
		public const int k_iCallback = 512;

		// Token: 0x04000692 RID: 1682
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000693 RID: 1683
		public ulong m_ulSteamIDAdmin;

		// Token: 0x04000694 RID: 1684
		public byte m_bKickedDueToDisconnect;
	}
}
