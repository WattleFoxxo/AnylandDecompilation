using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D4 RID: 468
	[CallbackIdentity(207)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSGameplayStats_t
	{
		// Token: 0x040005CB RID: 1483
		public const int k_iCallback = 207;

		// Token: 0x040005CC RID: 1484
		public EResult m_eResult;

		// Token: 0x040005CD RID: 1485
		public int m_nRank;

		// Token: 0x040005CE RID: 1486
		public uint m_unTotalConnects;

		// Token: 0x040005CF RID: 1487
		public uint m_unTotalMinutesPlayed;
	}
}
