using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B2 RID: 690
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct FriendGameInfo_t
	{
		// Token: 0x04000C4C RID: 3148
		public CGameID m_gameID;

		// Token: 0x04000C4D RID: 3149
		public uint m_unGameIP;

		// Token: 0x04000C4E RID: 3150
		public ushort m_usGamePort;

		// Token: 0x04000C4F RID: 3151
		public ushort m_usQueryPort;

		// Token: 0x04000C50 RID: 3152
		public CSteamID m_steamIDLobby;
	}
}
