using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000239 RID: 569
	[CallbackIdentity(3405)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ItemInstalled_t
	{
		// Token: 0x0400075A RID: 1882
		public const int k_iCallback = 3405;

		// Token: 0x0400075B RID: 1883
		public AppId_t m_unAppID;

		// Token: 0x0400075C RID: 1884
		public PublishedFileId_t m_nPublishedFileId;
	}
}
