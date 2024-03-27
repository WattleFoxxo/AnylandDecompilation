using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200020F RID: 527
	[CallbackIdentity(4110)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsLooped_t
	{
		// Token: 0x040006A7 RID: 1703
		public const int k_iCallback = 4110;

		// Token: 0x040006A8 RID: 1704
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLooped;
	}
}
