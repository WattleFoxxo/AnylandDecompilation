using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002AF RID: 687
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ControllerAnalogActionData_t
	{
		// Token: 0x04000C3C RID: 3132
		public EControllerSourceMode eMode;

		// Token: 0x04000C3D RID: 3133
		public float x;

		// Token: 0x04000C3E RID: 3134
		public float y;

		// Token: 0x04000C3F RID: 3135
		public byte bActive;
	}
}
