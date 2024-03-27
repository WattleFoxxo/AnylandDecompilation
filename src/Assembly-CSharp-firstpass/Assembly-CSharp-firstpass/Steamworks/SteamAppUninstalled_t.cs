using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B5 RID: 437
	[CallbackIdentity(3902)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppUninstalled_t
	{
		// Token: 0x04000567 RID: 1383
		public const int k_iCallback = 3902;

		// Token: 0x04000568 RID: 1384
		public AppId_t m_nAppID;
	}
}
