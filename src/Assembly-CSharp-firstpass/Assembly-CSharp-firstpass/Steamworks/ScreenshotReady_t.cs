using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000233 RID: 563
	[CallbackIdentity(2301)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ScreenshotReady_t
	{
		// Token: 0x04000746 RID: 1862
		public const int k_iCallback = 2301;

		// Token: 0x04000747 RID: 1863
		public ScreenshotHandle m_hLocal;

		// Token: 0x04000748 RID: 1864
		public EResult m_eResult;
	}
}
