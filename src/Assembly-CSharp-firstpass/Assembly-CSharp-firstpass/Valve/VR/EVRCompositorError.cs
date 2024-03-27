using System;

namespace Valve.VR
{
	// Token: 0x02000147 RID: 327
	public enum EVRCompositorError
	{
		// Token: 0x04000308 RID: 776
		None,
		// Token: 0x04000309 RID: 777
		RequestFailed,
		// Token: 0x0400030A RID: 778
		IncompatibleVersion = 100,
		// Token: 0x0400030B RID: 779
		DoNotHaveFocus,
		// Token: 0x0400030C RID: 780
		InvalidTexture,
		// Token: 0x0400030D RID: 781
		IsNotSceneApplication,
		// Token: 0x0400030E RID: 782
		TextureIsOnWrongDevice,
		// Token: 0x0400030F RID: 783
		TextureUsesUnsupportedFormat,
		// Token: 0x04000310 RID: 784
		SharedTexturesNotSupported,
		// Token: 0x04000311 RID: 785
		IndexOutOfRange
	}
}
