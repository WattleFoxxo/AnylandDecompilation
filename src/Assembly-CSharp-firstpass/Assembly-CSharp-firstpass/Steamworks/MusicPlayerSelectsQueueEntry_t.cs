using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000211 RID: 529
	[CallbackIdentity(4012)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsQueueEntry_t
	{
		// Token: 0x040006AB RID: 1707
		public const int k_iCallback = 4012;

		// Token: 0x040006AC RID: 1708
		public int nID;
	}
}
