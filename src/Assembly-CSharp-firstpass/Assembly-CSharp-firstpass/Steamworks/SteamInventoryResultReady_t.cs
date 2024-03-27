using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F5 RID: 501
	[CallbackIdentity(4700)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamInventoryResultReady_t
	{
		// Token: 0x04000660 RID: 1632
		public const int k_iCallback = 4700;

		// Token: 0x04000661 RID: 1633
		public SteamInventoryResult_t m_handle;

		// Token: 0x04000662 RID: 1634
		public EResult m_result;
	}
}
