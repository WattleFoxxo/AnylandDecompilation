using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000202 RID: 514
	[CallbackIdentity(513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyCreated_t
	{
		// Token: 0x04000695 RID: 1685
		public const int k_iCallback = 513;

		// Token: 0x04000696 RID: 1686
		public EResult m_eResult;

		// Token: 0x04000697 RID: 1687
		public ulong m_ulSteamIDLobby;
	}
}
