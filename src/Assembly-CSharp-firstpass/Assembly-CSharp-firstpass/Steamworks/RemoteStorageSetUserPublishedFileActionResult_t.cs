using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200022D RID: 557
	[CallbackIdentity(1327)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSetUserPublishedFileActionResult_t
	{
		// Token: 0x0400072D RID: 1837
		public const int k_iCallback = 1327;

		// Token: 0x0400072E RID: 1838
		public EResult m_eResult;

		// Token: 0x0400072F RID: 1839
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000730 RID: 1840
		public EWorkshopFileAction m_eAction;
	}
}
