using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200021E RID: 542
	[CallbackIdentity(1312)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserPublishedFilesResult_t
	{
		// Token: 0x040006D7 RID: 1751
		public const int k_iCallback = 1312;

		// Token: 0x040006D8 RID: 1752
		public EResult m_eResult;

		// Token: 0x040006D9 RID: 1753
		public int m_nResultsReturned;

		// Token: 0x040006DA RID: 1754
		public int m_nTotalResultCount;

		// Token: 0x040006DB RID: 1755
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
