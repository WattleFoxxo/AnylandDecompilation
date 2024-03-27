using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000200 RID: 512
	[CallbackIdentity(510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LobbyMatchList_t
	{
		// Token: 0x0400068F RID: 1679
		public const int k_iCallback = 510;

		// Token: 0x04000690 RID: 1680
		public uint m_nLobbiesMatching;
	}
}
