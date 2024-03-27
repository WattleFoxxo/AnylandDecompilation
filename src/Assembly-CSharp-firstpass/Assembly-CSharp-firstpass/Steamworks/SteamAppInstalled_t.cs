using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B4 RID: 436
	[CallbackIdentity(3901)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAppInstalled_t
	{
		// Token: 0x04000565 RID: 1381
		public const int k_iCallback = 3901;

		// Token: 0x04000566 RID: 1382
		public AppId_t m_nAppID;
	}
}
