using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C7 RID: 455
	[CallbackIdentity(342)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct JoinClanChatRoomCompletionResult_t
	{
		// Token: 0x040005A1 RID: 1441
		public const int k_iCallback = 342;

		// Token: 0x040005A2 RID: 1442
		public CSteamID m_steamIDClanChat;

		// Token: 0x040005A3 RID: 1443
		public EChatRoomEnterResponse m_eChatRoomEnterResponse;
	}
}
