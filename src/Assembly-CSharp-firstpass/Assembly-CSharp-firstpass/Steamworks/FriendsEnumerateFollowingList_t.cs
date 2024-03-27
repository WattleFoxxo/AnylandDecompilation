using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001CB RID: 459
	[CallbackIdentity(346)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct FriendsEnumerateFollowingList_t
	{
		// Token: 0x040005AF RID: 1455
		public const int k_iCallback = 346;

		// Token: 0x040005B0 RID: 1456
		public EResult m_eResult;

		// Token: 0x040005B1 RID: 1457
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public CSteamID[] m_rgSteamID;

		// Token: 0x040005B2 RID: 1458
		public int m_nResultsReturned;

		// Token: 0x040005B3 RID: 1459
		public int m_nTotalResultCount;
	}
}
