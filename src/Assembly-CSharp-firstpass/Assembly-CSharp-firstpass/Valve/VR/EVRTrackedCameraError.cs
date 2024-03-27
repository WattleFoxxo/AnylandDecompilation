using System;

namespace Valve.VR
{
	// Token: 0x0200013F RID: 319
	public enum EVRTrackedCameraError
	{
		// Token: 0x040002B8 RID: 696
		None,
		// Token: 0x040002B9 RID: 697
		OperationFailed = 100,
		// Token: 0x040002BA RID: 698
		InvalidHandle,
		// Token: 0x040002BB RID: 699
		InvalidFrameHeaderVersion,
		// Token: 0x040002BC RID: 700
		OutOfHandles,
		// Token: 0x040002BD RID: 701
		IPCFailure,
		// Token: 0x040002BE RID: 702
		NotSupportedForThisDevice,
		// Token: 0x040002BF RID: 703
		SharedMemoryFailure,
		// Token: 0x040002C0 RID: 704
		FrameBufferingFailure,
		// Token: 0x040002C1 RID: 705
		StreamSetupFailure,
		// Token: 0x040002C2 RID: 706
		InvalidGLTextureId,
		// Token: 0x040002C3 RID: 707
		InvalidSharedTextureHandle,
		// Token: 0x040002C4 RID: 708
		FailedToGetGLTextureId,
		// Token: 0x040002C5 RID: 709
		SharedTextureFailure,
		// Token: 0x040002C6 RID: 710
		NoFrameAvailable,
		// Token: 0x040002C7 RID: 711
		InvalidArgument,
		// Token: 0x040002C8 RID: 712
		InvalidFrameBufferSize
	}
}
