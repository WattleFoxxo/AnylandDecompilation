using System;

namespace Valve.VR
{
	// Token: 0x02000153 RID: 339
	public enum EVRScreenshotError
	{
		// Token: 0x0400035B RID: 859
		None,
		// Token: 0x0400035C RID: 860
		RequestFailed,
		// Token: 0x0400035D RID: 861
		IncompatibleVersion = 100,
		// Token: 0x0400035E RID: 862
		NotFound,
		// Token: 0x0400035F RID: 863
		BufferTooSmall,
		// Token: 0x04000360 RID: 864
		ScreenshotAlreadyInProgress = 108
	}
}
