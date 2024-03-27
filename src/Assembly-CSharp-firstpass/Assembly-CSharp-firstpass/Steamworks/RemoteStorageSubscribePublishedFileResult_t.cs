using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200021F RID: 543
	[CallbackIdentity(1313)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageSubscribePublishedFileResult_t
	{
		// Token: 0x040006DC RID: 1756
		public const int k_iCallback = 1313;

		// Token: 0x040006DD RID: 1757
		public EResult m_eResult;

		// Token: 0x040006DE RID: 1758
		public PublishedFileId_t m_nPublishedFileId;
	}
}
