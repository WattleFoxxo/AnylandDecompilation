using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000235 RID: 565
	[CallbackIdentity(3401)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUGCQueryCompleted_t
	{
		// Token: 0x0400074A RID: 1866
		public const int k_iCallback = 3401;

		// Token: 0x0400074B RID: 1867
		public UGCQueryHandle_t m_handle;

		// Token: 0x0400074C RID: 1868
		public EResult m_eResult;

		// Token: 0x0400074D RID: 1869
		public uint m_unNumResultsReturned;

		// Token: 0x0400074E RID: 1870
		public uint m_unTotalMatchingResults;

		// Token: 0x0400074F RID: 1871
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bCachedData;
	}
}
