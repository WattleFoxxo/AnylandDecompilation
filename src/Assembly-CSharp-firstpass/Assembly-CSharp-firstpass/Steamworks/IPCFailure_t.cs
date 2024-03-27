using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000245 RID: 581
	[CallbackIdentity(117)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct IPCFailure_t
	{
		// Token: 0x04000784 RID: 1924
		public const int k_iCallback = 117;

		// Token: 0x04000785 RID: 1925
		public byte m_eFailureType;
	}
}
