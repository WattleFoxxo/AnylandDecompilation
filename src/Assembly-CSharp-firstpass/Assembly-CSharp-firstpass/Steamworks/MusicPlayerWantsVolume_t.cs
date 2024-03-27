using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000210 RID: 528
	[CallbackIdentity(4011)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsVolume_t
	{
		// Token: 0x040006A9 RID: 1705
		public const int k_iCallback = 4011;

		// Token: 0x040006AA RID: 1706
		public float m_flNewVolume;
	}
}
