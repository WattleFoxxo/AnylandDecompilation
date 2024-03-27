using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D0 RID: 464
	[CallbackIdentity(202)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSClientDeny_t
	{
		// Token: 0x040005BE RID: 1470
		public const int k_iCallback = 202;

		// Token: 0x040005BF RID: 1471
		public CSteamID m_SteamID;

		// Token: 0x040005C0 RID: 1472
		public EDenyReason m_eDenyReason;

		// Token: 0x040005C1 RID: 1473
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchOptionalText;
	}
}
