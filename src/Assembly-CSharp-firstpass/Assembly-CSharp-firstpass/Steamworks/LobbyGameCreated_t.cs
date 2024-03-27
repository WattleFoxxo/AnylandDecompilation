using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001FF RID: 511
	[CallbackIdentity(509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyGameCreated_t
	{
		// Token: 0x0400068A RID: 1674
		public const int k_iCallback = 509;

		// Token: 0x0400068B RID: 1675
		public ulong m_ulSteamIDLobby;

		// Token: 0x0400068C RID: 1676
		public ulong m_ulSteamIDGameServer;

		// Token: 0x0400068D RID: 1677
		public uint m_unIP;

		// Token: 0x0400068E RID: 1678
		public ushort m_usPort;
	}
}
