using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C6 RID: 454
	[CallbackIdentity(341)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadClanActivityCountsResult_t
	{
		// Token: 0x0400059F RID: 1439
		public const int k_iCallback = 341;

		// Token: 0x040005A0 RID: 1440
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;
	}
}
