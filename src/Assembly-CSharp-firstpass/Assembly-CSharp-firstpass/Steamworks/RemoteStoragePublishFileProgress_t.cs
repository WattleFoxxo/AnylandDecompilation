using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200022F RID: 559
	[CallbackIdentity(1329)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileProgress_t
	{
		// Token: 0x04000738 RID: 1848
		public const int k_iCallback = 1329;

		// Token: 0x04000739 RID: 1849
		public double m_dPercentFile;

		// Token: 0x0400073A RID: 1850
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bPreview;
	}
}
