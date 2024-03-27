using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000232 RID: 562
	[CallbackIdentity(1332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileReadAsyncComplete_t
	{
		// Token: 0x04000741 RID: 1857
		public const int k_iCallback = 1332;

		// Token: 0x04000742 RID: 1858
		public SteamAPICall_t m_hFileReadAsync;

		// Token: 0x04000743 RID: 1859
		public EResult m_eResult;

		// Token: 0x04000744 RID: 1860
		public uint m_nOffset;

		// Token: 0x04000745 RID: 1861
		public uint m_cubRead;
	}
}
