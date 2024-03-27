using System;

namespace Steamworks
{
	// Token: 0x020002A7 RID: 679
	[Flags]
	public enum EMarketingMessageFlags
	{
		// Token: 0x04000BC8 RID: 3016
		k_EMarketingMessageFlagsNone = 0,
		// Token: 0x04000BC9 RID: 3017
		k_EMarketingMessageFlagsHighPriority = 1,
		// Token: 0x04000BCA RID: 3018
		k_EMarketingMessageFlagsPlatformWindows = 2,
		// Token: 0x04000BCB RID: 3019
		k_EMarketingMessageFlagsPlatformMac = 4,
		// Token: 0x04000BCC RID: 3020
		k_EMarketingMessageFlagsPlatformLinux = 8,
		// Token: 0x04000BCD RID: 3021
		k_EMarketingMessageFlagsPlatformRestrictions = 14
	}
}
