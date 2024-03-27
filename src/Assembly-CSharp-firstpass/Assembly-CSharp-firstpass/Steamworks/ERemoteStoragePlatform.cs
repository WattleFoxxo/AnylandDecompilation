using System;

namespace Steamworks
{
	// Token: 0x0200027D RID: 637
	[Flags]
	public enum ERemoteStoragePlatform
	{
		// Token: 0x04000A08 RID: 2568
		k_ERemoteStoragePlatformNone = 0,
		// Token: 0x04000A09 RID: 2569
		k_ERemoteStoragePlatformWindows = 1,
		// Token: 0x04000A0A RID: 2570
		k_ERemoteStoragePlatformOSX = 2,
		// Token: 0x04000A0B RID: 2571
		k_ERemoteStoragePlatformPS3 = 4,
		// Token: 0x04000A0C RID: 2572
		k_ERemoteStoragePlatformLinux = 8,
		// Token: 0x04000A0D RID: 2573
		k_ERemoteStoragePlatformReserved2 = 16,
		// Token: 0x04000A0E RID: 2574
		k_ERemoteStoragePlatformAll = -1
	}
}
