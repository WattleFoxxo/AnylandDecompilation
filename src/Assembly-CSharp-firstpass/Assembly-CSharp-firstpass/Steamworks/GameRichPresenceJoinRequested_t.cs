using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C2 RID: 450
	[CallbackIdentity(337)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameRichPresenceJoinRequested_t
	{
		// Token: 0x04000590 RID: 1424
		public const int k_iCallback = 337;

		// Token: 0x04000591 RID: 1425
		public CSteamID m_steamIDFriend;

		// Token: 0x04000592 RID: 1426
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchConnect;
	}
}
