using System;

namespace Valve.VR
{
	// Token: 0x02000143 RID: 323
	public enum EVRApplicationTransitionState
	{
		// Token: 0x040002F3 RID: 755
		VRApplicationTransition_None,
		// Token: 0x040002F4 RID: 756
		VRApplicationTransition_OldAppQuitSent = 10,
		// Token: 0x040002F5 RID: 757
		VRApplicationTransition_WaitingForExternalLaunch,
		// Token: 0x040002F6 RID: 758
		VRApplicationTransition_NewAppLaunched = 20
	}
}
