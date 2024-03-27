using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000220 RID: 544
	[CallbackIdentity(1314)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
	{
		// Token: 0x040006DF RID: 1759
		public const int k_iCallback = 1314;

		// Token: 0x040006E0 RID: 1760
		public EResult m_eResult;

		// Token: 0x040006E1 RID: 1761
		public int m_nResultsReturned;

		// Token: 0x040006E2 RID: 1762
		public int m_nTotalResultCount;

		// Token: 0x040006E3 RID: 1763
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040006E4 RID: 1764
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeSubscribed;
	}
}
