using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000213 RID: 531
	[CallbackIdentity(4114)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsPlayingRepeatStatus_t
	{
		// Token: 0x040006AF RID: 1711
		public const int k_iCallback = 4114;

		// Token: 0x040006B0 RID: 1712
		public int m_nPlayingRepeatStatus;
	}
}
