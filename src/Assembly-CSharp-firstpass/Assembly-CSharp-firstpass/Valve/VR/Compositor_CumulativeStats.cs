using System;

namespace Valve.VR
{
	// Token: 0x0200017C RID: 380
	public struct Compositor_CumulativeStats
	{
		// Token: 0x0400041F RID: 1055
		public uint m_nPid;

		// Token: 0x04000420 RID: 1056
		public uint m_nNumFramePresents;

		// Token: 0x04000421 RID: 1057
		public uint m_nNumDroppedFrames;

		// Token: 0x04000422 RID: 1058
		public uint m_nNumReprojectedFrames;

		// Token: 0x04000423 RID: 1059
		public uint m_nNumFramePresentsOnStartup;

		// Token: 0x04000424 RID: 1060
		public uint m_nNumDroppedFramesOnStartup;

		// Token: 0x04000425 RID: 1061
		public uint m_nNumReprojectedFramesOnStartup;

		// Token: 0x04000426 RID: 1062
		public uint m_nNumLoading;

		// Token: 0x04000427 RID: 1063
		public uint m_nNumFramePresentsLoading;

		// Token: 0x04000428 RID: 1064
		public uint m_nNumDroppedFramesLoading;

		// Token: 0x04000429 RID: 1065
		public uint m_nNumReprojectedFramesLoading;

		// Token: 0x0400042A RID: 1066
		public uint m_nNumTimedOut;

		// Token: 0x0400042B RID: 1067
		public uint m_nNumFramePresentsTimedOut;

		// Token: 0x0400042C RID: 1068
		public uint m_nNumDroppedFramesTimedOut;

		// Token: 0x0400042D RID: 1069
		public uint m_nNumReprojectedFramesTimedOut;
	}
}
