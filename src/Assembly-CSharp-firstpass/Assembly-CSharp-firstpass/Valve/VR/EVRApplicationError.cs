using System;

namespace Valve.VR
{
	// Token: 0x02000141 RID: 321
	public enum EVRApplicationError
	{
		// Token: 0x040002CF RID: 719
		None,
		// Token: 0x040002D0 RID: 720
		AppKeyAlreadyExists = 100,
		// Token: 0x040002D1 RID: 721
		NoManifest,
		// Token: 0x040002D2 RID: 722
		NoApplication,
		// Token: 0x040002D3 RID: 723
		InvalidIndex,
		// Token: 0x040002D4 RID: 724
		UnknownApplication,
		// Token: 0x040002D5 RID: 725
		IPCFailed,
		// Token: 0x040002D6 RID: 726
		ApplicationAlreadyRunning,
		// Token: 0x040002D7 RID: 727
		InvalidManifest,
		// Token: 0x040002D8 RID: 728
		InvalidApplication,
		// Token: 0x040002D9 RID: 729
		LaunchFailed,
		// Token: 0x040002DA RID: 730
		ApplicationAlreadyStarting,
		// Token: 0x040002DB RID: 731
		LaunchInProgress,
		// Token: 0x040002DC RID: 732
		OldApplicationQuitting,
		// Token: 0x040002DD RID: 733
		TransitionAborted,
		// Token: 0x040002DE RID: 734
		IsTemplate,
		// Token: 0x040002DF RID: 735
		BufferTooSmall = 200,
		// Token: 0x040002E0 RID: 736
		PropertyNotSet,
		// Token: 0x040002E1 RID: 737
		UnknownProperty,
		// Token: 0x040002E2 RID: 738
		InvalidParameter
	}
}
