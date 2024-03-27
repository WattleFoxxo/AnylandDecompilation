using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000260 RID: 608
	[CallbackIdentity(4605)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct BroadcastUploadStop_t
	{
		// Token: 0x040007D4 RID: 2004
		public const int k_iCallback = 4605;

		// Token: 0x040007D5 RID: 2005
		public EBroadcastUploadResult m_eResult;
	}
}
