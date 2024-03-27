using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000222 RID: 546
	[CallbackIdentity(1316)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdatePublishedFileResult_t
	{
		// Token: 0x040006E8 RID: 1768
		public const int k_iCallback = 1316;

		// Token: 0x040006E9 RID: 1769
		public EResult m_eResult;

		// Token: 0x040006EA RID: 1770
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040006EB RID: 1771
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
