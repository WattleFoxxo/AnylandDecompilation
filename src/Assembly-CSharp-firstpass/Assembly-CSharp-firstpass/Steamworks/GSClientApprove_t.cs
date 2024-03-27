using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001CF RID: 463
	[CallbackIdentity(201)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSClientApprove_t
	{
		// Token: 0x040005BB RID: 1467
		public const int k_iCallback = 201;

		// Token: 0x040005BC RID: 1468
		public CSteamID m_SteamID;

		// Token: 0x040005BD RID: 1469
		public CSteamID m_OwnerSteamID;
	}
}
