using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200021C RID: 540
	[CallbackIdentity(1309)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishFileResult_t
	{
		// Token: 0x040006D0 RID: 1744
		public const int k_iCallback = 1309;

		// Token: 0x040006D1 RID: 1745
		public EResult m_eResult;

		// Token: 0x040006D2 RID: 1746
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040006D3 RID: 1747
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
