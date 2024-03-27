using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001FD RID: 509
	[CallbackIdentity(506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatUpdate_t
	{
		// Token: 0x04000680 RID: 1664
		public const int k_iCallback = 506;

		// Token: 0x04000681 RID: 1665
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000682 RID: 1666
		public ulong m_ulSteamIDUserChanged;

		// Token: 0x04000683 RID: 1667
		public ulong m_ulSteamIDMakingChange;

		// Token: 0x04000684 RID: 1668
		public uint m_rgfChatMemberStateChange;
	}
}
