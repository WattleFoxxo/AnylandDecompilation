using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200022C RID: 556
	[CallbackIdentity(1326)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSharedWorkshopFilesResult_t
	{
		// Token: 0x04000728 RID: 1832
		public const int k_iCallback = 1326;

		// Token: 0x04000729 RID: 1833
		public EResult m_eResult;

		// Token: 0x0400072A RID: 1834
		public int m_nResultsReturned;

		// Token: 0x0400072B RID: 1835
		public int m_nTotalResultCount;

		// Token: 0x0400072C RID: 1836
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
