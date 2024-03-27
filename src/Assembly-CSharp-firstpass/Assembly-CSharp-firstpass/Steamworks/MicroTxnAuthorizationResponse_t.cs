using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000248 RID: 584
	[CallbackIdentity(152)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct MicroTxnAuthorizationResponse_t
	{
		// Token: 0x0400078B RID: 1931
		public const int k_iCallback = 152;

		// Token: 0x0400078C RID: 1932
		public uint m_unAppID;

		// Token: 0x0400078D RID: 1933
		public ulong m_ulOrderID;

		// Token: 0x0400078E RID: 1934
		public byte m_bAuthorized;
	}
}
