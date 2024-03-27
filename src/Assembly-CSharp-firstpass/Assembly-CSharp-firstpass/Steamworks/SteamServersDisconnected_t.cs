using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000243 RID: 579
	[CallbackIdentity(103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServersDisconnected_t
	{
		// Token: 0x0400077C RID: 1916
		public const int k_iCallback = 103;

		// Token: 0x0400077D RID: 1917
		public EResult m_eResult;
	}
}
