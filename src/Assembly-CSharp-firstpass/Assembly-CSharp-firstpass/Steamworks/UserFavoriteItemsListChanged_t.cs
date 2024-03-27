using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200023B RID: 571
	[CallbackIdentity(3407)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserFavoriteItemsListChanged_t
	{
		// Token: 0x04000761 RID: 1889
		public const int k_iCallback = 3407;

		// Token: 0x04000762 RID: 1890
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000763 RID: 1891
		public EResult m_eResult;

		// Token: 0x04000764 RID: 1892
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bWasAddRequest;
	}
}
