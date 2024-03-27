using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000237 RID: 567
	[CallbackIdentity(3403)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct CreateItemResult_t
	{
		// Token: 0x04000753 RID: 1875
		public const int k_iCallback = 3403;

		// Token: 0x04000754 RID: 1876
		public EResult m_eResult;

		// Token: 0x04000755 RID: 1877
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000756 RID: 1878
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
