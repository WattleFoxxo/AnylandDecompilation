using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200025B RID: 603
	[CallbackIdentity(703)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamAPICallCompleted_t
	{
		// Token: 0x040007C9 RID: 1993
		public const int k_iCallback = 703;

		// Token: 0x040007CA RID: 1994
		public SteamAPICall_t m_hAsyncCall;

		// Token: 0x040007CB RID: 1995
		public int m_iCallback;

		// Token: 0x040007CC RID: 1996
		public uint m_cubParam;
	}
}
