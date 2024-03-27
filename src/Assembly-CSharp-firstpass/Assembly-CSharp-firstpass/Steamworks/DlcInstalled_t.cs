using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B6 RID: 438
	[CallbackIdentity(1005)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DlcInstalled_t
	{
		// Token: 0x04000569 RID: 1385
		public const int k_iCallback = 1005;

		// Token: 0x0400056A RID: 1386
		public AppId_t m_nAppID;
	}
}
