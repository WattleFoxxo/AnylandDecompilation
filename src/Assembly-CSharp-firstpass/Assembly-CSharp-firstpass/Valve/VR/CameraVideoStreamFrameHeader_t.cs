using System;

namespace Valve.VR
{
	// Token: 0x02000179 RID: 377
	public struct CameraVideoStreamFrameHeader_t
	{
		// Token: 0x04000400 RID: 1024
		public EVRTrackedCameraFrameType eFrameType;

		// Token: 0x04000401 RID: 1025
		public uint nWidth;

		// Token: 0x04000402 RID: 1026
		public uint nHeight;

		// Token: 0x04000403 RID: 1027
		public uint nBytesPerPixel;

		// Token: 0x04000404 RID: 1028
		public uint nFrameSequence;

		// Token: 0x04000405 RID: 1029
		public TrackedDevicePose_t standingTrackedDevicePose;
	}
}
