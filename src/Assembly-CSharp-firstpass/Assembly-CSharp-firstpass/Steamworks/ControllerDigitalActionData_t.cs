using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B0 RID: 688
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ControllerDigitalActionData_t
	{
		// Token: 0x04000C40 RID: 3136
		public byte bState;

		// Token: 0x04000C41 RID: 3137
		public byte bActive;
	}
}
