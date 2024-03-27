using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D3 RID: 467
	[CallbackIdentity(115)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSPolicyResponse_t
	{
		// Token: 0x040005C9 RID: 1481
		public const int k_iCallback = 115;

		// Token: 0x040005CA RID: 1482
		public byte m_bSecure;
	}
}
