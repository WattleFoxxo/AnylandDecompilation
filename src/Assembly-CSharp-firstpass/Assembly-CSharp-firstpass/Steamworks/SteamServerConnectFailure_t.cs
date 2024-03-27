using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000242 RID: 578
	[CallbackIdentity(102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamServerConnectFailure_t
	{
		// Token: 0x04000779 RID: 1913
		public const int k_iCallback = 102;

		// Token: 0x0400077A RID: 1914
		public EResult m_eResult;

		// Token: 0x0400077B RID: 1915
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bStillRetrying;
	}
}
