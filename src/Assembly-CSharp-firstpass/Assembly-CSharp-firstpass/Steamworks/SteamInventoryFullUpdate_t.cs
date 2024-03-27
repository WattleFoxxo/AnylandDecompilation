using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F6 RID: 502
	[CallbackIdentity(4701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryFullUpdate_t
	{
		// Token: 0x04000663 RID: 1635
		public const int k_iCallback = 4701;

		// Token: 0x04000664 RID: 1636
		public SteamInventoryResult_t m_handle;
	}
}
