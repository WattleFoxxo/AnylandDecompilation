using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200020E RID: 526
	[CallbackIdentity(4109)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerWantsShuffled_t
	{
		// Token: 0x040006A5 RID: 1701
		public const int k_iCallback = 4109;

		// Token: 0x040006A6 RID: 1702
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bShuffled;
	}
}
