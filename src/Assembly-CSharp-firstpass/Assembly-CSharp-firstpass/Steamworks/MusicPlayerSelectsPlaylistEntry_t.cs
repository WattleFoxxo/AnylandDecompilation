using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000212 RID: 530
	[CallbackIdentity(4013)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MusicPlayerSelectsPlaylistEntry_t
	{
		// Token: 0x040006AD RID: 1709
		public const int k_iCallback = 4013;

		// Token: 0x040006AE RID: 1710
		public int nID;
	}
}
