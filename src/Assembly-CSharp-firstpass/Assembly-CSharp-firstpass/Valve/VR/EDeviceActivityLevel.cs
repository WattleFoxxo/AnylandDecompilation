using System;

namespace Valve.VR
{
	// Token: 0x02000132 RID: 306
	public enum EDeviceActivityLevel
	{
		// Token: 0x0400020F RID: 527
		k_EDeviceActivityLevel_Unknown = -1,
		// Token: 0x04000210 RID: 528
		k_EDeviceActivityLevel_Idle,
		// Token: 0x04000211 RID: 529
		k_EDeviceActivityLevel_UserInteraction,
		// Token: 0x04000212 RID: 530
		k_EDeviceActivityLevel_UserInteraction_Timeout,
		// Token: 0x04000213 RID: 531
		k_EDeviceActivityLevel_Standby
	}
}
