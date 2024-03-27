using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F8 RID: 504
	[CallbackIdentity(4703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryEligiblePromoItemDefIDs_t
	{
		// Token: 0x04000666 RID: 1638
		public const int k_iCallback = 4703;

		// Token: 0x04000667 RID: 1639
		public EResult m_result;

		// Token: 0x04000668 RID: 1640
		public CSteamID m_steamID;

		// Token: 0x04000669 RID: 1641
		public int m_numEligiblePromoItemDefs;

		// Token: 0x0400066A RID: 1642
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
