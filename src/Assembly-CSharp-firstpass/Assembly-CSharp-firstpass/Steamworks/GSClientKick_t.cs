using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D1 RID: 465
	[CallbackIdentity(203)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientKick_t
	{
		// Token: 0x040005C2 RID: 1474
		public const int k_iCallback = 203;

		// Token: 0x040005C3 RID: 1475
		public CSteamID m_SteamID;

		// Token: 0x040005C4 RID: 1476
		public EDenyReason m_eDenyReason;
	}
}
