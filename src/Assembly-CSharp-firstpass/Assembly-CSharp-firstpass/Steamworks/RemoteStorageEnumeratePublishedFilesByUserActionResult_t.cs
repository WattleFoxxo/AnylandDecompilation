using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200022E RID: 558
	[CallbackIdentity(1328)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumeratePublishedFilesByUserActionResult_t
	{
		// Token: 0x04000731 RID: 1841
		public const int k_iCallback = 1328;

		// Token: 0x04000732 RID: 1842
		public EResult m_eResult;

		// Token: 0x04000733 RID: 1843
		public EWorkshopFileAction m_eAction;

		// Token: 0x04000734 RID: 1844
		public int m_nResultsReturned;

		// Token: 0x04000735 RID: 1845
		public int m_nTotalResultCount;

		// Token: 0x04000736 RID: 1846
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x04000737 RID: 1847
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeUpdated;
	}
}
