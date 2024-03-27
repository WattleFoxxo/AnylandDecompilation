using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D5 RID: 469
	[CallbackIdentity(208)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct GSClientGroupStatus_t
	{
		// Token: 0x040005D0 RID: 1488
		public const int k_iCallback = 208;

		// Token: 0x040005D1 RID: 1489
		public CSteamID m_SteamIDUser;

		// Token: 0x040005D2 RID: 1490
		public CSteamID m_SteamIDGroup;

		// Token: 0x040005D3 RID: 1491
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bMember;

		// Token: 0x040005D4 RID: 1492
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bOfficer;
	}
}
