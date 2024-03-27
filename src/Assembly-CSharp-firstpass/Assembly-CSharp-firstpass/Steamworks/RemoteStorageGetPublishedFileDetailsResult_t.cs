using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000224 RID: 548
	[CallbackIdentity(1318)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedFileDetailsResult_t
	{
		// Token: 0x040006F3 RID: 1779
		public const int k_iCallback = 1318;

		// Token: 0x040006F4 RID: 1780
		public EResult m_eResult;

		// Token: 0x040006F5 RID: 1781
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x040006F6 RID: 1782
		public AppId_t m_nCreatorAppID;

		// Token: 0x040006F7 RID: 1783
		public AppId_t m_nConsumerAppID;

		// Token: 0x040006F8 RID: 1784
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
		public string m_rgchTitle;

		// Token: 0x040006F9 RID: 1785
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8000)]
		public string m_rgchDescription;

		// Token: 0x040006FA RID: 1786
		public UGCHandle_t m_hFile;

		// Token: 0x040006FB RID: 1787
		public UGCHandle_t m_hPreviewFile;

		// Token: 0x040006FC RID: 1788
		public ulong m_ulSteamIDOwner;

		// Token: 0x040006FD RID: 1789
		public uint m_rtimeCreated;

		// Token: 0x040006FE RID: 1790
		public uint m_rtimeUpdated;

		// Token: 0x040006FF RID: 1791
		public ERemoteStoragePublishedFileVisibility m_eVisibility;

		// Token: 0x04000700 RID: 1792
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bBanned;

		// Token: 0x04000701 RID: 1793
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
		public string m_rgchTags;

		// Token: 0x04000702 RID: 1794
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bTagsTruncated;

		// Token: 0x04000703 RID: 1795
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x04000704 RID: 1796
		public int m_nFileSize;

		// Token: 0x04000705 RID: 1797
		public int m_nPreviewFileSize;

		// Token: 0x04000706 RID: 1798
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_rgchURL;

		// Token: 0x04000707 RID: 1799
		public EWorkshopFileType m_eFileType;

		// Token: 0x04000708 RID: 1800
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bAcceptedForUse;
	}
}
