using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C5 RID: 453
	[CallbackIdentity(340)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GameConnectedChatLeave_t
	{
		// Token: 0x0400059A RID: 1434
		public const int k_iCallback = 340;

		// Token: 0x0400059B RID: 1435
		public CSteamID m_steamIDClanChat;

		// Token: 0x0400059C RID: 1436
		public CSteamID m_steamIDUser;

		// Token: 0x0400059D RID: 1437
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bKicked;

		// Token: 0x0400059E RID: 1438
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bDropped;
	}
}
