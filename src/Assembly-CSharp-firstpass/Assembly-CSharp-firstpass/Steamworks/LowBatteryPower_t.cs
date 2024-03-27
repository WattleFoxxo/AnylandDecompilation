using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200025A RID: 602
	[CallbackIdentity(702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LowBatteryPower_t
	{
		// Token: 0x040007C7 RID: 1991
		public const int k_iCallback = 702;

		// Token: 0x040007C8 RID: 1992
		public byte m_nMinutesBatteryLeft;
	}
}
