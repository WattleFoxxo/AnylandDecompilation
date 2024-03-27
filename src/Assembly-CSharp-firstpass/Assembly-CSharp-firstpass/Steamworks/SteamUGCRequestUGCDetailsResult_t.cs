using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000236 RID: 566
	[CallbackIdentity(3402)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCRequestUGCDetailsResult_t
	{
		// Token: 0x04000750 RID: 1872
		public const int k_iCallback = 3402;

		// Token: 0x04000751 RID: 1873
		public SteamUGCDetails_t m_details;

		// Token: 0x04000752 RID: 1874
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
