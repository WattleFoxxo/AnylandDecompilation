using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200023E RID: 574
	[CallbackIdentity(3410)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StartPlaytimeTrackingResult_t
	{
		// Token: 0x0400076F RID: 1903
		public const int k_iCallback = 3410;

		// Token: 0x04000770 RID: 1904
		public EResult m_eResult;
	}
}
