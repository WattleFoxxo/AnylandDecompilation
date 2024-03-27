using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200024F RID: 591
	[CallbackIdentity(1103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementStored_t
	{
		// Token: 0x0400079F RID: 1951
		public const int k_iCallback = 1103;

		// Token: 0x040007A0 RID: 1952
		public ulong m_nGameID;

		// Token: 0x040007A1 RID: 1953
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bGroupAchievement;

		// Token: 0x040007A2 RID: 1954
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		// Token: 0x040007A3 RID: 1955
		public uint m_nCurProgress;

		// Token: 0x040007A4 RID: 1956
		public uint m_nMaxProgress;
	}
}
