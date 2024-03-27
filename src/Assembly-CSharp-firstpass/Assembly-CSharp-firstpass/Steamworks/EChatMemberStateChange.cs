using System;

namespace Steamworks
{
	// Token: 0x02000277 RID: 631
	[Flags]
	public enum EChatMemberStateChange
	{
		// Token: 0x040009E1 RID: 2529
		k_EChatMemberStateChangeEntered = 1,
		// Token: 0x040009E2 RID: 2530
		k_EChatMemberStateChangeLeft = 2,
		// Token: 0x040009E3 RID: 2531
		k_EChatMemberStateChangeDisconnected = 4,
		// Token: 0x040009E4 RID: 2532
		k_EChatMemberStateChangeKicked = 8,
		// Token: 0x040009E5 RID: 2533
		k_EChatMemberStateChangeBanned = 16
	}
}
