using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000255 RID: 597
	[CallbackIdentity(1109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementIconFetched_t
	{
		// Token: 0x040007B8 RID: 1976
		public const int k_iCallback = 1109;

		// Token: 0x040007B9 RID: 1977
		public CGameID m_nGameID;

		// Token: 0x040007BA RID: 1978
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		// Token: 0x040007BB RID: 1979
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAchieved;

		// Token: 0x040007BC RID: 1980
		public int m_nIconHandle;
	}
}
