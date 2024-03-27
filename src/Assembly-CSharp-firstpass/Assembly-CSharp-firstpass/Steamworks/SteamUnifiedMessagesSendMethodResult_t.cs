using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000240 RID: 576
	[CallbackIdentity(2501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamUnifiedMessagesSendMethodResult_t
	{
		// Token: 0x04000773 RID: 1907
		public const int k_iCallback = 2501;

		// Token: 0x04000774 RID: 1908
		public ClientUnifiedMessageHandle m_hHandle;

		// Token: 0x04000775 RID: 1909
		public ulong m_unContext;

		// Token: 0x04000776 RID: 1910
		public EResult m_eResult;

		// Token: 0x04000777 RID: 1911
		public uint m_unResponseSize;
	}
}
