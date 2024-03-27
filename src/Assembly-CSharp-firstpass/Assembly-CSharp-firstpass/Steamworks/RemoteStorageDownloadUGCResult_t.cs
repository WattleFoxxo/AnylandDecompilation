using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000223 RID: 547
	[CallbackIdentity(1317)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDownloadUGCResult_t
	{
		// Token: 0x040006EC RID: 1772
		public const int k_iCallback = 1317;

		// Token: 0x040006ED RID: 1773
		public EResult m_eResult;

		// Token: 0x040006EE RID: 1774
		public UGCHandle_t m_hFile;

		// Token: 0x040006EF RID: 1775
		public AppId_t m_nAppID;

		// Token: 0x040006F0 RID: 1776
		public int m_nSizeInBytes;

		// Token: 0x040006F1 RID: 1777
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_pchFileName;

		// Token: 0x040006F2 RID: 1778
		public ulong m_ulSteamIDOwner;
	}
}
