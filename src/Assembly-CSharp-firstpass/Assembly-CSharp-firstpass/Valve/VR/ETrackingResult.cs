using System;

namespace Valve.VR
{
	// Token: 0x02000129 RID: 297
	public enum ETrackingResult
	{
		// Token: 0x04000122 RID: 290
		Uninitialized = 1,
		// Token: 0x04000123 RID: 291
		Calibrating_InProgress = 100,
		// Token: 0x04000124 RID: 292
		Calibrating_OutOfRange,
		// Token: 0x04000125 RID: 293
		Running_OK = 200,
		// Token: 0x04000126 RID: 294
		Running_OutOfRange
	}
}
