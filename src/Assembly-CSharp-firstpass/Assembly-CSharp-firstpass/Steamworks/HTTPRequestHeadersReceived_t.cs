using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F3 RID: 499
	[CallbackIdentity(2102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTTPRequestHeadersReceived_t
	{
		// Token: 0x04000658 RID: 1624
		public const int k_iCallback = 2102;

		// Token: 0x04000659 RID: 1625
		public HTTPRequestHandle m_hRequest;

		// Token: 0x0400065A RID: 1626
		public ulong m_ulContextValue;
	}
}
