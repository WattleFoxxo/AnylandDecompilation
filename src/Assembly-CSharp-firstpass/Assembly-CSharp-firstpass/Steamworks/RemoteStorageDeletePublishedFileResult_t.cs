using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200021D RID: 541
	[CallbackIdentity(1311)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageDeletePublishedFileResult_t
	{
		// Token: 0x040006D4 RID: 1748
		public const int k_iCallback = 1311;

		// Token: 0x040006D5 RID: 1749
		public EResult m_eResult;

		// Token: 0x040006D6 RID: 1750
		public PublishedFileId_t m_nPublishedFileId;
	}
}
