using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D2 RID: 466
	[CallbackIdentity(206)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientAchievementStatus_t
	{
		// Token: 0x040005C5 RID: 1477
		public const int k_iCallback = 206;

		// Token: 0x040005C6 RID: 1478
		public ulong m_SteamID;

		// Token: 0x040005C7 RID: 1479
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_pchAchievement;

		// Token: 0x040005C8 RID: 1480
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUnlocked;
	}
}
