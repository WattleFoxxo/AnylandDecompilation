using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000219 RID: 537
	[CallbackIdentity(1303)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncProgress_t
	{
		// Token: 0x040006C3 RID: 1731
		public const int k_iCallback = 1303;

		// Token: 0x040006C4 RID: 1732
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchCurrentFile;

		// Token: 0x040006C5 RID: 1733
		public AppId_t m_nAppID;

		// Token: 0x040006C6 RID: 1734
		public uint m_uBytesTransferredThisChunk;

		// Token: 0x040006C7 RID: 1735
		public double m_dAppPercentComplete;

		// Token: 0x040006C8 RID: 1736
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUploading;
	}
}
