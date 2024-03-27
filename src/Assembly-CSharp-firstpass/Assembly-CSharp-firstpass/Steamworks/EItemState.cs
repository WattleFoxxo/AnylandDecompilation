using System;

namespace Steamworks
{
	// Token: 0x0200028B RID: 651
	[Flags]
	public enum EItemState
	{
		// Token: 0x04000A81 RID: 2689
		k_EItemStateNone = 0,
		// Token: 0x04000A82 RID: 2690
		k_EItemStateSubscribed = 1,
		// Token: 0x04000A83 RID: 2691
		k_EItemStateLegacyItem = 2,
		// Token: 0x04000A84 RID: 2692
		k_EItemStateInstalled = 4,
		// Token: 0x04000A85 RID: 2693
		k_EItemStateNeedsUpdate = 8,
		// Token: 0x04000A86 RID: 2694
		k_EItemStateDownloading = 16,
		// Token: 0x04000A87 RID: 2695
		k_EItemStateDownloadPending = 32
	}
}
