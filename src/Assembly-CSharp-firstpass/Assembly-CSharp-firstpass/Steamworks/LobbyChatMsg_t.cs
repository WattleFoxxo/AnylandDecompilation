using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001FE RID: 510
	[CallbackIdentity(507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyChatMsg_t
	{
		// Token: 0x04000685 RID: 1669
		public const int k_iCallback = 507;

		// Token: 0x04000686 RID: 1670
		public ulong m_ulSteamIDLobby;

		// Token: 0x04000687 RID: 1671
		public ulong m_ulSteamIDUser;

		// Token: 0x04000688 RID: 1672
		public byte m_eChatEntryType;

		// Token: 0x04000689 RID: 1673
		public uint m_iChatID;
	}
}
