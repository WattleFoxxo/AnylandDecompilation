using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000249 RID: 585
	[CallbackIdentity(154)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct EncryptedAppTicketResponse_t
	{
		// Token: 0x0400078F RID: 1935
		public const int k_iCallback = 154;

		// Token: 0x04000790 RID: 1936
		public EResult m_eResult;
	}
}
