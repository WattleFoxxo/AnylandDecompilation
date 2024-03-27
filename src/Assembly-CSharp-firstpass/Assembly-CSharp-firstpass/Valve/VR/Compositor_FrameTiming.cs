using System;

namespace Valve.VR
{
	// Token: 0x0200017B RID: 379
	public struct Compositor_FrameTiming
	{
		// Token: 0x04000408 RID: 1032
		public uint m_nSize;

		// Token: 0x04000409 RID: 1033
		public uint m_nFrameIndex;

		// Token: 0x0400040A RID: 1034
		public uint m_nNumFramePresents;

		// Token: 0x0400040B RID: 1035
		public uint m_nNumDroppedFrames;

		// Token: 0x0400040C RID: 1036
		public uint m_nReprojectionFlags;

		// Token: 0x0400040D RID: 1037
		public double m_flSystemTimeInSeconds;

		// Token: 0x0400040E RID: 1038
		public float m_flPreSubmitGpuMs;

		// Token: 0x0400040F RID: 1039
		public float m_flPostSubmitGpuMs;

		// Token: 0x04000410 RID: 1040
		public float m_flTotalRenderGpuMs;

		// Token: 0x04000411 RID: 1041
		public float m_flCompositorRenderGpuMs;

		// Token: 0x04000412 RID: 1042
		public float m_flCompositorRenderCpuMs;

		// Token: 0x04000413 RID: 1043
		public float m_flCompositorIdleCpuMs;

		// Token: 0x04000414 RID: 1044
		public float m_flClientFrameIntervalMs;

		// Token: 0x04000415 RID: 1045
		public float m_flPresentCallCpuMs;

		// Token: 0x04000416 RID: 1046
		public float m_flWaitForPresentCpuMs;

		// Token: 0x04000417 RID: 1047
		public float m_flSubmitFrameMs;

		// Token: 0x04000418 RID: 1048
		public float m_flWaitGetPosesCalledMs;

		// Token: 0x04000419 RID: 1049
		public float m_flNewPosesReadyMs;

		// Token: 0x0400041A RID: 1050
		public float m_flNewFrameReadyMs;

		// Token: 0x0400041B RID: 1051
		public float m_flCompositorUpdateStartMs;

		// Token: 0x0400041C RID: 1052
		public float m_flCompositorUpdateEndMs;

		// Token: 0x0400041D RID: 1053
		public float m_flCompositorRenderStartMs;

		// Token: 0x0400041E RID: 1054
		public TrackedDevicePose_t m_HmdPose;
	}
}
