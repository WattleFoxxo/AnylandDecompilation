using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000154 RID: 340
	[StructLayout(LayoutKind.Explicit)]
	public struct VREvent_Data_t
	{
		// Token: 0x04000361 RID: 865
		[FieldOffset(0)]
		public VREvent_Reserved_t reserved;

		// Token: 0x04000362 RID: 866
		[FieldOffset(0)]
		public VREvent_Controller_t controller;

		// Token: 0x04000363 RID: 867
		[FieldOffset(0)]
		public VREvent_Mouse_t mouse;

		// Token: 0x04000364 RID: 868
		[FieldOffset(0)]
		public VREvent_Scroll_t scroll;

		// Token: 0x04000365 RID: 869
		[FieldOffset(0)]
		public VREvent_Process_t process;

		// Token: 0x04000366 RID: 870
		[FieldOffset(0)]
		public VREvent_Notification_t notification;

		// Token: 0x04000367 RID: 871
		[FieldOffset(0)]
		public VREvent_Overlay_t overlay;

		// Token: 0x04000368 RID: 872
		[FieldOffset(0)]
		public VREvent_Status_t status;

		// Token: 0x04000369 RID: 873
		[FieldOffset(0)]
		public VREvent_Ipd_t ipd;

		// Token: 0x0400036A RID: 874
		[FieldOffset(0)]
		public VREvent_Chaperone_t chaperone;

		// Token: 0x0400036B RID: 875
		[FieldOffset(0)]
		public VREvent_PerformanceTest_t performanceTest;

		// Token: 0x0400036C RID: 876
		[FieldOffset(0)]
		public VREvent_TouchPadMove_t touchPadMove;

		// Token: 0x0400036D RID: 877
		[FieldOffset(0)]
		public VREvent_SeatedZeroPoseReset_t seatedZeroPoseReset;

		// Token: 0x0400036E RID: 878
		[FieldOffset(0)]
		public VREvent_Screenshot_t screenshot;

		// Token: 0x0400036F RID: 879
		[FieldOffset(0)]
		public VREvent_Keyboard_t keyboard;
	}
}
