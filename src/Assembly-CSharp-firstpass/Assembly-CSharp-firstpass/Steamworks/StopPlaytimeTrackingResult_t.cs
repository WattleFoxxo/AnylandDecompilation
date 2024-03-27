using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200023F RID: 575
	[CallbackIdentity(3411)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct StopPlaytimeTrackingResult_t
	{
		// Token: 0x04000771 RID: 1905
		public const int k_iCallback = 3411;

		// Token: 0x04000772 RID: 1906
		public EResult m_eResult;
	}
}
