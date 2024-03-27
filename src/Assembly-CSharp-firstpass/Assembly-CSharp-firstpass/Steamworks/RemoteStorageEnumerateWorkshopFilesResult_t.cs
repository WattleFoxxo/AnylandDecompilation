using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000225 RID: 549
	[CallbackIdentity(1319)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateWorkshopFilesResult_t
	{
		// Token: 0x04000709 RID: 1801
		public const int k_iCallback = 1319;

		// Token: 0x0400070A RID: 1802
		public EResult m_eResult;

		// Token: 0x0400070B RID: 1803
		public int m_nResultsReturned;

		// Token: 0x0400070C RID: 1804
		public int m_nTotalResultCount;

		// Token: 0x0400070D RID: 1805
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x0400070E RID: 1806
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public float[] m_rgScore;

		// Token: 0x0400070F RID: 1807
		public AppId_t m_nAppId;

		// Token: 0x04000710 RID: 1808
		public uint m_unStartIndex;
	}
}
