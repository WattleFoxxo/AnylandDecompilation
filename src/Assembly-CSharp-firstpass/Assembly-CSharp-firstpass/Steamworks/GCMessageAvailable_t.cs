using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001CD RID: 461
	[CallbackIdentity(1701)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GCMessageAvailable_t
	{
		// Token: 0x040005B8 RID: 1464
		public const int k_iCallback = 1701;

		// Token: 0x040005B9 RID: 1465
		public uint m_nMessageSize;
	}
}
