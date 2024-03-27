using System;

namespace Valve.VR
{
	// Token: 0x02000177 RID: 375
	public struct VRControllerState_t
	{
		// Token: 0x040003EA RID: 1002
		public uint unPacketNum;

		// Token: 0x040003EB RID: 1003
		public ulong ulButtonPressed;

		// Token: 0x040003EC RID: 1004
		public ulong ulButtonTouched;

		// Token: 0x040003ED RID: 1005
		public VRControllerAxis_t rAxis0;

		// Token: 0x040003EE RID: 1006
		public VRControllerAxis_t rAxis1;

		// Token: 0x040003EF RID: 1007
		public VRControllerAxis_t rAxis2;

		// Token: 0x040003F0 RID: 1008
		public VRControllerAxis_t rAxis3;

		// Token: 0x040003F1 RID: 1009
		public VRControllerAxis_t rAxis4;
	}
}
