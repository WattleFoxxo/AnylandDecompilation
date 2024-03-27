using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B1 RID: 689
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ControllerMotionData_t
	{
		// Token: 0x04000C42 RID: 3138
		public float rotQuatX;

		// Token: 0x04000C43 RID: 3139
		public float rotQuatY;

		// Token: 0x04000C44 RID: 3140
		public float rotQuatZ;

		// Token: 0x04000C45 RID: 3141
		public float rotQuatW;

		// Token: 0x04000C46 RID: 3142
		public float posAccelX;

		// Token: 0x04000C47 RID: 3143
		public float posAccelY;

		// Token: 0x04000C48 RID: 3144
		public float posAccelZ;

		// Token: 0x04000C49 RID: 3145
		public float rotVelX;

		// Token: 0x04000C4A RID: 3146
		public float rotVelY;

		// Token: 0x04000C4B RID: 3147
		public float rotVelZ;
	}
}
